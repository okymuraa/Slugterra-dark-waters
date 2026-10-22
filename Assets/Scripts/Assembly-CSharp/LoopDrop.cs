using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class LoopDrop : MonoBehaviour
{
	public delegate void SomeDeligate();

	public enum dropType
	{
		Array = 0,
		Single = 1
	}

	[Serializable]
	public class dropsSetup
	{
		[Serializable]
		public class items
		{
			public GameObject item;

			[Range(0f, 100f)]
			public byte roll;
		}

		public dropType DropType = dropType.Single;

		public float radiusDropMin = 0.5f;

		public float radiusDropMax = 1f;

		[HideInInspector]
		public int valueDrops;

		[HideInInspector]
		public byte i;

		public LayerMask LayerPhysicsMask;

		[HideInInspector]
		public RaycastHit hit;

		[HideInInspector]
		public Vector3 randomOffset;

		public items[] dropsItem;
	}

	public GameObject[] EnemyList;

	public GameObject SpawnFX;

	public float SpawnTime = 1f;

	public float CheckTime = 1f;

	private float _checkTime;

	private Transform transformObj;

	public Animation AnimationObj;

	public AnimationClip open;

	private bool _LootOpen;

	private int _checkEmptyCount;

	private int i;

	private float time;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private SomeDeligate SomeEvent__BackingField;

	public dropsSetup Drops = new dropsSetup();

	public event SomeDeligate SomeEvent
	{
		add
		{
			SomeDeligate someDeligate = SomeEvent__BackingField;
			SomeDeligate someDeligate2;
			do
			{
				someDeligate2 = someDeligate;
				someDeligate = Interlocked.CompareExchange(ref SomeEvent__BackingField, (SomeDeligate)Delegate.Combine(someDeligate2, value), someDeligate);
			}
			while ((object)someDeligate != someDeligate2);
		}
		remove
		{
			SomeDeligate someDeligate = SomeEvent__BackingField;
			SomeDeligate someDeligate2;
			do
			{
				someDeligate2 = someDeligate;
				someDeligate = Interlocked.CompareExchange(ref SomeEvent__BackingField, (SomeDeligate)Delegate.Remove(someDeligate2, value), someDeligate);
			}
			while ((object)someDeligate != someDeligate2);
		}
	}

	private void Start()
	{
		_checkTime = CheckTime;
		transformObj = transform;
		if (AnimationObj == null)
		{
			AnimationObj = transformObj.GetComponent<Animation>();
		}
	}

	private void FixedUpdate()
	{
		if (_LootOpen || EnemyList.Length <= 0)
		{
			return;
		}
		_checkTime -= Time.deltaTime;
		if (!(_checkTime <= 0f))
		{
			return;
		}
		_checkTime = CheckTime;
		_checkEmptyCount = 0;
		for (i = 0; i < EnemyList.Length; i++)
		{
			if ((EnemyList[i] != null && !EnemyList[i].activeSelf) || EnemyList[i] == null)
			{
				_checkEmptyCount++;
			}
		}
		if (_checkEmptyCount == EnemyList.Length)
		{
			Open();
		}
	}

	public void Open()
	{
		_LootOpen = true;
		if (AnimationObj != null)
		{
			AnimationObj.Play(open.name);
		}
		DropItems();
	}

	private void DropItems()
	{
		if (SomeEvent__BackingField != null)
		{
			SomeEvent__BackingField();
		}
		if (Drops.DropType == dropType.Array)
		{
			Drops.i = 0;
			Drops.valueDrops = Drops.dropsItem.Length;
			while (Drops.valueDrops > 0)
			{
				if (UnityEngine.Random.Range(0, 100) < Drops.dropsItem[Drops.i].roll)
				{
					StartCoroutine(InstantiateItem(time, Drops.i));
					time += SpawnTime;
				}
				Drops.i++;
				if (Drops.i == Drops.dropsItem.Length)
				{
					Drops.i = 0;
				}
				Drops.valueDrops--;
			}
		}
		if (Drops.DropType != dropType.Single)
		{
			return;
		}
		Drops.i = 0;
		Drops.valueDrops = Drops.dropsItem.Length;
		while (Drops.valueDrops > 0)
		{
			if (UnityEngine.Random.Range(0, 100) < Drops.dropsItem[Drops.i].roll)
			{
				StartCoroutine(InstantiateItem(SpawnTime, Drops.i));
				break;
			}
			Drops.i++;
			if (Drops.i == Drops.dropsItem.Length)
			{
				Drops.i = 0;
			}
			Drops.valueDrops--;
		}
	}

	private IEnumerator InstantiateItem(float t, int index)
	{
		yield return new WaitForSeconds(t);
		Drops.randomOffset = Vector3.left * UnityEngine.Random.Range(Drops.radiusDropMin, Drops.radiusDropMax);
		Drops.randomOffset.y = Mathf.Abs(transformObj.position.y);
		Drops.randomOffset = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * Drops.randomOffset;
		Drops.randomOffset += transformObj.position;
		if (Physics.Raycast(Drops.randomOffset + Vector3.up, -Vector3.up, out Drops.hit, (Mathf.Abs(transform.position.y) + 1f) * 2f, Drops.LayerPhysicsMask) && Drops.dropsItem[index].item != null)
		{
			UnityEngine.Object.Instantiate(SpawnFX, Drops.hit.point, Quaternion.identity);
			UnityEngine.Object.Instantiate(Drops.dropsItem[index].item, Drops.hit.point, Quaternion.identity);
		}
	}
}
