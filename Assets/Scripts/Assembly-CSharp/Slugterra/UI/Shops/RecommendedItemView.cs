using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input.Window;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops.PaymentStates;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class RecommendedItemView : AnnouncementListItemView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnUpgaradeItemEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRedrawAllEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnEquipItemEvent__BackingField;

		[SerializeField]
		public TextMesh _titleUpgrade;

		[SerializeField]
		private TextMesh _headerUpgradableItems;

		[SerializeField]
		private TextMesh _headerOtherItems;

		[SerializeField]
		private GameObject _headerUpgradeLevelRoot;

		[SerializeField]
		private GameObject _headerUpgradeLevelNormal;

		[SerializeField]
		private GameObject _headerUpgradeLevelRecovery;

		[SerializeField]
		public GameObject _lockedView;

		[SerializeField]
		private TextMesh _lockedText;

		[SerializeField]
		private TextMesh _headerUpgradeLevelNormalText;

		[SerializeField]
		private TextMesh _headerUpgradeLevelRecoveryText;

		[SerializeField]
		private GameObject _upgradeView;

		[SerializeField]
		private GameObject _backGroundUpgrade;

		[SerializeField]
		private GameObject _backGroundNormal;

		[SerializeField]
		private GameObject _backGroundRecovery;

		[SerializeField]
		private GameObject _backGroundTest;

		[SerializeField]
		private GameObject _badgeTest;

		[SerializeField]
		private GameObject _forceRateUpgrade;

		[SerializeField]
		private GameObject _forceRateNormal;

		[SerializeField]
		private GameObject _forceRateRecovery;

		[SerializeField]
		private TextMesh _forceRateNormalCount;

		[SerializeField]
		private GameObject _information;

		[SerializeField]
		private GameObject _menu;

		[SerializeField]
		private TextMesh _description;

		[SerializeField]
		private SlugInfoController _slugInfo;

		[SerializeField]
		private BlasterInfoController _blasterInfo;

		[SerializeField]
		private AllyInfoController _companionInfo;

		[SerializeField]
		private ArmorInfoController _armorInfo;

		[SerializeField]
		public TextMesh _upgradeLevelText;

		[SerializeField]
		public TextMesh _slugTypeText;

		[SerializeField]
		public TextMesh _slugTypeTextSh;

		[SerializeField]
		public GameObject _equipView;

		[SerializeField]
		public GameObject _upgradeButtonView;

		[SerializeField]
		public GameObject _recoveryButtonView;

		[SerializeField]
		public GameObject _previewButtonView;

		[SerializeField]
		public GameObject _buyButtonView;

		[SerializeField]
		private PaymentButtonController _paymentButton;

		[SerializeField]
		public GameObject _consumableCountRootNormal;

		[SerializeField]
		public GameObject _consumableCountRootReady;

		[SerializeField]
		public TextMesh _consumableCountCurrent;

		[SerializeField]
		public TextMesh _consumableCountExtra;

		[SerializeField]
		public TextMesh _consumableCountTitle;

		[SerializeField]
		public TextMesh _consumableCountReady;

		[SerializeField]
		private ForceRateStats _forceRateStats;

		[SerializeField]
		private ForceRateStats _forceRateUpgradeStats;

		private bool _isOffered;

		[SerializeField]
		public tk2dUIToggleButton _infoButton;

		[SerializeField]
		private tk2dButton[] _curButton;

		[SerializeField]
		private tk2dUIItem[] _curUIItem;

		private bool isInformation;

		private bool _needRate = true;

		private AnnounceItemViewType _viewType;

		private SlugType _slugType;

		private BlasterType _blasterType;

		private ArmorType _armorType;

		private AllyType _allyType;

		private ConsumableType _consumableType = ConsumableType.None;

		private bool _islargeConsumable;

		public Action<bool> OnBtnInfo;

		private int _upgradeLevel;

		private float _allPrice;

		private bool _isRequired;

		[SerializeField]
		private GameObject _slugPreviewUI;

		[SerializeField]
		private tk2dButton _btnBack;

		public PaymentButtonController PaymentButton
		{
			get
			{
				return _paymentButton;
			}
		}

		public bool IsInformation
		{
			get
			{
				return isInformation;
			}
		}

		public bool NeedRate
		{
			get
			{
				return _needRate;
			}
			set
			{
				_needRate = value;
			}
		}

		public int UpgradeLevel
		{
			get
			{
				return _upgradeLevel;
			}
			set
			{
				_upgradeLevel = value;
			}
		}

		public bool IsRequired
		{
			get
			{
				return _isRequired;
			}
			set
			{
				_isRequired = value;
			}
		}

		public object ItemValue
		{
			get
			{
				return _itemValue;
			}
		}

		public SlugType SlugType
		{
			get
			{
				return _slugType;
			}
		}

		public BlasterType BlasterType
		{
			get
			{
				return _blasterType;
			}
		}

		public ArmorType ArmorType
		{
			get
			{
				return _armorType;
			}
		}

		public AllyType AllyType
		{
			get
			{
				return _allyType;
			}
		}

		public ConsumableType ConsumableType
		{
			get
			{
				return _consumableType;
			}
		}

		public event Action OnBtnUpgaradeItemEvent
		{
			add
			{
				Action action = OnBtnUpgaradeItemEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnUpgaradeItemEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnUpgaradeItemEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnUpgaradeItemEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRedrawAllEvent
		{
			add
			{
				Action action = OnRedrawAllEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRedrawAllEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRedrawAllEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRedrawAllEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnEquipItemEvent
		{
			add
			{
				Action action = OnEquipItemEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEquipItemEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnEquipItemEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEquipItemEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public tk2dButton CurButton(int Index)
		{
			return _curButton[Index];
		}

		public tk2dUIItem CurUIItem(int Index)
		{
			return _curUIItem[Index];
		}

		protected override void OnStart()
		{
			if (_slugPreviewUI == null)
			{
				_slugPreviewUI = GameObject.Find("UIEquip").transform.Find("SlugPreview").gameObject;
			}
			if (_btnBack == null)
			{
				_btnBack = _slugPreviewUI.transform.Find("UpperLeft/btnBackGameService/btnBack/btn").GetComponent<tk2dButton>();
			}
			if (_btnBack != null)
			{
				_btnBack.ButtonUpEvent += OnBtnBackClick;
			}
		}

		protected override void OnReleaseResource()
		{
			if (_btnBack != null)
			{
				_btnBack.ButtonUpEvent -= OnBtnBackClick;
			}
		}

		private void OnBtnBackClick(tk2dButton source)
		{
			ReDrawWindow();
		}

		public override void ShowAlly(AllyType type, AnnounceItemViewType viewType)
		{
			_allyType = type;
			_viewType = viewType;
			_blasterInfo.gameObject.SetActive(false);
			_companionInfo.gameObject.SetActive(true);
			_armorInfo.gameObject.SetActive(false);
			_slugInfo.gameObject.SetActive(false);
			_previewButtonView.SetActive(false);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			AssistantItem itemOwned = missionSetting.Assistant;
			AssistantItem assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == type);
			AssistantItem assistantItem2 = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == itemOwned.Type);
			if (itemOwned != assistantItem2)
			{
				_isOffered = true;
			}
			else
			{
				_isOffered = false;
			}
			ChangeState("NormalLVL");
			if (!_isRequired && itemOwned != null && itemOwned.Type != AllyType.None)
			{
				assistantItem = itemOwned;
				_allyType = assistantItem.Type;
				viewType = AnnounceItemViewType.Acquired;
				if (_isOffered)
				{
					ChangeState("ForTest");
					viewType = AnnounceItemViewType.AcquiredForTest;
				}
			}
			_itemValue = _allyType;
			SetInfoState(false);
			string localeForLevel = GetLocaleForLevel(assistantItem.Level);
			string localeForLevel2 = GetLocaleForLevel(_upgradeLevel);
			_headerUpgradeLevelNormalText.text = Localization.GetValue(localeForLevel, string.Empty);
			_upgradeLevelText.text = Localization.GetValue(localeForLevel2, string.Empty);
			bool flag = false;
			if (missionSetting.Assistant != null)
			{
				flag = assistantItem.Type == missionSetting.Assistant.Type;
			}
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredForTest)
			{
				if (assistantItem.IsBought && !flag)
				{
					ChangeState("NormalLVL");
					viewType = AnnounceItemViewType.AcquiredNeedEquip;
				}
				if (assistantItem.IsBought && IsUpgradeUnlocked(_upgradeLevel))
				{
					if (assistantItem.Level < _upgradeLevel)
					{
						viewType = AnnounceItemViewType.AcquiredNeedUpgrade;
						ChangeState("Upgrade");
					}
					else
					{
						ChangeState("NormalLVL");
						_paymentButton.gameObject.SetActive(false);
					}
					if (_equipView.activeSelf)
					{
						SetActiveEquipButton(false);
					}
				}
			}
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && assistantItem.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			base.ShowAlly(_allyType, viewType);
			string arg = assistantItem.Type.ToString().ToLower();
			string key = string.Format("companion_{0}_title", arg);
			string text = string.Format("companion_{0}_desc", arg);
			_title.text = Localization.GetValue(key, string.Empty);
			_titleUpgrade.text = Localization.GetValue(key, string.Empty);
			string levelWhenAssistantWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenAssistantWillUnlocked(assistantItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenAssistantWillUnlocked);
			if (levelItemByLevelName != null)
			{
				_lockedText.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name;
			}
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
			if (viewType == AnnounceItemViewType.Buy && assistantItem.IsLocked)
			{
				SetActiveLockedButton(true);
				SetActiveBuyButton(false);
				SetActiveUpgradeButton(false);
			}
			else
			{
				SetActiveLockedButton(false);
			}
			UpgradePaymentState component = _upgradeView.GetComponent<UpgradePaymentState>();
			BuyPaymentState component2 = _buyView.GetComponent<BuyPaymentState>();
			if (component != null && viewType == AnnounceItemViewType.AcquiredNeedUpgrade)
			{
				_allPrice = 0f;
				for (int num = assistantItem.Level + 1; num <= _upgradeLevel; num++)
				{
					ShopItemConfiguration assistantConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(assistantItem.Type, num);
					_allPrice += assistantConfig.GamePrice.Value;
				}
				component.SetPrice(PaymentButtonCurrency.Coins, _allPrice.ToString());
				SetActiveUpgradeButton(true);
			}
			else if (component2 != null && viewType == AnnounceItemViewType.AcquiredForTest)
			{
				component.Hide();
				ChangeState("ForTest");
			}
			else if (component2 != null && viewType == AnnounceItemViewType.Buy)
			{
				_allPrice = 0f;
				if (assistantItem.Level <= _upgradeLevel)
				{
					for (int num2 = assistantItem.Level; num2 <= _upgradeLevel; num2++)
					{
						ShopItemConfiguration assistantConfig2 = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(assistantItem.Type, num2);
						_allPrice += assistantConfig2.GamePrice.Value;
					}
				}
				else
				{
					ShopItemConfiguration assistantConfig3 = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(assistantItem.Type, assistantItem.Level);
					_allPrice += assistantConfig3.GamePrice.Value;
				}
				component2.SetPrice(PaymentButtonCurrency.Coins, _allPrice.ToString());
				_headerUpgradeLevelNormalText.text = Localization.GetValue(localeForLevel2, string.Empty);
			}
			float forceRate = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, assistantItem.Level).forceRate;
			float forceRate2 = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, _upgradeLevel).forceRate;
			SetForceRate(viewType == AnnounceItemViewType.AcquiredNeedUpgrade, forceRate, forceRate2);
		}

		public override void ShowArmor(ArmorType type, AnnounceItemViewType viewType)
		{
			_armorType = type;
			_viewType = viewType;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			_blasterInfo.gameObject.SetActive(false);
			_companionInfo.gameObject.SetActive(false);
			_armorInfo.gameObject.SetActive(true);
			_slugInfo.gameObject.SetActive(false);
			_previewButtonView.SetActive(false);
			ArmorItem armorItem = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(type);
			ArmorItem armor = missionSetting.Armor;
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission);
			if (levelItem.Type == LevelType.Arena || SurvivalController.IsSurvival(missionSetting.Mission))
			{
				List<ArmorItem> unlockedArmors = MonoSingleton<AppController>.Instance.Model.ShopModel.GetUnlockedArmors();
				float num = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level).forceRate;
				foreach (ArmorItem item in unlockedArmors)
				{
					float forceRate = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(item.Type, item.Level).forceRate;
					if (forceRate >= num)
					{
						num = forceRate;
						armorItem = item;
						_armorType = armorItem.Type;
					}
				}
			}
			else
			{
				ArmorItem armor2 = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(armor.Type);
				if (armor != armor2)
				{
					_isOffered = true;
				}
				else
				{
					_isOffered = false;
				}
			}
			ChangeState("Normal");
			if (!_isRequired && armor != null && armor.Type != ArmorType.Default)
			{
				float forceRate2 = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level).forceRate;
				float forceRate3 = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(armor.Type, armor.Level).forceRate;
				if (forceRate3 >= forceRate2)
				{
					armorItem = armor;
					_armorType = armorItem.Type;
					viewType = AnnounceItemViewType.Acquired;
					if (_isOffered)
					{
						ChangeState("ForTest");
						viewType = AnnounceItemViewType.AcquiredForTest;
					}
				}
			}
			_itemValue = _armorType;
			SetInfoState(false);
			bool flag = false;
			if (armorItem != null && missionSetting.Armor != null)
			{
				flag = armorItem.Type == missionSetting.Armor.Type;
			}
			if (viewType != AnnounceItemViewType.Price && armorItem.IsBought && !flag)
			{
				viewType = AnnounceItemViewType.AcquiredNeedEquip;
			}
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && viewType != AnnounceItemViewType.AcquiredForTest && armorItem.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			base.ShowArmor(_armorType, viewType);
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			ArmorConfig armorConfiguration = instance.ArmorConfigManager.GetArmorConfiguration(armorItem.Type);
			_title.text = Localization.GetValue(armorConfiguration.title, string.Empty);
			_description.text = Localization.GetValue(armorConfiguration.description, string.Empty);
			string levelWhenArmorWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenArmorWillUnlocked(armorItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenArmorWillUnlocked);
			if (levelItemByLevelName != null)
			{
				_lockedText.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name;
			}
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
			if (viewType == AnnounceItemViewType.Buy && armorItem.IsLocked)
			{
				SetActiveLockedButton(true);
				SetActiveBuyButton(false);
				SetActiveUpgradeButton(false);
			}
			else
			{
				SetActiveLockedButton(false);
			}
			float forceRate4 = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level).forceRate;
			float forceRate5 = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(armorItem.Type, _upgradeLevel).forceRate;
			SetForceRate(viewType == AnnounceItemViewType.AcquiredNeedUpgrade, forceRate4, forceRate5);
		}

		public override void ShowBlaster(BlasterType type, AnnounceItemViewType viewType)
		{
			_blasterType = type;
			_viewType = viewType;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			_blasterInfo.gameObject.SetActive(true);
			_companionInfo.gameObject.SetActive(false);
			_armorInfo.gameObject.SetActive(false);
			_slugInfo.gameObject.SetActive(false);
			_previewButtonView.SetActive(false);
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission);
			BlasterItem blasterItem = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(type);
			BlasterItem blaster = missionSetting.Blaster;
			if (levelItem.Type == LevelType.Arena || SurvivalController.IsSurvival(missionSetting.Mission))
			{
				List<BlasterItem> unlockedBlasters = MonoSingleton<AppController>.Instance.Model.ShopModel.GetUnlockedBlasters();
				float num = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level).forceRate;
				foreach (BlasterItem item in unlockedBlasters)
				{
					float forceRate = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(item.Type, item.Level).forceRate;
					if (forceRate > num)
					{
						BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(item.Type);
						if (blusterConfiguration.isShowInStore && NeedRate)
						{
							num = forceRate;
							blasterItem = item;
							_blasterType = blasterItem.Type;
						}
					}
				}
			}
			else
			{
				BlasterItem blaster2 = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(blaster.Type);
				if (blaster != blaster2)
				{
					_isOffered = true;
				}
				else
				{
					_isOffered = false;
				}
			}
			ChangeState("Normal");
			if (!_isRequired && blaster != null && blaster.Type != BlasterType.None && NeedRate)
			{
				float forceRate2 = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level).forceRate;
				float forceRate3 = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blaster.Type, blaster.Level).forceRate;
				BlasterConfig blusterConfiguration2 = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blaster.Type);
				if (forceRate3 >= forceRate2 && blusterConfiguration2.isShowInStore)
				{
					blasterItem = blaster;
					_blasterType = blasterItem.Type;
					viewType = AnnounceItemViewType.Acquired;
					if (_isOffered)
					{
						ChangeState("ForTest");
						viewType = AnnounceItemViewType.AcquiredForTest;
					}
				}
			}
			SetInfoState(false);
			bool flag = false;
			if (blasterItem != null && missionSetting.Blaster != null)
			{
				flag = blasterItem.Type == missionSetting.Blaster.Type;
			}
			if (viewType != AnnounceItemViewType.Price && blasterItem.IsBought && !flag)
			{
				viewType = AnnounceItemViewType.AcquiredNeedEquip;
			}
			_itemValue = _blasterType;
			BlasterConfig blusterConfiguration3 = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(_blasterType);
			float forceRate4 = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level).forceRate;
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredNeedEquip && viewType != AnnounceItemViewType.AcquiredNeedUpgrade && viewType != AnnounceItemViewType.AcquiredNeedRecover && viewType != AnnounceItemViewType.AcquiredForTest && blasterItem.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			_title.text = Localization.GetValue(blusterConfiguration3.title, string.Empty);
			_itemPreview.SetSprite(blusterConfiguration3.uiIcon);
			_forceRateNormalCount.text = forceRate4.ToString();
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(blasterItem.Type, 0);
			if (blasterInApp != string.Empty)
			{
				string formattedPrice = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(blasterInApp);
				_priceState.SetPrice(PaymentButtonCurrency.Money, formattedPrice);
			}
			else
			{
				float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(blasterItem.Type, 0);
				float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(blasterItem.Type, 0);
				if (blasterPriceGems > 0f)
				{
					_priceState.SetPrice(PaymentButtonCurrency.Gems, blasterPriceGems.ToString());
				}
				else
				{
					_priceState.SetPrice(PaymentButtonCurrency.Coins, blasterPriceGold.ToString());
				}
			}
			UpdateViewType(viewType);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
			string levelWhenBlasterWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBlasterWillUnlocked(blasterItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBlasterWillUnlocked);
			if (levelItemByLevelName != null)
			{
				_lockedText.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name;
			}
			if (viewType == AnnounceItemViewType.Buy && blasterItem.IsLocked)
			{
				SetActiveLockedButton(true);
				SetActiveBuyButton(false);
				SetActiveUpgradeButton(false);
			}
			else
			{
				SetActiveLockedButton(false);
			}
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
		}

		public override void ShowConsumable(ConsumableType type, AnnounceItemViewType viewType, bool islarge = false)
		{
			_consumableType = type;
			_viewType = viewType;
			_islargeConsumable = islarge;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			_blasterInfo.gameObject.SetActive(false);
			_companionInfo.gameObject.SetActive(false);
			_armorInfo.gameObject.SetActive(false);
			_slugInfo.gameObject.SetActive(false);
			_previewButtonView.SetActive(false);
			_itemValue = type;
			ShowInfo();
			SetInfoState(false);
			ChangeState("NormalConsumable");
			List<ConsumableItem> consumables = MonoSingleton<AppController>.Instance.Model.ShopModel.Consumables;
			ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(type);
			_consumableCountTitle.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_title_your_balance");
			_consumableCountCurrent.text = consumable.Count.ToString();
			_consumableCountReady.text = consumable.Count.ToString();
			_consumableCountExtra.text = "+" + ((!islarge) ? 5 : 10);
			_consumableCountExtra.gameObject.SetActive(_upgradeLevel > consumable.Count);
			if (_upgradeLevel <= consumable.Count)
			{
				viewType = AnnounceItemViewType.Acquired;
				ChangeState("ReadyConsumable");
			}
			base.ShowConsumable(type, viewType, islarge);
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
		}

		public void ShowFakeGUISlug(SlugType type, AnnounceItemViewType viewType, int level)
		{
			_slugType = type;
			_viewType = viewType;
			_itemValue = _slugType;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(type);
			SlugGroup slugGroup = SlugTypeHelper.GetGroup(type);
			_slugTypeText.gameObject.SetActive(true);
			switch (slugGroup)
			{
			case SlugGroup.Attack:
				_slugTypeText.text = Localization.GetValue("slug_attack_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_attack_section_title", string.Empty);
				break;
			case SlugGroup.Control:
				_slugTypeText.text = Localization.GetValue("slug_control_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_control_section_title", string.Empty);
				break;
			default:
				_slugTypeText.text = Localization.GetValue("slug_special_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_special_section_title", string.Empty);
				break;
			}
			ChangeState("NormalLVL");
			_blasterInfo.gameObject.SetActive(false);
			_companionInfo.gameObject.SetActive(false);
			_armorInfo.gameObject.SetActive(false);
			_slugInfo.gameObject.SetActive(true);
			_previewButtonView.SetActive(true);
			SetInfoState(false);
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(_slugType);
			_title.text = Localization.GetValue(slugConfiguration.title, string.Empty);
			_titleUpgrade.text = Localization.GetValue(slugConfiguration.title, string.Empty);
			string localeForLevel = GetLocaleForLevel(level);
			string localeForLevel2 = GetLocaleForLevel(_upgradeLevel);
			_headerUpgradeLevelNormalText.text = Localization.GetValue(localeForLevel, string.Empty);
			_upgradeLevelText.text = Localization.GetValue(localeForLevel2, string.Empty);
			CurrentViewType = viewType;
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
			float forceRate = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(slug.Type, _upgradeLevel).forceRate;
			float forceRate2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(slug.Type, level).forceRate;
			SetForceRate(false, forceRate2, forceRate);
		}

		public override void ShowSlug(SlugType type, AnnounceItemViewType viewType)
		{
			_slugType = type;
			_viewType = viewType;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(missionSetting.Mission.Name);
			SlugItem slugItem = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(type);
			SlugGroup slugGroup = SlugTypeHelper.GetGroup(type);
			_slugTypeText.gameObject.SetActive(true);
			SlugItem slugItem2;
			switch (slugGroup)
			{
			case SlugGroup.Attack:
			{
				slugItem2 = missionSetting.AttackSlug;
				SlugItem slug2 = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(slugItem2.Type);
				if (slugItem2.Type != SlugType.None)
				{
					if (slugItem2 != slug2)
					{
						_isOffered = true;
					}
					else
					{
						_isOffered = false;
					}
				}
				_slugTypeText.text = Localization.GetValue("slug_attack_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_attack_section_title", string.Empty);
				break;
			}
			case SlugGroup.Control:
				slugItem2 = missionSetting.DefenceSlug;
				if (slugItem2.Type != SlugType.None)
				{
					SlugItem slug3 = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(slugItem2.Type);
					if (slugItem2 != slug3)
					{
						_isOffered = true;
					}
					else
					{
						_isOffered = false;
					}
				}
				_slugTypeText.text = Localization.GetValue("slug_control_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_control_section_title", string.Empty);
				break;
			default:
				slugItem2 = missionSetting.SpecialSlug;
				if (slugItem2.Type != SlugType.None)
				{
					SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(slugItem2.Type);
					if (slugItem2 != slug)
					{
						_isOffered = true;
					}
					else
					{
						_isOffered = false;
					}
				}
				_slugTypeText.text = Localization.GetValue("slug_special_section_title", string.Empty);
				_slugTypeTextSh.text = Localization.GetValue("slug_special_section_title", string.Empty);
				break;
			}
			ChangeState("NormalLVL");
			if (!_isRequired && slugItem2 != null && slugItem2.Type != SlugType.None)
			{
				slugItem = slugItem2;
				_slugType = slugItem.Type;
				viewType = AnnounceItemViewType.Acquired;
				if (_isOffered)
				{
					ChangeState("ForTest");
					viewType = AnnounceItemViewType.AcquiredForTest;
				}
			}
			else if (_isRequired && _isOffered)
			{
				ChangeState("ForTest");
				viewType = AnnounceItemViewType.AcquiredForTest;
			}
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission);
			if (levelItem.Type == LevelType.Arena || SurvivalController.IsSurvival(missionSetting.Mission))
			{
				int num = 0;
				for (int num2 = _upgradeLevel; num2 >= slugItem.Level; num2--)
				{
					if (IsUpgradeUnlocked(num2))
					{
						num = num2;
						break;
					}
				}
				if (num > slugItem.Level && viewType == AnnounceItemViewType.Acquired)
				{
					if (slugItem.IsBought)
					{
						viewType = AnnounceItemViewType.AcquiredNeedUpgrade;
						ChangeState("Upgrade");
					}
					else
					{
						viewType = AnnounceItemViewType.Buy;
						ChangeState("NormalLVL");
					}
				}
				_upgradeLevel = num;
			}
			_blasterInfo.gameObject.SetActive(false);
			_companionInfo.gameObject.SetActive(false);
			_armorInfo.gameObject.SetActive(false);
			_slugInfo.gameObject.SetActive(true);
			_previewButtonView.SetActive(true);
			_itemValue = _slugType;
			SetInfoState(false);
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(_slugType);
			_title.text = Localization.GetValue(slugConfiguration.title, string.Empty);
			_titleUpgrade.text = Localization.GetValue(slugConfiguration.title, string.Empty);
			string localeForLevel = GetLocaleForLevel(slugItem.Level);
			string localeForLevel2 = GetLocaleForLevel(_upgradeLevel);
			_headerUpgradeLevelNormalText.text = Localization.GetValue(localeForLevel, string.Empty);
			_upgradeLevelText.text = Localization.GetValue(localeForLevel2, string.Empty);
			bool flag = false;
			SlugItem slugByGroup = missionSetting.GetSlugByGroup(slugGroup);
			if (slugByGroup != null)
			{
				flag = slugItem.Type == slugByGroup.Type;
			}
			if (viewType != AnnounceItemViewType.Price && viewType != AnnounceItemViewType.AcquiredForTest)
			{
				if (slugItem.IsBought && slugByGroup != null && !flag)
				{
					viewType = AnnounceItemViewType.AcquiredNeedEquip;
					ChangeState("NormalLVL");
				}
				if (slugItem.IsBought && IsUpgradeUnlocked(_upgradeLevel))
				{
					if (slugItem.Level < _upgradeLevel)
					{
						viewType = AnnounceItemViewType.AcquiredNeedUpgrade;
						ChangeState("Upgrade");
					}
					else
					{
						ChangeState("NormalLVL");
						_paymentButton.gameObject.SetActive(false);
					}
					if (_equipView.activeSelf)
					{
						SetActiveEquipButton(false);
					}
				}
			}
			if (viewType == AnnounceItemViewType.Buy && slugItem.IsBought)
			{
				viewType = AnnounceItemViewType.Acquired;
			}
			base.ShowSlug(_slugType, viewType);
			CurrentViewType = viewType;
			SetActiveEquipButton(viewType == AnnounceItemViewType.AcquiredNeedEquip);
			SetActiveAcquiredButton(viewType == AnnounceItemViewType.Acquired);
			string levelWhenSlugWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenSlugWillUnlocked(slugItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenSlugWillUnlocked);
			if (levelItemByLevelName != null)
			{
				_lockedText.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name;
			}
			if (viewType == AnnounceItemViewType.Buy && slugItem.IsLocked)
			{
				SetActiveLockedButton(true);
				SetActiveBuyButton(false);
				SetActiveUpgradeButton(false);
			}
			else
			{
				SetActiveLockedButton(false);
			}
			UpgradePaymentState component = _upgradeView.GetComponent<UpgradePaymentState>();
			BuyPaymentState component2 = _buyView.GetComponent<BuyPaymentState>();
			float forceRate = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(slugItem.Type, slugItem.Level).forceRate;
			if (component != null && viewType == AnnounceItemViewType.AcquiredNeedUpgrade)
			{
				_allPrice = 0f;
				_allPrice = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPrices(slugItem.Type, slugItem.Level + 1, _upgradeLevel);
				component.SetPrice(PaymentButtonCurrency.Coins, _allPrice.ToString());
				SetActiveUpgradeButton(true);
			}
			else if (component2 != null && viewType == AnnounceItemViewType.AcquiredForTest)
			{
				component.Hide();
				ChangeState("ForTest");
			}
			else if (component2 != null && viewType == AnnounceItemViewType.Buy)
			{
				_allPrice = 0f;
				_allPrice = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPrices(slugItem.Type, slugItem.Level, _upgradeLevel);
				component2.SetPrice(PaymentButtonCurrency.Coins, _allPrice.ToString());
				_headerUpgradeLevelNormalText.text = Localization.GetValue(localeForLevel2, string.Empty);
				forceRate = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(slugItem.Type, _upgradeLevel).forceRate;
			}
			float forceRate2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(slugItem.Type, _upgradeLevel).forceRate;
			SetForceRate(viewType == AnnounceItemViewType.AcquiredNeedUpgrade, forceRate, forceRate2);
		}

		public void ShowInfo()
		{
			if (_itemValue.GetType() == typeof(SlugType))
			{
				SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(_slugType);
				slug.UpgradeLevel = _upgradeLevel;
				_slugInfo.ApplyModel(slug);
			}
			else if (_itemValue.GetType() == typeof(BlasterType))
			{
				BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(_blasterType);
				_blasterInfo.ApplyModel(blaster);
			}
			else if (_itemValue.GetType() == typeof(ArmorType))
			{
				ArmorItem armor = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(_armorType);
				armor.IsBackpack = true;
				_armorInfo.ApplyModel(armor);
			}
			else if (_itemValue.GetType() == typeof(AllyType))
			{
				AssistantItem assistant = MonoSingleton<AppController>.Instance.Model.ShopModel.GetAssistant(_allyType);
				assistant.UpgradeLevel = _upgradeLevel;
				_companionInfo.ApplyModel(assistant);
			}
			else if (_itemValue.GetType() == typeof(ConsumableType))
			{
				ConsumableProperties consumableProperties = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableProperties(_consumableType);
				_description.text = MonoSingleton<Localization>.Instance.GetValueByKey(consumableProperties.baseData.description);
			}
		}

		private void ChangeState(string state)
		{
			_backGroundUpgrade.SetActive(false);
			_backGroundNormal.SetActive(false);
			_backGroundRecovery.SetActive(false);
			_backGroundTest.SetActive(false);
			_badgeTest.SetActive(false);
			_headerUpgradeLevelNormal.SetActive(false);
			_headerUpgradeLevelRecovery.SetActive(false);
			_headerUpgradableItems.gameObject.SetActive(false);
			_headerOtherItems.gameObject.SetActive(false);
			_headerUpgradeLevelRoot.SetActive(false);
			_headerUpgradeLevelRecovery.SetActive(false);
			_forceRateUpgrade.SetActive(false);
			_forceRateNormal.SetActive(false);
			_forceRateRecovery.SetActive(false);
			_consumableCountRootNormal.SetActive(false);
			_consumableCountRootReady.SetActive(false);
			switch (state)
			{
			case "Normal":
				_backGroundNormal.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				_forceRateNormal.SetActive(true);
				break;
			case "ForTest":
				_backGroundTest.SetActive(true);
				_badgeTest.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				_forceRateNormal.SetActive(true);
				_paymentButton.gameObject.SetActive(false);
				break;
			case "NormalConsumable":
				_backGroundNormal.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				_consumableCountRootNormal.SetActive(true);
				break;
			case "ReadyConsumable":
				_backGroundNormal.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				_consumableCountRootReady.SetActive(true);
				break;
			case "NormalLVL":
				_backGroundNormal.SetActive(true);
				_headerUpgradeLevelNormal.SetActive(true);
				_headerUpgradableItems.gameObject.SetActive(true);
				_headerUpgradeLevelRoot.SetActive(true);
				_forceRateNormal.SetActive(true);
				break;
			case "NormalNoForce":
				_backGroundNormal.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				break;
			case "Upgrade":
				_backGroundUpgrade.SetActive(true);
				_headerUpgradeLevelNormal.SetActive(true);
				_headerUpgradableItems.gameObject.SetActive(true);
				_headerUpgradeLevelRoot.SetActive(true);
				_forceRateUpgrade.SetActive(true);
				break;
			case "Recovery":
				_backGroundRecovery.SetActive(true);
				_headerUpgradeLevelRecovery.SetActive(true);
				_headerUpgradableItems.gameObject.SetActive(true);
				_headerUpgradeLevelRoot.SetActive(true);
				_headerUpgradeLevelRecovery.SetActive(true);
				_forceRateRecovery.SetActive(true);
				break;
			default:
				_backGroundNormal.SetActive(true);
				_headerOtherItems.gameObject.SetActive(true);
				break;
			}
		}

		private void OnBtnInfoClick()
		{
			isInformation = !isInformation;
			_information.SetActive(isInformation);
			_menu.SetActive(!isInformation);
			if (isInformation)
			{
				ShowInfo();
				UpgradePaymentState component = _upgradeView.GetComponent<UpgradePaymentState>();
				if (component != null)
				{
					component.ResetButtonsScale();
				}
				BuyPaymentState component2 = _buyView.GetComponent<BuyPaymentState>();
				if (component2 != null)
				{
					component2.ResetButtonsScale();
				}
			}
			else
			{
				ReDrawWindow();
			}
			if (OnBtnInfo != null)
			{
				OnBtnInfo(isInformation);
			}
			InputWindowTk2dScrollView component3 = gameObject.GetComponent<InputWindowTk2dScrollView>();
			if (component3 != null)
			{
				component3.ReCheckElements();
			}
		}

		private void SetInfoState(bool state)
		{
			isInformation = state;
			_information.SetActive(state);
			_menu.SetActive(!state);
			if (OnBtnInfo != null)
			{
				OnBtnInfo(isInformation);
			}
		}

		public void ReDrawWindow()
		{
			if (_itemValue.GetType() == typeof(SlugType))
			{
				ShowSlug(_slugType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(ArmorType))
			{
				ShowArmor(_armorType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(BlasterType))
			{
				ShowBlaster(_blasterType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(AllyType))
			{
				ShowAlly(_allyType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(ConsumableType))
			{
				ShowConsumable(_consumableType, _viewType, _islargeConsumable);
			}
			_infoButton.IsOn = true;
			isInformation = false;
		}

		private void OnEquipButtonClick()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_itemValue.GetType() == typeof(SlugType))
			{
				SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(_slugType);
				missionSetting.SetSlug(slug);
				ShowSlug(_slugType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(ArmorType))
			{
				ArmorItem armor = MonoSingleton<AppController>.Instance.Model.ShopModel.GetArmor(_armorType);
				if (armor.IsBought)
				{
					missionSetting.Armor = armor;
				}
				ShowArmor(_armorType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(BlasterType))
			{
				BlasterItem blaster = MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(_blasterType);
				if (blaster.IsBought)
				{
					missionSetting.Blaster = blaster;
				}
				ShowBlaster(_blasterType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(AllyType))
			{
				AssistantItem assistant = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistantItem) => assistantItem.Type == _allyType);
				missionSetting.Assistant = assistant;
				ShowAlly(_allyType, _viewType);
			}
			if (OnEquipItemEvent__BackingField != null)
			{
				OnEquipItemEvent__BackingField();
			}
		}

		public override void OnBtnBuyClick()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (_itemValue.GetType() == typeof(SlugType))
			{
				SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(_slugType);
				if (shopModel.CanBuySlug(slug, _allPrice))
				{
					float slugPriceGold = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(_slugType, slug.Level);
					_allPrice -= slugPriceGold;
					base.OnBtnBuyClick();
					OnUpgradeButtonClick();
					OnEquipButtonClick();
				}
			}
			else if (_itemValue.GetType() == typeof(AllyType))
			{
				AssistantItem assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == _allyType);
				if (shopModel.CanBuyAssistant(assistantItem, _allPrice))
				{
					ShopItemConfiguration assistantConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(assistantItem.Type, assistantItem.Level);
					_allPrice -= assistantConfig.GamePrice.Value;
					base.OnBtnBuyClick();
					OnUpgradeButtonClick();
					OnEquipButtonClick();
				}
			}
			else if (_itemValue.GetType() == typeof(ConsumableType))
			{
				base.OnBtnBuyClick();
				ShowConsumable(_consumableType, _viewType);
			}
			else
			{
				base.OnBtnBuyClick();
				OnEquipButtonClick();
			}
			ReDrawWindow();
			if (OnRedrawAllEvent__BackingField != null)
			{
				OnRedrawAllEvent__BackingField();
			}
		}

		private void OnUpgradeButtonClick()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_itemValue.GetType() == typeof(SlugType))
			{
				SlugItem slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(_slugType);
				if (shopModel.CanBuySlug(slug, _allPrice))
				{
					int num = slug.Level + 1;
					for (int i = num; i <= _upgradeLevel; i++)
					{
						slug = MonoSingleton<AppController>.Instance.Model.ShopModel.GetSlug(_slugType);
						shopModel.OnUpgradeSlug(slug);
					}
				}
				if (shopModel.AttackSlugs.Contains(slug))
				{
					missionSetting.AttackSlug = slug;
				}
				else if (shopModel.DefenceSlugs.Contains(slug))
				{
					missionSetting.DefenceSlug = slug;
				}
				else if (shopModel.SpecialSlugs.Contains(slug))
				{
					missionSetting.SpecialSlug = slug;
				}
				ShowSlug(_slugType, _viewType);
			}
			else if (_itemValue.GetType() == typeof(AllyType))
			{
				AssistantItem assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == _allyType);
				if (shopModel.CanBuyAssistant(assistantItem, _allPrice))
				{
					int num2 = assistantItem.Level + 1;
					for (int num3 = num2; num3 <= _upgradeLevel; num3++)
					{
						assistantItem = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == _allyType);
						shopModel.OnUpgradeAssistant(assistantItem);
					}
				}
				missionSetting.Assistant = assistantItem;
				ShowAlly(_allyType, _viewType);
			}
			if (OnBtnUpgaradeItemEvent__BackingField != null)
			{
				OnBtnUpgaradeItemEvent__BackingField();
			}
			ReDrawWindow();
		}

		private string GetLocaleForLevel(int level)
		{
			string result = "lvl_05_title";
			switch (level)
			{
			case 0:
				result = "lvl_00_title";
				break;
			case 1:
				result = "lvl_01_title";
				break;
			case 2:
				result = "lvl_02_title";
				break;
			case 3:
				result = "lvl_03_title";
				break;
			case 4:
				result = "lvl_04_title";
				break;
			case 5:
				result = "lvl_05_title_short";
				break;
			}
			return result;
		}

		protected bool IsUpgradeUnlocked(int upgradeLevel)
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			switch (upgradeLevel)
			{
			case 0:
				return true;
			case 1:
				return model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Unlocked || model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed;
			case 2:
				return model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Unlocked || model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Completed;
			case 3:
				return model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Unlocked || model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Completed;
			case 4:
				return model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Unlocked || model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Completed;
			case 5:
				return model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Unlocked || model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Completed;
			default:
				return false;
			}
		}

		private void SetActiveEquipButton(bool active)
		{
			if (_equipView != null && _equipView.activeSelf != active)
			{
				_equipView.gameObject.SetActive(active);
			}
			if (active)
			{
				_paymentButton.gameObject.SetActive(!active);
			}
		}

		private void SetActiveUpgradeButton(bool active)
		{
			if (_upgradeButtonView != null && _upgradeButtonView.activeSelf != active)
			{
				_upgradeButtonView.gameObject.SetActive(active);
			}
		}

		private void SetActiveBuyButton(bool active)
		{
			if (_buyButtonView != null && _buyButtonView.activeSelf != active)
			{
				_buyButtonView.gameObject.SetActive(active);
			}
		}

		private void SetActiveAcquiredButton(bool active)
		{
			if (_acquiredView != null && _acquiredView.activeSelf != active)
			{
				_acquiredView.gameObject.SetActive(active);
			}
			if (active)
			{
				_paymentButton.gameObject.SetActive(!active);
			}
		}

		private void SetActiveLockedButton(bool active)
		{
			if (_lockedView != null && _lockedView.activeSelf != active)
			{
				_lockedView.gameObject.SetActive(active);
			}
			if (active)
			{
				_paymentButton.gameObject.SetActive(!active);
			}
		}

		private void SetForceRate(bool isUpgrade, float forceRate, float nextForceRate)
		{
			if (isUpgrade)
			{
				if (_forceRateUpgradeStats != null && _forceRateUpgradeStats.NewTM != null && _forceRateUpgradeStats.TM != null)
				{
					if (!_forceRateUpgradeStats.gameObject.activeSelf)
					{
						_forceRateUpgradeStats.gameObject.SetActive(true);
					}
					_forceRateUpgradeStats.TM.text = forceRate.ToString();
					_forceRateUpgradeStats.NewTM.text = nextForceRate.ToString();
				}
				if (_forceRateStats != null && _forceRateStats.gameObject.activeSelf)
				{
					_forceRateStats.gameObject.SetActive(false);
				}
				return;
			}
			if (_forceRateStats != null && _forceRateStats.TM != null)
			{
				if (!_forceRateStats.gameObject.activeSelf)
				{
					_forceRateStats.gameObject.SetActive(true);
				}
				_forceRateStats.TM.text = forceRate.ToString();
			}
			if (_forceRateUpgradeStats != null && _forceRateUpgradeStats.gameObject.activeSelf)
			{
				_forceRateUpgradeStats.gameObject.SetActive(false);
			}
		}
	}
}
