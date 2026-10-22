using System;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Game
{
	[Serializable]
	public class Attacker
	{
		public Type type;

		public Transform transform;

		public Type weaponType;

		public SlugType slugType;

		public DamageType damageType;

		public Transform hitTransform;

		public Attacker(Type type, Transform transform, Type weaponType = null, SlugType slugType = SlugType.None, DamageType damageType = DamageType.Direct, Transform hitTransform = null)
		{
			this.type = type;
			this.transform = transform;
			this.weaponType = weaponType;
			this.slugType = slugType;
			this.damageType = damageType;
			this.hitTransform = hitTransform;
		}
	}
}
