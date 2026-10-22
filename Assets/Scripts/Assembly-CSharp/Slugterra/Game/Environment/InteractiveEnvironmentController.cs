using System.Collections.Generic;
using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public abstract class InteractiveEnvironmentController : EnvironmentController
	{
		[SerializeField]
		private string[] _layers;

		protected int[] layers;

		[SerializeField]
		private float _range;

		[SerializeField]
		private SoundContainer _explosionSound;

		protected ProgressBarController _progressBar;

		[SerializeField]
		protected bool _createProgressBar;

		[SerializeField]
		private Vector3 _porgressBarPosition;

		[SerializeField]
		private Vector3 _progressBarScale;

		private List<EnvironmentAbility> _abilities;

		protected bool exploideAfterDead;

		protected override void OnCreate()
		{
			base.OnCreate();
			if (_createProgressBar)
			{
				CreateProgressBar();
			}
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = transform;
			_progressBar.transform.localPosition = _porgressBarPosition;
			_progressBar.transform.localScale = _progressBarScale;
			_progressBar.Progress = _model.Health / _model.MaxHealth;
		}

		protected override void OnStart()
		{
			base.OnStart();
			layers = new int[_layers.Length];
			for (int i = 0; i < _layers.Length; i++)
			{
				layers[i] = LayerMask.NameToLayer(_layers[i]);
			}
			if (_explosionSound != null)
			{
				_explosionSound.RegisterInSoundManager();
			}
			EnvironmentAbility[] components = GetComponents<EnvironmentAbility>();
			_abilities = new List<EnvironmentAbility>(components);
		}

		public override void OnDamage(float damage, Attacker parent = null)
		{
			base.OnDamage(damage, parent);
			if (Model == null)
			{
				return;
			}
			if (Model.IsDead() && !exploideAfterDead)
			{
				if (parent != null)
				{
					parent.weaponType = GetType();
				}
				PlayExplosionSound();
				exploideAfterDead = true;
				int num = 0;
				for (int i = 0; i < layers.Length; i++)
				{
					num |= 1 << layers[i];
				}
				int num2 = 0;
				int num3 = LayerMask.NameToLayer("CoverLayer");
				num2 = 1 << num3;
				Collider[] array = PhysicsFacade.OverlapSphere(base.transform.position, _range, num);
				for (int j = 0; j < array.Length; j++)
				{
					Transform transform = array[j].transform;
					UnitController component = transform.GetComponent<UnitController>();
					if (component == null || component.Model.Invulnerability)
					{
						continue;
					}
					Vector3 position = base.transform.position;
					Vector3 zero = Vector3.zero;
					HitMarker component2 = transform.GetComponent<HitMarker>();
					if (!(component2 != null))
					{
						continue;
					}
					foreach (Transform marker in component2.Markers)
					{
						zero = marker.position - position;
						float magnitude = (marker.position - position).magnitude;
						Ray ray = new Ray(position, zero);
						RaycastHit hitInfo;
						if (!PhysicsFacade.Raycast(ray, out hitInfo, magnitude, num2))
						{
							UseAbilities(component, parent);
							break;
						}
					}
				}
			}
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
		}

		protected void UseAbilities(UnitController unitController, Attacker parent)
		{
			foreach (EnvironmentAbility ability in _abilities)
			{
				ability.ApplyEffect(unitController, parent);
			}
		}

		public SoundItem PlayExplosionSound()
		{
			return (!(_explosionSound != null)) ? null : _explosionSound.Play();
		}
	}
}
