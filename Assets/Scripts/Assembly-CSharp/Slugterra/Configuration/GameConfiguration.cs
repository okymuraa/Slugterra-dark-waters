using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Configuration;
using Slugterra.Domain.Game.HealthRestoration;

namespace Slugterra.Configuration
{
	[AppSingleton]
	public class GameConfiguration : MonoSingleton<GameConfiguration>
	{
		private WagonConfig _wagonConfig;

		private EnemyWagonConfig _enemyWagonConfig;

		private GameItemsConfig _gameItemsConfig;

		private SlugConfiguration _slugConfig;

		private BlasterConfiguration _blasterConfiguration;

		private ArmorConfiguration _armorConfiguration;

		private UnitConfiguration _unitConfiguration;

		private LevelsConfiguration _levelsConfiguration;

		private MobNamesConfiguration _mobNamesConfiguration;

		private AnnouncementConfiguration _announcementConfiguration;

		private RecommendationConfiguration _recommendationConfiguration;

		private SpecialPacksConfiguration _specialPacksConfiguration;

		private ConsumableConfiguration _consumableConfiguration;

		private RestoreHealthConfiguration _restoreHealthConfiguration;

		private OffersConfiguration _offersConfiguration;

		private bool _isLoaded;

		public WagonConfig WagonConfig
		{
			get
			{
				return _wagonConfig;
			}
		}

		public EnemyWagonConfig EnemyWagonConfig
		{
			get
			{
				return _enemyWagonConfig;
			}
		}

		public GameItemsConfig GameItemsConfig
		{
			get
			{
				return _gameItemsConfig;
			}
		}

		public SlugConfiguration SlugConfigManager
		{
			get
			{
				return _slugConfig;
			}
		}

		public BlasterConfiguration BlasterConfigManager
		{
			get
			{
				return _blasterConfiguration;
			}
		}

		public ArmorConfiguration ArmorConfigManager
		{
			get
			{
				return _armorConfiguration;
			}
		}

		public UnitConfiguration UnitConfigurationManager
		{
			get
			{
				return _unitConfiguration;
			}
		}

		public LevelsConfiguration LevelsConfigurationManager
		{
			get
			{
				return _levelsConfiguration;
			}
		}

		public MobNamesConfiguration MobNamesConfiguration
		{
			get
			{
				return _mobNamesConfiguration;
			}
		}

		public AnnouncementConfiguration AnnouncementConfiguration
		{
			get
			{
				return _announcementConfiguration;
			}
		}

		public RecommendationConfiguration RecommendationConfiguration
		{
			get
			{
				return _recommendationConfiguration;
			}
		}

		public SpecialPacksConfiguration SpecialPacksConfiguration
		{
			get
			{
				return _specialPacksConfiguration;
			}
		}

		public ConsumableConfiguration ConsumableConfigurationManager
		{
			get
			{
				return _consumableConfiguration;
			}
		}

		public RestoreHealthConfiguration RestoreHealthConfigurationManager
		{
			get
			{
				return _restoreHealthConfiguration;
			}
		}

		public OffersConfiguration OffersConfiguration
		{
			get
			{
				return _offersConfiguration;
			}
		}

		protected override void OnCreate()
		{
			_gameItemsConfig = new GameItemsConfig();
			Load();
			_restoreHealthConfiguration.OnUpdateValues += OnRestoreUpdateValuesHandler;
			CheckRestoreConfiguration();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			_restoreHealthConfiguration.OnUpdateValues -= OnRestoreUpdateValuesHandler;
		}

		private void CheckRestoreConfiguration()
		{
			if (_restoreHealthConfiguration.HasItemsToRestore())
			{
				Invoke("UpdateRestoreConfiguration", (float)_restoreHealthConfiguration.GetSmallestRestoreTime().TotalSeconds + 0.1f);
			}
		}

		private void UpdateRestoreConfiguration()
		{
			_restoreHealthConfiguration.RaiseOnUpdateValues();
		}

		private void OnRestoreUpdateValuesHandler()
		{
			CancelInvoke("UpdateRestoreConfiguration");
			CheckRestoreConfiguration();
		}

		private void OnLevelWasLoaded(int level)
		{
			UpdateRestoreConfiguration();
		}

		private void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
			{
				CancelInvoke("UpdateRestoreConfiguration");
			}
			else
			{
				UpdateRestoreConfiguration();
			}
		}

		public void Save()
		{
			_wagonConfig.Save();
			_enemyWagonConfig.Save();
			_gameItemsConfig.Save();
		}

		public void Load()
		{
			if (!_isLoaded)
			{
				_slugConfig = new SlugConfiguration();
				_slugConfig.Initialize();
				_blasterConfiguration = new BlasterConfiguration();
				_blasterConfiguration.Initialize();
				_armorConfiguration = new ArmorConfiguration();
				_armorConfiguration.Initialize();
				_unitConfiguration = new UnitConfiguration();
				_unitConfiguration.Initialize();
				_levelsConfiguration = new LevelsConfiguration();
				_levelsConfiguration.Initialize();
				_mobNamesConfiguration = new MobNamesConfiguration();
				_mobNamesConfiguration.Initialize();
				_consumableConfiguration = new ConsumableConfiguration();
				_consumableConfiguration.Initialize();
				_announcementConfiguration = new AnnouncementConfiguration();
				_announcementConfiguration.Initialize();
				_recommendationConfiguration = new RecommendationConfiguration();
				_recommendationConfiguration.Initialize();
				_specialPacksConfiguration = new SpecialPacksConfiguration();
				_specialPacksConfiguration.Initialize();
				_restoreHealthConfiguration = new RestoreHealthConfiguration();
				_restoreHealthConfiguration.Initialize();
				_offersConfiguration = new OffersConfiguration();
				_offersConfiguration.Initialize();
				_wagonConfig = Config.LoadTextAsset<WagonConfig>();
				_enemyWagonConfig = Config.LoadTextAsset<EnemyWagonConfig>();
				_gameItemsConfig.Load();
				_isLoaded = true;
			}
		}

		public static T ParseEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
	}
}
