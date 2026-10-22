using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class GameLevels
	{
		public List<LevelProperties> levelsProperties = new List<LevelProperties>();

		public GameLevels()
		{
			levelsProperties = new List<LevelProperties>();
		}
	}
}
