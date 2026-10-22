using System;
using System.Collections.Generic;
using System.IO;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using UnityEngine;

namespace Slugterra.Domain.App
{
	public class AppModel : DomainObject, IObserver
	{
		private Version _version;

		private GameSetting _gameSetting;

		private MissionSetting _missionSetting;

		private DuelSetting _duelSetting;

		private AnnouncementSetting _announcementSetting;

		private ShopModel _shopModel;

		private List<LevelItem> _levels;

		public const int MAX_AVAILABLE_LEVELS = 17;

		public Version Version
		{
			get
			{
				return _version;
			}
		}

		public GameSetting GameSetting
		{
			get
			{
				return _gameSetting;
			}
		}

		public MissionSetting MissionSetting
		{
			get
			{
				return _missionSetting;
			}
		}

		public DuelSetting DuelSetting
		{
			get
			{
				return _duelSetting;
			}
		}

		public AnnouncementSetting AnnouncementSetting
		{
			get
			{
				return _announcementSetting;
			}
		}

		public Wallet Wallet { get; private set; }

		public ShopModel ShopModel
		{
			get
			{
				return _shopModel;
			}
		}

		public List<LevelItem> Levels
		{
			get
			{
				return _levels;
			}
		}

		public AppModel()
		{
			_version = Version.V1;
			_gameSetting = new GameSetting();
			_gameSetting.AddObserver(this);
			_duelSetting = new DuelSetting();
			Wallet = new Wallet();
			Wallet.AddObserver(this);
			_shopModel = new ShopModel();
			_missionSetting = new MissionSetting();
			_missionSetting.AddObserver(this);
			_duelSetting = new DuelSetting();
			_duelSetting.AddObserver(this);
			_announcementSetting = new AnnouncementSetting();
			_levels = new List<LevelItem>();
			Initialize();
		}

		private void Initialize()
		{
			LevelItem.Current = 1;
			_levels.Add(new LevelItem(1, typeof(ShaneTutor_0_1), LevelType.Action, LevelStatus.Locked, "1.1"));
			_levels.Add(new LevelItem(2, typeof(QuietLawn_2_1), LevelType.Action, LevelStatus.Locked, "2.1"));
			_levels.Add(new LevelItem(3, typeof(QuietLawn_2_2), LevelType.Action, LevelStatus.Locked, "2.2"));
			_levels.Add(new LevelItem(4, typeof(ScorchedSparksTutorial_1_1), LevelType.Arena, LevelStatus.Locked, "1.1T"));
			_levels.Add(new LevelItem(4, typeof(QuietLawn_2_3), LevelType.Action, LevelStatus.Locked, "2.3"));
			_levels.Add(new LevelItem(5, typeof(QuietLawn_2_4), LevelType.Action, LevelStatus.Locked, "2.4"));
			_levels.Add(new LevelItem(5, typeof(ScorchedSparks_1_1), LevelType.Arena, LevelStatus.Locked, "1.1"));
			_levels.Add(new LevelItem(6, typeof(QuietLawn_2_5), LevelType.Action, LevelStatus.Locked, "2.5"));
			_levels.Add(new LevelItem(7, typeof(DarkPeriphery_3_1), LevelType.Action, LevelStatus.Locked, "3.1"));
			_levels.Add(new LevelItem(7, typeof(ScorchedSparks_1_2), LevelType.Arena, LevelStatus.Locked, "1.2"));
			_levels.Add(new LevelItem(8, typeof(DarkPeriphery_3_2), LevelType.Action, LevelStatus.Locked, "3.2"));
			_levels.Add(new LevelItem(9, typeof(DarkPeriphery_3_3), LevelType.Action, LevelStatus.Locked, "3.3"));
			_levels.Add(new LevelItem(10, typeof(Deadweed_4_1), LevelType.Action, LevelStatus.Locked, "4.1"));
			_levels.Add(new LevelItem(10, typeof(ScorchedSparks_1_3), LevelType.Arena, LevelStatus.Locked, "1.3"));
			_levels.Add(new LevelItem(11, typeof(Deadweed_4_2), LevelType.Action, LevelStatus.Locked, "4.2"));
			_levels.Add(new LevelItem(12, typeof(Deadweed_4_3), LevelType.Action, LevelStatus.Locked, "4.3"));
			_levels.Add(new LevelItem(13, typeof(GhoulDep_5_1), LevelType.Action, LevelStatus.Locked, "5.1"));
			_levels.Add(new LevelItem(13, typeof(ScorchedSparks_1_4), LevelType.Arena, LevelStatus.Locked, "1.4"));
			_levels.Add(new LevelItem(14, typeof(GhoulDep_5_2), LevelType.Action, LevelStatus.Locked, "5.2"));
			_levels.Add(new LevelItem(15, typeof(Train_6_1), LevelType.Action, LevelStatus.Locked, "6.1"));
			_levels.Add(new LevelItem(16, typeof(Train_6_2), LevelType.Action, LevelStatus.Locked, "6.2"));
			_levels.Add(new LevelItem(17, typeof(Train_6_3), LevelType.Action, LevelStatus.Locked, "6.3"));
			_levels.Add(new LevelItem(17, typeof(Arena_1_5), LevelType.Arena, LevelStatus.Locked, "1.5"));
			_levels.Add(new LevelItem(0, typeof(DailyMission_01), LevelType.Action, LevelStatus.Unlocked, "daily"));
			_levels.Add(new LevelItem(8, typeof(DailyMission_01_Assault_01), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(11, typeof(DailyMission_01_Defence_01), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(14, typeof(DailyMission_01_Escort_01), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(0, typeof(DailyMission_02), LevelType.Action, LevelStatus.Unlocked, "daily"));
			_levels.Add(new LevelItem(14, typeof(DailyMission_02_Assault), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(9, typeof(DailyMission_02_Defence), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(12, typeof(DailyMission_02_Escort), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(0, typeof(DailyMission_03), LevelType.Action, LevelStatus.Unlocked, "daily"));
			_levels.Add(new LevelItem(10, typeof(DailyMission_03_Assault_01), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(13, typeof(DailyMission_03_Assault_02), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(15, typeof(DailyMission_03_Defence), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(0, typeof(DailyMission_04), LevelType.Action, LevelStatus.Unlocked, "daily"));
			_levels.Add(new LevelItem(15, typeof(DailyMission_04_Assault), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(13, typeof(DailyMission_04_Defence), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(10, typeof(DailyMission_04_Escort), LevelType.Action, LevelStatus.Locked, "daily"));
			_levels.Add(new LevelItem(0, typeof(DesertBase_1), LevelType.Action, LevelStatus.Unlocked, "survival"));
			_levels.Add(new LevelItem(0, typeof(DesertBase_2), LevelType.Action, LevelStatus.Unlocked, "survival"));
			_levels.Add(new LevelItem(0, typeof(EastSurvival_1), LevelType.Action, LevelStatus.Unlocked, "survival"));
			_levels.Add(new LevelItem(0, typeof(TutorialMultiplayer), LevelType.Action, LevelStatus.Unlocked, "multiplayer"));
			_levels.Add(new LevelItem(0, typeof(DesertBase1Multiplayer), LevelType.Action, LevelStatus.Unlocked, "multiplayer"));
			_levels.Add(new LevelItem(0, typeof(EastSet1Multiplayer), LevelType.Action, LevelStatus.Unlocked, "multiplayer"));
			_levels.Add(new LevelItem(0, typeof(ShopSceneController), LevelType.Action, LevelStatus.Unlocked, "slugPreview"));
			_levels.Add(new LevelItem(0, typeof(BackpackSceneController), LevelType.Action, LevelStatus.Unlocked, "slugPreview"));
			_levels.Add(new LevelItem(0, typeof(MultiplayerSceneController), LevelType.Action, LevelStatus.Unlocked, "slugPreview"));
			for (int i = 0; i < _levels.Count; i++)
			{
				_levels[i].AddObserver(this);
			}
		}

		public LevelItem GetLevelItem(Type levelType)
		{
			return _levels.Find((LevelItem level) => level.Scene.Equals(levelType));
		}

		public LevelItem GetLevelItemByLevelName(string levelName)
		{
			for (int i = 0; i < _levels.Count; i++)
			{
				if (_levels[i].Scene.Name == levelName)
				{
					return _levels[i];
				}
			}
			return null;
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Wallet)
			{
				SaveWallet();
			}
			if (observable is GameSetting)
			{
				SaveGameSettings();
			}
			if (observable is LevelItem)
			{
				SaveLevels();
			}
			if (observable is MissionSetting || observable is SlugItem || observable is BlasterItem)
			{
				SaveMissionSettings();
			}
			if (observable is DuelSetting || observable is FoeItem)
			{
				SaveDuelSettings();
			}
			if (observable is AnnouncementSetting)
			{
				SaveAnnouncementSettings();
			}
		}

		public void Save()
		{
			SaveLevels();
			SaveGameSettings();
			SaveWallet();
			SaveMissionSettings();
			SaveDuelSettings();
			SaveAnnouncementSettings();
		}

		public void SaveLevels()
		{
			string stringToSave = JsonConvert.SerializeObject(_levels);
			IOSystem.SaveStrings(Application.persistentDataPath, "Levels.cfg", stringToSave, false, true);
		}

		public void SaveGameSettings()
		{
			GameSetting.Serialize(GameSetting);
		}

		public void SaveWallet()
		{
			Wallet.Serialize(Wallet);
		}

		public void SaveMissionSettings()
		{
			MissionSetting.Serialize(_missionSetting);
		}

		public void SaveDuelSettings()
		{
			DuelSetting.Serialize(_duelSetting);
		}

		public void SaveAnnouncementSettings()
		{
			AnnouncementSetting.Serialize(_announcementSetting);
		}

		public void Load()
		{
			_shopModel.Deserialize();
			BlasterItem blasterItem = _shopModel.Blasters[3];
			_gameSetting = GameSetting.Deserialize();
			_gameSetting.RemoveObserver(this);
			_gameSetting.AddObserver(this);
			Wallet = Wallet.Deserialize();
			Wallet.RemoveObserver(this);
			Wallet.AddObserver(this);
			_missionSetting = MissionSetting.Deserialize();
			_missionSetting.RemoveObserver(this);
			if (_missionSetting.AttackSlug.Type != SlugType.None)
			{
				_missionSetting.AttackSlug = _shopModel.AttackSlugs.Find((SlugItem slugItem4) => slugItem4.Type == _missionSetting.AttackSlug.Type);
			}
			if (_missionSetting.DefenceSlug.Type != SlugType.None)
			{
				_missionSetting.DefenceSlug = _shopModel.DefenceSlugs.Find((SlugItem slugItem4) => slugItem4.Type == _missionSetting.DefenceSlug.Type);
			}
			if (_missionSetting.SpecialSlug.Type != SlugType.None)
			{
				_missionSetting.SpecialSlug = _shopModel.SpecialSlugs.Find((SlugItem slugItem4) => slugItem4.Type == _missionSetting.SpecialSlug.Type);
			}
			if (_missionSetting.Assistant.Type != AllyType.None)
			{
				_missionSetting.Assistant = _shopModel.Assistants.Find((AssistantItem assistantItem) => assistantItem.Type == _missionSetting.Assistant.Type);
			}
			_missionSetting.Blaster = _shopModel.Blasters.Find((BlasterItem blasterItem2) => blasterItem2.Type == _missionSetting.Blaster.Type);
			_missionSetting.Armor = _shopModel.Armors.Find((ArmorItem armorItem) => armorItem.Type == _missionSetting.Armor.Type);
			_missionSetting.AddObserver(this);
			_duelSetting = DuelSetting.Deserialize();
			_duelSetting.RemoveObserver(this);
			if (_duelSetting.Foe.Type != string.Empty)
			{
				_duelSetting.Foe = _shopModel.Foes.Find((FoeItem foeItem) => foeItem.Type == _duelSetting.Foe.Type);
			}
			_duelSetting.AddObserver(this);
			_announcementSetting = AnnouncementSetting.Deserialize();
			_announcementSetting.AddObserver(this);
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Levels.cfg")))
			{
				string value = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Levels.cfg"));
				List<LevelItem> list = JsonConvert.DeserializeObject<List<LevelItem>>(value);
				foreach (LevelItem item in _levels)
				{
					LevelItem levelItem = list.Find((LevelItem level) => level.Scene.Name == item.Scene.Name);
					if (levelItem == null)
					{
						list.Add(item);
					}
					else
					{
						levelItem.Order = item.Order;
					}
				}
				_levels = list;
				for (int num = 0; num < _levels.Count; num++)
				{
					_levels[num].RemoveObserver(this);
					_levels[num].AddObserver(this);
				}
			}
			if (GetLevelItem(typeof(Deadweed_4_3)).Status == LevelStatus.Completed && GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Locked)
			{
				GetLevelItem(typeof(GhoulDep_5_1)).Status = LevelStatus.Unlocked;
			}
			LevelItem levelItem2 = _levels.Find((LevelItem level) => level.Status == LevelStatus.Unlocked && level.Type == LevelType.Action && !level.Name.Contains("daily") && level.Order != 0);
			if (levelItem2 != null)
			{
				LevelItem.Current = levelItem2.Order;
			}
			else
			{
				LevelItem.Current = 17;
			}
			List<LevelItem> list2 = _levels.FindAll((LevelItem level) => level.Name.Contains("daily") && level.Order != 0 && level.Order > LevelItem.Current);
			foreach (LevelItem item2 in list2)
			{
				item2.Status = LevelStatus.Locked;
			}
			if (!PlayerPrefsSystem.HasKey("SlugsAlreadyRestored"))
			{
				if (GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed)
				{
					SlugItem slugItem = _shopModel.SpecialSlugs.Find((SlugItem slugItem4) => slugItem4.Type == SlugType.Geoshard);
					slugItem.IsLocked = false;
					slugItem.IsBought = true;
				}
				if (GetLevelItem(typeof(DarkPeriphery_3_2)).Status == LevelStatus.Completed)
				{
					SlugItem slugItem2 = _shopModel.SpecialSlugs.Find((SlugItem slugItem4) => slugItem4.Type == SlugType.Fandango);
					slugItem2.IsLocked = false;
					slugItem2.IsBought = true;
				}
				if (GetLevelItem(typeof(Deadweed_4_2)).Status == LevelStatus.Completed)
				{
					SlugItem slugItem3 = _shopModel.SpecialSlugs.Find((SlugItem slugItem4) => slugItem4.Type == SlugType.Negashade);
					slugItem3.IsLocked = false;
					slugItem3.IsBought = true;
				}
				PlayerPrefs.SetInt("SlugsAlreadyRestored", 1);
			}
			string key = AppConfiguration.Name + "_" + AppConfiguration.Version;
			if (!PlayerPrefsSystem.HasKey(key))
			{
				foreach (BlasterItem blaster in _shopModel.Blasters)
				{
					blaster.IsLocked = true;
				}
				foreach (ArmorItem armor in _shopModel.Armors)
				{
					armor.IsLocked = true;
				}
				foreach (SlugItem attackSlug in _shopModel.AttackSlugs)
				{
					attackSlug.IsLocked = true;
				}
				foreach (SlugItem defenceSlug in _shopModel.DefenceSlugs)
				{
					defenceSlug.IsLocked = true;
				}
				foreach (SlugItem specialSlug in _shopModel.SpecialSlugs)
				{
					specialSlug.IsLocked = true;
				}
				foreach (AssistantItem assistant in _shopModel.Assistants)
				{
					assistant.IsLocked = true;
				}
				foreach (FoeItem foe in _shopModel.Foes)
				{
					foe.IsLocked = true;
				}
				for (int num2 = 0; num2 < _levels.Count; num2++)
				{
					LevelItem levelItem3 = _levels[num2];
					if (levelItem3 != null && levelItem3.Status == LevelStatus.Completed)
					{
						List<ArmorType> armorsToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetArmorsToUnlock(levelItem3.Scene.Name);
						List<BlasterType> blastersToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetBlastersToUnlock(levelItem3.Scene.Name);
						List<SlugType> slugsToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetSlugsToUnlock(levelItem3.Scene.Name);
						List<AllyType> alliesToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetAlliesToUnlock(levelItem3.Scene.Name);
						List<string> bossesToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetBossesToUnlock(levelItem3.Scene.Name);
						ShopModel shopModel = ShopModel;
						shopModel.UnlockArmors(armorsToUnlock);
						shopModel.UnlockBlasters(blastersToUnlock);
						shopModel.UnlockSlugs(slugsToUnlock);
						shopModel.UnlockAllies(alliesToUnlock);
						shopModel.UnlockBosses(bossesToUnlock);
					}
				}
				if (_missionSetting.Blaster.IsLocked)
				{
					_missionSetting.Blaster = _shopModel.Blasters.Find((BlasterItem blasterItem2) => blasterItem2.Type == BlasterType.Default);
				}
				PlayerPrefs.SetInt(key, 1);
			}
			if (GetLevelItem(typeof(QuietLawn_2_5)).Status == LevelStatus.Completed && !PlayerPrefsSystem.HasKey("enable_daily_missions"))
			{
				PlayerPrefs.SetInt("enable_daily_missions", 1);
				PlayerPrefs.SetInt("daily_missions_available", 1);
				DailySceneManager.SendPushForFirstDailyMission();
			}
		}
	}
}
