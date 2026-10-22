using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class InteractiveObjGame : EnvironmentController
	{
		public GameObject mostController;

		public SkinnedMeshRenderer _glow;

		private ParticleSystem _star;

		public BoxCollider TouchBox;

		public SphereCollider TouchSphere;

		public GameObject FX;

		private EliController _eliController;

		protected ProgressBarController _progressBar;

		[SerializeField]
		private bool _createProgressBar;

		[SerializeField]
		private Vector3 _porgressBarPosition;

		[SerializeField]
		private Vector3 _progressBarScale;

		private GameObject progressBarPrefab;

		[SerializeField]
		private SoundContainer _explosionSound;

		public GameObject UseFx;

		public Transform UsePoint;

		private AssistantController[] _assistant;

		private int _baseLayer;

		protected override void OnCreate()
		{
			base.OnCreate();
			_baseLayer = base.gameObject.layer;
			_eliController = Object.FindObjectOfType<EliController>();
			_assistant = Object.FindObjectsOfType<AssistantController>();
			if (UseFx != null && UsePoint != null)
			{
				GameObject gameObject = Object.Instantiate(UseFx, UsePoint.position, Quaternion.identity);
				_star = gameObject.GetComponent<ParticleSystem>();
				_star.transform.parent = UsePoint.transform;
			}
			if (_createProgressBar)
			{
				progressBarPrefab = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
				CreateProgressBar();
			}
		}

		protected override void OnStart()
		{
			if (_explosionSound != null)
			{
				_explosionSound.RegisterInSoundManager();
			}
		}

		public void Respawn()
		{
			_model.Health = _model.MaxHealth;
			if (_createProgressBar)
			{
				CreateProgressBar();
			}
			if ((bool)TouchBox)
			{
				TouchBox.enabled = true;
			}
			if ((bool)TouchSphere)
			{
				TouchSphere.enabled = true;
			}
			transform.GetComponent<Animation>().Stop();
			_glow.enabled = true;
			gameObject.layer = _baseLayer;
		}

		protected void CreateProgressBar()
		{
			_progressBar = Object.Instantiate(progressBarPrefab).GetComponent<ProgressBarController>();
			_progressBar.gameObject.transform.GetChild(0).gameObject.layer = 30;
			_progressBar.transform.parent = transform;
			_progressBar.transform.localPosition = _porgressBarPosition;
			_progressBar.transform.localScale = _progressBarScale;
			_progressBar.Progress = _model.Health / _model.MaxHealth;
			_progressBar.Color = Color.yellow;
		}

		public override void OnDamage(float damage, Attacker parent = null)
		{
			if (Model == null || _model.Health <= 0f || (parent != null && parent.damageType == DamageType.Splash))
			{
				return;
			}
			if (_createProgressBar)
			{
				_progressBar.gameObject.transform.GetChild(0).gameObject.layer = 1;
			}
			_model.Health--;
			if (_model.Health == 0f)
			{
				_glow.enabled = false;
				if ((bool)TouchBox)
				{
					TouchBox.enabled = false;
				}
				if ((bool)TouchSphere)
				{
					TouchSphere.enabled = false;
				}
				gameObject.layer = LayerMask.NameToLayer("Default");
				if (_eliController.AttackTarget != null && _eliController.AttackTarget == transform)
				{
					_eliController.AttackTarget = null;
				}
				if (_assistant.Length > 0)
				{
					for (int i = 0; i < _assistant.Length; i++)
					{
						if (_assistant[i].AttackTarget == transform)
						{
							_assistant[i].AttackTarget = null;
						}
					}
				}
				if (_star != null)
				{
					_star.maxParticles = 0;
				}
				transform.GetComponent<Animation>().Play("error_weit");
				PlayExplosionSound();
				if ((bool)mostController)
				{
					mostController.SendMessage("Temp");
				}
				if (_createProgressBar)
				{
					Object.Destroy(_progressBar.gameObject, 1f);
					Object.Destroy(_progressBar, 1f);
				}
				if ((bool)FX)
				{
					Object.Instantiate(FX, transform.position, Quaternion.identity);
				}
			}
			if (_createProgressBar && _progressBar != null)
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
