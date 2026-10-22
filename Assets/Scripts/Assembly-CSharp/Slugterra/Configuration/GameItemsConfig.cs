using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	public class GameItemsConfig
	{
		private HeartConfig _heartConfig;

		private EnergyConfig _energyConfig;

		private MoneyConfig _moneyConfig;

		public HeartConfig HeartConfig
		{
			get
			{
				return _heartConfig;
			}
		}

		public EnergyConfig EnergyConfig
		{
			get
			{
				return _energyConfig;
			}
		}

		public MoneyConfig MoneyConfig
		{
			get
			{
				return _moneyConfig;
			}
		}

		public void Load()
		{
			_energyConfig = Config.LoadTextAsset<EnergyConfig>();
			_heartConfig = Config.LoadTextAsset<HeartConfig>();
			_moneyConfig = Config.LoadTextAsset<MoneyConfig>();
		}

		public void Save()
		{
			_energyConfig.Save();
			_heartConfig.Save();
			_moneyConfig.Save();
		}
	}
}
