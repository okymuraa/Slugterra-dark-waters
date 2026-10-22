using Slugterra.Domain.Game.Units;

namespace Slugterra.Game.Units.ConfigStrategy
{
	public interface IUnitConfigStrategy<T> where T : Unit
	{
		void LoadConfiguration(ref T model);
	}
}
