using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Ally.Assistant;

namespace Slugterra.Game.Units.ConfigStrategy
{
	public class FollowerConfigStrategy : IUnitConfigStrategy<Humanoid>
	{
		private FollowerController _controller;

		public FollowerConfigStrategy(FollowerController controller)
		{
			_controller = controller;
		}

		public void LoadConfiguration(ref Humanoid model)
		{
			HumanoidLevels alliesConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_controller.Type, _controller.Level);
			model = new Humanoid(alliesConfigurationLevel);
			model.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(alliesConfigurationLevel.ArmorType, alliesConfigurationLevel.ArmorLevel));
			_controller.SetMaxHealth(model.MaxHealth);
		}
	}
}
