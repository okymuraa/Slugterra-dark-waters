using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.Domain.App.Settings
{
	public class AnnouncementSetting : DomainObject, IObserver
	{
		public Dictionary<ArmorType, AnnouncementViewStatus> ArmorsToUnlock = new Dictionary<ArmorType, AnnouncementViewStatus>();

		public Dictionary<SlugType, AnnouncementViewStatus> SlugsToUnlock = new Dictionary<SlugType, AnnouncementViewStatus>();

		public Dictionary<BlasterType, AnnouncementViewStatus> BlastersToUnlock = new Dictionary<BlasterType, AnnouncementViewStatus>();

		public Dictionary<AllyType, AnnouncementViewStatus> AlliesToUnlock = new Dictionary<AllyType, AnnouncementViewStatus>();

		public Dictionary<string, AnnouncementViewStatus> Announces = new Dictionary<string, AnnouncementViewStatus>();

		public AnnouncementSetting()
		{
			if (MonoSingleton<AppController>.Instance.Model == null)
			{
				return;
			}
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			foreach (ArmorItem armor in shopModel.Armors)
			{
				if (!armor.IsLocked)
				{
					ArmorsToUnlock.Add(armor.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			foreach (SlugItem attackSlug in shopModel.AttackSlugs)
			{
				if (!attackSlug.IsLocked)
				{
					SlugsToUnlock.Add(attackSlug.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			foreach (SlugItem defenceSlug in shopModel.DefenceSlugs)
			{
				if (!defenceSlug.IsLocked)
				{
					SlugsToUnlock.Add(defenceSlug.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			foreach (SlugItem specialSlug in shopModel.SpecialSlugs)
			{
				if (!specialSlug.IsLocked)
				{
					SlugsToUnlock.Add(specialSlug.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			foreach (BlasterItem blaster in shopModel.Blasters)
			{
				if (!blaster.IsLocked)
				{
					BlastersToUnlock.Add(blaster.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			foreach (AssistantItem assistant in shopModel.Assistants)
			{
				if (!assistant.IsLocked)
				{
					AlliesToUnlock.Add(assistant.Type, new AnnouncementViewStatus(AnnounceStatus.Done));
				}
			}
			Initialize();
		}

		public AnnouncementViewStatus GetAnnounce(string id)
		{
			if (!Announces.ContainsKey(id))
			{
				Announces.Add(id, new AnnouncementViewStatus(AnnounceStatus.ToView));
			}
			return Announces[id];
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is ArmorItem)
			{
				ArmorItem armorItem = observable as ArmorItem;
				if (!armorItem.IsLocked && !ArmorsToUnlock.ContainsKey(armorItem.Type))
				{
					ArmorsToUnlock.Add(armorItem.Type, new AnnouncementViewStatus(AnnounceStatus.ToView));
				}
				armorItem.RemoveObserver(this);
			}
			if (observable is BlasterItem)
			{
				BlasterItem blasterItem = observable as BlasterItem;
				if (!blasterItem.IsLocked && !BlastersToUnlock.ContainsKey(blasterItem.Type))
				{
					BlastersToUnlock.Add(blasterItem.Type, new AnnouncementViewStatus(AnnounceStatus.ToView));
				}
				blasterItem.RemoveObserver(this);
			}
			if (observable is SlugItem)
			{
				SlugItem slugItem = observable as SlugItem;
				if (!slugItem.IsLocked && !SlugsToUnlock.ContainsKey(slugItem.Type))
				{
					SlugsToUnlock.Add(slugItem.Type, new AnnouncementViewStatus(AnnounceStatus.ToView));
				}
				slugItem.RemoveObserver(this);
			}
			if (observable is AssistantItem)
			{
				AssistantItem assistantItem = observable as AssistantItem;
				if (!assistantItem.IsLocked && !AlliesToUnlock.ContainsKey(assistantItem.Type))
				{
					AlliesToUnlock.Add(assistantItem.Type, new AnnouncementViewStatus(AnnounceStatus.ToView));
				}
				assistantItem.RemoveObserver(this);
			}
			CommitAndNotify();
		}

		public void Initialize()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.AddObserver(this);
			foreach (ArmorItem armor in shopModel.Armors)
			{
				if (armor.IsLocked)
				{
					armor.AddObserver(this);
				}
			}
			foreach (SlugItem attackSlug in shopModel.AttackSlugs)
			{
				if (attackSlug.IsLocked)
				{
					attackSlug.AddObserver(this);
				}
			}
			foreach (SlugItem defenceSlug in shopModel.DefenceSlugs)
			{
				if (defenceSlug.IsLocked)
				{
					defenceSlug.AddObserver(this);
				}
			}
			foreach (SlugItem specialSlug in shopModel.SpecialSlugs)
			{
				if (specialSlug.IsLocked)
				{
					specialSlug.AddObserver(this);
				}
			}
			foreach (BlasterItem blaster in shopModel.Blasters)
			{
				if (blaster.IsLocked)
				{
					blaster.AddObserver(this);
				}
			}
			foreach (AssistantItem assistant in shopModel.Assistants)
			{
				if (assistant.IsLocked)
				{
					assistant.AddObserver(this);
				}
			}
		}

		public static void Serialize(AnnouncementSetting announcementSettings)
		{
			string stringToSave = JsonConvert.SerializeObject(announcementSettings);
			IOSystem.SaveStrings(Application.persistentDataPath, "Announcement.cfg", stringToSave, false, true);
		}

		public static AnnouncementSetting Deserialize()
		{
			AnnouncementSetting announcementSetting = null;
			if (IOSystem.IsFileExist(Application.persistentDataPath, "Announcement.cfg"))
			{
				string value = IOSystem.ReadStrings(Application.persistentDataPath, "Announcement.cfg");
				announcementSetting = JsonConvert.DeserializeObject<AnnouncementSetting>(value);
				announcementSetting.Initialize();
			}
			else
			{
				announcementSetting = new AnnouncementSetting();
			}
			return announcementSetting;
		}
	}
}
