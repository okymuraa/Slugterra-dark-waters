using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class ResultMultiplayerView : UIView
	{
		[Serializable]
		public struct ChangePlatformObjects
		{
			public GameObject _objectTVOS;

			public GameObject _objectDefault;

			public ChangePlatformObjects(GameObject objectTVOS, GameObject objectDefault)
			{
				_objectTVOS = objectTVOS;
				_objectDefault = objectDefault;
			}

			public void SetActiveObjects()
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

			public void SetDisableObjects()
			{
				_objectTVOS.SetActive(false);
				_objectDefault.SetActive(false);
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSharePhotoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShareVideoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHelpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnLeaderboardEvent__BackingField;

		[SerializeField]
		public GameObject _saleInStore;

		public GameSetting _settings;

		public TextMesh _playerNickname;

		public TextMesh _rivalNickname;

		public TextMesh _playerCurRank;

		public TextMesh _playerRivalCurRank;

		public TextMesh _playerTextRank;

		public tk2dButton _btnNext;

		private string _playerNickNameText;

		private string _rivalNickNameText;

		private float _prayerRank;

		private float _rivalRank;

		[SerializeField]
		public GameObject _btnShareVideo;

		[SerializeField]
		public GameObject _btnFbShare;

		[SerializeField]
		public GameObject _btnLeaderBoard;

		[SerializeField]
		public MissionRewards _commonRewards;

		[SerializeField]
		public MultiplayerStatisticsController _statisticController;

		public PopupConfirmView _popupConfirm;

		public bool doubleCheckForReplayKit;

		private GameController _gameController;

		[SerializeField]
		public ChangePlatformObjects _btns;

		public string PlayerNickname
		{
			get
			{
				return _playerNickNameText;
			}
			set
			{
				_playerNickNameText = value;
				_playerNickname.text = _playerNickNameText;
			}
		}

		public string RivalNickName
		{
			get
			{
				return _rivalNickNameText;
			}
			set
			{
				_rivalNickNameText = value;
				_rivalNickname.text = _rivalNickNameText;
			}
		}

		public float PlayerRank
		{
			get
			{
				return _prayerRank;
			}
			set
			{
				_prayerRank = value;
			}
		}

		public float RivalRank
		{
			get
			{
				return _rivalRank;
			}
			set
			{
				_rivalRank = value;
				_playerRivalCurRank.text = _rivalRank.ToString("####0");
			}
		}

		public event Action OnSharePhotoEvent
		{
			add
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShareVideoEvent
		{
			add
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextEvent
		{
			add
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnHelpEvent
		{
			add
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnLeaderboardEvent
		{
			add
			{
				Action action = OnBtnLeaderboardEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnLeaderboardEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnLeaderboardEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnLeaderboardEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		}

		protected virtual void OnEnable()
		{
			SetPlayerNickname();
			SetPlayerRank();
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			StartCoroutine(UpdateViews());
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			if (_gameController != null && (_gameController.Model.GameWon || _gameController.Model.GameFailed))
			{
				SetButtonsState(false);
				_btnLeaderBoard.SetActive(true);
			}
		}

		protected virtual void OnDisable()
		{
			SetPlayerNickname();
			_btnLeaderBoard.SetActive(false);
		}

		private void SetPlayerNickname()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
			{
				string playerAlias = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerAlias;
				PlayerNickname = playerAlias;
			}
		}

		public virtual void SetPlayerRank()
		{
		}

		protected void OnBtnSharePhotoClick()
		{
			if (OnSharePhotoEvent__BackingField != null)
			{
				OnSharePhotoEvent__BackingField();
			}
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
			UnityEngine.Debug.Log("UpdateViews");
		}

		protected void OnBtnShareVideoClick()
		{
			if (OnShareVideoEvent__BackingField != null)
			{
				OnShareVideoEvent__BackingField();
			}
		}

		protected void OnBtnLeaderboardClick()
		{
			if (OnBtnLeaderboardEvent__BackingField != null)
			{
				OnBtnLeaderboardEvent__BackingField();
			}
		}

		private void OnBtnNextClick()
		{
			if (OnNextEvent__BackingField != null)
			{
				OnNextEvent__BackingField();
			}
		}

		private void OnBtnHelpClick()
		{
			if (OnHelpEvent__BackingField != null)
			{
				OnHelpEvent__BackingField();
			}
		}

		public void SetButtonsState(bool isShareVideoEnable)
		{
			_btnFbShare.SetActive(!isShareVideoEnable);
			_btnShareVideo.SetActive(isShareVideoEnable);
		}
	}
}
