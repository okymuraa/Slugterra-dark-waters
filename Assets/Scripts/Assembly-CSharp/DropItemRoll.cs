using System;
using UnityEngine;

[ExecuteInEditMode]
public class DropItemRoll : MonoBehaviour
{
	public enum dropType
	{
		Array = 0,
		Single = 1
	}

	[Serializable]
	public class items
	{
		public GameObject item;

		public byte roll;
	}

	public bool dropping;

	public dropType DropType = dropType.Single;

	public float radiusDropMin = 0.5f;

	public float radiusDropMax = 1f;

	private int valueDrops;

	private byte i;

	private Transform transformObj;

	private GameObject clone;

	public LayerMask LayerPhysicsMask;

	private RaycastHit hit;

	private Vector3 randomOffset;

	public items[] drops;

	private void Awake()
	{
		transformObj = transform;
	}

	private void Update()
	{
		if (dropping)
		{
			dropping = false;
			DropItems();
		}
	}

	private void LateUpdate()
	{
		if (radiusDropMax < radiusDropMin)
		{
			radiusDropMax = radiusDropMin;
		}
	}

	private void DropItems()
	{
		if (DropType == dropType.Array)
		{
			i = 0;
			for (valueDrops = drops.Length; valueDrops > 0; valueDrops--)
			{
				if (UnityEngine.Random.Range(0, 100) < drops[i].roll)
				{
					InstantiateItem();
				}
				i++;
				if (i == drops.Length)
				{
					i = 0;
				}
			}
		}
		if (DropType != dropType.Single)
		{
			return;
		}
		i = 0;
		for (valueDrops = drops.Length; valueDrops > 0; valueDrops--)
		{
			if (UnityEngine.Random.Range(0, 100) < drops[i].roll)
			{
				InstantiateItem();
				break;
			}
			i++;
			if (i == drops.Length)
			{
				i = 0;
			}
		}
	}

	private void InstantiateItem()
	{
		randomOffset = Vector3.left * UnityEngine.Random.Range(radiusDropMin, radiusDropMax);
		randomOffset.y = transform.position.y;
		randomOffset = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * randomOffset;
		randomOffset += transform.position;
		if (PhysicsFacade.Raycast(randomOffset + Vector3.up, -Vector3.up, out hit, 5f, LayerPhysicsMask) && drops[i].item != null)
		{
			clone = UnityEngine.Object.Instantiate(drops[i].item, hit.point, Quaternion.identity);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
		Gizmos.DrawSphere(transform.position, radiusDropMax);
		Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
		Gizmos.DrawSphere(transform.position, radiusDropMin);
	}
}
