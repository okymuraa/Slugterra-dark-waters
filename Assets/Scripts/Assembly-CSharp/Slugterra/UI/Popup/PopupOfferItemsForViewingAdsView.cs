using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupOfferItemsForViewingAdsView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnDecline__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuy__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[SerializeField]
		private List<OfferedItemView> _items;

		public List<OfferedItemView> Items
		{
			get
			{
				return _items;
			}
		}

		public event Action OnDecline
		{
			add
			{
				Action action = OnDecline__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDecline__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnDecline__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDecline__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBuy
		{
			add
			{
				Action action = OnBuy__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuy__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBuy__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuy__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		public void ApplyModel()
		{
			OffersConfiguration offersConfiguration = MonoSingleton<GameConfiguration>.Instance.OffersConfiguration;
			Offer offer = offersConfiguration.GetOffer(LevelItemController.SelectedLevel.Model.Scene.Name);
			List<object> listOfferedItems = offer.GetListOfferedItems();
			foreach (OfferedItemView item in Items)
			{
				if (listOfferedItems.Count > 0)
				{
					item.ApplyModel(listOfferedItems[0]);
					listOfferedItems.RemoveAt(0);
				}
			}
		}

		public void Show()
		{
			gameObject.SetActive(true);
			ApplyModel();
		}

		private void OnBtnDeclineClick()
		{
			if (OnDecline__BackingField != null)
			{
				OnDecline__BackingField();
			}
		}

		private void OnBtnBuyClick()
		{
			if (OnBuy__BackingField != null)
			{
				OnBuy__BackingField();
			}
		}

		public void OnBtnCloseClick()
		{
			Close();
		}

		public override void Close()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			OnCloseEvent__BackingField = null;
			gameObject.SetActive(false);
		}
	}
}
