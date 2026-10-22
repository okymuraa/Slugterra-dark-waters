using System.Collections.Generic;
using Newtonsoft.Json;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Game;

namespace Slugterra.Configuration
{
	public class Offer
	{
		public string Mission;

		public string _attackSlug;

		public int AttackSlugLevel;

		public string _defenceSlug;

		public int DefenceSlugLevel;

		public string _specialSlug;

		public int SpecialSlugLevel;

		public string _blaster;

		public string _armor;

		public string _assistant;

		public int AssistantLevel;

		[JsonIgnore]
		public SlugType AttackSlug
		{
			get
			{
				return StringToSlugType(_attackSlug);
			}
		}

		[JsonIgnore]
		public SlugType DefenceSlug
		{
			get
			{
				return StringToSlugType(_defenceSlug);
			}
		}

		[JsonIgnore]
		public SlugType SpecialSlug
		{
			get
			{
				return StringToSlugType(_specialSlug);
			}
		}

		[JsonIgnore]
		public BlasterType Blaster
		{
			get
			{
				for (int i = 0; i < 18; i++)
				{
					string blaster = _blaster;
					BlasterType blasterType = (BlasterType)i;
					if (blaster == blasterType.ToString())
					{
						return (BlasterType)i;
					}
				}
				return BlasterType.None;
			}
		}

		[JsonIgnore]
		public ArmorType Armor
		{
			get
			{
				for (int i = 0; i < 5; i++)
				{
					string armor = _armor;
					ArmorType armorType = (ArmorType)i;
					if (armor == armorType.ToString())
					{
						return (ArmorType)i;
					}
				}
				return ArmorType.None;
			}
		}

		[JsonIgnore]
		public AllyType Assistant
		{
			get
			{
				for (int i = 0; i < 6; i++)
				{
					string assistant = _assistant;
					AllyType allyType = (AllyType)i;
					if (assistant == allyType.ToString())
					{
						return (AllyType)i;
					}
				}
				return AllyType.None;
			}
		}

		public Offer()
		{
			Mission = typeof(QuietLawn_2_5).Name;
			_attackSlug = SlugType.None.ToString();
			AttackSlugLevel = 0;
			_defenceSlug = SlugType.None.ToString();
			DefenceSlugLevel = 0;
			_specialSlug = SlugType.None.ToString();
			SpecialSlugLevel = 0;
			_blaster = BlasterType.None.ToString();
			_armor = ArmorType.None.ToString();
			_assistant = AllyType.None.ToString();
			AssistantLevel = 0;
		}

		private SlugType StringToSlugType(string name)
		{
			for (int i = 0; i < 45; i++)
			{
				SlugType slugType = (SlugType)i;
				if (name == slugType.ToString())
				{
					return (SlugType)i;
				}
			}
			return SlugType.None;
		}

		public List<object> GetListOfferedItems()
		{
			List<object> list = new List<object>();
			if (AttackSlug != SlugType.None)
			{
				list.Add(new SlugItem(AttackSlug, AttackSlugLevel));
			}
			if (DefenceSlug != SlugType.None)
			{
				list.Add(new SlugItem(DefenceSlug, DefenceSlugLevel));
			}
			if (SpecialSlug != SlugType.None)
			{
				list.Add(new SlugItem(SpecialSlug, SpecialSlugLevel));
			}
			if (Blaster != BlasterType.None)
			{
				list.Add(new BlasterItem(Blaster));
			}
			if (Armor != ArmorType.None)
			{
				list.Add(new ArmorItem(Armor, 0, false, true));
			}
			if (Assistant != AllyType.None)
			{
				list.Add(new AssistantItem(Assistant, AssistantLevel, false, true));
			}
			return list;
		}
	}
}
