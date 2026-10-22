using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.InAppPurchases;
using Slugterra.Scene.Game;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.Domain.Shops
{
	public class ShopModel : DomainObject, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNotEnoughGoldEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNotEnoughGemsEvent__BackingField;

		private List<BlasterItem> _blasters;

		private List<ArmorItem> _armors;

		private List<SlugItem> _attackSlugs;

		private List<SlugItem> _defenceSlugs;

		private List<SlugItem> _specialSlugs;

		private List<AssistantItem> _assistants;

		private List<ConsumableItem> _consumables;

		private List<PackItem> _packs;

		private List<FoeItem> _foes;

		private LastUnlockedItems _lastUnlockedItems;

		private int _availableMultiplier = 1;

		private List<SpecialPacksConfig> _specialPacks;

		public List<BlasterItem> Blasters
		{
			get
			{
				return _blasters;
			}
			set
			{
				_blasters = value;
			}
		}

		public List<ArmorItem> Armors
		{
			get
			{
				return _armors;
			}
			set
			{
				_armors = value;
			}
		}

		public List<SlugItem> AttackSlugs
		{
			get
			{
				return _attackSlugs;
			}
			set
			{
				_attackSlugs = value;
			}
		}

		public List<SlugItem> DefenceSlugs
		{
			get
			{
				return _defenceSlugs;
			}
			set
			{
				_defenceSlugs = value;
			}
		}

		public List<SlugItem> SpecialSlugs
		{
			get
			{
				return _specialSlugs;
			}
			set
			{
				_specialSlugs = value;
			}
		}

		public List<AssistantItem> Assistants
		{
			get
			{
				return _assistants;
			}
			set
			{
				_assistants = value;
			}
		}

		public List<ConsumableItem> Consumables
		{
			get
			{
				return _consumables;
			}
			set
			{
				_consumables = value;
			}
		}

		public List<PackItem> Packs
		{
			get
			{
				return _packs;
			}
			set
			{
				_packs = value;
			}
		}

		public List<FoeItem> Foes
		{
			get
			{
				return _foes;
			}
			set
			{
				_foes = value;
			}
		}

		public LastUnlockedItems LastUnlocked
		{
			get
			{
				return _lastUnlockedItems;
			}
		}

		public int AvailableMultiplier
		{
			get
			{
				return _availableMultiplier;
			}
			set
			{
				_availableMultiplier = value;
				PlayerPrefs.SetInt("multiplierCoefficient", _availableMultiplier);
			}
		}

		public List<SpecialPacksConfig> SpecialPacks
		{
			get
			{
				return _specialPacks;
			}
			set
			{
				_specialPacks = value;
			}
		}

		public event Action OnNotEnoughGoldEvent
		{
			add
			{
				Action action = OnNotEnoughGoldEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNotEnoughGoldEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNotEnoughGoldEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNotEnoughGoldEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNotEnoughGemsEvent
		{
			add
			{
				Action action = OnNotEnoughGemsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNotEnoughGemsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNotEnoughGemsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNotEnoughGemsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public ShopModel()
		{
			_blasters = new List<BlasterItem>();
			_armors = new List<ArmorItem>();
			_attackSlugs = new List<SlugItem>();
			_defenceSlugs = new List<SlugItem>();
			_specialSlugs = new List<SlugItem>();
			_assistants = new List<AssistantItem>();
			_consumables = new List<ConsumableItem>();
			_packs = new List<PackItem>();
			_foes = new List<FoeItem>();
			_specialPacks = new List<SpecialPacksConfig>();
			_lastUnlockedItems = new LastUnlockedItems();
			_availableMultiplier = PlayerPrefs.GetInt("multiplierCoefficient", 1);
			_availableMultiplier = Mathf.Clamp(_availableMultiplier, 1, 10);
			Reset();
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is ArmorItem)
			{
				CommitAndNotify();
				SaveArmor();
			}
			if (observable is BlasterItem)
			{
				CommitAndNotify();
				SaveBlasters();
			}
			if (observable is SlugItem)
			{
				CommitAndNotify();
				SlugItem slugItem = observable as SlugItem;
				SlugGroup slugGroup = SlugTypeHelper.GetGroup(slugItem.Type);
				if (slugGroup == SlugGroup.Control)
				{
					SaveControlSlugs();
				}
				if (slugGroup == SlugGroup.Attack)
				{
					SaveAttacksSlugs();
				}
				if (slugGroup == SlugGroup.Special)
				{
					SaveSpecialSlugs();
				}
			}
			if (observable is AssistantItem)
			{
				CommitAndNotify();
				SaveAssistants();
			}
			if (observable is ConsumableItem)
			{
				CommitAndNotify();
				SaveConsumables();
			}
			if (observable is PackItem)
			{
				CommitAndNotify();
				SavePacks();
			}
			if (observable is FoeItem)
			{
				CommitAndNotify();
				SaveFoes();
			}
			if (observable is SpecialPacksConfig)
			{
				CommitAndNotify();
				SaveSpecialPacks();
			}
			SaveLastUnlockedItems();
		}

		public void Serialize()
		{
			SaveArmor();
			SaveBlasters();
			SaveAttacksSlugs();
			SaveControlSlugs();
			SaveSpecialSlugs();
			SaveAssistants();
			SaveConsumables();
			SavePacks();
			SaveFoes();
			SaveSpecialPacks();
			SaveLastUnlockedItems();
		}

		private void SaveArmor()
		{
			string stringToSave = JsonConvert.SerializeObject(_armors);
			IOSystem.SaveStrings(Application.persistentDataPath, "Armors.cfg", stringToSave, false, true);
		}

		private void SaveBlasters()
		{
			string stringToSave = JsonConvert.SerializeObject(_blasters);
			IOSystem.SaveStrings(Application.persistentDataPath, "Blasters.cfg", stringToSave, false, true);
		}

		private void SaveAttacksSlugs()
		{
			string stringToSave = JsonConvert.SerializeObject(_attackSlugs);
			IOSystem.SaveStrings(Application.persistentDataPath, "AttackSlugs.cfg", stringToSave, false, true);
		}

		private void SaveControlSlugs()
		{
			string stringToSave = JsonConvert.SerializeObject(_defenceSlugs);
			IOSystem.SaveStrings(Application.persistentDataPath, "DefenseSlugs.cfg", stringToSave, false, true);
		}

		private void SaveSpecialSlugs()
		{
			string stringToSave = JsonConvert.SerializeObject(_specialSlugs);
			IOSystem.SaveStrings(Application.persistentDataPath, "SpecialSlugs.cfg", stringToSave, false, true);
		}

		private void SaveAssistants()
		{
			string stringToSave = JsonConvert.SerializeObject(_assistants);
			IOSystem.SaveStrings(Application.persistentDataPath, "Assistants.cfg", stringToSave, false, true);
		}

		private void SaveConsumables()
		{
			string stringToSave = JsonConvert.SerializeObject(_consumables);
			IOSystem.SaveStrings(Application.persistentDataPath, "Consumables.cfg", stringToSave, false, true);
		}

		private void SavePacks()
		{
			string stringToSave = JsonConvert.SerializeObject(_packs);
			IOSystem.SaveStrings(Application.persistentDataPath, "Packs.cfg", stringToSave, false, true);
		}

		private void SaveFoes()
		{
			string stringToSave = JsonConvert.SerializeObject(_foes);
			IOSystem.SaveStrings(Application.persistentDataPath, "Foes.cfg", stringToSave, false, true);
		}

		private void SaveSpecialPacks()
		{
			string stringToSave = JsonConvert.SerializeObject(_specialPacks);
			IOSystem.SaveStrings(Application.persistentDataPath, "SpecialPacks.cfg", stringToSave, false, true);
		}

		private void SaveLastUnlockedItems()
		{
			string stringToSave = JsonConvert.SerializeObject(_lastUnlockedItems);
			IOSystem.SaveStrings(Application.persistentDataPath, "LastUnlockedItems.cfg", stringToSave, false, true);
		}

		public void Deserialize()
		{
			string empty = string.Empty;
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Armors.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Armors.cfg"));
				_armors = JsonConvert.DeserializeObject<List<ArmorItem>>(empty);
				CheckArmor();
				for (int i = 0; i < _armors.Count; i++)
				{
					_armors[i].RemoveObserver(this);
					_armors[i].AddObserver(this);
				}
			}
			else
			{
				InitArmors();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Blasters.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Blasters.cfg"));
				_blasters = JsonConvert.DeserializeObject<List<BlasterItem>>(empty);
				for (int j = 0; j < _blasters.Count; j++)
				{
					_blasters[j].RemoveObserver(this);
					_blasters[j].AddObserver(this);
				}
			}
			else
			{
				InitBlasters();
			}
			UpdateBlastersList();
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("AttackSlugs.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("AttackSlugs.cfg"));
				_attackSlugs = JsonConvert.DeserializeObject<List<SlugItem>>(empty);
				CheckSlug(_attackSlugs);
			}
			else
			{
				InitAttackSlugs();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("DefenseSlugs.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("DefenseSlugs.cfg"));
				_defenceSlugs = JsonConvert.DeserializeObject<List<SlugItem>>(empty);
				CheckSlug(_defenceSlugs);
			}
			else
			{
				InitDefenceSlugs();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("SpecialSlugs.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("SpecialSlugs.cfg"));
				_specialSlugs = JsonConvert.DeserializeObject<List<SlugItem>>(empty);
				CheckSlug(_specialSlugs);
			}
			else
			{
				InitSpecialSlugs();
			}
			UpdateSlugsList();
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Assistants.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Assistants.cfg"));
				_assistants = JsonConvert.DeserializeObject<List<AssistantItem>>(empty);
				for (int k = 0; k < _assistants.Count; k++)
				{
					_assistants[k].RemoveObserver(this);
					_assistants[k].AddObserver(this);
				}
			}
			else
			{
				InitAssistants();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Consumables.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Consumables.cfg"));
				_consumables = JsonConvert.DeserializeObject<List<ConsumableItem>>(empty);
				for (int l = 0; l < _consumables.Count; l++)
				{
					_consumables[l].RemoveObserver(this);
					_consumables[l].AddObserver(this);
				}
			}
			else
			{
				InitConsumables();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Packs.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Packs.cfg"));
				_packs = JsonConvert.DeserializeObject<List<PackItem>>(empty);
				for (int m = 0; m < _packs.Count; m++)
				{
					_packs[m].RemoveObserver(this);
					_packs[m].AddObserver(this);
				}
			}
			else
			{
				InitPacks();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Foes.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Foes.cfg"));
				_foes = JsonConvert.DeserializeObject<List<FoeItem>>(empty);
				for (int n = 0; n < _foes.Count; n++)
				{
					_foes[n].RemoveObserver(this);
					_foes[n].AddObserver(this);
				}
			}
			else
			{
				InitFoes();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("SpecialPacks.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("SpecialPacks.cfg"));
				_specialPacks = JsonConvert.DeserializeObject<List<SpecialPacksConfig>>(empty);
				for (int num = 0; num < _specialPacks.Count; num++)
				{
					_specialPacks[num].RemoveObserver(this);
					_specialPacks[num].AddObserver(this);
				}
			}
			else
			{
				InitSpecialPacks();
			}
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("LastUnlockedItems.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("LastUnlockedItems.cfg"));
				_lastUnlockedItems = JsonConvert.DeserializeObject<LastUnlockedItems>(empty);
			}
		}

		public void CheckSlugs()
		{
			CheckSlug(_attackSlugs);
			CheckSlug(_defenceSlugs);
			CheckSlug(_specialSlugs);
			UpdateSlugsList();
		}

		private void CheckSlug(List<SlugItem> slugs)
		{
			foreach (SlugItem slug in slugs)
			{
				slug.IsLocked = false;
				if (slug.OldLevel > slug.Level)
				{
					slug.Level = slug.OldLevel;
				}
				if (IsUpgradeUnlocked(slug.Level))
				{
					continue;
				}
				slug.OldLevel = slug.Level;
				for (int num = slug.Level; num >= 0; num--)
				{
					if (IsUpgradeUnlocked(num))
					{
						slug.Level = num;
						break;
					}
				}
			}
		}

		public void CheckArmor()
		{
			ArmorItem armor = MonoSingleton<AppController>.Instance.Model.MissionSetting.Armor;
			foreach (ArmorItem armor2 in _armors)
			{
				string levelWhenArmorWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenArmorWillUnlocked(armor2.Type);
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenArmorWillUnlocked);
				bool flag = true;
				if (levelItemByLevelName != null)
				{
					flag = levelItemByLevelName.Status == LevelStatus.Unlocked || levelItemByLevelName.Status == LevelStatus.Completed;
				}
				if (armor2.IsOldBought && flag)
				{
					armor2.IsBought = true;
					armor2.IsLocked = false;
				}
				else if (armor2.IsBought && !flag)
				{
					armor2.IsOldBought = armor2.IsBought;
					armor2.IsBought = false;
					armor2.IsLocked = true;
					if (armor.Type == armor2.Type)
					{
						MonoSingleton<AppController>.Instance.Model.MissionSetting.SetDefaultArmor();
					}
				}
			}
		}

		private bool IsUpgradeUnlocked(int upgradeLevel)
		{
			switch (upgradeLevel)
			{
			case 0:
				return true;
			case 1:
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed;
			case 2:
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_5)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_5)).Status == LevelStatus.Completed;
			case 3:
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_3)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_3)).Status == LevelStatus.Completed;
			case 4:
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_3)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_3)).Status == LevelStatus.Completed;
			case 5:
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_2)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_2)).Status == LevelStatus.Completed;
			default:
				return false;
			}
		}

		private void InitBlasters()
		{
			List<BlasterConfig> blasterConfigs = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterConfigs();
			for (int i = 0; i < blasterConfigs.Count; i++)
			{
				BlasterItem blasterItem = null;
				BlasterConfig blasterConfig = blasterConfigs[i];
				BlasterType blasterType = GameConfiguration.ParseEnum<BlasterType>(blasterConfig.blasterName);
				bool isUnLocked = blasterConfig.isUnLocked;
				float energy = blasterConfig.levels[0].Energy;
				int level = 0;
				bool isBought = false;
				if (blasterType == BlasterType.SimpleBlaster)
				{
					isBought = true;
					level = 5;
				}
				blasterItem = new BlasterItem(blasterType, level, !isUnLocked, isBought, energy);
				blasterItem.AddObserver(this);
				_blasters.Add(blasterItem);
			}
		}

		private void InitArmors()
		{
			for (ArmorType armorType = ArmorType.Default; armorType <= ArmorType.Heavy; armorType++)
			{
				ArmorItem armorItem = null;
				bool isUnLocked = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armorType).isUnLocked;
				armorItem = new ArmorItem(armorType, 0, !isUnLocked, false);
				armorItem.AddObserver(this);
				_armors.Add(armorItem);
			}
		}

		private void InitAttackSlugs()
		{
			SlugType[] slugsInGroup = SlugTypeHelper.GetSlugsInGroup(SlugGroup.Attack);
			for (int i = 0; i < slugsInGroup.Length; i++)
			{
				_attackSlugs.Add(new SlugItem(slugsInGroup[i], 0, false, false));
			}
			SlugItem slugItem = _attackSlugs.Find((SlugItem slug) => slug.Type == SlugType.Infurnus);
			slugItem.IsBought = true;
			foreach (SlugItem attackSlug in _attackSlugs)
			{
				bool isUnLocked = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(attackSlug.Type).isUnLocked;
				attackSlug.IsLocked = !isUnLocked;
			}
		}

		private void InitDefenceSlugs()
		{
			SlugType[] slugsInGroup = SlugTypeHelper.GetSlugsInGroup(SlugGroup.Control);
			for (int i = 0; i < slugsInGroup.Length; i++)
			{
				_defenceSlugs.Add(new SlugItem(slugsInGroup[i], 0, false, false));
			}
			foreach (SlugItem defenceSlug in _defenceSlugs)
			{
				bool isUnLocked = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(defenceSlug.Type).isUnLocked;
				defenceSlug.IsLocked = !isUnLocked;
			}
		}

		private void InitSpecialSlugs()
		{
			SlugType[] slugsInGroup = SlugTypeHelper.GetSlugsInGroup(SlugGroup.Special);
			for (int i = 0; i < slugsInGroup.Length; i++)
			{
				_specialSlugs.Add(new SlugItem(slugsInGroup[i], 0, false, false));
			}
			foreach (SlugItem specialSlug in _specialSlugs)
			{
				bool isUnLocked = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(specialSlug.Type).isUnLocked;
				specialSlug.IsLocked = !isUnLocked;
			}
		}

		private void InitAssistants()
		{
			_assistants.Add(new AssistantItem(AllyType.Pronto, 0, false, true));
			_assistants.Add(new AssistantItem(AllyType.Trixie, 0, true, false));
			_assistants.Add(new AssistantItem(AllyType.Kord, 0, true, false));
			foreach (AssistantItem assistant in _assistants)
			{
				assistant.AddObserver(this);
			}
		}

		private void InitFoes()
		{
			_foes.Add(new FoeItem(EnemyType.BossPronto.ToString(), 0, false, true));
			_foes.Add(new FoeItem(EnemyType.BossTrixie.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossKord.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossGlasses.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossShorty.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossBilly.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossLock.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossLode.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossNacho.ToString(), 0, true, true));
			_foes.Add(new FoeItem(EnemyType.BossSentinelBig.ToString(), 0, true, true));
			foreach (FoeItem foe in _foes)
			{
				foe.AddObserver(this);
			}
		}

		private void InitSpecialPacks()
		{
			List<SpecialPacksConfig> allSpecialPacks = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetAllSpecialPacks();
			foreach (SpecialPacksConfig item in allSpecialPacks)
			{
				item.AddObserver(this);
				_specialPacks.Add(item);
			}
		}

		private void InitConsumables()
		{
			for (ConsumableType consumableType = ConsumableType.First; consumableType < ConsumableType.Last; consumableType++)
			{
				ConsumableItem consumableItem = new ConsumableItem(consumableType, 0);
				consumableItem.AddObserver(this);
				_consumables.Add(consumableItem);
			}
		}

		private void InitPacks()
		{
			for (PackType packType = PackType.First; packType < PackType.Last; packType++)
			{
				PackItem packItem = new PackItem(packType);
				packItem.AddObserver(this);
				_packs.Add(packItem);
			}
		}

		public void Reset()
		{
			foreach (BlasterItem blaster in _blasters)
			{
				blaster.RemoveObserver(this);
			}
			_blasters.Clear();
			foreach (ArmorItem armor in _armors)
			{
				armor.RemoveObserver(this);
			}
			_armors.Clear();
			foreach (SlugItem attackSlug in _attackSlugs)
			{
				attackSlug.RemoveObserver(this);
			}
			_attackSlugs.Clear();
			foreach (SlugItem defenceSlug in _defenceSlugs)
			{
				defenceSlug.RemoveObserver(this);
			}
			_defenceSlugs.Clear();
			foreach (SlugItem specialSlug in _specialSlugs)
			{
				specialSlug.RemoveObserver(this);
			}
			_specialSlugs.Clear();
			foreach (AssistantItem assistant in _assistants)
			{
				assistant.RemoveObserver(this);
			}
			_assistants.Clear();
			foreach (ConsumableItem consumable in _consumables)
			{
				consumable.RemoveObserver(this);
			}
			_consumables.Clear();
			foreach (PackItem pack in _packs)
			{
				pack.RemoveObserver(this);
			}
			_packs.Clear();
			foreach (FoeItem foe in _foes)
			{
				foe.RemoveObserver(this);
			}
			_foes.Clear();
			foreach (SpecialPacksConfig specialPack in _specialPacks)
			{
				specialPack.RemoveObserver(this);
			}
			_specialPacks.Clear();
			CommitAndNotify();
		}

		public bool OnBuyArmor(ArmorItem item)
		{
			if (!CanBuyArmor(item))
			{
				return false;
			}
			item.IsBought = true;
			float armorPriceGold = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(item.Type, item.Level);
			OnBuyItem(armorPriceGold);
			return true;
		}

		public bool OnTakeArmorFree(ArmorItem item)
		{
			item.IsBought = true;
			OnBuyItem(0f);
			return true;
		}

		public bool OnUpgradeArmor(ArmorItem item)
		{
			if (!CanBuyArmor(item))
			{
				return false;
			}
			float armorPriceGold = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(item.Type, item.Level + 1);
			OnBuyItem(armorPriceGold);
			item.Level++;
			return true;
		}

		public bool OnUpgradeArmorFree(ArmorItem item, int level)
		{
			item.Level = level;
			return true;
		}

		public List<ArmorItem> GetAvailebleArmors()
		{
			List<ArmorItem> list = new List<ArmorItem>();
			return _armors.FindAll((ArmorItem armor) => !armor.IsLocked && armor.IsBought);
		}

		public List<ArmorItem> GetUnlockedArmors()
		{
			List<ArmorItem> list = new List<ArmorItem>();
			return _armors.FindAll((ArmorItem armor) => !armor.IsLocked);
		}

		public bool OnBuyBlaster(BlasterItem item)
		{
			if (!CanBuyBlaster(item))
			{
				return false;
			}
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(item.Type, item.Level);
			if (blasterInApp != string.Empty)
			{
				DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(blasterInApp);
			}
			else
			{
				float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(item.Type, item.Level);
				float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(item.Type, item.Level);
				if (blasterPriceGems > 0f)
				{
					OnBuyItem(0f, blasterPriceGems);
				}
				else
				{
					OnBuyItem(blasterPriceGold);
				}
				item.IsBought = true;
			}
			return true;
		}

		public bool OnTakeBlasterFree(BlasterItem item)
		{
			OnBuyItem(0f);
			item.IsBought = true;
			return true;
		}

		public bool OnUpgradeBlaster(BlasterItem item)
		{
			if (!CanBuyBlaster(item))
			{
				return false;
			}
			float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(item.Type, item.Level + 1);
			OnBuyItem(blasterPriceGold);
			item.Level++;
			return true;
		}

		public bool OnRecoverBlaster(BlasterItem item)
		{
			RestoreHealthConfiguration restoreHealthConfigurationManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			BlasterLevels blasterParams = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(item.Type, item.Level);
			float healPrice = restoreHealthConfigurationManager.GetHealPrice(item.Type, item.Level);
			if (!CanBuySimple(healPrice))
			{
				return false;
			}
			OnBuyItem(healPrice);
			item.Energy = blasterParams.Energy;
			restoreHealthConfigurationManager.GetHealthConfiguration(item.Type).Heal();
			restoreHealthConfigurationManager.Save();
			return true;
		}

		public bool OnRecoverBlasterByAD(BlasterItem item)
		{
			RestoreHealthConfiguration restoreHealthConfigurationManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			BlasterLevels blasterParams = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(item.Type, item.Level);
			item.Energy += Mathf.Round(blasterParams.Energy * 0.25f);
			restoreHealthConfigurationManager.GetHealthConfiguration(item.Type).Heal();
			restoreHealthConfigurationManager.Save();
			return true;
		}

		public bool BuySimple(float price, PaymentButtonCurrency currency)
		{
			if (CanBuySimple(price, currency))
			{
				Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
				switch (currency)
				{
				case PaymentButtonCurrency.Coins:
					wallet.Gold -= price;
					break;
				case PaymentButtonCurrency.Gems:
					wallet.Gems -= price;
					break;
				}
				return true;
			}
			return false;
		}

		public bool CanBuySimple(float price)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (wallet.Gold < price)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuySimple(float price, bool showPopup)
		{
			bool flag = false;
			if (showPopup)
			{
				return CanBuySimple(price);
			}
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			return !(wallet.Gold < price);
		}

		public bool CanBuySimple(float price, PaymentButtonCurrency currency)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			switch (currency)
			{
			case PaymentButtonCurrency.Coins:
				if (wallet.Gold < price)
				{
					RaiseNotEnoughGold();
					return false;
				}
				break;
			case PaymentButtonCurrency.Gems:
				if (wallet.Gems < price)
				{
					RaiseNotEnoughGems();
					return false;
				}
				break;
			default:
				return false;
			}
			UnityEngine.Debug.Log("true");
			return true;
		}

		public bool CanBuyArmor(ArmorItem item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			float armorPriceGold = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(item.Type, item.Level + 1);
			if (!item.IsBought)
			{
				armorPriceGold = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(item.Type, item.Level);
			}
			if (wallet.Gold < armorPriceGold)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuyBlaster(BlasterItem item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(item.Type, item.Level);
			if (blasterInApp != string.Empty && !item.IsBought)
			{
				return true;
			}
			float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(item.Type, item.Level);
			float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(item.Type, item.Level);
			if (item.IsBought)
			{
				blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(item.Type, item.Level + 1);
				blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(item.Type, item.Level + 1);
			}
			if (blasterPriceGems > 0f)
			{
				if (wallet.Gems < blasterPriceGems)
				{
					RaiseNotEnoughGems();
					return false;
				}
			}
			else if (wallet.Gold < blasterPriceGold)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuySlug(SlugItem item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			float num = 0f;
			num = ((!item.IsBought) ? MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(item.Type, item.Level) : MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(item.Type, item.Level + 1));
			if (wallet.Gold < num)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuySlug(SlugItem item, float price)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (wallet.Gold < price)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuyAssistant(AssistantItem item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			AssistantItemConfiguration assistantItemConfiguration = null;
			assistantItemConfiguration = ((!item.IsBought) ? MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(item.Type, item.Level) : MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(item.Type, item.Level + 1));
			if (wallet.Gold < assistantItemConfiguration.GamePrice.Value)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuySpecialPack(SpecialPacksConfig item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (item.IsBought)
			{
				return false;
			}
			if (wallet.Gems < (float)item.Price)
			{
				RaiseNotEnoughGems();
				return false;
			}
			return true;
		}

		public bool CanBuyAssistant(AssistantItem item, float price)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (wallet.Gold < price)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuyConsumable(ConsumableItem item, int count = 1)
		{
			count = Mathf.Clamp(count, 1, int.MaxValue);
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			int packType = 0;
			if (count == 10)
			{
				packType = 1;
			}
			float consumablePrice = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(item.Type, packType);
			if (wallet.Gold < consumablePrice)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		public bool CanBuyPack(PackItem item)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			PackItemConfiguration packConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetPackConfig(item.Type);
			if (wallet.Gold < packConfig.GamePrice.Value)
			{
				RaiseNotEnoughGold();
				return false;
			}
			return true;
		}

		private void RaiseNotEnoughGold()
		{
			if (OnNotEnoughGoldEvent__BackingField != null)
			{
				OnNotEnoughGoldEvent__BackingField();
			}
		}

		private void RaiseNotEnoughGems()
		{
			if (OnNotEnoughGemsEvent__BackingField != null)
			{
				OnNotEnoughGemsEvent__BackingField();
			}
		}

		public List<BlasterItem> GetAvailebleBlasters()
		{
			List<BlasterItem> list = new List<BlasterItem>();
			return _blasters.FindAll((BlasterItem blaster) => !blaster.IsLocked && blaster.IsBought);
		}

		public List<BlasterItem> GetUnlockedBlasters()
		{
			List<BlasterItem> list = new List<BlasterItem>();
			return _blasters.FindAll((BlasterItem blaster) => !blaster.IsLocked);
		}

		public bool OnBuySlug(SlugItem item)
		{
			if (!CanBuySlug(item))
			{
				return false;
			}
			float slugPriceGold = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(item.Type, item.Level);
			OnBuyItem(slugPriceGold);
			item.IsBought = true;
			return true;
		}

		public bool OnTakeSlugFree(SlugItem item)
		{
			OnBuyItem(0f);
			item.IsBought = true;
			return true;
		}

		public bool OnUpgradeSlug(SlugItem item)
		{
			if (!CanBuySlug(item))
			{
				return false;
			}
			float slugPriceGold = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(item.Type, item.Level + 1);
			OnBuyItem(slugPriceGold);
			item.Level++;
			return true;
		}

		public bool OnUpgradeSlugFree(SlugItem item, int level)
		{
			item.Level = level;
			return true;
		}

		public bool OnRecoverSlug(SlugItem item)
		{
			RestoreHealthConfiguration restoreHealthConfigurationManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			float healPrice = restoreHealthConfigurationManager.GetHealPrice(item.Type, item.Level);
			if (!CanBuySimple(healPrice))
			{
				return false;
			}
			OnBuyItem(healPrice);
			restoreHealthConfigurationManager.GetHealthConfiguration(item.Type).Heal();
			restoreHealthConfigurationManager.Save();
			return true;
		}

		public List<SlugItem> GetAvailebleAttackSlugs()
		{
			List<SlugItem> list = new List<SlugItem>();
			return _attackSlugs.FindAll((SlugItem slug) => !slug.IsLocked && slug.IsBought);
		}

		public List<SlugItem> GetAvailebleDefenceSlugs()
		{
			List<SlugItem> list = new List<SlugItem>();
			return _defenceSlugs.FindAll((SlugItem slug) => !slug.IsLocked && slug.IsBought);
		}

		public List<SlugItem> GetAvailebleSpecialSlugs()
		{
			List<SlugItem> list = new List<SlugItem>();
			return _specialSlugs.FindAll((SlugItem slug) => !slug.IsLocked && slug.IsBought);
		}

		public SlugItem GetSlug(SlugType type)
		{
			SlugItem slugItem = _attackSlugs.Find((SlugItem slug) => slug.Type == type);
			if (slugItem == null)
			{
				slugItem = _defenceSlugs.Find((SlugItem slug) => slug.Type == type);
			}
			if (slugItem == null)
			{
				slugItem = _specialSlugs.Find((SlugItem slug) => slug.Type == type);
			}
			return slugItem;
		}

		public bool OnBuyAssistant(AssistantItem item)
		{
			if (!CanBuyAssistant(item))
			{
				return false;
			}
			OnBuyItem(MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(item.Type, item.Level));
			item.IsBought = true;
			return true;
		}

		public bool OnTakeAssistantFree(AssistantItem item)
		{
			OnBuyItem(0f);
			item.IsBought = true;
			return true;
		}

		public bool OnUpgradeAssistant(AssistantItem item)
		{
			if (!CanBuyAssistant(item))
			{
				return false;
			}
			OnBuyItem(MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(item.Type, item.Level + 1));
			item.Level++;
			return true;
		}

		public bool OnUpgradeAssistantFree(AssistantItem item, int level)
		{
			item.Level = level;
			return true;
		}

		public bool OnRecoverAssistant(AssistantItem assistant)
		{
			RestoreHealthConfiguration restoreHealthConfigurationManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			float healPrice = restoreHealthConfigurationManager.GetHealPrice(assistant.Type, assistant.Level);
			if (!CanBuySimple(healPrice))
			{
				return false;
			}
			OnBuyItem(healPrice);
			restoreHealthConfigurationManager.GetHealthConfiguration(assistant.Type).Heal();
			restoreHealthConfigurationManager.Save();
			return true;
		}

		public bool OnBuySpecialPack(SpecialPacksConfig item)
		{
			if (!CanBuySpecialPack(item))
			{
				return false;
			}
			OnBuyItem(0f, item.Price, 1);
			item.IsBought = true;
			return true;
		}

		public List<AssistantItem> GetAvailableAssistants()
		{
			List<AssistantItem> list = new List<AssistantItem>();
			return _assistants.FindAll((AssistantItem assistant) => !assistant.IsLocked && assistant.IsBought);
		}

		public AssistantItem GetAssistant(AllyType assistantType)
		{
			return _assistants.Find((AssistantItem assisstant) => assisstant.Type == assistantType);
		}

		public bool OnBuyConsumable(ConsumableItem item, int count = 1)
		{
			count = Mathf.Clamp(count, 1, int.MaxValue);
			if (!CanBuyConsumable(item, count))
			{
				return false;
			}
			float num = 0f;
			int packType = 0;
			if (count == 10)
			{
				packType = 1;
			}
			num = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumablePrice(item.Type, packType);
			OnBuyItem(num);
			item.Count += count;
			return true;
		}

		public bool OnTakeConsumableFree(ConsumableItem item, int count = 1)
		{
			item.Count += count;
			return true;
		}

		public List<ConsumableItem> GetAvailableConsumables()
		{
			List<ConsumableItem> list = new List<ConsumableItem>();
			return _consumables.FindAll((ConsumableItem consumable) => consumable.Count > 0);
		}

		public ConsumableItem GetConsumable(ConsumableType type)
		{
			return _consumables.Find((ConsumableItem consumable) => consumable.Type == type);
		}

		public List<ConsumableItem> GetGameShopConsumables()
		{
			List<ConsumableItem> list = new List<ConsumableItem>();
			list.Add(GetConsumable(ConsumableType.First));
			list.Add(GetConsumable(ConsumableType.SpeedBoost));
			list.Add(GetConsumable(ConsumableType.ArmorBoost));
			list.Add(GetConsumable(ConsumableType.SlugFood));
			return list;
		}

		public bool OnBuyPack(PackItem item, string levelName)
		{
			if (!CanBuyPack(item))
			{
				return false;
			}
			OnBuyItem(MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetPackConfig(item.Type));
			item.AddLevel(levelName);
			return true;
		}

		public PackItem GetPack(PackType type)
		{
			return _packs.Find((PackItem pack) => pack.Type == type);
		}

		private void OnBuyItem(ShopItemConfiguration shopItem, int count = 1)
		{
			MonoSingleton<AppController>.Instance.Model.Wallet.Gold -= shopItem.GamePrice.Value * (float)count;
		}

		private void OnBuyItem(float goldPrice, int count = 1)
		{
			MonoSingleton<AppController>.Instance.Model.Wallet.Gold -= goldPrice * (float)count;
		}

		private void OnBuyItem(float goldPrice, float gemPrice, int count = 1)
		{
			Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (goldPrice > 0f)
			{
				wallet.Gold -= goldPrice * (float)count;
			}
			if (gemPrice > 0f)
			{
				wallet.Gems -= gemPrice * (float)count;
			}
		}

		public ArmorItem GetArmor(ArmorType type)
		{
			return _armors.Find((ArmorItem armor) => armor.Type == type);
		}

		public BlasterItem GetBlaster(BlasterType type)
		{
			return _blasters.Find((BlasterItem blaster) => blaster.Type == type);
		}

		public SlugItem GetAttackSlug(SlugType type)
		{
			return _attackSlugs.Find((SlugItem slug) => slug.Type == type);
		}

		public List<FoeItem> GetAvailableFoes()
		{
			List<FoeItem> list = new List<FoeItem>();
			return _foes.FindAll((FoeItem foe) => !foe.IsLocked && foe.IsBought);
		}

		public List<SpecialPacksConfig> GetAvailableSpecialPacks()
		{
			List<SpecialPacksConfig> list = new List<SpecialPacksConfig>();
			return _specialPacks.FindAll((SpecialPacksConfig specialPack) => specialPack.IsBought);
		}

		public FoeItem GetFoe(string foeName)
		{
			return _foes.Find((FoeItem foe) => foe.Type == foeName);
		}

		public SpecialPacksConfig GetSpecialPack(string specialPackId)
		{
			return _specialPacks.Find((SpecialPacksConfig specialPack) => specialPack.Id == specialPackId);
		}

		public void UnlockArmors(List<ArmorType> armorsToUnlock)
		{
			for (int i = 0; i < _armors.Count; i++)
			{
				if (armorsToUnlock.Contains(_armors[i].Type) && _armors[i].IsLocked)
				{
					_lastUnlockedItems.Armor = _armors[i].Type;
					_armors[i].IsLocked = false;
				}
			}
		}

		public void UnlockBlasters(List<BlasterType> blastersToUnlock)
		{
			for (int i = 0; i < _blasters.Count; i++)
			{
				if (blastersToUnlock.Contains(_blasters[i].Type) && _blasters[i].IsLocked)
				{
					_lastUnlockedItems.Blaster = _blasters[i].Type;
					_blasters[i].IsLocked = false;
				}
				if (_blasters[i].Type == BlasterType.Default)
				{
					BlasterItem blaster = GetBlaster(BlasterType.Default);
					blaster.IsBought = true;
				}
			}
		}

		public void UnlockSlugs(List<SlugType> slugsToUnlock)
		{
			for (int i = 0; i < _attackSlugs.Count; i++)
			{
				if (slugsToUnlock.Contains(_attackSlugs[i].Type) && _attackSlugs[i].IsLocked)
				{
					_lastUnlockedItems.Slug = _attackSlugs[i].Type;
					_attackSlugs[i].IsLocked = false;
				}
			}
			for (int j = 0; j < _specialSlugs.Count; j++)
			{
				if (slugsToUnlock.Contains(_specialSlugs[j].Type) && _specialSlugs[j].IsLocked)
				{
					_lastUnlockedItems.Slug = _specialSlugs[j].Type;
					_specialSlugs[j].IsLocked = false;
				}
			}
			for (int k = 0; k < _defenceSlugs.Count; k++)
			{
				if (slugsToUnlock.Contains(_defenceSlugs[k].Type) && _defenceSlugs[k].IsLocked)
				{
					_lastUnlockedItems.Slug = _defenceSlugs[k].Type;
					_defenceSlugs[k].IsLocked = false;
				}
			}
		}

		public void UnlockAllies(List<AllyType> alliesToUnlock)
		{
			for (int i = 0; i < _assistants.Count; i++)
			{
				if (alliesToUnlock.Contains(_assistants[i].Type) && _assistants[i].IsLocked)
				{
					_lastUnlockedItems.Assistant = _assistants[i].Type;
					_assistants[i].IsLocked = false;
				}
			}
		}

		public void UnlockBosses(List<string> bossesToUnlock)
		{
			for (int i = 0; i < _foes.Count; i++)
			{
				string item = _foes[i].Type.Replace("Controller", string.Empty);
				if (bossesToUnlock.Contains(item) && _foes[i].IsLocked)
				{
					_lastUnlockedItems.Foe = _foes[i].Type;
					_foes[i].IsLocked = false;
				}
			}
		}

		public void RemoveJustUnlockedItems()
		{
			foreach (ArmorItem armor in _armors)
			{
				if (armor.IsJustUnlocked)
				{
					armor.IsJustUnlocked = false;
				}
			}
			foreach (BlasterItem blaster in _blasters)
			{
				if (blaster.IsJustUnlocked)
				{
					blaster.IsJustUnlocked = false;
				}
			}
			foreach (SlugItem specialSlug in _specialSlugs)
			{
				if (specialSlug.IsJustUnlocked)
				{
					specialSlug.IsJustUnlocked = false;
				}
			}
			foreach (SlugItem defenceSlug in _defenceSlugs)
			{
				if (defenceSlug.IsJustUnlocked)
				{
					defenceSlug.IsJustUnlocked = false;
				}
			}
			foreach (SlugItem attackSlug in _attackSlugs)
			{
				if (attackSlug.IsJustUnlocked)
				{
					attackSlug.IsJustUnlocked = false;
				}
			}
			foreach (AssistantItem assistant in _assistants)
			{
				if (assistant.IsJustUnlocked)
				{
					assistant.IsJustUnlocked = false;
				}
			}
		}

		private void UpdateSlugsList()
		{
			List<SlugType> list = new List<SlugType>(SlugTypeHelper.GetSlugsInGroup(SlugGroup.Special));
			foreach (SlugItem specialSlug in _specialSlugs)
			{
				if (list.Contains(specialSlug.Type))
				{
					list.Remove(specialSlug.Type);
				}
			}
			List<SlugType> list2 = new List<SlugType>(SlugTypeHelper.GetSlugsInGroup(SlugGroup.Attack));
			foreach (SlugItem attackSlug in _attackSlugs)
			{
				if (list2.Contains(attackSlug.Type))
				{
					list2.Remove(attackSlug.Type);
				}
			}
			List<SlugType> list3 = new List<SlugType>(SlugTypeHelper.GetSlugsInGroup(SlugGroup.Control));
			foreach (SlugItem defenceSlug in _defenceSlugs)
			{
				if (list3.Contains(defenceSlug.Type))
				{
					list3.Remove(defenceSlug.Type);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				_specialSlugs.Add(new SlugItem(list[i], 0, !MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(list[i]).isUnLocked, false));
			}
			for (int j = 0; j < list2.Count; j++)
			{
				_attackSlugs.Add(new SlugItem(list2[j], 0, !MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(list2[j]).isUnLocked, false));
			}
			for (int k = 0; k < list3.Count; k++)
			{
				_defenceSlugs.Add(new SlugItem(list3[k], 0, !MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(list3[k]).isUnLocked, false));
			}
			for (int l = 0; l < _specialSlugs.Count; l++)
			{
				_specialSlugs[l].RemoveObserver(this);
				_specialSlugs[l].AddObserver(this);
			}
			for (int m = 0; m < _attackSlugs.Count; m++)
			{
				_attackSlugs[m].RemoveObserver(this);
				_attackSlugs[m].AddObserver(this);
			}
			for (int n = 0; n < _defenceSlugs.Count; n++)
			{
				_defenceSlugs[n].RemoveObserver(this);
				_defenceSlugs[n].AddObserver(this);
			}
		}

		private void UpdateBlastersList()
		{
			List<BlasterType> list = new List<BlasterType>((BlasterType[])Enum.GetValues(typeof(BlasterType)));
			for (int i = 0; i < _blasters.Count; i++)
			{
				BlasterItem blasterItem = _blasters[i];
				if (list.Contains(blasterItem.Type))
				{
					list.Remove(blasterItem.Type);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				BlasterItem blasterItem2 = null;
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(list[j]);
				if (blusterConfiguration != null)
				{
					bool isUnLocked = blusterConfiguration.isUnLocked;
					float energy = blusterConfiguration.levels[0].Energy;
					int level = 0;
					bool isBought = false;
					if (list[j] == BlasterType.SimpleBlaster)
					{
						isBought = true;
						level = 5;
					}
					blasterItem2 = new BlasterItem(list[j], level, !isUnLocked, isBought, energy);
					_blasters.Add(blasterItem2);
				}
			}
			List<BlasterConfig> blasterConfigs = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterConfigs();
			List<BlasterType> newOrder = new List<BlasterType>();
			for (int k = 0; k < blasterConfigs.Count; k++)
			{
				BlasterConfig blasterConfig = blasterConfigs[k];
				BlasterType item = GameConfiguration.ParseEnum<BlasterType>(blasterConfig.blasterName);
				newOrder.Add(item);
			}
			_blasters = _blasters.OrderBy((BlasterItem d) => newOrder.IndexOf(d.Type)).ToList();
			for (int num = 0; num < _blasters.Count; num++)
			{
				_blasters[num].RemoveObserver(this);
				_blasters[num].AddObserver(this);
			}
		}
	}
}
