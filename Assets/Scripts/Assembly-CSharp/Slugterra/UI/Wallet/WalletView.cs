using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using Slugterra.Domain.App;
using UnityEngine;

namespace Slugterra.UI.Wallet
{
	public class WalletView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnOpenIAPPopupEvent__BackingField;

		[SerializeField]
		private TextMesh _gold;

		[SerializeField]
		private TextMesh _stars;

		[SerializeField]
		private TextMesh _gems;

		[SerializeField]
		private Transform _iconGold;

		[SerializeField]
		private Transform _iconStars;

		[SerializeField]
		private SoundContainer OnGoldSound;

		[SerializeField]
		private GameObject TVOSUIPrefab;

		[SerializeField]
		private GameObject OtherUIPrefab;

		private int oldGemsValue;

		private int newGemsValue = -1;

		private int oldGoldValue;

		private int newGoldValue = -1;

		public event Action OnOpenIAPPopupEvent
		{
			add
			{
				Action action = OnOpenIAPPopupEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnOpenIAPPopupEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnOpenIAPPopupEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnOpenIAPPopupEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (OnGoldSound != null)
			{
				OnGoldSound.RegisterInSoundManager();
			}
			if (TVOSUIPrefab != null)
			{
				TVOSUIPrefab.SetActive(false);
			}
			if (OtherUIPrefab != null)
			{
				OtherUIPrefab.SetActive(true);
			}
		}

		private void OnEnable()
		{
		}

		protected override void OnApplyModel(object model)
		{
			if (!(model is Slugterra.Domain.App.Wallet))
			{
				return;
			}
			Slugterra.Domain.App.Wallet wallet = model as Slugterra.Domain.App.Wallet;
			if (newGoldValue == -1)
			{
				oldGoldValue = (newGoldValue = (int)wallet.Gold);
				_gold.text = newGoldValue.ToString();
			}
			if (newGemsValue == -1)
			{
				oldGemsValue = (newGemsValue = (int)wallet.Gems);
				if (_gems != null)
				{
					_gems.text = newGemsValue.ToString();
				}
			}
			if (_stars != null)
			{
				_stars.text = ((int)wallet.Stars/*cast due to constrained. prefix*/).ToString();
			}
			newGoldValue = (int)wallet.Gold;
			newGemsValue = (int)wallet.Gems;
		}

		private void OnOpenIAPPopupBtnClick()
		{
			if (OnOpenIAPPopupEvent__BackingField != null)
			{
				OnOpenIAPPopupEvent__BackingField();
			}
		}

		private void Update()
		{
			if (oldGoldValue != newGoldValue)
			{
				if (OnGoldSound != null)
				{
					OnGoldSound.Play();
				}
				int num = newGoldValue - oldGoldValue;
				int num2 = ((num <= 0) ? ((int)Math.Floor((float)num / 10f)) : ((int)Math.Ceiling((float)num / 10f)));
				oldGoldValue += num2;
				_gold.text = oldGoldValue.ToString();
			}
			if (oldGemsValue != newGemsValue)
			{
				int num3 = newGemsValue - oldGemsValue;
				int num4 = ((num3 <= 0) ? ((int)Math.Floor((float)num3 / 10f)) : ((int)Math.Ceiling((float)num3 / 10f)));
				oldGemsValue += num4;
				if (_gems != null)
				{
					_gems.text = oldGemsValue.ToString();
				}
			}
		}
	}
}
