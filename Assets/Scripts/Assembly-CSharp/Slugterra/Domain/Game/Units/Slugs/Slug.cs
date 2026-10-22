using AppsMinistry.Core;
using AppsMinistry.Domain;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Domain.Game.Units.Slugs
{
	public class Slug : DomainObject
	{
		private float _damage;

		private float _callDown;

		private float _projectileSpeed;

		private float _hittingArea;

		public readonly SlugType SlugType;

		public readonly SlugType GhoulType;

		public readonly int Level;

		public readonly bool IsGhoul;

		private bool _available;

		private float _nextFire;

		public float Damage
		{
			get
			{
				return _damage;
			}
			set
			{
				_damage = value;
			}
		}

		public float CallDown
		{
			get
			{
				return _callDown;
			}
			set
			{
				_callDown = value;
			}
		}

		public float ProjectileSpeed
		{
			get
			{
				return _projectileSpeed;
			}
			set
			{
				_projectileSpeed = value;
			}
		}

		public float HittingArea
		{
			get
			{
				return _hittingArea;
			}
			set
			{
				_hittingArea = value;
			}
		}

		public SlugType Type
		{
			get
			{
				if (IsGhoul)
				{
					return GhoulType;
				}
				return SlugType;
			}
		}

		public bool Available
		{
			get
			{
				return _available;
			}
			set
			{
				_available = value;
			}
		}

		public float NextFire
		{
			get
			{
				return _nextFire;
			}
			set
			{
				_nextFire = value;
			}
		}

		public bool IsSatisfied { get; private set; }

		public Slug(SlugType type = SlugType.None, int level = 0)
		{
			Level = level;
			IsSatisfied = true;
			SlugLevels slugParams = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(type, level);
			SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(type);
			IsGhoul = slugConfiguration.isGhoul;
			GhoulType = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(slugConfiguration.ghoulName);
			SlugType = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugTypeByName(slugConfiguration.slugName);
			if (slugParams != null)
			{
				Damage = slugParams.Damage;
				ProjectileSpeed = slugParams.ProjectileSpeed;
				HittingArea = slugParams.HittingArea;
				CallDown = slugParams.CallDown;
			}
			else
			{
				Debug.LogError(string.Concat("Slug::Slug ", type, " level ", level, " missing in config "));
			}
			_available = true;
		}

		public void UpdateCallDown(float currentTime, float fireCooficient)
		{
			if (!(_nextFire < currentTime))
			{
				float num = _nextFire - currentTime;
				if (num > _callDown)
				{
					num = _callDown;
				}
				_nextFire = num * fireCooficient + currentTime;
			}
		}

		public void Reload(float currentTime, float fireCooficient)
		{
			_nextFire = currentTime + CallDown * fireCooficient;
			IsSatisfied = false;
		}

		public void Clear(float currentTime)
		{
			_nextFire = currentTime;
			IsSatisfied = true;
		}

		public void ResetCallDown(float time)
		{
			_nextFire -= time;
		}
	}
}
