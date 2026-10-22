using Slugterra.Configuration;

namespace Slugterra.UI.Utils
{
	public static class SpriteNamesHelper
	{
		public static readonly string DummyErrorSprite = "line_red";

		public static readonly string ButtonBuySprite = "btn_orange";

		public static readonly string ButtonUpgradeSprite = "btn_blue";

		public static string GetAssistantSpriteName(AllyType allyType)
		{
			string result = DummyErrorSprite;
			switch (allyType)
			{
			case AllyType.Eli:
				result = "icon_eli";
				break;
			case AllyType.Kord:
				result = "item_companion_Kord";
				break;
			case AllyType.Pronto:
				result = "item_companion_Pronto";
				break;
			case AllyType.Trixie:
				result = "item_companion_Trixie";
				break;
			}
			return result;
		}

		public static string GetConsumableSpriteName(ConsumableType consumableType)
		{
			string result = DummyErrorSprite;
			switch (consumableType)
			{
			case ConsumableType.First:
				result = "item_consumable_health";
				break;
			case ConsumableType.ArmorBoost:
				result = "item_consumable_armor";
				break;
			case ConsumableType.SpeedBoost:
				result = "item_consumable_speed";
				break;
			case ConsumableType.SlugFood:
				result = "item_consumable_slugfood";
				break;
			}
			return result;
		}

		public static string GetFoeSpriteName(EnemyType bossType)
		{
			string result = DummyErrorSprite;
			switch (bossType)
			{
			case EnemyType.BossPronto:
				result = "item_companion_Pronto";
				break;
			case EnemyType.BossKord:
				result = "item_companion_Kord";
				break;
			case EnemyType.BossTrixie:
				result = "item_companion_Trixie";
				break;
			case EnemyType.BossBilly:
				result = "item_rival_Billy";
				break;
			case EnemyType.BossGlasses:
				result = "item_rival_Glasses";
				break;
			case EnemyType.BossLock:
				result = "item_rival_Loke";
				break;
			case EnemyType.BossLode:
				result = "item_rival_Lode";
				break;
			case EnemyType.BossNacho:
				result = "item_rival_Nacho";
				break;
			case EnemyType.BossShorty:
				result = "item_rival_Shorty";
				break;
			case EnemyType.BossTwist:
				result = "item_rival_Twist";
				break;
			case EnemyType.BossSentinelBig:
				result = "item_rival_Sentinel";
				break;
			}
			return result;
		}

		public static string GetCorrecSlugIconName(SlugType slugType)
		{
			string result = string.Empty;
			if (slugType != SlugType.None)
			{
				result = string.Format("icon_slug-{0}", slugType.ToString());
			}
			switch (slugType)
			{
			case SlugType.ArachnetGhoul:
				result = string.Format("icon_slug-{0}", "Attacknet");
				break;
			case SlugType.ArmasheltGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.BlastipedeGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.BoondocGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.CrystalidGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.FandangoGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.FlaringoGhoul:
				result = string.Format("icon_slug-{0}", "Pyringo");
				break;
			case SlugType.FlopperGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.FrightgeistGhoul:
				result = string.Format("icon_slug-{0}", "Nightgeist");
				break;
			case SlugType.FrostcrawlerGhoul:
				result = string.Format("icon_slug-{0}", "Frostfang");
				break;
			case SlugType.GeoshardGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.GrenukeGhoul:
				result = string.Format("icon_slug-{0}", "Greneater");
				break;
			case SlugType.HypnogriffGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.InfurnusGhoul:
				result = string.Format("icon_slug-{0}", "Darkfurnus");
				break;
			case SlugType.NegashadeGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.NeotoxGhoul:
				result = string.Format("icon_slug-{0}", "Neurotox");
				break;
			case SlugType.PhosphoroGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.RammstoneGhoul:
				result = string.Format("icon_slug-{0}", "Grimmstone");
				break;
			case SlugType.SlicksilverGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			case SlugType.TazerlingGhoul:
				result = string.Format("icon_slug-{0}", "Amperling");
				break;
			case SlugType.ThresherGhoul:
				result = string.Format("icon_slug-{0}", "Thrasher");
				break;
			case SlugType.TormatoGhoul:
				result = string.Format("icon_slug-{0}", slugType.ToString());
				break;
			}
			return result;
		}

		public static string GetSlugSpriteName(SlugType slugType)
		{
			if (slugType != SlugType.None)
			{
				return GetCorrecSlugIconName(slugType);
			}
			return DummyErrorSprite;
		}

		public static string GetGameCenterIcon()
		{
			return "icon_playgames";
		}
	}
}
