using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupGiftsController : UIController<PopupGiftsView>
	{
		public const float MinValue = 50f;

		public const float StepValue = 50f;

		private float _value;

		private Slugterra.Domain.App.Wallet _wallet;

		protected override void OnCreate()
		{
			_wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
		}

		protected override void _OnEnable()
		{
			SetValue(50f);
			View.OnCloseEvent += OnClose;
			View.OnYourGiftsEvent += OnYourGifts;
			View.OnSendEvent += OnSend;
			View.OnIncreaseValueEvent += OnIncreaseValue;
			View.OnDecreaseValueEvent += OnDecreaseValue;
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnGiftRequestSentSuccess += OnGiftRequestSentSuccessEvent;
		}

		protected override void _OnDisable()
		{
			View.OnCloseEvent -= OnClose;
			View.OnYourGiftsEvent -= OnYourGifts;
			View.OnSendEvent -= OnSend;
			View.OnIncreaseValueEvent -= OnIncreaseValue;
			View.OnDecreaseValueEvent -= OnDecreaseValue;
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnGiftRequestSentSuccess -= OnGiftRequestSentSuccessEvent;
		}

		private void OnClose()
		{
			View.gameObject.SetActive(false);
		}

		private void OnYourGifts()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameGifts();
		}

		private void OnSend()
		{
			if (_value <= _wallet.Gold)
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.SendGiftRequest(GPGameRequestType.TYPE_GIFT, 5, null, MonoSingleton<Localization>.Instance.GetValueByKey("gift_massage_text"), _value.ToString());
				return;
			}
			ShopIAPView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<ShopIAPView>();
			if (registeredPopup != null)
			{
				registeredPopup.Show();
			}
		}

		private void OnIncreaseValue()
		{
			SetValue(_value + 50f);
		}

		private void OnDecreaseValue()
		{
			SetValue(_value - 50f);
		}

		private void SetValue(float value)
		{
			_value = Mathf.Clamp(value, 50f, Mathf.Max(50f, _wallet.Gold));
			View.ApplyModel(_value);
		}

		private void OnGiftRequestSentSuccessEvent()
		{
			_wallet.Gold -= _value;
			SetValue(0f);
		}
	}
}
