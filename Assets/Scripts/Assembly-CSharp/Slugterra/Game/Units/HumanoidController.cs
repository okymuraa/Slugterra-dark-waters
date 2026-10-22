using System;
using System.Collections.Generic;
using AppsMinistry.Core.Sound;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[Serializable]
	public abstract class HumanoidController : UnitController
	{
		[SerializeField]
		private BlasterType _blasterType = BlasterType.Default;

		[SerializeField]
		private int _blasterLevel;

		[SerializeField]
		private ArmorType _armorType;

		[SerializeField]
		private int _armorLevel;

		[SerializeField]
		protected int _intensitySpecialSlug;

		protected int _currectShotCount;

		[SerializeField]
		protected List<SlugType> _availableSlugs;

		protected List<SlugType> _currentSlugMagazine;

		[SerializeField]
		protected List<int> _availableSlugsLevels;

		[SerializeField]
		private Transform _shotPoint;

		[SerializeField]
		private SoundContainer _defaultShotSound;

		[SerializeField]
		private SoundContainer _specialShotSound;

		private float _nextFire;

		public bool DisabledSpecialSlug;

		public BlasterType BlasterType
		{
			get
			{
				return _blasterType;
			}
			set
			{
				_blasterType = value;
			}
		}

		public int BlasterLevel
		{
			get
			{
				return _blasterLevel;
			}
		}

		public ArmorType ArmorType
		{
			get
			{
				return _armorType;
			}
		}

		public int ArmorLevel
		{
			get
			{
				return _armorLevel;
			}
		}

		public int IntensitySpecialSlug
		{
			get
			{
				return _intensitySpecialSlug;
			}
		}

		public List<SlugType> AvailableSlugs
		{
			get
			{
				return _availableSlugs;
			}
		}

		public Transform ShotPoint
		{
			get
			{
				return _shotPoint;
			}
		}

		public float NextFire
		{
			get
			{
				return _nextFire;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (_defaultShotSound != null)
			{
				_defaultShotSound.RegisterInSoundManager();
			}
			if (_specialShotSound != null)
			{
				_specialShotSound.RegisterInSoundManager();
			}
			if ((Model as Humanoid).Blaster != null)
			{
				ViewArea.SetRadius((Model as Humanoid).Blaster.Range + 1f);
			}
			else
			{
				ViewArea.SetRadius(10f);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			OnUseConfigStrategy();
		}

		public void MoveTo(Vector3 newPosition)
		{
			if (NewVersionAI)
			{
				MyBehavior.MoveTo(newPosition);
			}
			else
			{
				Debug.LogWarning("Old AI need to use old functions");
			}
		}

		public void MoveTo(Vector3 newPosition, float speed)
		{
			if (NewVersionAI)
			{
				MyBehavior.MoveTo(newPosition);
				MyBehavior.UseCustomWalkSpeed(speed);
			}
			else
			{
				Debug.LogWarning("Old AI need to use old functions");
			}
		}

		public void MoveTo(Transform target, float speed)
		{
			MoveTo(target.position, speed);
		}

		public void MoveTo(Transform target)
		{
			MoveTo(target.position);
		}

		public bool CanUseSpecialSlug()
		{
			if (_currectShotCount < _intensitySpecialSlug || DisabledSpecialSlug)
			{
				return false;
			}
			return true;
		}

		public SlugType GetShotSlug()
		{
			if (CanUseSpecialSlug() && _availableSlugs.Count > 0)
			{
				_currectShotCount = 0;
				if (_currentSlugMagazine == null || _currentSlugMagazine.Count == 0)
				{
					ReloadSlugMagazine();
				}
				int index = UnityEngine.Random.Range(0, _currentSlugMagazine.Count);
				SlugType result = _currentSlugMagazine[index];
				_currentSlugMagazine.RemoveAt(index);
				return result;
			}
			_currectShotCount++;
			return SlugType.Flopper;
		}

		public void ClearAvailableSlug()
		{
			_availableSlugs.Clear();
			(Model as Humanoid).ClearSlugs();
			_currentSlugMagazine.Clear();
		}

		public void AddSlug(Slug slug)
		{
			if (_availableSlugs == null)
			{
				_availableSlugs = new List<SlugType>();
			}
			(Model as Humanoid).AddSlug(slug);
			_availableSlugs.Add(slug.Type);
		}

		public void RemoveSlug(SlugType type)
		{
			if (_availableSlugs != null && _availableSlugs.Contains(type))
			{
				(Model as Humanoid).RemoveSlug(type);
				_availableSlugs.Remove(type);
			}
		}

		public void ReloadSlugMagazine()
		{
			if (_currentSlugMagazine == null)
			{
				_currentSlugMagazine = new List<SlugType>();
			}
			foreach (SlugType availableSlug in _availableSlugs)
			{
				_currentSlugMagazine.Add(availableSlug);
			}
		}

		public void UpdateBlasterCallDown()
		{
			Humanoid humanoid = Model as Humanoid;
			if (NewVersionAI)
			{
				MyBehavior.AttackSpeedCoefChangged();
			}
			if (!(_nextFire < Time.time))
			{
				float num = _nextFire - Time.time;
				if (num > humanoid.Blaster.FireRate)
				{
					num = humanoid.Blaster.FireRate;
				}
				_nextFire = num * humanoid.CurrentAttackSpeedCoeficient() + Time.time;
			}
		}

		public void UpdateNextFire()
		{
			Humanoid humanoid = Model as Humanoid;
			_nextFire = Time.time + humanoid.Blaster.FireRate * humanoid.CurrentAttackSpeedCoeficient();
		}

		public void ReloadSlug(Slug slug)
		{
			slug.Available = true;
			slug.Reload(Time.time, Model.CurrentAttackSpeedCoeficient());
		}

		public bool IsCanUseSlug(Slug slug)
		{
			if (slug == null)
			{
				return false;
			}
			if (slug.NextFire > Time.time)
			{
				return false;
			}
			return true;
		}

		protected void SetBlasterType(BlasterType newType)
		{
			_blasterType = newType;
		}

		public SoundItem PlaySlugShotSound(SlugType slugType)
		{
			SoundItem soundItem = null;
			if (slugType == SlugType.Flopper)
			{
				return (!(_defaultShotSound != null)) ? soundItem : _defaultShotSound.Play();
			}
			return (!(_specialShotSound != null)) ? soundItem : _specialShotSound.Play();
		}
	}
}
