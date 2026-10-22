using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_controller.php")]
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	public class RVOController : MonoBehaviour
	{
		[Tooltip("Radius of the agent")]
		public float radius = 5f;

		[Tooltip("Max speed of the agent. In world units/second")]
		public float maxSpeed = 2f;

		[Tooltip("Height of the agent. In world units")]
		public float height = 1f;

		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quailty is not the best")]
		public bool locked;

		[Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving = true;

		[Tooltip("How far in the time to look for collisions with other agents")]
		public float agentTimeHorizon = 2f;

		[HideInInspector]
		public float obstacleTimeHorizon = 2f;

		[Tooltip("Maximum distance to other agents to take them into account for collisions.\nDecreasing this value can lead to better performance, increasing it can lead to better quality of the simulation")]
		public float neighbourDist = 10f;

		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		[Tooltip("Layer mask for the ground. The RVOController will raycast down to check for the ground to figure out where to place the agent")]
		public LayerMask mask = -1;

		public RVOLayer layer = RVOLayer.DefaultAgent;

		[AstarEnumFlag]
		public RVOLayer collidesWith = (RVOLayer)(-1);

		[HideInInspector]
		public float wallAvoidForce = 1f;

		[HideInInspector]
		public float wallAvoidFalloff = 1f;

		[Tooltip("Center of the agent relative to the pivot point of this game object")]
		public Vector3 center;

		private IAgent rvoAgent;

		public bool enableRotation = true;

		public float rotationSpeed = 30f;

		private Simulator simulator;

		private float adjustedY;

		private Transform tr;

		private Vector3 desiredVelocity;

		public bool debug;

		private Vector3 lastPosition;

		private static RVOSimulator cachedSimulator;

		private static readonly Color GizmoColor = new Color(240f / 255f, 213f / 255f, 30f / 255f);

		public Vector3 position
		{
			get
			{
				return rvoAgent.InterpolatedPosition;
			}
		}

		public Vector3 velocity
		{
			get
			{
				return rvoAgent.Velocity;
			}
		}

		public void OnDisable()
		{
			if (simulator != null)
			{
				simulator.RemoveAgent(rvoAgent);
			}
		}

		public void Awake()
		{
			tr = transform;
			if (cachedSimulator == null)
			{
				cachedSimulator = Object.FindObjectOfType<RVOSimulator>();
			}
			if (cachedSimulator == null)
			{
				Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
			}
			else
			{
				simulator = cachedSimulator.GetSimulator();
			}
		}

		public void OnEnable()
		{
			if (simulator != null)
			{
				if (rvoAgent != null)
				{
					simulator.AddAgent(rvoAgent);
				}
				else
				{
					rvoAgent = simulator.AddAgent(transform.position);
				}
				UpdateAgentProperties();
				rvoAgent.Teleport(transform.position);
				adjustedY = rvoAgent.Position.y;
			}
		}

		protected void UpdateAgentProperties()
		{
			rvoAgent.Radius = radius;
			rvoAgent.MaxSpeed = maxSpeed;
			rvoAgent.Height = height;
			rvoAgent.AgentTimeHorizon = agentTimeHorizon;
			rvoAgent.ObstacleTimeHorizon = obstacleTimeHorizon;
			rvoAgent.Locked = locked;
			rvoAgent.MaxNeighbours = maxNeighbours;
			rvoAgent.DebugDraw = debug;
			rvoAgent.NeighbourDist = neighbourDist;
			rvoAgent.Layer = layer;
			rvoAgent.CollidesWith = collidesWith;
		}

		public void Move(Vector3 vel)
		{
			desiredVelocity = vel;
		}

		public void Teleport(Vector3 pos)
		{
			tr.position = pos;
			lastPosition = pos;
			rvoAgent.Teleport(pos);
			adjustedY = pos.y;
		}

		public void Update()
		{
			if (rvoAgent == null)
			{
				return;
			}
			if (lastPosition != tr.position)
			{
				Teleport(tr.position);
			}
			if (lockWhenNotMoving)
			{
				locked = desiredVelocity == Vector3.zero;
			}
			UpdateAgentProperties();
			Vector3 interpolatedPosition = rvoAgent.InterpolatedPosition;
			interpolatedPosition.y = adjustedY;
			RaycastHit hitInfo;
			if ((int)mask != 0 && Physics.Raycast(interpolatedPosition + Vector3.up * height * 0.5f, Vector3.down, out hitInfo, float.PositiveInfinity, mask))
			{
				adjustedY = hitInfo.point.y;
			}
			else
			{
				adjustedY = 0f;
			}
			interpolatedPosition.y = adjustedY;
			rvoAgent.SetYPosition(adjustedY);
			Vector3 zero = Vector3.zero;
			if (wallAvoidFalloff > 0f && wallAvoidForce > 0f)
			{
				List<ObstacleVertex> neighbourObstacles = rvoAgent.NeighbourObstacles;
				if (neighbourObstacles != null)
				{
					for (int i = 0; i < neighbourObstacles.Count; i++)
					{
						Vector3 vector = neighbourObstacles[i].position;
						Vector3 vector2 = neighbourObstacles[i].next.position;
						Vector3 vector3 = position - VectorMath.ClosestPointOnSegment(vector, vector2, position);
						if (!(vector3 == vector) && !(vector3 == vector2))
						{
							float sqrMagnitude = vector3.sqrMagnitude;
							vector3 /= sqrMagnitude * wallAvoidFalloff;
							zero += vector3;
						}
					}
				}
			}
			rvoAgent.DesiredVelocity = desiredVelocity + zero * wallAvoidForce;
			tr.position = interpolatedPosition + Vector3.up * height * 0.5f - center;
			lastPosition = tr.position;
			if (enableRotation && velocity != Vector3.zero)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed * Mathf.Min(velocity.magnitude, 0.2f));
			}
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = GizmoColor;
			Gizmos.DrawWireSphere(transform.position + center - Vector3.up * height * 0.5f + Vector3.up * radius * 0.5f, radius);
			Gizmos.DrawLine(transform.position + center - Vector3.up * height * 0.5f, transform.position + center + Vector3.up * height * 0.5f);
			Gizmos.DrawWireSphere(transform.position + center + Vector3.up * height * 0.5f - Vector3.up * radius * 0.5f, radius);
		}
	}
}
