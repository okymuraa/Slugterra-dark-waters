using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.UI.Game;
using Slugterra.UI.Shops;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.UI.Wallet
{
	public class WalletController : UIController<WalletView>, IObserver
	{
		private Slugterra.Domain.App.Wallet _wallet;

		private ShopIAPView _iapPopup;

		protected override void OnStart()
		{
			_iapPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<ShopIAPView>();
			_wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			_wallet.AddObserver(this);
			View.ApplyModel(_wallet);
		}

		protected override void _OnEnable()
		{
			View.OnOpenIAPPopupEvent += OnOpenIAPPopup;
		}

		protected override void _OnDisable()
		{
			View.OnOpenIAPPopupEvent -= OnOpenIAPPopup;
		}

		private void OnOpenIAPPopupSpecial()
		{
			if (_iapPopup == null)
			{
				_iapPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<ShopIAPView>();
			}
			if (_iapPopup != null)
			{
				WinController winController = Object.FindObjectOfType<WinController>();
				PauseController pauseController = Object.FindObjectOfType<PauseController>();
				MinimapController minimapController = Object.FindObjectOfType<MinimapController>();
				LoseController loseController = Object.FindObjectOfType<LoseController>();
				SurvivalController survivalController = Object.FindObjectOfType<SurvivalController>();
				string text = SceneManager.GetActiveScene().name;
				if ((winController != null && winController.isActiveAndEnabled) || (loseController != null && loseController.isActiveAndEnabled) || (minimapController != null && minimapController.isActiveAndEnabled) || (pauseController != null && pauseController.isActiveAndEnabled) || (survivalController != null && survivalController.isActiveAndEnabled))
				{
					_iapPopup.Show();
				}
			}
		}

		private void OnOpenIAPPopup()
		{
			if (_iapPopup == null)
			{
				_iapPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<ShopIAPView>();
			}
			if (_iapPopup != null)
			{
				_iapPopup.Show();
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Slugterra.Domain.App.Wallet)
			{
				View.ApplyModel(observable as Slugterra.Domain.App.Wallet);
			}
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_wallet != null)
			{
				_wallet.RemoveObserver(this);
			}
		}
	}
}
