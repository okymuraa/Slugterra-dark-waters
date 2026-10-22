using AppsMinistry.Core;
using AppsMinistry.Core.Input.Window;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public abstract class ItemDashboardView : UIView, IObserver
	{
		public static readonly Color LockedLabelItemsColor = new Color32(120, 120, 120, byte.MaxValue);

		public static readonly Color RecoveryLabelColor = new Color32(byte.MaxValue, 0, 30, byte.MaxValue);

		public static readonly string LockedProgressbarSprite = "scroll_grey";

		public static readonly string RecoveryProgressbarSprite = "scroll_red";

		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		protected TextMesh _level;

		[SerializeField]
		private TextMesh[] _labels;

		[SerializeField]
		private tk2dBaseSprite[] _progressBarSprites;

		[SerializeField]
		private GameObject _defaultColorScheme;

		[SerializeField]
		private GameObject _recoveryColorScheme;

		[SerializeField]
		private GameObject _upgradeColorScheme;

		[SerializeField]
		private GameObject _lockedStatus;

		[SerializeField]
		private GameObject _readyStatus;

		[SerializeField]
		private GameObject _upgradeLockedStatus;

		[SerializeField]
		private GameObject _upgradeForceRate;

		[SerializeField]
		private GameObject _normalForceRate;

		[SerializeField]
		private GameObject _recoveryForceRate;

		[SerializeField]
		private TextMesh _upgradeForceRateTM;

		[SerializeField]
		private TextMesh _upgradeForceRateNewTM;

		[SerializeField]
		private TextMesh _normalForceRateTM;

		[SerializeField]
		private TextMesh _recoveryForceRateTM;

		[SerializeField]
		protected PaymentButtonController _paymentButton;

		[SerializeField]
		protected GameObject _equipButton;

		[SerializeField]
		protected GameObject _unequipButton;

		[SerializeField]
		protected GameObject _newBadge;

		[SerializeField]
		protected GameObject _newItemInCategoryBadge;

		[SerializeField]
		protected GameObject _attensionItemInCategoryBadge;

		private Color[] _labelsDefaultColor;

		private int[] _progressBarSpritesDefault;

		private MissionSetting _missionSetting;

		private DashboardColorScheme _colorScheme;

		protected static bool isTabInfoClose = true;

		protected virtual void OnCreate()
		{
			if (_labels != null && _labels.Length > 0)
			{
				_labelsDefaultColor = new Color[_labels.Length];
				for (int i = 0; i < _labels.Length; i++)
				{
					_labelsDefaultColor[i] = _labels[i].color;
				}
			}
			if (_progressBarSprites != null && _progressBarSprites.Length > 0)
			{
				_progressBarSpritesDefault = new int[_progressBarSprites.Length];
				for (int j = 0; j < _progressBarSprites.Length; j++)
				{
					_progressBarSpritesDefault[j] = _progressBarSprites[j].spriteId;
				}
			}
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_title);
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_description);
			_missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			_missionSetting.AddObserver(this);
			RecalculateTabBadges();
		}

		private void Awake()
		{
			OnCreate();
		}

		protected bool IsUpgradeUnlocked(int upgradeLevel)
		{
			switch (upgradeLevel)
			{
			case 0:
				return true;
			case 1:
				Debug.LogWarningFormat("ItemDashboardView::IsUpgradeUnlocked Status of {1} is {0}", typeof(QuietLawn_2_1).Name, MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status);
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed;
			case 2:
				Debug.LogWarningFormat("ItemDashboardView::IsUpgradeUnlocked Status of {1} is {0}", typeof(QuietLawn_2_5).Name, MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_5)).Status);
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_5)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_5)).Status == LevelStatus.Completed;
			case 3:
				Debug.LogWarningFormat("ItemDashboardView::IsUpgradeUnlocked Status of {1} is {0}", typeof(DarkPeriphery_3_3).Name, MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_3)).Status);
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_3)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_3)).Status == LevelStatus.Completed;
			case 4:
				Debug.LogWarningFormat("ItemDashboardView::IsUpgradeUnlocked Status of {1} is {0}", typeof(Deadweed_4_3).Name, MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_3)).Status);
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_3)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_3)).Status == LevelStatus.Completed;
			case 5:
				Debug.LogWarningFormat("ItemDashboardView::IsUpgradeUnlocked Status of {1} is {0}", typeof(GhoulDep_5_2).Name, MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_2)).Status);
				return MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_2)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_2)).Status == LevelStatus.Completed;
			default:
				return false;
			}
		}

		protected virtual string UnlockUpgradeMission(int level)
		{
			switch (level)
			{
			case 1:
				return "2.5";
			case 2:
				return "3.3";
			case 3:
				return "4.3";
			case 4:
				return "5.2";
			default:
				return string.Empty;
			}
		}

		protected void SetLockedText(string text)
		{
			if (_lockedStatus != null)
			{
				_lockedStatus.GetComponentInChildren<TextMesh>().text = text;
			}
		}

		private void SetStatus(DashboardStatus status)
		{
			if (_lockedStatus != null && _lockedStatus.activeSelf != (status == DashboardStatus.Locked))
			{
				_lockedStatus.SetActive(status == DashboardStatus.Locked);
			}
		}

		protected void RecalculateForceRate()
		{
			if (!IsItemRecovered())
			{
				if (_recoveryForceRate != null && _recoveryForceRateTM != null)
				{
					if (!_recoveryForceRate.activeSelf)
					{
						_recoveryForceRate.SetActive(true);
					}
					_recoveryForceRateTM.text = string.Format("<b><color=#FF001D>0</color></b><b><color=#B1B1B1>/</color></b><b><color=#B1B1B1>{0}</color></b>", ForceRate());
				}
				if (_upgradeForceRate != null && _upgradeForceRate.activeSelf)
				{
					_upgradeForceRate.SetActive(false);
				}
				if (_normalForceRate != null && _normalForceRate.activeSelf)
				{
					_normalForceRate.SetActive(false);
				}
				return;
			}
			if (CanUpgrade() && IsUpgradeAvailable() && IsItemBought())
			{
				if (_upgradeForceRate != null && _upgradeForceRateNewTM != null && _upgradeForceRateTM != null)
				{
					if (!_upgradeForceRate.activeSelf)
					{
						_upgradeForceRate.SetActive(true);
					}
					_upgradeForceRateTM.text = ForceRate().ToString();
					_upgradeForceRateNewTM.text = NextLevelForceRate().ToString();
				}
				if (_normalForceRate != null && _normalForceRate.activeSelf)
				{
					_normalForceRate.SetActive(false);
				}
				if (_recoveryForceRate != null && _recoveryForceRate.activeSelf)
				{
					_recoveryForceRate.SetActive(false);
				}
				if (_normalForceRate != null && _normalForceRateTM != null)
				{
					_normalForceRateTM.text = ForceRate().ToString();
				}
				return;
			}
			if (_normalForceRate != null && _normalForceRateTM != null)
			{
				if (!_normalForceRate.activeSelf)
				{
					_normalForceRate.SetActive(true);
				}
				_normalForceRateTM.text = ForceRate().ToString();
			}
			if (_upgradeForceRate != null && _upgradeForceRate.activeSelf)
			{
				_upgradeForceRate.SetActive(false);
			}
			if (_recoveryForceRate != null && _recoveryForceRate.activeSelf)
			{
				_recoveryForceRate.SetActive(false);
			}
		}

		protected void RecalculateButtons(bool isBackpack = false, int level = 0)
		{
			if (!IsItemLocked() || IsItemBought())
			{
				if (!IsItemBought())
				{
					SetActivePaymentButton(true);
					SetActiveEquipButton(false);
					SetActiveUnEquipButton(false);
					_paymentButton.SetSate(PaymentButtonState.Buy);
					_paymentButton.View.SetPrice(BuyCurrency(), BuyPrice().ToString());
					return;
				}
				SetActiveEquipButton(!IsEquipped());
				SetActiveReadyStatusButton(IsEquipped() && !CanUnequip());
				SetActiveUnEquipButton(IsEquipped() && CanUnequip());
				if (!IsItemRecovered())
				{
					SetActivePaymentButton(true);
					ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
					if (shopModel.CanBuySimple(RecoverPrice(), false))
					{
						_paymentButton.SetSate(PaymentButtonState.Recover);
						_paymentButton.View.SetPrice(BuyCurrency(), RecoverPrice().ToString());
					}
					else
					{
						_paymentButton.SetSate(PaymentButtonState.RecoverByAd);
						_paymentButton.View.SetPrice(PaymentButtonCurrency.Ads, "0");
					}
				}
				else if (CanUpgrade() && IsUpgradeAvailable())
				{
					_paymentButton.SetSate((PaymentButtonState)(3 + ItemLevel()));
					if (isBackpack)
					{
						_paymentButton.View.SetPrice(BuyCurrency(), UpgradePrices(level).ToString());
					}
					else
					{
						_paymentButton.View.SetPrice(BuyCurrency(), UpgradePrice().ToString());
					}
					SetActivePaymentButton(isTabInfoClose);
					_paymentButton.SetSate((PaymentButtonState)(3 + ItemLevel()));
					if (!isTabInfoClose && (_paymentButton.State == PaymentButtonState.LevelDefault || _paymentButton.State == PaymentButtonState.Level1 || _paymentButton.State == PaymentButtonState.Level2 || _paymentButton.State == PaymentButtonState.Level3 || _paymentButton.State == PaymentButtonState.Level4 || _paymentButton.State == PaymentButtonState.Level5))
					{
						_paymentButton.gameObject.SetActive(false);
					}
				}
				else if (CanUpgrade() && !IsUpgradeAvailable() && !isBackpack)
				{
					SetActivePaymentButton(true);
					_paymentButton.SetSate(PaymentButtonState.UpgradeLocked);
					_paymentButton.View.SetLockedText(MonoSingleton<Localization>.Instance["menu_title_upgrade_unlock"] + " " + UnlockUpgradeMission(ItemLevel()));
					if (!isTabInfoClose && _paymentButton.State == PaymentButtonState.UpgradeLocked)
					{
						_paymentButton.gameObject.SetActive(false);
					}
				}
				else if (CanUpgrade() && !IsUpgradeAvailable() && isBackpack)
				{
					SetActivePaymentButton(false);
					SetActiveReadyStatusButton(true);
				}
				else if (!CanUpgrade())
				{
					SetActivePaymentButton(true);
					_paymentButton.SetSate(PaymentButtonState.Level5);
				}
				else
				{
					SetActivePaymentButton(false);
				}
			}
			else
			{
				SetActiveEquipButton(false);
				SetActiveUnEquipButton(false);
				SetActivePaymentButton(false);
			}
		}

		public virtual void RecalculateTabBadges()
		{
			if (AnyItemIsNotFullyRestored())
			{
				if (_attensionItemInCategoryBadge != null && !_attensionItemInCategoryBadge.activeSelf)
				{
					_attensionItemInCategoryBadge.SetActive(true);
				}
				if (_newItemInCategoryBadge != null && _newItemInCategoryBadge.activeSelf)
				{
					_newItemInCategoryBadge.SetActive(false);
				}
			}
			else if (AnyItemNotShownInStore())
			{
				if (_newItemInCategoryBadge != null && !_newItemInCategoryBadge.activeSelf)
				{
					_newItemInCategoryBadge.SetActive(true);
				}
				if (_attensionItemInCategoryBadge != null && _attensionItemInCategoryBadge.activeSelf)
				{
					_attensionItemInCategoryBadge.SetActive(false);
				}
			}
			else
			{
				if (_newItemInCategoryBadge != null && _newItemInCategoryBadge.activeSelf)
				{
					_newItemInCategoryBadge.SetActive(false);
				}
				if (_attensionItemInCategoryBadge != null && _attensionItemInCategoryBadge.activeSelf)
				{
					_attensionItemInCategoryBadge.SetActive(false);
				}
			}
		}

		protected void RecalculateStatus()
		{
			if (_level != null)
			{
				int num = ItemLevel();
				string text = "lvl_0{0}_title";
				if (num == 5)
				{
					text += "_short";
				}
				_level.text = MonoSingleton<Localization>.Instance.GetValueByKey(string.Format(text, num));
			}
			if (!IsItemLocked() || IsItemBought())
			{
				if (_readyStatus != null && _readyStatus.activeSelf != IsEquipped())
				{
					_readyStatus.SetActive(IsEquipped());
				}
				if (!IsItemBought())
				{
					SetStatus(DashboardStatus.Default);
				}
				else if (CanUpgrade() && !IsUpgradeAvailable())
				{
					SetStatus(DashboardStatus.UpgradeLocked);
				}
				else
				{
					SetStatus(DashboardStatus.Default);
				}
			}
			else
			{
				SetStatus(DashboardStatus.Locked);
				if (_readyStatus != null && _readyStatus.activeSelf)
				{
					_readyStatus.SetActive(false);
				}
			}
		}

		private void SetActiveEquipButton(bool active)
		{
			if (_equipButton != null && _equipButton.activeSelf != active)
			{
				_equipButton.gameObject.SetActive(active);
			}
		}

		private void SetActiveUnEquipButton(bool active)
		{
			if (_unequipButton != null && _unequipButton.activeSelf != active)
			{
				_unequipButton.gameObject.SetActive(active);
			}
		}

		private void SetActiveReadyStatusButton(bool active)
		{
			if (_readyStatus != null && _readyStatus.activeSelf != active)
			{
				_readyStatus.gameObject.SetActive(active);
			}
		}

		private void SetActivePaymentButton(bool active)
		{
			if (_paymentButton != null && _paymentButton.View != null && _paymentButton.View.gameObject.activeSelf != active)
			{
				_paymentButton.SetSate(PaymentButtonState.Hide);
				_paymentButton.View.gameObject.SetActive(active);
			}
		}

		protected virtual bool CanUnequip()
		{
			return true;
		}

		protected virtual bool AnyItemIsNotFullyRestored()
		{
			return false;
		}

		protected virtual bool AnyItemNotShownInStore()
		{
			return false;
		}

		protected virtual float NextLevelForceRate()
		{
			return -2f;
		}

		protected virtual float ForceRate()
		{
			return -2f;
		}

		protected virtual bool CanUpgrade()
		{
			return false;
		}

		protected virtual bool IsItemLocked()
		{
			return true;
		}

		protected virtual bool IsUpgradeAvailable()
		{
			return false;
		}

		protected virtual bool IsEquipped()
		{
			return false;
		}

		protected virtual bool IsItemRecovered()
		{
			return true;
		}

		protected virtual bool IsItemBought()
		{
			return false;
		}

		protected virtual string BuyPrice()
		{
			return "0";
		}

		protected virtual PaymentButtonCurrency BuyCurrency()
		{
			return PaymentButtonCurrency.Coins;
		}

		protected virtual float UpgradePrice()
		{
			return 0f;
		}

		protected virtual float UpgradePriceByLevel(int level)
		{
			return 0f;
		}

		protected virtual float UpgradePrices(int levelTo, int levelFrom = -1)
		{
			return 0f;
		}

		protected virtual float RecoverPrice()
		{
			return 0f;
		}

		protected virtual int ItemLevel()
		{
			return 0;
		}

		protected void RecalculateColorScheme()
		{
			if (!IsItemLocked() || IsItemBought())
			{
				if (!IsItemRecovered())
				{
					SetColorScheme(DashboardColorScheme.Recovery);
				}
				else if (CanUpgrade() && IsUpgradeAvailable())
				{
					SetColorScheme(DashboardColorScheme.Upgrade);
				}
			}
			SetColorScheme(DashboardColorScheme.Default);
		}

		protected void SetColorScheme(DashboardColorScheme scheme)
		{
			if (_colorScheme != scheme)
			{
				if (_defaultColorScheme != null && _defaultColorScheme.activeSelf != (scheme == DashboardColorScheme.Default))
				{
					_defaultColorScheme.SetActive(scheme == DashboardColorScheme.Default);
				}
				if (_recoveryColorScheme != null && _recoveryColorScheme.activeSelf != (scheme == DashboardColorScheme.Recovery))
				{
					_recoveryColorScheme.SetActive(scheme == DashboardColorScheme.Recovery);
				}
				if (_upgradeColorScheme != null && _upgradeColorScheme.activeSelf != (scheme == DashboardColorScheme.Upgrade))
				{
					_upgradeColorScheme.SetActive(scheme == DashboardColorScheme.Upgrade);
				}
				_colorScheme = scheme;
				SetLabelsColorScheme(_colorScheme);
				SetProgressbarsColorScheme(_colorScheme);
			}
		}

		private void SetLabelsColorScheme(DashboardColorScheme scheme)
		{
			if (_labels == null || _labels.Length <= 0)
			{
				return;
			}
			Color color;
			switch (scheme)
			{
			case DashboardColorScheme.Default:
			{
				for (int i = 0; i < _labels.Length; i++)
				{
					_labels[i].color = _labelsDefaultColor[i];
				}
				return;
			}
			case DashboardColorScheme.Locked:
				color = LockedLabelItemsColor;
				break;
			default:
				color = RecoveryLabelColor;
				break;
			}
			Color color2 = color;
			for (int j = 0; j < _labels.Length; j++)
			{
				_labels[j].color = color2;
				if (scheme == DashboardColorScheme.Locked && _labels[j].gameObject.name == "lblName")
				{
					_labels[j].color = new Color32(153, 153, 153, byte.MaxValue);
				}
			}
		}

		private void SetProgressbarsColorScheme(DashboardColorScheme scheme)
		{
			if (_progressBarSprites == null || _progressBarSprites.Length <= 0)
			{
				return;
			}
			string text;
			switch (scheme)
			{
			case DashboardColorScheme.Default:
			{
				for (int i = 0; i < _progressBarSprites.Length; i++)
				{
					_progressBarSprites[i].SetSprite(_progressBarSpritesDefault[i]);
				}
				return;
			}
			case DashboardColorScheme.Locked:
				text = LockedProgressbarSprite;
				break;
			default:
				text = RecoveryProgressbarSprite;
				break;
			}
			string sprite = text;
			for (int j = 0; j < _progressBarSprites.Length; j++)
			{
				_progressBarSprites[j].SetSprite(sprite);
			}
		}

		public virtual void OnObjectChanged(object observable)
		{
		}

		protected override void OnReleaseResource()
		{
			if (_missionSetting != null)
			{
				_missionSetting.RemoveObserver(this);
			}
		}

		protected void OnBtnInfoShow(tk2dUIToggleButton toggle)
		{
			isTabInfoClose = toggle.IsOn;
			switch (_paymentButton.State)
			{
			case PaymentButtonState.LevelDefault:
			case PaymentButtonState.Level1:
			case PaymentButtonState.Level2:
			case PaymentButtonState.Level3:
			case PaymentButtonState.Level4:
			case PaymentButtonState.Level5:
			case PaymentButtonState.UpgradeLocked:
				_paymentButton.gameObject.SetActive(toggle.IsOn);
				break;
			}
			InputWindowView component = gameObject.GetComponent<InputWindowView>();
			if (component != null)
			{
				component.ReCheckElements();
			}
		}

		protected void OnBtnInfoHide(tk2dUIToggleButton toggle)
		{
			isTabInfoClose = toggle.IsOn;
			switch (_paymentButton.State)
			{
			case PaymentButtonState.LevelDefault:
			case PaymentButtonState.Level1:
			case PaymentButtonState.Level2:
			case PaymentButtonState.Level3:
			case PaymentButtonState.Level4:
			case PaymentButtonState.Level5:
			case PaymentButtonState.UpgradeLocked:
				_paymentButton.gameObject.SetActive(toggle.IsOn);
				break;
			}
			InputWindowView component = gameObject.GetComponent<InputWindowView>();
			if (component != null)
			{
				component.ReCheckElements();
			}
		}
	}
}
