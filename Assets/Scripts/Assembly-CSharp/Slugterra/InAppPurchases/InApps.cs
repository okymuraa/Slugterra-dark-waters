using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.InAppPurchases;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.InAppPurchases
{
	public static class InApps
	{
		private static List<InAppPurchase> _allInAPPS = new List<InAppPurchase>();

		public static readonly List<InAppPurchase> GOLD_INAPPS = new List<InAppPurchase>
		{
			new InAppPurchase("com.appsministry.slugterra.gold_pack_1", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 5000f;
			}, "1000_gc_title")
			{
				GoldValue = 5000,
				StoreSpriteName = "item_coins_01"
			},
			new InAppPurchase("com.appsministry.slugterra.gold_pack_3", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 18000f;
			}, "1000_gc_title")
			{
				GoldValue = 18000,
				StoreSpriteName = "item_coins_03"
			},
			new InAppPurchase("com.appsministry.slugterra.gold_pack_6", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 130000f;
			}, "1000_gc_title")
			{
				GoldValue = 130000,
				StoreSpriteName = "item_coins_06"
			},
			new InAppPurchase("com.appsministry.slugterra.gold_pack_2", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 8500f;
			}, "1000_gc_title")
			{
				GoldValue = 8500,
				StoreSpriteName = "item_coins_02"
			},
			new InAppPurchase("com.appsministry.slugterra.gold_pack_4", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 40000f;
			}, "1000_gc_title")
			{
				GoldValue = 40000,
				StoreSpriteName = "item_coins_04"
			},
			new InAppPurchase("com.appsministry.slugterra.gold_pack_5", InAppType.Consumable, InAppPurchase.InAppCategory.Coins, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gold += 55000f;
			}, "1000_gc_title")
			{
				GoldValue = 55000,
				StoreSpriteName = "item_coins_05"
			}
		};

		public static readonly List<InAppPurchase> TICKETS_INAPPS = new List<InAppPurchase>
		{
			new InAppPurchase("com.appsministry.slugterra.5mptickets", InAppType.Consumable, InAppPurchase.InAppCategory.Tickets, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Ticket += 10f;
			}, "5_tickets_title")
			{
				TicketValue = 5,
				StoreSpriteName = "item_coins_01"
			}
		};

		public static readonly List<InAppPurchase> GEMS_INAPPS = new List<InAppPurchase>
		{
			new InAppPurchase("com.appsministry.slugterra.gem_pack_1", InAppType.Consumable, InAppPurchase.InAppCategory.Gems, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gems += 1500f;
			}, string.Empty)
			{
				GemsValue = 1500,
				StoreSpriteName = "item_gems_01"
			},
			new InAppPurchase("com.appsministry.slugterra.gem_pack_3", InAppType.Consumable, InAppPurchase.InAppCategory.Gems, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gems += 7000f;
			}, string.Empty)
			{
				GemsValue = 7000,
				StoreSpriteName = "item_gems_03"
			},
			new InAppPurchase("com.appsministry.slugterra.gem_pack_5", InAppType.Consumable, InAppPurchase.InAppCategory.Gems, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gems += 25000f;
			}, string.Empty)
			{
				GemsValue = 25000,
				StoreSpriteName = "item_gems_05"
			},
			new InAppPurchase("com.appsministry.slugterra.gem_pack_2", InAppType.Consumable, InAppPurchase.InAppCategory.Gems, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gems += 3000f;
			}, string.Empty)
			{
				GemsValue = 3000,
				StoreSpriteName = "item_gems_02"
			},
			new InAppPurchase("com.appsministry.slugterra.gem_pack_4", InAppType.Consumable, InAppPurchase.InAppCategory.Gems, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.Wallet.Gems += 15000f;
			}, string.Empty)
			{
				GemsValue = 15000,
				StoreSpriteName = "item_gems_04"
			}
		};

		public static readonly List<InAppPurchase> SUBSCRIPTION_INAPPS = new List<InAppPurchase>
		{
			new InAppPurchase("com.appsministry.slugterra.2x_multiplier", InAppType.Subscriptions, InAppPurchase.InAppCategory.Subscription, market, (int expirationTimeStamp) =>
			{
				OnBuySubscription("com.appsministry.slugterra.2x_multiplier", expirationTimeStamp);
			}, string.Empty)
		};

		public static readonly List<InAppPurchase> BLASTERS_INAPPS = new List<InAppPurchase>
		{
			new InAppPurchase("com.appsministry.slugterra.blaster_premium_01", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_premium_02", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_premium_03", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_glasses", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_lode", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_nacho", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumNacho).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_twist", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumTwist).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_mdrake", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumMDrake).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_bdrake", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBDrake).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.blaster_blakk", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBlakk).IsBought = true;
			}, string.Empty)
			{
				Category = InAppPurchase.InAppCategory.Blasters
			},
			new InAppPurchase("com.appsministry.slugterra.3billygang_blasters", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.2billygang_1_blasters", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.2billygang_2_blasters", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.2billygang_3_blasters", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.loke_lode_blasters", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.bandits_arsenal", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.blakk_henchmen_arsenal", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumNacho).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.abordage_blaster_set", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumMDrake).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBDrake).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.ghoul_masters_set", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumNacho).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumTwist).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBlakk).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.super_blaster_set", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBlakk).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumNacho).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumTwist).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumMDrake).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBDrake).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumFirst).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumGlasses).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumThird).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.bad_brothers_set", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumSecond).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumLode).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumMDrake).IsBought = true;
				MonoSingleton<AppController>.Instance.Model.ShopModel.GetBlaster(BlasterType.PremiumBDrake).IsBought = true;
			}, string.Empty),
			new InAppPurchase("com.appsministry.slugterra.2x_multiplier", InAppType.NonConsumable, InAppPurchase.InAppCategory.Blasters, market, (int expirationTimeStamp) =>
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier = 2;
			}, string.Empty)
		};

		public const string SUBSCRIPTION_WEEK = "com.appsministry.slugterra.sub1week";

		public const string SUBSCRIPTION_MONTH = "com.appsministry.slugterra.sub2_1month";

		public const string SUBSCRIPTION_THREE_MONTH = "com.appsministry.slugterra.sub3_3month";

		public const string SUBSCRIPTION_SLAG_WEEK = "com.appsministry.slugterra.darkfurnus_sub";

		public const string SUBSCRIPTION_MULTIPLIER2X = "com.appsministry.slugterra.2x_multiplier";

		public static InAppMarket market
		{
			get
			{
				return InAppMarket.GoogleMarket;
			}
		}

		public static List<InAppPurchase> ALL_INAPPS
		{
			get
			{
				if (_allInAPPS.Count == 0)
				{
					_allInAPPS.AddRange(GOLD_INAPPS);
					_allInAPPS.AddRange(GEMS_INAPPS);
					_allInAPPS.AddRange(SUBSCRIPTION_INAPPS);
					_allInAPPS.AddRange(BLASTERS_INAPPS);
					_allInAPPS.AddRange(TICKETS_INAPPS);
				}
				return _allInAPPS;
			}
		}

		private static void OnBuySubscription(string productID, int expirationTimeStamp)
		{
			MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier = 2;
			DateTime dateTime = WWWUtils.UnixTimeStampToDateTime(expirationTimeStamp);
			Debug.LogFormat("Start BuySubscription {0}, expired {1} ({2}); Current Date {3}", productID, expirationTimeStamp, dateTime, DateTime.Now);
			DateTime dateTime2 = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
			int num = (int)WWWUtils.ConvertToTimestamp(dateTime2);
			Debug.LogFormat("After Correction BuySubscription {0}, expired {1} ({2}); Current Date {3}", productID, num, dateTime2, DateTime.Now);
			DonDestroyMonoSingleton<SubscriptionManager>.Instance.OnSubscriptionPurchased(productID, num);
		}
	}
}
