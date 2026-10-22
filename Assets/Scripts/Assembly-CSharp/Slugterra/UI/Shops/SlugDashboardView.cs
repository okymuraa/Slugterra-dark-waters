using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugDashboardView : ItemDashboardView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnInfoClickEvent__BackingField;

		[SerializeField]
		private tk2dUIProgressBar _damageProgress;

		[SerializeField]
		private tk2dSprite _damageArrow;

		[SerializeField]
		private tk2dUIProgressBar _damageProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _speedProgress;

		[SerializeField]
		private tk2dSprite _speedArrow;

		[SerializeField]
		private tk2dUIProgressBar _speedProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _splashAreaProgress;

		[SerializeField]
		private tk2dSprite _splashAreaArrow;

		[SerializeField]
		private tk2dUIProgressBar _splashAreaProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _rechargeProgress;

		[SerializeField]
		private tk2dSprite _rechargeArrow;

		[SerializeField]
		private tk2dUIProgressBar _rechargeProgressCompare;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private TextMesh _speedCount;

		[SerializeField]
		private TextMesh _splashAreaCount;

		[SerializeField]
		private TextMesh _rechargeCount;

		[SerializeField]
		private CountDownBarViewController _countdownBar;

		[SerializeField]
		private GameObject _equipedSlugsContainer;

		[SerializeField]
		private NextUpgradeLevel _nextUpgradeLevel;

		private SlugItem _item;

		private float _maxDamage;

		private float _maxProjectileSpeed;

		private float _maxHittingArea;

		private float _maxCallDown;

		[SerializeField]
		private PreviewSlugs _slugPreview;

		[SerializeField]
		private GameObject _UIEquip;

		[SerializeField]
		private GameObject _slugPreviewCam;

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

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				if (MonoSingleton<GameConfiguration>.Instance != null)
				{
					return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
				}
				return null;
			}
		}

		public event Action OnBtnInfoClickEvent
		{
			add
			{
				Action action = OnBtnInfoClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnInfoClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnInfoClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnInfoClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			if (_slugPreview == null)
			{
				_slugPreview = UnityEngine.Object.FindObjectOfType<PreviewSlugs>();
			}
			if (_UIEquip == null)
			{
				_UIEquip = UnityEngine.Object.FindObjectOfType<BackpackController>().gameObject;
			}
			if (_slugPreviewCam == null)
			{
				_slugPreviewCam = GameObject.Find("SlugPreviewRoot").transform.Find("Misc/SlugPreviewDetector/Animaton/SlugCamera").gameObject;
			}
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
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxDamage = (_maxProjectileSpeed = (_maxHittingArea = (_maxCallDown = 0f)));
			foreach (SlugConfig slugConfig in instance.SlugConfigManager.GetSlugConfigs())
			{
				for (int i = 0; i < slugConfig.levels.Count; i++)
				{
					SlugLevels slugLevels = slugConfig.levels[i];
					if (_maxDamage < slugLevels.Damage)
					{
						_maxDamage = slugLevels.Damage;
					}
					if (_maxProjectileSpeed < slugLevels.ProjectileSpeed)
					{
						_maxProjectileSpeed = slugLevels.ProjectileSpeed;
					}
					if (_maxHittingArea < slugLevels.HittingArea)
					{
						_maxHittingArea = slugLevels.HittingArea;
					}
					if (_maxCallDown < slugLevels.CallDown)
					{
						_maxCallDown = slugLevels.CallDown;
					}
				}
			}
		}

		private void OnEnable()
		{
			healthRestoreManager.OnUpdateValues += OnRestorationValuesUpdatedHandler;
		}

		private void OnDisable()
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnRestorationValuesUpdatedHandler;
			}
			if (_countdownBar.gameObject.activeSelf)
			{
				_countdownBar.Show(false);
				_countdownBar.gameObject.SetActive(false);
			}
			_nextUpgradeLevel.parent.SetActive(false);
			_newBadge.SetActive(false);
		}

		public override void OnObjectChanged(object model)
		{
			OnApplyModel(_item);
		}

		private bool IsCharged(SlugItem slugItem)
		{
			if (slugItem != null)
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				SlugGroup slugGroup = SlugTypeHelper.GetGroup(slugItem.Type);
				SlugItem slugByGroup = missionSetting.GetSlugByGroup(slugGroup);
				if (slugByGroup != null)
				{
					return slugItem.Type == slugByGroup.Type;
				}
			}
			return false;
		}

		protected override void OnApplyModel(object model)
		{
			Color color = new Color(1f, 0f, 30f / 255f, 1f);
			Color color2 = new Color(0f, 226f / 255f, 50f / 255f, 1f);
			Color color3 = new Color(1f, 229f / 255f, 0f, 1f);
			base.OnApplyModel(model);
			if (!gameObject.activeSelf)
			{
				return;
			}
			_nextUpgradeLevel.parent.SetActive(false);
			if (!(model is SlugItem))
			{
				return;
			}
			Localize(model);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			SlugItem slugItem = (_item = model as SlugItem);
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			SlugLevels slugParams = instance.SlugConfigManager.GetSlugParams(slugItem.Type, slugItem.Level);
			if (!IsItemLocked() && IsItemBought() && CanUpgrade() && IsUpgradeAvailable() && slugItem.Level < 5)
			{
				SlugLevels slugParams2 = instance.SlugConfigManager.GetSlugParams(slugItem.Type, slugItem.Level + 1);
				PlaceProgress(slugParams2.Damage, slugParams.Damage, _maxDamage, _damageProgress, _damageProgressCompare, _damageArrow);
				PlaceProgress(slugParams2.ProjectileSpeed, slugParams.ProjectileSpeed, _maxProjectileSpeed, _speedProgress, _speedProgressCompare, _speedArrow);
				PlaceProgress(slugParams2.HittingArea, slugParams.HittingArea, _maxHittingArea, _splashAreaProgress, _splashAreaProgressCompare, _splashAreaArrow);
				PlaceProgress(slugParams2.CallDown, slugParams.CallDown, _maxCallDown, _rechargeProgress, _rechargeProgressCompare, _rechargeArrow, true);
				if (slugItem.IsBought)
				{
					_nextUpgradeLevel.parent.SetActive(true);
					_newBadge.SetActive(false);
					int num = ItemLevel() + 1;
					string text = "lvl_0{0}_title";
					if (num == 5)
					{
						text += "_short";
					}
					TextMesh shadowNextLevel = _nextUpgradeLevel.shadowNextLevel;
					string valueByKey = MonoSingleton<Localization>.Instance.GetValueByKey(string.Format(text, num));
					_nextUpgradeLevel.nextLevel.text = valueByKey;
					shadowNextLevel.text = valueByKey;
				}
			}
			else
			{
				PlaceProgress(slugParams.Damage, slugParams.Damage, _maxDamage, _damageProgress, _damageProgressCompare, _damageArrow);
				PlaceProgress(slugParams.ProjectileSpeed, slugParams.ProjectileSpeed, _maxProjectileSpeed, _speedProgress, _speedProgressCompare, _speedArrow);
				PlaceProgress(slugParams.HittingArea, slugParams.HittingArea, _maxHittingArea, _splashAreaProgress, _splashAreaProgressCompare, _splashAreaArrow);
				PlaceProgress(slugParams.CallDown, slugParams.CallDown, _maxCallDown, _rechargeProgress, _rechargeProgressCompare, _rechargeArrow, true);
				_damageProgressCompare.gameObject.SetActive(false);
				_speedProgressCompare.gameObject.SetActive(false);
				_splashAreaProgressCompare.gameObject.SetActive(false);
				_rechargeProgressCompare.gameObject.SetActive(false);
			}
			if (_equipedSlugsContainer != null)
			{
				_equipedSlugsContainer.SetActive(true);
			}
			_countdownBar.Show(false);
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(slugItem.Type);
			if (!IsItemLocked() && slugConfiguration.isShowInStore && !GetIsShown(_item.Type))
			{
				if (_newBadge != null)
				{
					_newBadge.SetActive(!_nextUpgradeLevel.parent.activeSelf);
				}
				SetIsShown(_item.Type);
			}
			else if (_newBadge != null)
			{
				_newBadge.SetActive(false);
			}
			RecalculateStatus();
			RecalculateColorScheme();
			RecalculateButtons();
			RecalculateForceRate();
			RecalculateTabBadges();
			if (IsItemLocked())
			{
				string levelWhenSlugWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenSlugWillUnlocked(slugItem.Type);
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenSlugWillUnlocked);
				if (levelItemByLevelName != null)
				{
					SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
				}
			}
			else
			{
				if (!slugItem.IsBought)
				{
					return;
				}
				if (!healthRestoreManager.GetHealthConfiguration(slugItem.Type).IsFullyRestored)
				{
					if (_equipedSlugsContainer != null)
					{
						_equipedSlugsContainer.SetActive(false);
					}
					_countdownBar.SetTimer(healthRestoreManager.GetHealthConfiguration(slugItem.Type).TimeToRestore);
					_countdownBar.SetIcon("icon_injury").SetTitle("slug_recovery").Show(true);
				}
				else if (_countdownBar.gameObject.activeSelf)
				{
					_countdownBar.Show(false);
				}
			}
		}

		protected override bool AnyItemIsNotFullyRestored()
		{
			foreach (SlugItem attackSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.AttackSlugs)
			{
				if (!healthRestoreManager.GetHealthConfiguration(attackSlug.Type).IsFullyRestored)
				{
					return true;
				}
			}
			foreach (SlugItem defenceSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs)
			{
				if (!healthRestoreManager.GetHealthConfiguration(defenceSlug.Type).IsFullyRestored)
				{
					return true;
				}
			}
			foreach (SlugItem specialSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs)
			{
				if (!healthRestoreManager.GetHealthConfiguration(specialSlug.Type).IsFullyRestored)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool AnyItemNotShownInStore()
		{
			foreach (SlugItem attackSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.AttackSlugs)
			{
				SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(attackSlug.Type);
				if ((attackSlug.IsBought || !attackSlug.IsLocked) && attackSlug.Type != SlugType.Flopper && slugConfiguration.isShowInStore && !GetIsShown(attackSlug.Type))
				{
					return true;
				}
			}
			foreach (SlugItem defenceSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs)
			{
				SlugConfig slugConfiguration2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(defenceSlug.Type);
				if ((defenceSlug.IsBought || !defenceSlug.IsLocked) && slugConfiguration2.isShowInStore && !GetIsShown(defenceSlug.Type))
				{
					return true;
				}
			}
			foreach (SlugItem specialSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs)
			{
				SlugConfig slugConfiguration3 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(specialSlug.Type);
				if ((specialSlug.IsBought || !specialSlug.IsLocked) && slugConfiguration3.isShowInStore && !GetIsShown(specialSlug.Type))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool CanUnequip()
		{
			return _item.Type != SlugType.Infurnus;
		}

		protected override float NextLevelForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(_item.Type, _item.Level + 1).forceRate;
		}

		protected override float ForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(_item.Type, _item.Level).forceRate;
		}

		protected override float RecoverPrice()
		{
			return healthRestoreManager.GetHealPrice(_item.Type, _item.Level);
		}

		protected override float UpgradePrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(_item.Type, _item.Level + 1);
		}

		protected override float UpgradePriceByLevel(int level)
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(_item.Type, level);
		}

		protected override float UpgradePrices(int levelTo, int levelFrom = -1)
		{
			float num = 0f;
			if (levelFrom == -1)
			{
				levelFrom = _item.Level;
			}
			for (int i = levelFrom; i <= levelTo; i++)
			{
				num += UpgradePriceByLevel(i);
			}
			return num;
		}

		protected override bool IsItemRecovered()
		{
			return healthRestoreManager.GetHealthConfiguration(_item.Type).IsFullyRestored;
		}

		protected override bool IsItemLocked()
		{
			return _item.IsLocked && !_item.IsBought;
		}

		protected override bool IsItemBought()
		{
			return _item.IsBought;
		}

		protected override bool IsEquipped()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			SlugGroup slugGroup = SlugTypeHelper.GetGroup(_item.Type);
			SlugItem slugByGroup = missionSetting.GetSlugByGroup(slugGroup);
			if (slugByGroup != null)
			{
				return _item.Type == slugByGroup.Type;
			}
			return false;
		}

		protected override bool IsUpgradeAvailable()
		{
			if (IsItemLocked())
			{
				return false;
			}
			return IsUpgradeUnlocked(_item.Level + 1);
		}

		protected override int ItemLevel()
		{
			return _item.Level;
		}

		protected override bool CanUpgrade()
		{
			return _item.Level < 5;
		}

		protected override string BuyPrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugPriceGold(_item.Type, _item.Level).ToString();
		}

		private void SetIsShown(SlugType slug)
		{
			PlayerPrefs.SetInt(slug.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(SlugType slug)
		{
			return PlayerPrefs.GetInt(slug.ToString() + "IsShown", 0) == 1;
		}

		private IEnumerator UpdateRecoverPrice()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();
				float price = healthRestoreManager.GetHealPrice(_item.Type, _item.Level);
				_paymentButton.View.SetPrice(PaymentButtonCurrency.Coins, price.ToString());
			}
		}

		private void Localize(object model)
		{
			if (model is SlugItem)
			{
				SlugItem slugItem = model as SlugItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				SlugConfig slugConfiguration = instance.SlugConfigManager.GetSlugConfiguration(slugItem.Type);
				_title.text = Localization.GetValue(slugConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(slugConfiguration.description, string.Empty);
			}
		}

		private void PlaceProgress(float value, float valueOwned, float maxValue, tk2dUIProgressBar progress, tk2dUIProgressBar progressCompare, tk2dSprite arrow, bool invertAlgoritm = false)
		{
			Color color = new Color(1f, 0f, 30f / 255f, 1f);
			Color color2 = new Color(0f, 226f / 255f, 50f / 255f, 1f);
			Color color3 = new Color(1f, 229f / 255f, 0f, 1f);
			float num;
			float num2;
			if (invertAlgoritm)
			{
				num = 1f - value / maxValue;
				num2 = 1f - valueOwned / maxValue;
			}
			else
			{
				num = value / maxValue;
				num2 = valueOwned / maxValue;
			}
			Color color4 = color2;
			progress.Value = num;
			if (value == valueOwned)
			{
				color4 = color3;
				progressCompare.gameObject.SetActive(false);
				arrow.gameObject.SetActive(false);
			}
			else
			{
				progressCompare.gameObject.SetActive(true);
				arrow.gameObject.SetActive(true);
				if (num > num2)
				{
					progressCompare.Value = num;
					progress.Value = num2;
					arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				}
				else
				{
					color4 = color;
					progressCompare.Value = num2;
					arrow.transform.rotation = Quaternion.identity;
				}
			}
			progressCompare.slicedSpriteBar.color = color4;
			arrow.color = color4;
		}

		private void OnBtnPreviewClick()
		{
			if (OnBtnInfoClickEvent__BackingField != null)
			{
				OnBtnInfoClickEvent__BackingField();
			}
			_UIEquip.SetActive(false);
			_slugPreviewUI.SetActive(true);
			_slugPreviewCam.SetActive(true);
			_slugPreview.Type = _item.Type;
			_slugPreview.Attack();
		}

		private void OnBtnBackClick(tk2dButton source)
		{
			_UIEquip.SetActive(true);
			_slugPreviewUI.SetActive(false);
			_slugPreviewCam.SetActive(false);
			_slugPreview.StopElliFire();
		}

		private void OnRestorationValuesUpdatedHandler()
		{
			CheckHealthRestorationState();
		}

		private void CheckHealthRestorationState()
		{
			OnApplyModel(_item);
		}
	}
}
