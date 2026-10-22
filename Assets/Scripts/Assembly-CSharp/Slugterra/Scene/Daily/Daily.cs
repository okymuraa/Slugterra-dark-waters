using System;

namespace Slugterra.Scene.Daily
{
	public class Daily
	{
		public Type Geometry;

		public Type Logic;

		public Daily(Type geometry, Type logic)
		{
			Geometry = geometry;
			Logic = logic;
		}
	}
}
