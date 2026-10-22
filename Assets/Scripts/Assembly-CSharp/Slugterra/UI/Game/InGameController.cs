using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Duel;
using Slugterra.Scene.Game;
using Slugterra.UI.Wallet;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class InGameController : UIController<InGameView>, IObserver
	{
		[SerializeField]
		private AvatarInfoController _mainAvatarInfo;

		[SerializeField]
		private AvatarInfoController _assistAvatarInfo;

		[SerializeField]
		private AvatarInfoController _bossAvatarInfo;

		[SerializeField]
		private EnemyAvatarInfoController _netPlayerAvatarInfo;

		[SerializeField]
		private WalletController _walletController;

		[SerializeField]
		private WalletController _walletControllerTVOS;

		[SerializeField]
		private SlugBarController _slugBarControllerAction;

		[SerializeField]
		private SlugBarController _slugBarControllerArena;

		private InventoryBarController _inventoryBarController;

		private GameController gameController;

		private InGameModel _model;

		public AvatarInfoController MainAvatarInfo
		{
			get
			{
				return _mainAvatarInfo;
			}
		}

		public AvatarInfoController AssistAvatarInfo
		{
			get
			{
				return _assistAvatarInfo;
			}
		}

		public AvatarInfoController BossAvatarInfo
		{
			get
			{
				return _bossAvatarInfo;
			}
		}

		public EnemyAvatarInfoController NetPlayerAvatarInfo
		{
			get
			{
				return _netPlayerAvatarInfo;
			}
		}

		public SlugBarController SlugBarControllerAction
		{
			get
			{
				return _slugBarControllerAction;
			}
		}

		public bool IsActiveAssistantAvatar
		{
			get
			{
				return _assistAvatarInfo.gameObject.activeSelf;
			}
		}

		public InGameModel Model
		{
			get
			{
				return _model;
			}
		}

		protected override void OnCreate()
		{
		}

		public void OnObjectChanged(object observable)
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (gameController == null)
			{
				gameController = Object.FindObjectOfType<GameController>();
			}
			if (!(observable is InGameModel))
			{
				return;
			}
			View.ApplyModel(_model);
			InGameModel model = _model;
			if (_model.LevelType == LevelType.Action)
			{
				_slugBarControllerArena.gameObject.SetActive(false);
				_slugBarControllerAction.gameObject.SetActive(true);
				_slugBarControllerAction.OnApplyModel(_model);
				if (currentGameLevel.Scene != typeof(ShaneTutor_0_1))
				{
					_inventoryBarController = View.InventoryBarControllerAction.GetComponent<InventoryBarController>();
					_inventoryBarController.gameObject.SetActive(true);
				}
				if (currentGameLevel.Scene == typeof(QuietLawn_2_2))
				{
					_slugBarControllerArena.gameObject.SetActive(false);
					_slugBarControllerAction.gameObject.SetActive(false);
				}
			}
			else
			{
				if (_slugBarControllerAction != null)
				{
					_slugBarControllerAction.gameObject.SetActive(false);
				}
				_inventoryBarController = View.InventoryBarControllerArena.GetComponent<InventoryBarController>();
				_inventoryBarController.gameObject.SetActive(true);
				if (_model.CurrentDuelState != null)
				{
					DuelState currentDuelState = _model.CurrentDuelState;
					if (currentDuelState is SlugFlyState || currentDuelState is SlugFightState)
					{
						if ((bool)View.ReleaseBar && currentDuelState is SlugFlyState)
						{
							View.ReleaseBar.SetActive(true);
						}
						_slugBarControllerArena.gameObject.SetActive(false);
						_inventoryBarController.gameObject.SetActive(false);
					}
				}
				else
				{
					_slugBarControllerArena.gameObject.SetActive(true);
					_inventoryBarController.gameObject.SetActive(true);
					if ((bool)View.ReleaseBar)
					{
						View.ReleaseBar.SetActive(false);
					}
				}
				_slugBarControllerArena.OnApplyModel(_model);
			}
			if (_assistAvatarInfo != null)
			{
				if (_model.Assistant != null)
				{
					_assistAvatarInfo.gameObject.SetActive(true);
					_assistAvatarInfo.OnApplyModel(model.Assistant);
				}
				else
				{
					_assistAvatarInfo.gameObject.SetActive(false);
				}
			}
			if (_model.Eli != null)
			{
				_mainAvatarInfo.OnApplyModel(model.Eli);
			}
			if (_model.Boss != null)
			{
				_bossAvatarInfo.gameObject.SetActive(true);
				_bossAvatarInfo.OnApplyModel(model.Boss);
			}
			else
			{
				_bossAvatarInfo.gameObject.SetActive(false);
			}
			if (gameController.IsMultiplayer)
			{
				_netPlayerAvatarInfo.gameObject.SetActive(true);
			}
			else
			{
				_netPlayerAvatarInfo.gameObject.SetActive(false);
			}
			if (currentGameLevel.Scene != typeof(ShaneTutor_0_1) && _model.ConsumableItems != null)
			{
				_inventoryBarController.ApplyModel(model.ConsumableItems);
			}
		}

		private void ShowWallet(bool show)
		{
		}

		public void OnApplyModel(InGameModel model)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = model;
			_model.AddObserver(this);
			_model.CommitAndNotify();
		}
	}
}
