namespace Slugterra.Configuration
{
	public class UnitSlugConfigContainer
	{
		public SlugType slugType;

		public int level;

		public UnitSlugConfigContainer(SlugType type, int lvl)
		{
			slugType = type;
			level = lvl;
		}
	}
}
