using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.UI.Announcements
{
	public class InAppAnnouncementsController : UIController<InAppAnnouncementsView>
	{
		private AnnouncementSetting _settings;

		private AnnouncementConfiguration _configs;

		private List<List<AnnouncementContents>> _announcementQueue;

		private int _currentPriorityIndex;

		private int _currentAnnounceIndex;

		private string _currentLevelName = string.Empty;

		private bool _isMap;

		protected override void OnCreate()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.AnnouncementSetting;
			_configs = MonoSingleton<GameConfiguration>.Instance.AnnouncementConfiguration;
			_announcementQueue = new List<List<AnnouncementContents>>();
			SlugterraSceneController slugterraSceneController = Object.FindObjectOfType<SlugterraSceneController>();
			_isMap = slugterraSceneController.GetType() == typeof(MapSceneController);
			if (_isMap)
			{
				ProcessAnnouncements();
			}
			else
			{
				ProcessAnnouncements(false);
			}
			ShowAnnouncements(true);
		}

		private void ProcessUnlockAlliesPopup()
		{
			AnnouncementContents announcementContents = new AnnouncementContents();
			announcementContents.Conditions.ViewPriority = 1;
			announcementContents.Conditions.ShowAnyway = false;
			announcementContents.AnnouncementType = AnnouncementType.UnlockAssistant;
			foreach (KeyValuePair<AllyType, AnnouncementViewStatus> item in _settings.AlliesToUnlock)
			{
				if (item.Value.CurrentStatus == AnnounceStatus.ToView)
				{
					announcementContents.ListContents.Allies.Add(item.Key);
					break;
				}
			}
			if (!announcementContents.ListContents.IsEmpty())
			{
				AddAnnounceToQueue(announcementContents);
			}
		}

		private void ProcessUnlockItemsPopup()
		{
			AnnouncementContents announcementContents = new AnnouncementContents();
			announcementContents.Conditions.ViewPriority = 1;
			announcementContents.Conditions.ShowAnyway = false;
			announcementContents.AnnouncementType = AnnouncementType.Unlock;
			foreach (KeyValuePair<ArmorType, AnnouncementViewStatus> item in _settings.ArmorsToUnlock)
			{
				if (item.Value.CurrentStatus == AnnounceStatus.ToView)
				{
					announcementContents.ListContents.Armors.Add(item.Key);
				}
			}
			foreach (KeyValuePair<SlugType, AnnouncementViewStatus> item2 in _settings.SlugsToUnlock)
			{
				if (item2.Value.CurrentStatus == AnnounceStatus.ToView)
				{
					announcementContents.ListContents.Slugs.Add(item2.Key);
				}
			}
			foreach (KeyValuePair<BlasterType, AnnouncementViewStatus> item3 in _settings.BlastersToUnlock)
			{
				if (item3.Value.CurrentStatus == AnnounceStatus.ToView)
				{
					announcementContents.ListContents.Blasters.Add(item3.Key);
				}
			}
			if (!announcementContents.ListContents.IsEmpty())
			{
				announcementContents.TitleTextKey = "unlock_popup1_title";
				announcementContents.ContentTextKey = ((announcementContents.ListContents.Count() <= 1) ? "unlock_popup3_desc" : "unlock_popup1_desc");
				AddAnnounceToQueue(announcementContents);
			}
		}

		private void ProcessAnnouncements(bool isMap = true)
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			_currentLevelName = ((!isMap) ? MonoSingleton<AppController>.Instance.Model.GetLevelItem(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission).Scene.Name : PlayerPrefs.GetString("lastPassedLevel"));
			if (PlayerPrefs.GetInt("Tutor_CutMegaShopScene") == 0 && !_isMap)
			{
				return;
			}
			foreach (AnnouncementContents gameAnnouncement in _configs.GameAnnouncements)
			{
				if ((gameAnnouncement.AnnouncementType == AnnouncementType.LowBalance && !CheckGoldSubscriptionConditions()) || (gameAnnouncement.AnnouncementType == AnnouncementType.RewardMultiplier && !CheckRewardMultiplierSubcriptionConditions()) || gameAnnouncement.Conditions.ShowBefore == isMap || (gameAnnouncement.Conditions.FilterLevels.Count != 0 && !gameAnnouncement.Conditions.FilterLevels.Contains(_currentLevelName)))
				{
					continue;
				}
				if (gameAnnouncement.AnnouncementType != AnnouncementType.Unlock && gameAnnouncement.AnnouncementType != AnnouncementType.UnlockAssistant)
				{
					bool flag = false;
					foreach (ArmorType armor2 in gameAnnouncement.ListContents.Armors)
					{
						ArmorItem armor = shopModel.GetArmor(armor2);
						if (armor.IsBought || armor.IsLocked)
						{
							flag = true;
							break;
						}
					}
					foreach (BlasterType blaster2 in gameAnnouncement.ListContents.Blasters)
					{
						BlasterItem blaster = shopModel.GetBlaster(blaster2);
						if (blaster.IsBought || blaster.IsLocked)
						{
							flag = true;
							break;
						}
					}
					foreach (SlugType slug2 in gameAnnouncement.ListContents.Slugs)
					{
						SlugItem slug = shopModel.GetSlug(slug2);
						if (slug.IsBought || slug.IsLocked)
						{
							flag = true;
							break;
						}
					}
					foreach (AllyType item in gameAnnouncement.ListContents.Allies)
					{
						if (shopModel.GetAvailableAssistants().Find((AssistantItem assistant) => assistant.Type == item) != null)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
				}
				else
				{
					bool flag2 = true;
					foreach (ArmorType armor3 in gameAnnouncement.ListContents.Armors)
					{
						if (!shopModel.GetArmor(armor3).IsBought)
						{
							flag2 = false;
							break;
						}
					}
					foreach (BlasterType blaster3 in gameAnnouncement.ListContents.Blasters)
					{
						if (!shopModel.GetBlaster(blaster3).IsBought)
						{
							flag2 = false;
							break;
						}
					}
					foreach (SlugType slug3 in gameAnnouncement.ListContents.Slugs)
					{
						if (!shopModel.GetSlug(slug3).IsBought)
						{
							flag2 = false;
							break;
						}
					}
					foreach (AllyType item2 in gameAnnouncement.ListContents.Allies)
					{
						if (shopModel.GetAvailableAssistants().Find((AssistantItem assistant) => assistant.Type == item2) == null)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						continue;
					}
				}
				if (!gameAnnouncement.Conditions.FilterObtainedItems.IsEmpty())
				{
					bool flag3 = false;
					foreach (ArmorType armor4 in gameAnnouncement.Conditions.FilterObtainedItems.Armors)
					{
						if (!shopModel.GetArmor(armor4).IsBought)
						{
							flag3 = true;
							break;
						}
					}
					foreach (BlasterType blaster4 in gameAnnouncement.Conditions.FilterObtainedItems.Blasters)
					{
						if (!shopModel.GetBlaster(blaster4).IsBought)
						{
							flag3 = true;
							break;
						}
					}
					foreach (SlugType slug4 in gameAnnouncement.Conditions.FilterObtainedItems.Slugs)
					{
						if (!shopModel.GetSlug(slug4).IsBought)
						{
							flag3 = true;
							break;
						}
					}
					foreach (AllyType item3 in gameAnnouncement.Conditions.FilterObtainedItems.Allies)
					{
						if (shopModel.GetAvailableAssistants().Find((AssistantItem assistant) => assistant.Type == item3) == null)
						{
							flag3 = true;
							break;
						}
					}
					if (flag3)
					{
						continue;
					}
				}
				if (!gameAnnouncement.Conditions.ShowAnyway && GetAnnounceQueueTotalCount() > 0)
				{
					continue;
				}
				AnnouncementViewStatus announce = _settings.GetAnnounce(gameAnnouncement.AnnouncementId);
				switch (announce.CurrentStatus)
				{
				case AnnounceStatus.Done:
					continue;
				}
				if ((gameAnnouncement.Conditions.MaxViewCount == 0 || announce.LevelViews.Count < gameAnnouncement.Conditions.MaxViewCount) && (!announce.LevelViews.Contains(_currentLevelName) || !announce.ViewedSessionLevelName.Contains(_currentLevelName)))
				{
					announce.CurrentStatus = AnnounceStatus.ToView;
					if (gameAnnouncement.AnnouncementType == AnnouncementType.Unlock)
					{
						AddAnnounceToQueue(ExcludeBoughtedItemsFromUnlockAnnounce(gameAnnouncement, shopModel));
					}
					else
					{
						AddAnnounceToQueue(gameAnnouncement);
					}
				}
			}
		}

		private AnnouncementContents ExcludeBoughtedItemsFromUnlockAnnounce(AnnouncementContents contents, ShopModel shop)
		{
			contents = (AnnouncementContents)contents.Clone();
			for (int num = contents.ListContents.Armors.Count - 1; num >= 0; num--)
			{
				if (shop.GetArmor(contents.ListContents.Armors[num]).IsBought)
				{
					contents.ListContents.Armors.RemoveAt(num);
				}
			}
			for (int num2 = contents.ListContents.Slugs.Count - 1; num2 >= 0; num2--)
			{
				if (shop.GetSlug(contents.ListContents.Slugs[num2]).IsBought)
				{
					contents.ListContents.Slugs.RemoveAt(num2);
				}
			}
			for (int num3 = contents.ListContents.Blasters.Count - 1; num3 >= 0; num3--)
			{
				if (shop.GetBlaster(contents.ListContents.Blasters[num3]).IsBought)
				{
					contents.ListContents.Blasters.RemoveAt(num3);
				}
			}
			return contents;
		}

		private bool CheckGoldSubscriptionConditions()
		{
			if (!(MonoSingleton<AppController>.Instance.Model.Wallet.Gold <= 100f))
			{
				return false;
			}
			foreach (SubscriptionController value in DonDestroyMonoSingleton<SubscriptionManager>.Instance.SubscriptionControllers.Values)
			{
				if (value is SubscriptionControllerGold)
				{
					SubscriptionControllerGold subscriptionControllerGold = value as SubscriptionControllerGold;
					if (subscriptionControllerGold.ActiveSubContext != null && subscriptionControllerGold.ActiveSubContext.IsSubscriptionActive())
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool CheckRewardMultiplierSubcriptionConditions()
		{
			return MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier != 2;
		}

		private void ProcessNotViewedAnnouncements()
		{
			foreach (KeyValuePair<string, AnnouncementViewStatus> item in _settings.Announces)
			{
				if (item.Value.CurrentStatus == AnnounceStatus.ToView)
				{
					AddAnnounceToQueue(_configs.GameAnnouncements.Find((AnnouncementContents announce) => announce.AnnouncementId == item.Key));
				}
			}
		}

		private void AddAnnounceToQueue(AnnouncementContents announce)
		{
			for (int i = _announcementQueue.Count; i < announce.Conditions.ViewPriority + 1; i++)
			{
				_announcementQueue.Add(new List<AnnouncementContents>());
			}
			_announcementQueue[announce.Conditions.ViewPriority].Add(announce);
		}

		private int GetAnnounceQueueTotalCount()
		{
			int num = 0;
			foreach (List<AnnouncementContents> item in _announcementQueue)
			{
				foreach (AnnouncementContents item2 in item)
				{
					num++;
				}
			}
			return num;
		}

		private AnnouncementContents GetAnnounceFromQueue(AnnouncementType type)
		{
			AnnouncementContents announcementContents = null;
			foreach (List<AnnouncementContents> item in _announcementQueue)
			{
				foreach (AnnouncementContents item2 in item)
				{
					if (item2.AnnouncementType == type)
					{
						announcementContents = item2;
						break;
					}
				}
				if (announcementContents != null)
				{
					break;
				}
			}
			return announcementContents;
		}

		private bool RemoveAnnounceFromQueue(string id)
		{
			foreach (List<AnnouncementContents> item in _announcementQueue)
			{
				for (int num = item.Count - 1; num >= 0; num--)
				{
					if (item[num].AnnouncementId == id)
					{
						item.RemoveAt(num);
						return true;
					}
				}
			}
			return false;
		}

		private bool IsPlayerLowBalance()
		{
			return true;
		}

		private void SetBadgeAnnouncement(AnnouncementContents announce)
		{
			View.SetBadgeOffer(announce, OnBadgeOfferClosed);
		}

		private void ShowAnnouncements(bool isFirst = false)
		{
			if (_announcementQueue.Count != _currentPriorityIndex)
			{
				if (_announcementQueue[_currentPriorityIndex].Count == _currentAnnounceIndex)
				{
					_currentPriorityIndex++;
					_currentAnnounceIndex = 0;
					ShowAnnouncements(isFirst);
				}
				else if (isFirst || _announcementQueue[_currentPriorityIndex][_currentAnnounceIndex].Conditions.ShowAnyway)
				{
					View.CreateAnnounce(_announcementQueue[_currentPriorityIndex][_currentAnnounceIndex], OnAnnouncementClosed);
				}
			}
		}

		private void CompleteUnlockShowing(AnnounceStatus reply)
		{
			AnnouncementPopupListContents listContents = _announcementQueue[_currentPriorityIndex][_currentAnnounceIndex].ListContents;
			foreach (ArmorType armor in listContents.Armors)
			{
				_settings.ArmorsToUnlock[armor].UpdateStatus(reply, _currentLevelName);
			}
			foreach (BlasterType blaster in listContents.Blasters)
			{
				_settings.BlastersToUnlock[blaster].UpdateStatus(reply, _currentLevelName);
			}
			foreach (SlugType slug in listContents.Slugs)
			{
				_settings.SlugsToUnlock[slug].UpdateStatus(reply, _currentLevelName);
			}
			foreach (AllyType ally in listContents.Allies)
			{
				_settings.AlliesToUnlock[ally].UpdateStatus(reply, _currentLevelName);
			}
			_settings.CommitAndNotify();
		}

		private void CompleteAnnounceShowing(AnnounceStatus reply)
		{
			_settings.GetAnnounce(_announcementQueue[_currentPriorityIndex][_currentAnnounceIndex].AnnouncementId).UpdateStatus(reply, _currentLevelName);
			_settings.CommitAndNotify();
		}

		public void SetBadgeOfferBlaster(BlasterItem blaster)
		{
			AnnouncementContents badgeAnnouncement = null;
			if (blaster != null && !blaster.IsLocked && !blaster.IsBought)
			{
				ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
				List<AnnouncementContents> announcesContainingBlaster = _configs.GetAnnouncesContainingBlaster(blaster.Type);
				foreach (AnnouncementContents item in announcesContainingBlaster)
				{
					if (item.AnnouncementType != AnnouncementType.SpecialOffer)
					{
						continue;
					}
					bool flag = true;
					foreach (BlasterType blaster3 in item.ListContents.Blasters)
					{
						BlasterItem blaster2 = shopModel.GetBlaster(blaster3);
						if (blaster2.IsLocked || blaster2.IsBought)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						badgeAnnouncement = item;
						break;
					}
				}
			}
			SetBadgeAnnouncement(badgeAnnouncement);
		}

		private void OnAnnouncementClosed(AnnounceStatus reply = AnnounceStatus.Done)
		{
			View.SetBGShadow(false);
			switch (_announcementQueue[_currentPriorityIndex][_currentAnnounceIndex].AnnouncementType)
			{
			case AnnouncementType.LimitedOffer:
			case AnnouncementType.SpecialOffer:
			case AnnouncementType.VipOffer:
			case AnnouncementType.Unlock:
			case AnnouncementType.UnlockAssistant:
				CompleteAnnounceShowing(reply);
				break;
			}
			_currentAnnounceIndex++;
			ShowAnnouncements();
		}

		private void OnBadgeOfferClosed(AnnounceStatus reply = AnnounceStatus.Done)
		{
			if (reply == AnnounceStatus.Done)
			{
				View.SetBGShadow(false);
				View.SetBadge(false);
			}
			else
			{
				View.HideBadgeOffer();
			}
		}
	}
}
