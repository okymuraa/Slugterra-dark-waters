using AppsMinistry.Core.Command;
using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using Slugterra.Game.Units;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class ExplosionEnvironmentController : EnvironmentController
	{
		[SerializeField]
		private float _range;

		[SerializeField]
		private float _damage;

		[SerializeField]
		private string[] _layers;

		[SerializeField]
		private SoundContainer _explosionSound;

		protected ProgressBarController _progressBar;

		[SerializeField]
		private bool _createProgressBar;

		[SerializeField]
		private Vector3 _porgressBarPosition;

		[SerializeField]
		private Vector3 _progressBarScale;

		private bool exploideAfterDead;

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
			_progressBar.Color = Color.yellow;
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (_explosionSound != null)
			{
				_explosionSound.RegisterInSoundManager();
			}
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
				int[] array = new int[_layers.Length];
				for (int i = 0; i < _layers.Length; i++)
				{
					array[i] = LayerMask.NameToLayer(_layers[i]);
				}
				if (parent != null)
				{
					parent.weaponType = GetType();
				}
				CameraSmoothFollow cameraSmoothFollow = Object.FindObjectOfType<CameraSmoothFollow>();
				if (cameraSmoothFollow != null)
				{
					cameraSmoothFollow.DoShake();
				}
				PlayExplosionSound();
				exploideAfterDead = true;
				Collider component = gameObject.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = false;
				}
				MonoCommand.Execute<AreaDamageCommand>(new object[5]
				{
					gameObject.transform.position,
					_range,
					_damage * 2f,
					array,
					parent
				});
			}
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
		}

		public SoundItem PlayExplosionSound()
		{
			return (!(_explosionSound != null)) ? null : _explosionSound.Play();
		}
	}
}
