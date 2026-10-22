using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class TicketItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuyEvent__BackingField;

		[SerializeField]
		private TextMesh _titleText;

		[SerializeField]
		private tk2dButton _buyButton;

		[SerializeField]
		private TextMesh _buttonText;

		[SerializeField]
		private TextMesh _moneyCountText;

		[SerializeField]
		private GameObject _saleBadge;

		[SerializeField]
		private GameObject _bestSellerBadge;

		public event Action OnBuyEvent
		{
			add
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_titleText);
		}

		private void OnEnable()
		{
			_buyButton.ButtonPressedEvent += OnBuy;
		}

		private void OnDisable()
		{
			_buyButton.ButtonPressedEvent -= OnBuy;
		}

		private void OnBuy(tk2dButton source)
		{
			if (OnBuyEvent__BackingField != null)
			{
				OnBuyEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object _model)
		{
			TicketItem ticketItem = _model as TicketItem;
			if (ticketItem != null)
			{
				_titleText.text = MonoSingleton<Localization>.Instance[ticketItem.Title];
				_buttonText.text = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(string.Format("{0}", ticketItem.SKU));
				_moneyCountText.text = ticketItem.Tickets;
				bool isBestSeller = ticketItem.IsBestSeller;
				bool isSale = ticketItem.IsSale;
				_bestSellerBadge.SetActive(false);
				_saleBadge.SetActive(false);
				if (isBestSeller)
				{
					_bestSellerBadge.SetActive(true);
				}
				if (isSale)
				{
					_saleBadge.SetActive(true);
				}
			}
		}
	}
}
