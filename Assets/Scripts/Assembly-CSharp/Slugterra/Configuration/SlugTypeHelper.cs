using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public static class SlugTypeHelper
	{
		public static Dictionary<SlugType, SlugGroup> TypeToGroup = new Dictionary<SlugType, SlugGroup>();

		public static SlugGroup GetGroup(SlugType type)
		{
			SlugGroup value;
			if (TypeToGroup.TryGetValue(type, out value))
			{
				return value;
			}
			return SlugGroup.None;
		}

		public static SlugType[] GetSlugsInGroup(SlugGroup group)
		{
			List<SlugType> list = new List<SlugType>();
			foreach (KeyValuePair<SlugType, SlugGroup> item in TypeToGroup)
			{
				if (item.Value == group)
				{
					list.Add(item.Key);
				}
			}
			return list.ToArray();
		}
	}
}
