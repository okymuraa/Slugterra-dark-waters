using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Equipments;
using Slugterra.Domain.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Domain.Game.Units
{
	public class Humanoid : Unit
	{
		[JsonProperty]
		private string _avatar = "icon_pronto";

		[JsonProperty]
		private float _meleeDamage;

		[JsonProperty]
		private bool _reloadInMotion;

		[JsonProperty]
		private IDictionary<SlugType, Slug> _slugs;

		[JsonProperty]
		private Blaster _blaster;

		[JsonProperty]
		private Armor _armor;

		[JsonIgnore]
		public override float CurrentSpeed
		{
			get
			{
				if (_armor != null)
				{
					return base.CurrentSpeed * _armor.SpeedRatio;
				}
				return base.CurrentSpeed;
			}
			set
			{
				base.CurrentSpeed = value;
			}
		}

		[JsonIgnore]
		public string Avatar
		{
			get
			{
				return _avatar;
			}
		}

		[JsonIgnore]
		public float MeleeDamage
		{
			get
			{
				return _meleeDamage;
			}
		}

		[JsonIgnore]
		public bool ReloadInMotion
		{
			get
			{
				return _reloadInMotion;
			}
			set
			{
				_reloadInMotion = value;
			}
		}

		[JsonIgnore]
		public IDictionary<SlugType, Slug> Slugs
		{
			get
			{
				return _slugs;
			}
		}

		[JsonIgnore]
		public Blaster Blaster
		{
			get
			{
				return _blaster;
			}
			set
			{
				_blaster = value;
			}
		}

		[JsonIgnore]
		public Armor Armor
		{
			get
			{
				return _armor;
			}
			set
			{
				_armor = value;
			}
		}

		[JsonConstructor]
		protected Humanoid()
		{
		}

		public Humanoid(HumanoidLevels config)
			: base(config)
		{
			_slugs = new Dictionary<SlugType, Slug>();
			_slugs.Add(SlugType.Flopper, new Slug(SlugType.Flopper));
			_meleeDamage = config.MeleeDamage;
			_avatar = config.Avatar;
		}

		public override void ChangeConfig(BaseUnitConfig config)
		{
			base.ChangeConfig(config);
			_meleeDamage = (config as HumanoidLevels).MeleeDamage;
			_avatar = (config as HumanoidLevels).Avatar;
		}

		public void AddSlug(Slug slug)
		{
			_slugs[slug.Type] = slug;
		}

		public void RemoveSlug(SlugType type)
		{
			if (_slugs.ContainsKey(type))
			{
				_slugs.Remove(type);
			}
		}

		public void ClearSlugs()
		{
			_slugs.Clear();
		}

		public Slug GetSlug(SlugType slugType)
		{
			if (_slugs.ContainsKey(slugType))
			{
				return _slugs[slugType];
			}
			return null;
		}

		public void SetBlaster(BlasterLevels config, BlasterType type)
		{
			_blaster = new Blaster(config, type);
			if (!_slugs.ContainsKey(SlugType.Flopper))
			{
				_slugs.Add(SlugType.Flopper, new Slug(SlugType.Flopper));
			}
			Slug slug = _slugs[SlugType.Flopper];
			slug.CallDown = _blaster.FireRate;
			slug.Damage = _blaster.DamagePerHit;
			slug.ProjectileSpeed = _blaster.SlugSpeed;
		}

		public void SetArmor(ArmorLevels config)
		{
			_armor = new Armor(config);
		}

		public void Clear(float currentTime)
		{
			foreach (SlugType key in _slugs.Keys)
			{
				_slugs[key].Clear(currentTime);
			}
		}

		public void ReloadSlugs(float currentTime)
		{
			foreach (SlugType key in _slugs.Keys)
			{
				_slugs[key].Reload(currentTime, CurrentAttackSpeedCoeficient());
			}
		}

		private void UpdateSlugsCalldown(float currentTime)
		{
			foreach (SlugType key in _slugs.Keys)
			{
				_slugs[key].UpdateCallDown(currentTime, CurrentAttackSpeedCoeficient());
			}
		}

		public void ResetCallDown(float time)
		{
			foreach (SlugType key in _slugs.Keys)
			{
				_slugs[key].ResetCallDown(time);
			}
		}

		public override float CurrentDamageCoeficient()
		{
			if (_armor != null)
			{
				return base.CurrentDamageCoeficient() * (1f - _armor.DamageAbsorption);
			}
			return base.CurrentDamageCoeficient();
		}

		public override void AddAttackSpeedCoeficient(Type type, float coeficient)
		{
			base.AddAttackSpeedCoeficient(type, coeficient);
			UpdateSlugsCalldown(Time.time);
		}

		public override void RemoveAttackSpeedCoeficient(Type type)
		{
			base.RemoveAttackSpeedCoeficient(type);
			UpdateSlugsCalldown(Time.time);
		}
	}
}
