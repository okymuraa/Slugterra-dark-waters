using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Slugterra.Domain.Game.Units;
using Slugterra.Game;
using Slugterra.Game.Units.Animals;
using UnityEngine;

public class SelectPointToMove : Conditional
{
	public enum SelectMoveTargetType
	{
		None = 0,
		Point = 1,
		Position = 2,
		PatrolPoint = 3,
		MeleeAttackPoint = 4,
		RangeAttackPoint = 5,
		StepBackPoint = 6,
		RetreatPoint = 7,
		ObstacleAvoid = 8,
		FearPoint = 9
	}

	public UnitControllerVar unitController;

	public SelectMoveTargetType selectType;

	public PathHolderVar pathHolder;

	public CoverNodesVar coverNodes;

	public MoveTargetTypeSharedVariable moveTargetType;

	public SharedTransform point;

	public SharedVector3 position;

	public SharedTransform attackTarget;

	public SharedTransform attackTargetRoot;

	public SharedTransform PointToMove;

	public SharedFloat targetReachedDistance;

	public SharedBool advancedMovement;

	private Dictionary<SelectMoveTargetType, Func<Vector3>> GetPositionOfMovePoint;

	private Dictionary<SelectMoveTargetType, Func<Transform>> GetParentOfMovePoint;

	private Dictionary<SelectMoveTargetType, Func<float>> GetTargetReachedDistance;

	private TaskStatus pointPositionChanged = TaskStatus.Failure;

	private Vector3 fearPoint = Vector3.zero;

	public override void OnAwake()
	{
		if (PointToMove.Value == null)
		{
			PointToMove.Value = new GameObject("~pointToMove").transform;
		}
		if (GetPositionOfMovePoint == null)
		{
			GetPositionOfMovePoint = new Dictionary<SelectMoveTargetType, Func<Vector3>>
			{
				{
					SelectMoveTargetType.Point,
					GetPointFromPoint
				},
				{
					SelectMoveTargetType.Position,
					GetPointFromPosition
				},
				{
					SelectMoveTargetType.PatrolPoint,
					GetPointFromPatrolPoint
				},
				{
					SelectMoveTargetType.StepBackPoint,
					StepBackPoint
				},
				{
					SelectMoveTargetType.RetreatPoint,
					RetreatCoverPoint
				},
				{
					SelectMoveTargetType.MeleeAttackPoint,
					MeleeChasePoint
				},
				{
					SelectMoveTargetType.RangeAttackPoint,
					RangeChasePoint
				},
				{
					SelectMoveTargetType.ObstacleAvoid,
					GetObstacleAvoidPoint
				},
				{
					SelectMoveTargetType.FearPoint,
					FearPoint
				}
			};
		}
		if (GetParentOfMovePoint == null)
		{
			GetParentOfMovePoint = new Dictionary<SelectMoveTargetType, Func<Transform>>
			{
				{
					SelectMoveTargetType.Point,
					GetNullParent
				},
				{
					SelectMoveTargetType.Position,
					GetNullParent
				},
				{
					SelectMoveTargetType.PatrolPoint,
					GetNullParent
				},
				{
					SelectMoveTargetType.StepBackPoint,
					GetNullParent
				},
				{
					SelectMoveTargetType.RetreatPoint,
					GetNullParent
				},
				{
					SelectMoveTargetType.MeleeAttackPoint,
					GetAttackTargetParent
				},
				{
					SelectMoveTargetType.ObstacleAvoid,
					GetNullParent
				},
				{
					SelectMoveTargetType.FearPoint,
					GetNullParent
				}
			};
		}
		if (GetTargetReachedDistance == null)
		{
			GetTargetReachedDistance = new Dictionary<SelectMoveTargetType, Func<float>>
			{
				{
					SelectMoveTargetType.Point,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.Position,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.PatrolPoint,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.StepBackPoint,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.RetreatPoint,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.MeleeAttackPoint,
					GetMeleeTargetReachedDistance
				},
				{
					SelectMoveTargetType.RangeAttackPoint,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.ObstacleAvoid,
					GetDefaultTargetReachedDistance
				},
				{
					SelectMoveTargetType.FearPoint,
					GetDefaultTargetReachedDistance
				}
			};
		}
		if (advancedMovement.Value)
		{
			Debug.Log("advanced movement");
		}
	}

	public override void OnStart()
	{
		if (selectType == SelectMoveTargetType.PatrolPoint && pathHolder.Value == null)
		{
			pointPositionChanged = TaskStatus.Failure;
		}
		if (!GetParentOfMovePoint.ContainsKey(SelectMoveTargetType.RangeAttackPoint))
		{
			if (advancedMovement.Value)
			{
				GetParentOfMovePoint.Add(SelectMoveTargetType.RangeAttackPoint, GetAttackTargetParent);
			}
			else
			{
				GetParentOfMovePoint.Add(SelectMoveTargetType.RangeAttackPoint, GetNullParent);
			}
		}
		if (fearPoint == Vector3.zero)
		{
			CalculateFearPoint();
		}
		PointToMove.Value.parent = GetParentOfMovePoint[selectType]();
		PointToMove.Value.position = GetPositionOfMovePoint[selectType]();
		targetReachedDistance.Value = GetTargetReachedDistance[selectType]();
		moveTargetType.Value = selectType;
		pointPositionChanged = TaskStatus.Success;
	}

	public override TaskStatus OnUpdate()
	{
		return pointPositionChanged;
	}

	private Vector3 GetPointFromPosition()
	{
		return position.Value;
	}

	private Vector3 GetPointFromPoint()
	{
		return point.Value.position;
	}

	private Vector3 StepBackPoint()
	{
		float num = 2f;
		if (attackTarget.Value == null)
		{
			return GetPointFromPatrolPoint();
		}
		return unitController.Value.transform.position - unitController.Value.transform.rotation * Vector3.forward * num;
	}

	private Vector3 RetreatPoint()
	{
		float num = 10f;
		int num2 = 0;
		Vector3 vector2;
		bool flag;
		bool flag2;
		do
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(-1, 1), 0f, UnityEngine.Random.Range(-1, 1));
			if (attackTarget.Value == null)
			{
				return GetPointFromPatrolPoint();
			}
			vector2 = unitController.Value.transform.position - unitController.Value.transform.rotation * vector * num;
			num2++;
			if (num2 > 20)
			{
				return GetPointFromPatrolPoint();
			}
			RaycastHit hitInfo;
			flag = Physics.Raycast(vector2, Vector3.down, out hitInfo, 10f) && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) == "Terrain";
			flag2 = !Physics.Raycast(vector2, Vector3.up, out hitInfo, 10f) || (LayerMask.LayerToName(hitInfo.collider.gameObject.layer) != "Terrain" && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) != "PropLayer" && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) != "Default");
		}
		while (!flag || !flag2);
		return vector2;
	}

	private Vector3 RetreatCoverPoint()
	{
		Transform transform = unitController.Value.transform;
		Transform transform2 = attackTarget.Value.transform;
		CoverNode optimalNode = MonoSingleton<CoverSystem>.Instance.GetOptimalNode(transform, transform2, 20f);
		if (optimalNode != null)
		{
			return (!(transform2 != null)) ? GetPointFromPatrolPoint() : optimalNode.GetNextPathPoint(transform.position, 20f).position;
		}
		return GetPointFromPatrolPoint();
	}

	private Vector3 GetPointFromPatrolPoint()
	{
		return pathHolder.Value.GetNextPathPoint().position;
	}

	private Vector3 MeleeChasePoint()
	{
		return attackTargetRoot.Value.transform.position;
	}

	private Vector3 RangeChasePoint()
	{
		float num = 0.5f * (unitController.Value.Model as Humanoid).Blaster.Range;
		if (attackTargetRoot.Value == null || unitController.Value == null || unitController.Value.transform == null)
		{
			return GetPointFromPatrolPoint();
		}
		float num2 = Vector3.Angle(Vector3.forward, (unitController.Value.transform.position - attackTargetRoot.Value.transform.position).normalized);
		num2 = ((!((unitController.Value.transform.position - attackTarget.Value.position).x > 0f)) ? (360f - num2) : num2);
		num2 += Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * 30f;
		Vector3 vector = attackTargetRoot.Value.transform.position + Quaternion.AngleAxis(num2, Vector3.up) * Vector3.forward * num;
		Vector3 vector2 = vector + Vector3.up;
		RaycastHit hitInfo;
		Physics.Raycast(new Ray(vector + new Vector3(0f, 20f, 0f), Vector3.down), out hitInfo, 40f, 4096);
		RaycastHit hitInfo2;
		Physics.Raycast(new Ray(vector2, (attackTarget.Value.position - vector2).normalized), out hitInfo2, (attackTarget.Value.position - vector2).magnitude, 4096);
		Debug.DrawRay(vector + new Vector3(0f, 20f, 0f), Vector3.down * 50f, Color.white, 5f);
		if (hitInfo.collider == null)
		{
			RaycastHit hitInfo3;
			Physics.Raycast(new Ray(vector + new Vector3(0f, 20f, 0f), Vector3.down), out hitInfo3, 40f, 2048);
			if (hitInfo3.collider != null)
			{
				return hitInfo3.point;
			}
			return unitController.Value.transform.position;
		}
		WarNode node = MonoSingleton<WarSystem>.Instance.GetNode(unitController.Value.Transform, attackTarget.Value.transform, num);
		if (node != null)
		{
			return node.GetOptimalPoint(unitController.Value.Transform.position, attackTarget.Value.transform.position, num).position;
		}
		Debug.Log("war system failed, point is null");
		return vector;
	}

	private Vector3 GetObstacleAvoidPoint()
	{
		RaycastHit hitInfo;
		Physics.SphereCast(new Ray(unitController.Value.Transform.position, unitController.Value.Transform.forward), 0.4f, out hitInfo, 1f, 4096);
		RaycastHit hitInfo2;
		Physics.SphereCast(new Ray(unitController.Value.Transform.position, unitController.Value.Transform.right), 0.4f, out hitInfo2, 1f, 4096);
		RaycastHit hitInfo3;
		Physics.SphereCast(new Ray(unitController.Value.Transform.position, -unitController.Value.Transform.right), 0.4f, out hitInfo3, 1f, 4096);
		RaycastHit hitInfo4;
		Physics.SphereCast(new Ray(unitController.Value.Transform.position, -unitController.Value.Transform.forward), 0.4f, out hitInfo4, 1f, 4096);
		if (hitInfo.collider == null)
		{
			return unitController.Value.Transform.position + unitController.Value.Transform.forward;
		}
		if (hitInfo4.collider == null)
		{
			return unitController.Value.Transform.position - unitController.Value.Transform.forward;
		}
		if (hitInfo2.collider == null)
		{
			return unitController.Value.Transform.position + unitController.Value.Transform.right;
		}
		if (hitInfo3.collider == null)
		{
			return unitController.Value.Transform.position - unitController.Value.Transform.forward;
		}
		Debug.Log("avoid obstacle failed");
		return unitController.Value.Transform.position;
	}

	private void CalculateFearPoint()
	{
		if (!(unitController.Value == null) && !(unitController.Value.Transform == null))
		{
			if (attackTargetRoot.Value != null)
			{
				FleePath fleePath = FleePath.Construct(unitController.Value.Transform.position, attackTargetRoot.Value.position, 10000, FearPointCalculated);
				fleePath.spread = 2000;
				fleePath.aimStrength = 0.5f;
				AstarPath.StartPath(fleePath);
			}
			else
			{
				RandomPath randomPath = RandomPath.Construct(unitController.Value.Transform.position, 10000, FearPointCalculated);
				randomPath.spread = 2000;
				randomPath.aimStrength = 0.5f;
				randomPath.aim = Vector3.zero;
				AstarPath.StartPath(randomPath);
			}
		}
	}

	private void FearPointCalculated(Path p)
	{
		fearPoint = ((RandomPath)p).endPoint;
	}

	private Vector3 FearPoint()
	{
		return (!(fearPoint == Vector3.zero)) ? fearPoint : unitController.Value.Transform.position;
	}

	private Transform GetNullParent()
	{
		return null;
	}

	private Transform GetAttackTargetParent()
	{
		return (!(attackTargetRoot.Value == null) && !(attackTargetRoot.Value.transform == null)) ? attackTargetRoot.Value.transform : null;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}

	private float GetMeleeTargetReachedDistance()
	{
		return Mathf.Pow((unitController.Value as AnimalController).AttackDistance, 2f) + 0.1f;
	}
}
