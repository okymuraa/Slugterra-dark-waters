using System.IO;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.Domain.App
{
	public class Wallet : DomainObject
	{
		public enum CurrencyType
		{
			Gold = 0,
			Gems = 1,
			Stars = 2
		}

		private float _gold;

		private float _gems;

		private const float maxTicketCount = 10f;

		private float _ticket;

		private int _goldReward = 100;

		private int _gemsReward = 20;

		private int _ticketReward = 10;

		private float _stars;

		public float Gold
		{
			get
			{
				return 888888900f;
			}
			set
			{
				_gold = value;
				CommitAndNotify();
			}
		}

		public float Gems
		{
			get
			{
				return 888888900f;
			}
			set
			{
				_gems = value;
				CommitAndNotify();
			}
		}

		public float Ticket
		{
			get
			{
				return _ticket;
			}
			set
			{
				_ticket = value;
				if (_ticket < 0f)
				{
					_ticket = 0f;
				}
				if (_ticket > 10f)
				{
					_ticket = 10f;
				}
				CommitAndNotify();
			}
		}

		public int GoldReward
		{
			get
			{
				return _goldReward;
			}
		}

		public int GemsReward
		{
			get
			{
				return _gemsReward;
			}
		}

		public int TicketReward
		{
			get
			{
				return _ticketReward;
			}
		}

		public float Stars
		{
			get
			{
				return _stars;
			}
			set
			{
				_stars = value;
				CommitAndNotify();
			}
		}

		public Wallet()
		{
			Gold = 0f;
			Stars = 0f;
		}

		public static void Serialize(Wallet wallet)
		{
			string stringToSave = JsonConvert.SerializeObject(wallet);
			IOSystem.SaveStrings(Application.persistentDataPath, "Wallet.cfg", stringToSave, false, true);
		}

		public static Wallet Deserialize()
		{
			string empty = string.Empty;
			Wallet wallet = null;
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("Wallet.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("Wallet.cfg"));
				return JsonConvert.DeserializeObject<Wallet>(empty);
			}
			return new Wallet();
		}

		public void OnGetGemsRewardSuccessEvent()
		{
			Gems += _gemsReward;
			PopupAdsFeedBackView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupAdsFeedBackView>();
			if (registeredPopup != null)
			{
				registeredPopup.ShowPopUpGems();
			}
			else
			{
				Debug.LogError("Wallet::OnGetGemsRewardSuccessEvent PopupAdsFeedBackView is NULL");
			}
		}

		public void OnGetGemsRewardFailEvent()
		{
		}

		public void OnGetRewardSuccessEvent()
		{
			Gold += _goldReward;
			PopupAdsFeedBackView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupAdsFeedBackView>();
			if (registeredPopup != null)
			{
				registeredPopup.ShowPopUpCoins();
			}
			else
			{
				Debug.LogError("Wallet::OnGetRewardSuccessEvent PopupAdsFeedBackView is NULL");
			}
		}

		public void OnGetRewardFailEvent()
		{
		}

		public void OnGetTicketRewardSuccessEvent()
		{
			Ticket += _ticketReward;
		}

		public void OnGetTicketRewardFailEvent()
		{
		}

		public void SetFacebookReward(int rewardValue)
		{
			Gold += rewardValue;
		}
	}
}
