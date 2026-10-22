using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Animals;

namespace Slugterra.Game.Units.ConfigStrategy
{
	public class AnimalConfigStrategy : IUnitConfigStrategy<Animal>
	{
		private AnimalController _controller;

		public AnimalConfigStrategy(AnimalController controller)
		{
			_controller = controller;
		}

		public void LoadConfiguration(ref Animal model)
		{
			AnimalLevels animalsConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAnimalsConfigurationLevel(_controller.mAnimalType, _controller.Level);
			model = new Animal(animalsConfigurationLevel);
			if (!_controller.UseDefaultConfig)
			{
				model.MaxHealth = _controller.MaxHealth;
				model.AddHealth(model.MaxHealth, true);
			}
			_controller.SetMaxHealth(model.MaxHealth);
		}
	}
}
