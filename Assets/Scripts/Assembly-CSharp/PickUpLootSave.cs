using AppsMinistry.Core;
using Slugterra.App;
using UnityEngine;

public class PickUpLootSave : MonoBehaviour
{
	public ushort IdLoot;

	public bool UniqueLootOnePickUpOnly;

	[Range(0f, 100f)]
	public byte RollUniqueLoot = 100;

	public GameObject UniqueLoot;

	public GameObject DefaultLoot;

	public LoopDrop LootEvent;

	private int lootOpen;

	private Vector2 currentPos;

	private void Start()
	{
		lootOpen = PlayerPrefs.GetInt(string.Concat("MissionLoot_", MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene, "_Loot_OPEN_id_", IdLoot));
		if (lootOpen == 1)
		{
			if (UniqueLootOnePickUpOnly)
			{
				lootOpen = 5;
			}
			else
			{
				lootOpen = Random.Range(0, 100);
				if (lootOpen < RollUniqueLoot)
				{
					lootOpen = 1;
				}
			}
		}
		else
		{
			lootOpen = Random.Range(0, 100);
			if (lootOpen < RollUniqueLoot)
			{
				lootOpen = 1;
			}
		}
		if (lootOpen == 1)
		{
			DefaultLoot.SetActive(false);
		}
		else
		{
			UniqueLoot.SetActive(false);
		}
		if (LootEvent != null)
		{
			LootEvent.SomeEvent += SaveEvent;
		}
	}

	private void SaveEvent()
	{
		PlayerPrefs.SetInt(string.Concat("MissionLoot_", MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene, "_Loot_OPEN_id_", IdLoot), 1);
	}
}
