using System;
using System.Collections;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupContinueView : PopupView
	{
		public Action onClosedEvent;

		[SerializeField]
		private GameObject _oneButtonState;

		[SerializeField]
		private GameObject _twoButtonState;

		[SerializeField]
		private GameObject _adsButtonEnabled;

		[SerializeField]
		private GameObject _adsButtonDisabled;

		[SerializeField]
		private TextMesh _countdownText;

		[SerializeField]
		private TextMesh _countdownTextOne;

		[SerializeField]
		private TextMesh _countdownTextTwo;

		private bool pauseCountdown;

		[SerializeField]
		private PaymentButtonController _paymentButtonOne;

		[SerializeField]
		private PaymentButtonController _paymentButtonTwo;

		[SerializeField]
		private PaymentButtonController _paymentButton;

		private bool _oneButtonStatus;

		public Action OnCountdownComplete;

		private static readonly float CountdownValue = 9f;

		private bool _needCountdown = true;

		public PaymentButtonController PaymentButton
		{
			get
			{
				return _paymentButton;
			}
		}

		public PaymentButtonController PaymentButtonOne
		{
			get
			{
				return _paymentButtonOne;
			}
		}

		public bool OneButtonStatus
		{
			get
			{
				return _oneButtonStatus;
			}
			set
			{
				_oneButtonStatus = value;
				SetState();
			}
		}

		public bool NeedCountdown
		{
			get
			{
				return _needCountdown;
			}
			set
			{
				_needCountdown = value;
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
			_adsButtonEnabled.SetActive(true);
			SetState();
		}

		private void SetState()
		{
			if (_oneButtonState != null)
			{
				_oneButtonState.SetActive(_oneButtonStatus);
			}
			if (_twoButtonState != null)
			{
				_twoButtonState.SetActive(!_oneButtonStatus);
			}
			if (_paymentButtonOne != null && _paymentButtonTwo != null)
			{
				_paymentButton = ((!_oneButtonStatus) ? _paymentButtonTwo : _paymentButtonOne);
			}
			if (_countdownTextOne != null && _countdownTextTwo != null)
			{
				_countdownText = ((!_oneButtonStatus) ? _countdownTextTwo : _countdownTextOne);
			}
		}

		protected override void OnEnable()
		{
			_needCountdown = true;
			base.OnEnable();
			StartCoroutine(UpdateViews());
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			StopAllCoroutines();
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
			Debug.Log("UpdateViews");
		}

		private IEnumerator Countdown()
		{
			float _countdownValue = CountdownValue;
			while (_countdownValue > 0f)
			{
				yield return new WaitForEndOfFrame();
				if (!pauseCountdown)
				{
					_countdownValue = Mathf.MoveTowards(_countdownValue, 0f, Time.unscaledDeltaTime);
				}
				_countdownText.text = Mathf.Ceil(_countdownValue).ToString("00");
			}
			if (OnCountdownComplete != null && _needCountdown)
			{
				OnCountdownComplete();
			}
		}

		public void PauseCountdown(bool pause)
		{
			pauseCountdown = pause;
		}

		public void ResetCountdown()
		{
			pauseCountdown = false;
			NeedCountdown = true;
			StopCoroutine("Countdown");
			StartCoroutine("Countdown");
		}

		public void SetActiveAdssButton(bool active)
		{
			_adsButtonEnabled.SetActive(active);
			_adsButtonDisabled.SetActive(!active);
		}

		public void StartCountdown()
		{
			StartCoroutine("Countdown");
		}

		public override void Close()
		{
			if (onClosedEvent != null)
			{
				onClosedEvent();
			}
		}
	}
}
