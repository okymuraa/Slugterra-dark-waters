using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.InAppPurchases;
using Slugterra.UI.Survival;
using UnityEngine;

namespace Slugterra.UI.SurvivalMenu
{
	public class MultiplayerMenuView : UIView, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnNextClickEvent__BackingField;

		public GameObject _lobby;

		public TextMesh _playerNickname;

		public TextMesh _playerRank;

		[SerializeField]
		private TextMesh _moneyText;

		private string _productID = "com.appsministry.slugterra.5mptickets";

		[SerializeField]
		private GameObject _mapItemPrefab;

		[SerializeField]
		private tk2dUIScrollableArea _scrollArea;

		[SerializeField]
		private BadgeIconView _challengeCounter;

		[SerializeField]
		private GameObject _saleInStore;

		[SerializeField]
		private GameObject _NextButtonOnState;

		[SerializeField]
		private GameObject _NextButtonOffState;

		public TextMesh textTicketsCountdown;

		public GameObject Tutorial;

		public TextMesh[] infoCount;

		[SerializeField]
		private float curTicketCount;

		private bool _connectionLost;

		private List<SurvivalMapItemView> _rivalItemUI = new List<SurvivalMapItemView>();

		public bool returnclick;

		public GameObject TicketsEnough;

		public GameObject TicketsNoEnough;

		public float CurTicketCount
		{
			get
			{
				return curTicketCount;
			}
			set
			{
				curTicketCount = value;
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

		public event Action OnBtnGameServiceEvent
		{
			add
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnNextClickEvent
		{
			add
			{
				Action action = OnBtnNextClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnNextClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnNextClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnNextClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			MonoSingleton<AppController>.Instance.Model.MissionSetting.AddObserver(this);
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Combine(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Combine(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
		}

		private void OnDisable()
		{
			if (MonoSingleton<AppController>.IsAlive)
			{
				MonoSingleton<AppController>.Instance.Model.MissionSetting.RemoveObserver(this);
			}
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Remove(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
		}

		private void InternetConnectionLost()
		{
			_NextButtonOnState.SetActive(false);
			_NextButtonOffState.SetActive(true);
			_connectionLost = true;
		}

		private void InternetConnectionAppear()
		{
			UpdateItem();
			_connectionLost = false;
		}

		private void SetPlayerNickname()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
			{
				string playerAlias = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerAlias;
				string text = MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank.ToString("#####");
				_playerNickname.text = playerAlias;
				_playerRank.text = text;
			}
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			curTicketCount = MonoSingleton<AppController>.Instance.Model.Wallet.Ticket;
			UpdateItem();
			SetPlayerNickname();
			_moneyText.text = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(_productID);
		}

		private void OnBtnGameServiceClick()
		{
			if (OnBtnGameServiceEvent__BackingField != null)
			{
				OnBtnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnNextClick()
		{
			if (!returnclick && OnBtnNextClickEvent__BackingField != null)
			{
				curTicketCount--;
				returnclick = true;
				UpdateItem();
				OnBtnNextClickEvent__BackingField();
			}
		}

		public void OnBtnBuyClick()
		{
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(_productID, OnSuccessBuy);
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.OnSuccessPurchase += OnSuccessBuy;
			curTicketCount = MonoSingleton<AppController>.Instance.Model.Wallet.Ticket;
			UpdateItem();
		}

		private void OnSuccessBuy()
		{
			PlayerPrefs.DeleteKey("NextTicketsDate");
			curTicketCount = MonoSingleton<AppController>.Instance.Model.Wallet.Ticket;
			UpdateItem();
		}

		private void OnSuccessBuy(string id)
		{
			if (id == _productID)
			{
				PlayerPrefs.DeleteKey("NextTicketsDate");
				curTicketCount = MonoSingleton<AppController>.Instance.Model.Wallet.Ticket;
				UpdateItem();
			}
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.OnSuccessPurchase -= OnSuccessBuy;
		}

		public void UpdateItem()
		{
			if (curTicketCount > 0f)
			{
				TicketsEnough.SetActive(true);
				TicketsNoEnough.SetActive(false);
				if (!_connectionLost)
				{
					_NextButtonOnState.SetActive(true);
					_NextButtonOffState.SetActive(false);
				}
				for (int i = 0; i < infoCount.Length; i++)
				{
					infoCount[i].text = curTicketCount.ToString();
				}
			}
			else
			{
				TicketsEnough.SetActive(false);
				TicketsNoEnough.SetActive(true);
				_NextButtonOnState.SetActive(false);
				_NextButtonOffState.SetActive(true);
				for (int j = 0; j < infoCount.Length; j++)
				{
					infoCount[j].text = curTicketCount.ToString();
				}
			}
		}

		public void OnObjectChanged(object observable)
		{
		}
	}
}
