using System;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Domain.App.Settings
{
	public class MissionSetting : DomainObject, IObserver
	{
		private static bool _isSerialization;

		private Type _mission;

		private AssistantItem _assistant;

		private AssistantItem _offeredAssistant;

		private BlasterItem _blaster;

		private BlasterItem _offeredBlaster;

		private ArmorItem _armor;

		private ArmorItem _offeredArmor;

		private SlugItem _attackSlug;

		private SlugItem _offeredAttackSlug;

		private SlugItem _defenceSlug;

		private SlugItem _offeredDefenceSlug;

		private SlugItem _specialSlug;

		private SlugItem _offeredSpecialSlug;

		public Type Mission
		{
			get
			{
				return _mission;
			}
			set
			{
				_mission = value;
				CommitAndNotify();
			}
		}

		public AssistantItem Assistant
		{
			get
			{
				if (_offeredAssistant != null && !_isSerialization)
				{
					return _offeredAssistant;
				}
				if (_assistant == null)
				{
					SetDefaultAssistant();
					Debug.LogError("Assistant is Null ");
				}
				return _assistant;
			}
			set
			{
				_assistant = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public AssistantItem OfferedAssistant
		{
			get
			{
				return _offeredAssistant;
			}
			set
			{
				_offeredAssistant = value;
			}
		}

		public BlasterItem Blaster
		{
			get
			{
				if (_offeredBlaster != null && !_isSerialization)
				{
					return _offeredBlaster;
				}
				if (_blaster == null)
				{
					SetDefaultBlaster();
					Debug.LogError("Blaster is Null ");
				}
				return _blaster;
			}
			set
			{
				_blaster = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public BlasterItem OfferedBlaster
		{
			get
			{
				return _offeredBlaster;
			}
			set
			{
				_offeredBlaster = value;
			}
		}

		public ArmorItem Armor
		{
			get
			{
				if (_offeredArmor != null && !_isSerialization)
				{
					return _offeredArmor;
				}
				if (_armor == null)
				{
					SetDefaultArmor();
					Debug.LogError("Armor is Null ");
				}
				return _armor;
			}
			set
			{
				_armor = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public ArmorItem OfferedArmor
		{
			get
			{
				return _offeredArmor;
			}
			set
			{
				_offeredArmor = value;
			}
		}

		public SlugItem AttackSlug
		{
			get
			{
				if (_offeredAttackSlug != null && !_isSerialization)
				{
					return _offeredAttackSlug;
				}
				if (_attackSlug == null)
				{
					SetDefaultDefenceSlug();
					Debug.LogError("AttackSlug is Null ");
				}
				return _attackSlug;
			}
			set
			{
				_attackSlug = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public SlugItem OfferedAttackSlug
		{
			get
			{
				return _offeredAttackSlug;
			}
			set
			{
				_offeredAttackSlug = value;
			}
		}

		public SlugItem DefenceSlug
		{
			get
			{
				if (_offeredDefenceSlug != null && !_isSerialization)
				{
					return _offeredDefenceSlug;
				}
				if (_defenceSlug == null)
				{
					SetDefaultDefenceSlug();
					Debug.LogError("DefenseSlug is Null ");
				}
				return _defenceSlug;
			}
			set
			{
				_defenceSlug = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public SlugItem OfferedDefenceSlug
		{
			get
			{
				return _offeredDefenceSlug;
			}
			set
			{
				_offeredDefenceSlug = value;
			}
		}

		public SlugItem SpecialSlug
		{
			get
			{
				if (_offeredSpecialSlug != null && !_isSerialization)
				{
					return _offeredSpecialSlug;
				}
				if (_specialSlug == null)
				{
					SetDefaultSpecialSlug();
					Debug.LogError("SpecialSlug is Null ");
				}
				return _specialSlug;
			}
			set
			{
				_specialSlug = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public SlugItem OfferedSpecialSlug
		{
			get
			{
				return _offeredSpecialSlug;
			}
			set
			{
				_offeredSpecialSlug = value;
			}
		}

		public MissionSetting()
		{
			Mission = typeof(QuietLawn_2_1);
			SetDefaultAssistant();
			SetDefaultArmor();
			SetDefaultBlaster();
			SetDefaultAttackSlug();
			SetDefaultDefenceSlug();
			SetDefaultSpecialSlug();
		}

		public void SetDefaultAssistant()
		{
			Assistant = new AssistantItem(AllyType.None, 0, false, true);
		}

		public void SetDefaultArmor(ShopModel shopModel = null)
		{
			ArmorItem armorItem = null;
			if (shopModel != null)
			{
				armorItem = shopModel.GetArmor(ArmorType.Default);
			}
			if (armorItem == null)
			{
				armorItem = new ArmorItem(ArmorType.Default, 0, false, true);
			}
			Armor = armorItem;
		}

		public void SetDefaultBlaster(ShopModel shopModel = null)
		{
			BlasterItem blasterItem = null;
			if (shopModel != null)
			{
				blasterItem = shopModel.GetBlaster(BlasterType.Default);
			}
			if (blasterItem == null)
			{
				blasterItem = new BlasterItem(BlasterType.Default, 0, false, true, 100f);
			}
			Blaster = blasterItem;
		}

		public void SetDefaultAttackSlug(ShopModel shopModel = null)
		{
			SlugItem slugItem = null;
			if (shopModel != null)
			{
				slugItem = shopModel.GetAttackSlug(SlugType.Infurnus);
			}
			if (slugItem == null)
			{
				slugItem = new SlugItem(SlugType.Infurnus, 0);
			}
			AttackSlug = slugItem;
		}

		public void SetDefaultDefenceSlug()
		{
			DefenceSlug = new SlugItem(SlugType.None, 0, false, false);
		}

		public void SetDefaultSpecialSlug()
		{
			SpecialSlug = new SlugItem(SlugType.None, 0, false, false);
		}

		public void OnObjectChanged(object observable)
		{
			if (!(observable is ConsumableItem))
			{
			}
		}

		public static void Serialize(MissionSetting missionSettings)
		{
			_isSerialization = true;
			string stringToSave = JsonConvert.SerializeObject(missionSettings);
			IOSystem.SaveStrings(Application.persistentDataPath, "Mission.cfg", stringToSave, false, true);
			_isSerialization = false;
		}

		public static MissionSetting Deserialize()
		{
			MissionSetting missionSetting = null;
			if (IOSystem.IsFileExist(Application.persistentDataPath, "Mission.cfg"))
			{
				string value = IOSystem.ReadStrings(Application.persistentDataPath, "Mission.cfg");
				return JsonConvert.DeserializeObject<MissionSetting>(value);
			}
			return new MissionSetting();
		}

		public SlugItem GetSlugByGroup(SlugGroup slugGroup)
		{
			switch (slugGroup)
			{
			case SlugGroup.Attack:
				return AttackSlug;
			case SlugGroup.Control:
				return DefenceSlug;
			case SlugGroup.Special:
				return SpecialSlug;
			default:
				return null;
			}
		}

		public void SetSlug(SlugItem item)
		{
			switch (SlugTypeHelper.GetGroup(item.Type))
			{
			case SlugGroup.Attack:
				AttackSlug = item;
				break;
			case SlugGroup.Control:
				DefenceSlug = item;
				break;
			case SlugGroup.Special:
				SpecialSlug = item;
				break;
			}
		}

		public void ClearOfferedItems()
		{
			_offeredAttackSlug = null;
			_offeredDefenceSlug = null;
			_offeredSpecialSlug = null;
			_offeredBlaster = null;
			_offeredArmor = null;
			_offeredAssistant = null;
		}
	}
}
