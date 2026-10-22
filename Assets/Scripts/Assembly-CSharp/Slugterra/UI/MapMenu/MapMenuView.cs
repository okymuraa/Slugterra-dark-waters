using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Core.Input.Window;
using AppsMinistry.Core.Motion;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.MapMenu
{
	public class MapMenuView : UIView
	{
		private struct InputWindowStruct
		{
			public InputWindowView prev;

			public InputWindowView cur;

			public InputWindowView next;

			public InputWindowView arena;

			public InputWindowView daily;

			public InputWindowView daily1;

			public LevelItem nextLevelItem;

			public int order;

			public InputWindowStruct(InputWindowView _prev, InputWindowView _cur, InputWindowView _next, int _order)
			{
				prev = _prev;
				cur = _cur;
				next = _next;
				order = _order;
				arena = null;
				daily = null;
				daily1 = null;
				nextLevelItem = null;
			}

			public bool NextItemIsLocked()
			{
				return nextLevelItem != null && (nextLevelItem.Status == LevelStatus.Completed || nextLevelItem.Status == LevelStatus.Unlocked);
			}
		}

		[Serializable]
		private struct ChangePlatformObjects
		{
			public GameObject _objectTVOS;

			public GameObject _objectDefault;

			public ChangePlatformObjects(GameObject objectTVOS, GameObject objectDefault)
			{
				_objectTVOS = objectTVOS;
				_objectDefault = objectDefault;
			}

			public void SetAcriveObjects()
			{
				bool flag = false;
				flag = false;
				if (_objectTVOS != null)
				{
					_objectTVOS.SetActive(flag);
				}
				if (_objectDefault != null)
				{
					_objectDefault.SetActive(!flag);
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnEquipEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackpackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnLevelItemCreatedEvent__BackingField;

		[SerializeField]
		private Camera _levelItemsCamera;

		[SerializeField]
		private TextMesh _nextBtnLabel;

		[SerializeField]
		private TextMesh _nextBtnLabelBack;

		[SerializeField]
		private GameObject _levelItemPrefab;

		[SerializeField]
		private GameObject _levelItemFXPrefab;

		[SerializeField]
		private GameObject _saleInStore;

		[SerializeField]
		private SwapGameObjectsState _swapArsenalButtonState;

		[SerializeField]
		private GameObject parentGameObject;

		private List<LevelItemPlaceHolderView> _placeHolders = new List<LevelItemPlaceHolderView>();

		private List<LevelItemController> _levelItems = new List<LevelItemController>();

		private Dictionary<LevelItem, InputWindowStruct> _levelItemsWindowViews = new Dictionary<LevelItem, InputWindowStruct>();

		private GameObject _FxChosenItem;

		private ScaleMotion _dailyScaler;

		private string _targetNeededAsset = string.Empty;

		[SerializeField]
		private GameObject _btnArsenal;

		[SerializeField]
		private GameObject _badgeForArsenal;

		[SerializeField]
		private GameObject _lowerRight;

		[SerializeField]
		private GameObject _upperLeft;

		[SerializeField]
		private GameObject _lowerLeft;

		[SerializeField]
		private ChangePlatformObjects _btns;

		private bool loadAfterBundleDownloading;

		public List<LevelItemController> LevelItems
		{
			get
			{
				return _levelItems;
			}
		}

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnEquipEvent
		{
			add
			{
				Action action = OnEquipEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEquipEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnEquipEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnEquipEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBackpackEvent
		{
			add
			{
				Action action = OnBackpackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackpackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackpackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackpackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGameServiceEvent
		{
			add
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnLevelItemCreatedEvent
		{
			add
			{
				Action action = OnLevelItemCreatedEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLevelItemCreatedEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnLevelItemCreatedEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLevelItemCreatedEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Awake()
		{
			_placeHolders.AddRange(UnityEngine.Object.FindObjectsOfType<LevelItemPlaceHolderView>());
		}

		private void OnEnable()
		{
			LevelItemController.OnSelectlevelItem = (Action<LevelItemController>)Delegate.Combine(LevelItemController.OnSelectlevelItem, new Action<LevelItemController>(SelectLevelItem));
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Combine(instance.finishedBundleLoading, new Action<string>(LoadLevelAfterBundleDownload));
			Invoke("ShowPopupMultiplayerAvailability", 0.6f);
			MonoSingleton<AppController>.Instance.Model.ShopModel.CheckSlugs();
			MonoSingleton<AppController>.Instance.Model.ShopModel.CheckArmor();
			_badgeForArsenal.SetActive(false);
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		private bool CheskSlugsBadges()
		{
			foreach (SlugItem attackSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.AttackSlugs)
			{
				SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(attackSlug.Type);
				if ((attackSlug.IsBought || !attackSlug.IsLocked) && attackSlug.Type != SlugType.Flopper && slugConfiguration.isShowInStore && !GetIsShown(attackSlug.Type.ToString()))
				{
					return true;
				}
			}
			foreach (SlugItem defenceSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.DefenceSlugs)
			{
				SlugConfig slugConfiguration2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(defenceSlug.Type);
				if ((defenceSlug.IsBought || !defenceSlug.IsLocked) && slugConfiguration2.isShowInStore && !GetIsShown(defenceSlug.Type.ToString()))
				{
					return true;
				}
			}
			foreach (SlugItem specialSlug in MonoSingleton<AppController>.Instance.Model.ShopModel.SpecialSlugs)
			{
				SlugConfig slugConfiguration3 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(specialSlug.Type);
				if ((specialSlug.IsBought || !specialSlug.IsLocked) && slugConfiguration3.isShowInStore && !GetIsShown(specialSlug.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskAssistantsBadges()
		{
			foreach (AssistantItem assistant in MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants)
			{
				HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistant.Type);
				if ((!assistant.IsLocked || assistant.IsBought) && alliesConfig.isShowInStore && !GetIsShown(assistant.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskArmorBadges()
		{
			foreach (ArmorItem armor in MonoSingleton<AppController>.Instance.Model.ShopModel.Armors)
			{
				ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armor.Type);
				if ((!armor.IsLocked || armor.IsBought) && armorConfiguration.isShowInStore && !GetIsShown(armor.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private bool CheskBlastersBadges()
		{
			foreach (BlasterItem blaster in MonoSingleton<AppController>.Instance.Model.ShopModel.Blasters)
			{
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blaster.Type);
				if ((!blaster.IsLocked || blaster.IsBought) && blusterConfiguration.isShowInStore && !GetIsShown(blaster.Type.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		private void ShowPopUpSurvivalAvailability()
		{
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed && !PlayerPrefsSystem.HasKey("survivalAvailable"))
			{
				PopupSurvivalAvailabilityView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupSurvivalAvailabilityView>();
				if (registeredPopup != null)
				{
					registeredPopup.gameObject.SetActive(true);
					PlayerPrefs.SetString("survivalAvailable", "available");
				}
			}
		}

		private void ShowPopupMultiplayerAvailability()
		{
			PopupMultiplayerAvailabilityView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupMultiplayerAvailabilityView>();
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_3)).Status == LevelStatus.Completed && !PlayerPrefsSystem.HasKey("multiplayeravailabilitymessageshowed"))
			{
				PlayerPrefs.SetInt("multiplayeravailabilitymessageshowed", 0);
				registeredPopup.ShowAvalablePopUp();
			}
		}

		private void OnDisable()
		{
			LevelItemController.OnSelectlevelItem = (Action<LevelItemController>)Delegate.Remove(LevelItemController.OnSelectlevelItem, new Action<LevelItemController>(SelectLevelItem));
			if (DonDestroyMonoSingleton<DownloadManager>.IsAlive)
			{
				DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
				instance.finishedBundleLoading = (Action<string>)Delegate.Remove(instance.finishedBundleLoading, new Action<string>(LoadLevelAfterBundleDownload));
			}
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnEquipClick()
		{
			_targetNeededAsset = string.Empty;
			if (LevelItemController.SelectedLevel == null)
			{
				UnityEngine.Debug.LogWarning("MapMenuView::OnBtnEquipClick [Obsolete] selectedLevel is NULL");
				return;
			}
			Type scene = LevelItemController.SelectedLevel.Model.Scene;
			if (scene == typeof(ShaneTutor_0_1) || scene == typeof(QuietLawn_2_1) || scene == typeof(QuietLawn_2_2))
			{
				if (OnPlayEvent__BackingField != null)
				{
					OnPlayEvent__BackingField();
				}
			}
			else if (OnEquipEvent__BackingField != null)
			{
				OnEquipEvent__BackingField();
			}
		}

		private void OnBtnBackpackClick()
		{
			_targetNeededAsset = string.Empty;
			if (LevelItemController.SelectedLevel == null)
			{
				UnityEngine.Debug.LogWarning("MapMenuView::OnBtnBackpackClick [Obsolete] selectedLevel is NULL");
				return;
			}
			Type scene = LevelItemController.SelectedLevel.Model.Scene;
			if (scene == typeof(ShaneTutor_0_1) || scene == typeof(QuietLawn_2_1) || scene == typeof(QuietLawn_2_2))
			{
				if (OnPlayEvent__BackingField != null)
				{
					OnPlayEvent__BackingField();
				}
			}
			else if (OnBackpackEvent__BackingField != null)
			{
				OnBackpackEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnGameServiceEvent__BackingField != null)
			{
				OnGameServiceEvent__BackingField();
			}
		}

		private void SelectLevelItem(LevelItemController item)
		{
			if (item != null)
			{
				MonoSingleton<AppController>.Instance.Model.MissionSetting.ClearOfferedItems();
				Type scene = item.Model.Scene;
				CheckAndDownloadLevel(scene.Name);
				if (_FxChosenItem == null)
				{
					_FxChosenItem = UnityEngine.Object.Instantiate(_levelItemFXPrefab);
				}
				Vector3 position = item.transform.position;
				position.y = 0.8f;
				_FxChosenItem.transform.position = position;
				if (_dailyScaler != null)
				{
					if (item.Model.IsDaily())
					{
						_dailyScaler.enabled = false;
						_dailyScaler.Transform.localScale = _dailyScaler.BaseScale;
					}
					else
					{
						_dailyScaler.enabled = true;
					}
				}
				_nextBtnLabel.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_next");
				_nextBtnLabelBack.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_arsenal");
				Type scene2 = item.Model.Scene;
				if (scene2 == typeof(ShaneTutor_0_1) || scene2 == typeof(QuietLawn_2_1) || scene2 == typeof(QuietLawn_2_2))
				{
					_nextBtnLabel.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_start");
					_nextBtnLabelBack.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_start");
					_btnArsenal.SetActive(false);
				}
				else
				{
					_btnArsenal.SetActive(true);
				}
			}
			else
			{
				_swapArsenalButtonState.Swap();
			}
		}

		private void LoadLevelAfterBundleDownload(string bundleName)
		{
			if (_targetNeededAsset == bundleName && loadAfterBundleDownloading)
			{
				loadAfterBundleDownloading = false;
				if (OnBackpackEvent__BackingField != null)
				{
					OnBackpackEvent__BackingField();
				}
			}
		}

		private void UpdateArsenalButtonState(string bundleName)
		{
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			InputWindowView component = parentGameObject.GetComponent<InputWindowView>();
			int num = 0;
			if (model is List<LevelItem>)
			{
				parentGameObject.transform.position = Vector3.zero;
				List<LevelItem> list = model as List<LevelItem>;
				_levelItems.Clear();
				LevelItem levelItem = list.Find((LevelItem level) => level.Order == LevelItem.Current && level.Type == LevelType.Action);
				if (levelItem == null)
				{
					levelItem = list.Find((LevelItem level) => level.Order == LevelItem.Current);
				}
				Transform transform = null;
				foreach (LevelItemPlaceHolderView placeHolder in _placeHolders)
				{
					if (placeHolder.LevelName == levelItem.Scene.Name)
					{
						transform = placeHolder.transform;
					}
				}
				bool flag = false;
				InputWindowView prev = null;
				InputWindowView inputWindowView = null;
				InputWindowStruct inputWindowStruct = default(InputWindowStruct);
				InputWindowStruct value = default(InputWindowStruct);
				LevelItem levelItem2 = null;
				LevelItem levelItem3 = null;
				foreach (LevelItem item in list)
				{
					string text = item.Scene.Name;
					foreach (LevelItemPlaceHolderView placeHolder2 in _placeHolders)
					{
						if (!(placeHolder2.LevelName == text))
						{
							continue;
						}
						GameObject gameObject = null;
						gameObject = UnityEngine.Object.Instantiate(_levelItemPrefab);
						gameObject.name += num;
						if (!(gameObject != null))
						{
							continue;
						}
						InputWindowView component2 = gameObject.GetComponent<InputWindowView>();
						if (item.Type == LevelType.Arena)
						{
							inputWindowStruct.arena = component2;
							value = inputWindowStruct;
							levelItem3 = levelItem2;
						}
						if (levelItem2 != null && levelItem2.Type != LevelType.Arena)
						{
							prev = inputWindowView;
							inputWindowView = (inputWindowStruct.next = component2);
							inputWindowStruct.nextLevelItem = item;
							if (levelItem2 != null && _levelItemsWindowViews.ContainsKey(levelItem2))
							{
								_levelItemsWindowViews[levelItem2] = inputWindowStruct;
							}
						}
						else
						{
							inputWindowView = (value.next = component2);
							value.nextLevelItem = item;
							if (levelItem3 != null && _levelItemsWindowViews.ContainsKey(levelItem3))
							{
								_levelItemsWindowViews[levelItem3] = value;
							}
						}
						inputWindowStruct = new InputWindowStruct(prev, inputWindowView, null, num);
						_levelItemsWindowViews.Add(item, inputWindowStruct);
						levelItem2 = item;
						num--;
						gameObject.transform.parent = parentGameObject.transform;
						LevelItemController component3 = gameObject.GetComponent<LevelItemController>();
						component3.View.LevelButton.viewCamera = _levelItemsCamera;
						component3.ApplyModel(item);
						placeHolder2.LevelItemController = component3;
						_levelItems.Add(component3);
						if (placeHolder2.LevelName.Contains("Daily"))
						{
							gameObject.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
						}
					}
					if (DonDestroyMonoSingleton<CheckInternetService>.Instance.IsInternetAvailable)
					{
						CheckAndDownloadLevel(levelItem.Scene.Name);
					}
				}
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("ScorchedSparks_1_1");
				LevelItem levelItemByLevelName2 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("ScorchedSparks_1_2");
				LevelItem levelItemByLevelName3 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("ScorchedSparks_1_3");
				LevelItem levelItemByLevelName4 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("ScorchedSparks_1_4");
				LevelItem levelItemByLevelName5 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("Arena_1_5");
				LevelItem levelItemByLevelName6 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_01_Assault_01");
				LevelItem levelItemByLevelName7 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_02_Defence");
				LevelItem levelItemByLevelName8 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_04_Escort");
				LevelItem levelItemByLevelName9 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_01_Defence_01");
				LevelItem levelItemByLevelName10 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_02_Escort");
				LevelItem levelItemByLevelName11 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_03_Assault_02");
				LevelItem levelItemByLevelName12 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_01_Escort_01");
				LevelItem levelItemByLevelName13 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_02_Assault");
				LevelItem levelItemByLevelName14 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_03_Defence");
				LevelItem levelItemByLevelName15 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_04_Assault");
				LevelItem levelItemByLevelName16 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_03_Assault_01");
				LevelItem levelItemByLevelName17 = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName("DailyMission_04_Defence");
				InputWindowStruct inputWindowStruct2 = _levelItemsWindowViews[levelItemByLevelName];
				InputWindowStruct inputWindowStruct3 = _levelItemsWindowViews[levelItemByLevelName2];
				InputWindowStruct inputWindowStruct4 = _levelItemsWindowViews[levelItemByLevelName3];
				InputWindowStruct inputWindowStruct5 = _levelItemsWindowViews[levelItemByLevelName4];
				InputWindowStruct inputWindowStruct6 = _levelItemsWindowViews[levelItemByLevelName5];
				InputWindowStruct inputWindowStruct7 = _levelItemsWindowViews[levelItemByLevelName6];
				InputWindowStruct inputWindowStruct8 = _levelItemsWindowViews[levelItemByLevelName7];
				InputWindowStruct inputWindowStruct9 = _levelItemsWindowViews[levelItemByLevelName8];
				InputWindowStruct inputWindowStruct10 = _levelItemsWindowViews[levelItemByLevelName9];
				InputWindowStruct inputWindowStruct11 = _levelItemsWindowViews[levelItemByLevelName10];
				InputWindowStruct inputWindowStruct12 = _levelItemsWindowViews[levelItemByLevelName11];
				InputWindowStruct inputWindowStruct13 = _levelItemsWindowViews[levelItemByLevelName12];
				InputWindowStruct inputWindowStruct14 = _levelItemsWindowViews[levelItemByLevelName13];
				InputWindowStruct inputWindowStruct15 = _levelItemsWindowViews[levelItemByLevelName14];
				InputWindowStruct inputWindowStruct16 = _levelItemsWindowViews[levelItemByLevelName15];
				InputWindowStruct inputWindowStruct17 = _levelItemsWindowViews[levelItemByLevelName16];
				InputWindowStruct inputWindowStruct18 = _levelItemsWindowViews[levelItemByLevelName17];
				InputWindowStruct inputWindowStruct19 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct20 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct21 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct22 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct23 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct24 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct25 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct26 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct27 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct28 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct29 = default(InputWindowStruct);
				InputWindowStruct inputWindowStruct30 = default(InputWindowStruct);
				bool flag2 = false;
				foreach (KeyValuePair<LevelItem, InputWindowStruct> levelItemsWindowView in _levelItemsWindowViews)
				{
					levelItemsWindowView.Value.cur.Priority = levelItemsWindowView.Value.order;
					switch (levelItemsWindowView.Key.Name)
					{
					case "1.1":
						if (levelItemsWindowView.Key.Type != LevelType.Arena)
						{
							List<InputEvent> list26 = new List<InputEvent>();
							list26.Add(InputEvent.Right);
							list26.Add(InputEvent.Right);
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(levelItemsWindowView.Value, list26));
							list26.Clear();
							list26.Add(InputEvent.Left);
							InputWindowTransition transition = new InputWindowTransition(component, list26);
							levelItemsWindowView.Value.cur.AddTransition(transition);
						}
						else
						{
							List<InputEvent> list27 = new List<InputEvent>();
							list27.Add(InputEvent.Left);
							list27.Add(InputEvent.Left);
							InputWindowStruct value25 = levelItemsWindowView.Value;
							value25.prev = inputWindowStruct19.cur;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value25, list27));
						}
						break;
					case "2.1":
						if (!flag2)
						{
							List<InputEvent> list24 = new List<InputEvent>();
							list24.Add(InputEvent.Left);
							list24.Add(InputEvent.Down);
							InputWindowStruct value23 = levelItemsWindowView.Value;
							if (!value23.NextItemIsLocked())
							{
								value23.next = null;
								value23.order = 2;
								flag2 = true;
							}
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value23, list24));
							levelItemsWindowView.Value.cur.Priority = value23.order;
						}
						break;
					case "2.2":
						if (!flag2)
						{
							List<InputEvent> list35 = new List<InputEvent>();
							list35.Add(InputEvent.Up);
							list35.Add(InputEvent.Right);
							list35.Add(InputEvent.Left);
							InputWindowStruct value33 = levelItemsWindowView.Value;
							if (!value33.NextItemIsLocked())
							{
								value33.next = null;
								value33.order = 2;
								flag2 = true;
							}
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(levelItemsWindowView.Value, list35));
							levelItemsWindowView.Value.cur.Priority = value33.order;
						}
						break;
					case "1.1T":
					{
						List<InputEvent> list21 = new List<InputEvent>();
						list21.Add(InputEvent.Right);
						list21.Add(InputEvent.Right);
						levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(levelItemsWindowView.Value, list21));
						break;
					}
					case "2.3":
						if (!flag2)
						{
							List<InputEvent> list14 = new List<InputEvent>();
							list14.Add(InputEvent.Left);
							list14.Add(InputEvent.Up);
							list14.Add(InputEvent.Right);
							InputWindowStruct value14 = levelItemsWindowView.Value;
							if (!value14.NextItemIsLocked())
							{
								value14.next = null;
								value14.order = 2;
								flag2 = true;
							}
							else
							{
								value14.arena = inputWindowStruct2.cur;
							}
							inputWindowStruct19 = levelItemsWindowView.Value;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value14, list14));
							levelItemsWindowView.Value.cur.Priority = value14.order;
						}
						break;
					case "2.4":
						if (!flag2)
						{
							List<InputEvent> list31 = new List<InputEvent>();
							list31.Add(InputEvent.Down);
							list31.Add(InputEvent.Right);
							list31.Add(InputEvent.Left);
							InputWindowStruct value29 = levelItemsWindowView.Value;
							value29.arena = null;
							if (!value29.NextItemIsLocked())
							{
								value29.next = null;
								value29.order = 2;
								flag2 = true;
							}
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value29, list31));
							levelItemsWindowView.Value.cur.Priority = value29.order;
						}
						break;
					case "2.5":
						if (!flag2)
						{
							List<InputEvent> list15 = new List<InputEvent>();
							list15.Add(InputEvent.Left);
							list15.Add(InputEvent.Down);
							list15.Add(InputEvent.Right);
							InputWindowStruct value15 = levelItemsWindowView.Value;
							value15.arena = null;
							if (!value15.NextItemIsLocked())
							{
								value15.next = null;
								value15.order = 2;
								flag2 = true;
							}
							else
							{
								value15.arena = inputWindowStruct3.cur;
							}
							inputWindowStruct20 = value15;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value15, list15));
							levelItemsWindowView.Value.cur.Priority = value15.order;
						}
						break;
					case "1.2":
					{
						List<InputEvent> list33 = new List<InputEvent>();
						list33.Add(InputEvent.Left);
						list33.Add(InputEvent.Left);
						InputWindowStruct value31 = levelItemsWindowView.Value;
						value31.prev = inputWindowStruct20.cur;
						levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value31, list33));
						break;
					}
					case "3.1":
						if (!flag2)
						{
							List<InputEvent> list36 = new List<InputEvent>();
							list36.Add(InputEvent.Up);
							list36.Add(InputEvent.Right);
							list36.Add(InputEvent.Down);
							list36.Add(InputEvent.Down);
							InputWindowStruct value34 = levelItemsWindowView.Value;
							value34.arena = null;
							if (!value34.NextItemIsLocked())
							{
								value34.next = null;
								value34.order = 2;
								flag2 = true;
							}
							else
							{
								value34.daily = inputWindowStruct7.cur;
							}
							inputWindowStruct21 = value34;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value34, list36));
							levelItemsWindowView.Value.cur.Priority = value34.order;
						}
						break;
					case "3.2":
						if (!flag2)
						{
							List<InputEvent> list30 = new List<InputEvent>();
							list30.Add(InputEvent.Left);
							list30.Add(InputEvent.Up);
							list30.Add(InputEvent.Down);
							list30.Add(InputEvent.Right);
							InputWindowStruct value28 = levelItemsWindowView.Value;
							value28.arena = null;
							if (!value28.NextItemIsLocked())
							{
								value28.next = null;
								value28.order = 2;
								flag2 = true;
							}
							else
							{
								value28.daily = inputWindowStruct8.cur;
							}
							inputWindowStruct22 = value28;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value28, list30));
							levelItemsWindowView.Value.cur.Priority = value28.order;
						}
						break;
					case "3.3":
						if (!flag2)
						{
							List<InputEvent> list28 = new List<InputEvent>();
							list28.Add(InputEvent.Down);
							list28.Add(InputEvent.Right);
							list28.Add(InputEvent.Left);
							list28.Add(InputEvent.Up);
							InputWindowStruct value26 = levelItemsWindowView.Value;
							value26.arena = null;
							if (!value26.NextItemIsLocked())
							{
								value26.next = null;
								value26.order = 2;
								flag2 = true;
							}
							else
							{
								value26.arena = inputWindowStruct4.cur;
								value26.daily = inputWindowStruct9.cur;
							}
							inputWindowStruct23 = value26;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value26, list28));
							levelItemsWindowView.Value.cur.Priority = value26.order;
						}
						break;
					case "1.3":
					{
						List<InputEvent> list22 = new List<InputEvent>();
						list22.Add(InputEvent.Right);
						list22.Add(InputEvent.Right);
						InputWindowStruct value21 = levelItemsWindowView.Value;
						value21.prev = inputWindowStruct23.cur;
						levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value21, list22));
						break;
					}
					case "4.1":
						if (!flag2)
						{
							List<InputEvent> list32 = new List<InputEvent>();
							list32.Add(InputEvent.Left);
							list32.Add(InputEvent.Right);
							list32.Add(InputEvent.Up);
							list32.Add(InputEvent.Down);
							list32.Add(InputEvent.Up);
							InputWindowStruct value30 = levelItemsWindowView.Value;
							value30.arena = null;
							if (!value30.NextItemIsLocked())
							{
								value30.next = null;
								value30.order = 2;
								flag2 = true;
							}
							else
							{
								value30.daily = inputWindowStruct10.cur;
								value30.daily1 = inputWindowStruct17.cur;
							}
							inputWindowStruct24 = value30;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value30, list32));
							levelItemsWindowView.Value.cur.Priority = value30.order;
						}
						break;
					case "4.2":
						if (!flag2)
						{
							List<InputEvent> list25 = new List<InputEvent>();
							list25.Add(InputEvent.Left);
							list25.Add(InputEvent.Right);
							list25.Add(InputEvent.Up);
							list25.Add(InputEvent.Up);
							InputWindowStruct value24 = levelItemsWindowView.Value;
							value24.arena = null;
							if (!value24.NextItemIsLocked())
							{
								value24.next = null;
								value24.order = 2;
								flag2 = true;
							}
							else
							{
								value24.daily = inputWindowStruct11.cur;
							}
							inputWindowStruct25 = value24;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value24, list25));
							levelItemsWindowView.Value.cur.Priority = value24.order;
						}
						break;
					case "4.3":
						if (!flag2)
						{
							List<InputEvent> list20 = new List<InputEvent>();
							list20.Add(InputEvent.Left);
							list20.Add(InputEvent.Right);
							list20.Add(InputEvent.Down);
							list20.Add(InputEvent.Up);
							InputWindowStruct value20 = levelItemsWindowView.Value;
							value20.arena = null;
							if (!value20.NextItemIsLocked())
							{
								value20.next = null;
								value20.order = 2;
								flag2 = true;
							}
							else
							{
								value20.arena = inputWindowStruct5.cur;
								value20.daily = inputWindowStruct12.cur;
							}
							inputWindowStruct26 = value20;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value20, list20));
							levelItemsWindowView.Value.cur.Priority = value20.order;
						}
						break;
					case "1.4":
					{
						List<InputEvent> list17 = new List<InputEvent>();
						list17.Add(InputEvent.Up);
						list17.Add(InputEvent.Up);
						InputWindowStruct value17 = levelItemsWindowView.Value;
						value17.prev = inputWindowStruct26.cur;
						levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value17, list17));
						break;
					}
					case "5.1":
						if (!flag2)
						{
							List<InputEvent> list34 = new List<InputEvent>();
							list34.Add(InputEvent.Left);
							list34.Add(InputEvent.Right);
							list34.Add(InputEvent.Down);
							list34.Add(InputEvent.Up);
							list34.Add(InputEvent.Down);
							InputWindowStruct value32 = levelItemsWindowView.Value;
							value32.arena = null;
							if (!value32.NextItemIsLocked())
							{
								value32.next = null;
								value32.order = 2;
								flag2 = true;
							}
							else
							{
								value32.daily = inputWindowStruct13.cur;
								value32.daily1 = inputWindowStruct18.cur;
							}
							inputWindowStruct27 = value32;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value32, list34));
							levelItemsWindowView.Value.cur.Priority = value32.order;
						}
						break;
					case "5.2":
						if (!flag2)
						{
							List<InputEvent> list29 = new List<InputEvent>();
							list29.Add(InputEvent.Left);
							list29.Add(InputEvent.Right);
							list29.Add(InputEvent.Down);
							list29.Add(InputEvent.Up);
							list29.Add(InputEvent.Down);
							InputWindowStruct value27 = levelItemsWindowView.Value;
							value27.arena = null;
							if (!value27.NextItemIsLocked())
							{
								value27.next = null;
								value27.order = 2;
								flag2 = true;
							}
							else
							{
								value27.daily = inputWindowStruct14.cur;
								value27.daily1 = inputWindowStruct16.cur;
							}
							inputWindowStruct28 = value27;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value27, list29));
							levelItemsWindowView.Value.cur.Priority = value27.order;
						}
						break;
					case "6.1":
						if (!flag2)
						{
							List<InputEvent> list23 = new List<InputEvent>();
							list23.Add(InputEvent.Left);
							list23.Add(InputEvent.Down);
							list23.Add(InputEvent.Down);
							list23.Add(InputEvent.Up);
							InputWindowStruct value22 = levelItemsWindowView.Value;
							value22.arena = null;
							if (!value22.NextItemIsLocked())
							{
								value22.next = null;
								value22.order = 2;
								flag2 = true;
							}
							else
							{
								value22.daily = inputWindowStruct15.cur;
							}
							inputWindowStruct29 = value22;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value22, list23));
							levelItemsWindowView.Value.cur.Priority = value22.order;
						}
						break;
					case "6.2":
						if (!flag2)
						{
							List<InputEvent> list19 = new List<InputEvent>();
							list19.Add(InputEvent.Up);
							list19.Add(InputEvent.Right);
							list19.Add(InputEvent.Down);
							InputWindowStruct value19 = levelItemsWindowView.Value;
							value19.arena = null;
							if (!value19.NextItemIsLocked())
							{
								value19.next = null;
								value19.order = 2;
								flag2 = true;
							}
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value19, list19));
							levelItemsWindowView.Value.cur.Priority = value19.order;
						}
						break;
					case "1.5":
					{
						List<InputEvent> list18 = new List<InputEvent>();
						list18.Add(InputEvent.Down);
						list18.Add(InputEvent.Down);
						InputWindowStruct value18 = levelItemsWindowView.Value;
						value18.prev = inputWindowStruct30.cur;
						levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value18, list18));
						break;
					}
					case "6.3":
						if (!flag2)
						{
							List<InputEvent> list16 = new List<InputEvent>();
							list16.Add(InputEvent.Left);
							list16.Add(InputEvent.Right);
							list16.Add(InputEvent.Up);
							InputWindowStruct value16 = levelItemsWindowView.Value;
							value16.arena = null;
							value16.next = null;
							value16.order = 2;
							inputWindowStruct30 = value16;
							value16.arena = inputWindowStruct6.cur;
							levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value16, list16));
							levelItemsWindowView.Value.cur.Priority = value16.order;
						}
						break;
					case "daily":
						if (levelItemsWindowView.Key.Status != LevelStatus.Locked)
						{
							if (levelItemsWindowView.Key.Scene.Name == "DailyMission_01_Assault_01")
							{
								List<InputEvent> list2 = new List<InputEvent>();
								list2.Add(InputEvent.Up);
								InputWindowStruct value2 = levelItemsWindowView.Value;
								value2.arena = null;
								value2.next = null;
								value2.prev = inputWindowStruct21.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value2, list2));
								levelItemsWindowView.Value.cur.Priority = value2.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_02_Defence")
							{
								List<InputEvent> list3 = new List<InputEvent>();
								list3.Add(InputEvent.Left);
								InputWindowStruct value3 = levelItemsWindowView.Value;
								value3.arena = null;
								value3.next = null;
								value3.prev = inputWindowStruct22.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value3, list3));
								levelItemsWindowView.Value.cur.Priority = value3.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_04_Escort")
							{
								List<InputEvent> list4 = new List<InputEvent>();
								list4.Add(InputEvent.Down);
								InputWindowStruct value4 = levelItemsWindowView.Value;
								value4.arena = null;
								value4.next = null;
								value4.prev = inputWindowStruct23.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value4, list4));
								levelItemsWindowView.Value.cur.Priority = value4.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_01_Defence_01")
							{
								List<InputEvent> list5 = new List<InputEvent>();
								list5.Add(InputEvent.Up);
								InputWindowStruct value5 = levelItemsWindowView.Value;
								value5.arena = null;
								value5.next = null;
								value5.prev = inputWindowStruct24.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value5, list5));
								levelItemsWindowView.Value.cur.Priority = value5.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_02_Escort")
							{
								List<InputEvent> list6 = new List<InputEvent>();
								list6.Add(InputEvent.Down);
								InputWindowStruct value6 = levelItemsWindowView.Value;
								value6.arena = null;
								value6.next = null;
								value6.prev = inputWindowStruct25.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value6, list6));
								levelItemsWindowView.Value.cur.Priority = value6.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_03_Assault_02")
							{
								List<InputEvent> list7 = new List<InputEvent>();
								list7.Add(InputEvent.Down);
								InputWindowStruct value7 = levelItemsWindowView.Value;
								value7.arena = null;
								value7.next = null;
								value7.prev = inputWindowStruct26.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value7, list7));
								levelItemsWindowView.Value.cur.Priority = value7.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_02_Assault")
							{
								List<InputEvent> list8 = new List<InputEvent>();
								list8.Add(InputEvent.Down);
								InputWindowStruct value8 = levelItemsWindowView.Value;
								value8.arena = null;
								value8.next = null;
								value8.prev = inputWindowStruct28.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value8, list8));
								levelItemsWindowView.Value.cur.Priority = value8.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_01_Escort_01")
							{
								List<InputEvent> list9 = new List<InputEvent>();
								list9.Add(InputEvent.Down);
								InputWindowStruct value9 = levelItemsWindowView.Value;
								value9.arena = null;
								value9.next = null;
								value9.prev = inputWindowStruct27.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value9, list9));
								levelItemsWindowView.Value.cur.Priority = value9.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_03_Defence")
							{
								List<InputEvent> list10 = new List<InputEvent>();
								list10.Add(InputEvent.Down);
								InputWindowStruct value10 = levelItemsWindowView.Value;
								value10.arena = null;
								value10.next = null;
								value10.prev = inputWindowStruct29.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value10, list10));
								levelItemsWindowView.Value.cur.Priority = value10.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_04_Assault")
							{
								List<InputEvent> list11 = new List<InputEvent>();
								list11.Add(InputEvent.Up);
								InputWindowStruct value11 = levelItemsWindowView.Value;
								value11.arena = null;
								value11.next = null;
								value11.prev = inputWindowStruct28.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value11, list11));
								levelItemsWindowView.Value.cur.Priority = value11.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_04_Defence")
							{
								List<InputEvent> list12 = new List<InputEvent>();
								list12.Add(InputEvent.Up);
								InputWindowStruct value12 = levelItemsWindowView.Value;
								value12.arena = null;
								value12.next = null;
								value12.prev = inputWindowStruct27.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value12, list12));
								levelItemsWindowView.Value.cur.Priority = value12.order;
							}
							else if (levelItemsWindowView.Key.Scene.Name == "DailyMission_03_Assault_01")
							{
								List<InputEvent> list13 = new List<InputEvent>();
								list13.Add(InputEvent.Down);
								InputWindowStruct value13 = levelItemsWindowView.Value;
								value13.arena = null;
								value13.next = null;
								value13.prev = inputWindowStruct24.cur;
								levelItemsWindowView.Value.cur.AddTransition(CreateTransitions(value13, list13));
								levelItemsWindowView.Value.cur.Priority = value13.order;
							}
						}
						break;
					}
				}
				SelectLevelItem(LevelItemController.SelectedLevel);
				if (LevelItemController.LastPlayedLevel != null)
				{
					LevelItemController.LastPlayedLevel.HighlightStars();
				}
			}
			if (OnLevelItemCreatedEvent__BackingField != null)
			{
				OnLevelItemCreatedEvent__BackingField();
			}
		}

		private List<InputWindowTransition> CreateTransitions(InputWindowStruct windowStruct, List<InputEvent> events)
		{
			List<InputWindowTransition> list = new List<InputWindowTransition>();
			if (windowStruct.prev != null)
			{
				List<InputEvent> list2 = new List<InputEvent>();
				list2.Add(events[0]);
				list.Add(new InputWindowTransition(windowStruct.prev, list2));
			}
			if (windowStruct.next != null)
			{
				List<InputEvent> list3 = new List<InputEvent>();
				list3.Add(events[1]);
				list.Add(new InputWindowTransition(windowStruct.next, list3));
			}
			if (windowStruct.arena != null)
			{
				List<InputEvent> list4 = new List<InputEvent>();
				list4.Add(events[2]);
				list.Add(new InputWindowTransition(windowStruct.arena, list4));
			}
			if (windowStruct.daily != null)
			{
				List<InputEvent> list5 = new List<InputEvent>();
				list5.Add(events[3]);
				list.Add(new InputWindowTransition(windowStruct.daily, list5));
			}
			if (windowStruct.daily1 != null)
			{
				List<InputEvent> list6 = new List<InputEvent>();
				list6.Add(events[4]);
				list.Add(new InputWindowTransition(windowStruct.daily1, list6));
			}
			return list;
		}

		private void CheckAndDownloadLevel(string levelName)
		{
		}

		private void SetIsShown(string name)
		{
			PlayerPrefs.SetInt(name + "IsShown", 1);
		}

		private bool GetIsShown(string name)
		{
			return PlayerPrefs.GetInt(name + "IsShown", 0) == 1;
		}
	}
}
