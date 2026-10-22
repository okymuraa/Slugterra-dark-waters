using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using Slugterra.Game.Units;
using Slugterra.UI.Game;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_a_i_path.php")]
[AddComponentMenu("Pathfinding/AI/AIPath (3D)")]
[RequireComponent(typeof(Seeker))]
public class AIPath : MonoBehaviour
{
	private UnitController.UnitMovementType _movementType;

	[HideInInspector]
	public Transform CustomLookPoint;

	private Quaternion CustomLookRotation = Quaternion.identity;

	public float repathRate = 0.5f;

	public Transform target;

	public bool canSearch = true;

	public bool canMove = true;

	public float speed = 3f;

	public float turningSpeed = 5f;

	public float slowdownDistance = 0.6f;

	public float pickNextWaypointDist = 2f;

	public float forwardLook = 1f;

	public float endReachedDistance = 0.2f;

	public bool closestOnPathCheck = true;

	protected float minMoveScale = 0.05f;

	protected Seeker seeker;

	protected Transform tr;

	protected float lastRepath = -9999f;

	protected Path path;

	protected CharacterController controller;

	protected RVOController rvoController;

	protected Rigidbody rigid;

	private GameController _gameController;

	protected int currentWaypointIndex;

	protected bool targetReached;

	protected bool canSearchAgain = true;

	protected Vector3 lastFoundWaypointPosition;

	protected float lastFoundWaypointTime = -9999f;

	private bool startHasRun;

	protected Vector3 targetPoint;

	protected Vector3 targetDirection;

	public UnitController.UnitMovementType MovementType
	{
		get
		{
			return _movementType;
		}
		set
		{
			_movementType = value;
		}
	}

	public bool TargetReached
	{
		get
		{
			return targetReached;
		}
	}

	protected virtual void Awake()
	{
		seeker = GetComponent<Seeker>();
		tr = transform;
		controller = GetComponent<CharacterController>();
		rvoController = GetComponent<RVOController>();
		if (rvoController != null)
		{
			rvoController.enableRotation = false;
		}
		rigid = GetComponent<Rigidbody>();
		if (_gameController == null)
		{
			_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		}
	}

	protected virtual void Start()
	{
		startHasRun = true;
		OnEnable();
	}

	protected virtual void OnEnable()
	{
		lastRepath = -9999f;
		canSearchAgain = true;
		lastFoundWaypointPosition = GetFeetPosition();
		if (startHasRun)
		{
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Combine(seeker.pathCallback, new OnPathDelegate(OnPathComplete));
			StartCoroutine(RepeatTrySearchPath());
		}
	}

	public void OnDisable()
	{
		if (this.seeker != null && !this.seeker.IsDone())
		{
			this.seeker.GetCurrentPath().Error();
		}
		if (path != null)
		{
			path.Release(this);
		}
		path = null;
		Seeker seeker = this.seeker;
		seeker.pathCallback = (OnPathDelegate)Delegate.Remove(seeker.pathCallback, new OnPathDelegate(OnPathComplete));
	}

	protected IEnumerator RepeatTrySearchPath()
	{
		while (true)
		{
			float v = TrySearchPath();
			yield return new WaitForSeconds(v);
		}
	}

	public float TrySearchPath()
	{
		if (Time.time - lastRepath >= repathRate && canSearchAgain && canSearch && target != null)
		{
			SearchPath();
			return repathRate;
		}
		float num = repathRate - (Time.time - lastRepath);
		return (!(num < 0f)) ? num : 0f;
	}

	public virtual void SearchPath()
	{
		if (target == null)
		{
			throw new InvalidOperationException("Target is null");
		}
		lastRepath = Time.time;
		Vector3 position = target.position;
		canSearchAgain = false;
		seeker.StartPath(GetFeetPosition(), position);
	}

	public virtual void OnTargetReached()
	{
	}

	public virtual void OnPathComplete(Path _p)
	{
		ABPath aBPath = _p as ABPath;
		if (aBPath == null)
		{
			throw new Exception("This function only handles ABPaths, do not use special path types");
		}
		canSearchAgain = true;
		aBPath.Claim(this);
		if (aBPath.error)
		{
			aBPath.Release(this);
			return;
		}
		if (path != null)
		{
			path.Release(this);
		}
		path = aBPath;
		currentWaypointIndex = 0;
		targetReached = false;
		if (closestOnPathCheck)
		{
			Vector3 vector = ((!(Time.time - lastFoundWaypointTime < 0.3f)) ? aBPath.originalStartPoint : lastFoundWaypointPosition);
			Vector3 feetPosition = GetFeetPosition();
			Vector3 vector2 = feetPosition - vector;
			float magnitude = vector2.magnitude;
			vector2 /= magnitude;
			int num = (int)(magnitude / pickNextWaypointDist);
			for (int i = 0; i <= num; i++)
			{
				CalculateVelocity(vector);
				vector += vector2;
			}
		}
	}

	public virtual Vector3 GetFeetPosition()
	{
		if (rvoController != null)
		{
			return tr.position - Vector3.up * rvoController.height * 0.5f;
		}
		if (controller != null)
		{
			return tr.position - Vector3.up * controller.height * 0.5f;
		}
		return tr.position;
	}

	public virtual void Update()
	{
		if (canMove)
		{
			Vector3 vector = CalculateVelocity(GetFeetPosition());
			RotateTowards(targetDirection);
			if (rvoController != null)
			{
				rvoController.Move(vector);
			}
			else if (controller != null)
			{
				controller.SimpleMove(vector);
			}
			else if (rigid != null)
			{
				rigid.AddForce(vector);
			}
			else
			{
				tr.Translate(vector * Time.deltaTime, Space.World);
			}
		}
	}

	protected float XZSqrMagnitude(Vector3 a, Vector3 b)
	{
		float num = b.x - a.x;
		float num2 = b.z - a.z;
		return num * num + num2 * num2;
	}

	protected Vector3 CalculateVelocity(Vector3 currentPosition)
	{
		if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
		{
			return Vector3.zero;
		}
		List<Vector3> vectorPath = path.vectorPath;
		if (vectorPath.Count == 1)
		{
			vectorPath.Insert(0, currentPosition);
		}
		if (currentWaypointIndex >= vectorPath.Count)
		{
			currentWaypointIndex = vectorPath.Count - 1;
		}
		if (currentWaypointIndex <= 1)
		{
			currentWaypointIndex = 1;
		}
		while (currentWaypointIndex < vectorPath.Count - 1)
		{
			float num = XZSqrMagnitude(vectorPath[currentWaypointIndex], currentPosition);
			if (num < pickNextWaypointDist * pickNextWaypointDist)
			{
				lastFoundWaypointPosition = currentPosition;
				lastFoundWaypointTime = Time.time;
				currentWaypointIndex++;
				continue;
			}
			break;
		}
		Vector3 vector = vectorPath[currentWaypointIndex] - vectorPath[currentWaypointIndex - 1];
		Vector3 vector2 = CalculateTargetPoint(currentPosition, vectorPath[currentWaypointIndex - 1], vectorPath[currentWaypointIndex]);
		vector = vector2 - currentPosition;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num2 = Mathf.Clamp01(magnitude / slowdownDistance);
		targetDirection = vector;
		targetPoint = vector2;
		if (currentWaypointIndex == vectorPath.Count - 1 && magnitude <= endReachedDistance)
		{
			if (!targetReached)
			{
				targetReached = true;
				OnTargetReached();
			}
			return Vector3.zero;
		}
		if (!_gameController.IsMultiplayer)
		{
			Vector3 vector3 = Vector3.zero;
			float a = 0f;
			if (_movementType == UnitController.UnitMovementType.Forward)
			{
				vector3 = tr.forward;
				a = Vector3.Dot(vector.normalized, vector3);
			}
			else if (_movementType == UnitController.UnitMovementType.CustomLookPoint)
			{
				CustomLookRotation = Quaternion.Slerp(CustomLookRotation, Quaternion.LookRotation(vector), turningSpeed * Time.deltaTime * 2f);
				vector3 = CustomLookRotation * new Vector3(1f, 0f, 1f);
				a = Vector3.Dot(vector.normalized, vector3);
			}
			else if (_movementType == UnitController.UnitMovementType.Backward)
			{
				vector3 = Quaternion.AngleAxis(180f, tr.up) * tr.forward;
				a = Vector3.Dot(vector.normalized, vector3);
			}
			float num3 = speed * Mathf.Max(a, minMoveScale) * num2;
			if (Time.deltaTime > 0f)
			{
				num3 = Mathf.Clamp(num3, 0f, magnitude / (Time.deltaTime * 2f));
			}
			return vector3 * num3;
		}
		return vector * speed * 1.15f * num2;
	}

	protected virtual void RotateTowards(Vector3 dir)
	{
		if (!(dir == Vector3.zero))
		{
			Quaternion rotation = tr.rotation;
			Quaternion b = Quaternion.LookRotation(dir);
			Vector3 eulerAngles = Quaternion.Slerp(rotation, b, turningSpeed * Time.deltaTime).eulerAngles;
			eulerAngles.z = 0f;
			eulerAngles.x = 0f;
			rotation = Quaternion.Euler(eulerAngles);
			tr.rotation = rotation;
		}
	}

	protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
	{
		a.y = p.y;
		b.y = p.y;
		float magnitude = (a - b).magnitude;
		if (magnitude == 0f)
		{
			return a;
		}
		float num = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(a, b, p));
		Vector3 vector = (b - a) * num + a;
		float magnitude2 = (vector - p).magnitude;
		float num2 = Mathf.Clamp(forwardLook - magnitude2, 0f, forwardLook);
		float num3 = num2 / magnitude;
		num3 = Mathf.Clamp(num3 + num, 0f, 1f);
		return (b - a) * num3 + a;
	}
}
