using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class InteractiveObjType_2 : EnvironmentController
	{
		public GameObject mostController;

		public SkinnedMeshRenderer _glow;

		public GameObject _glow2;

		public BoxCollider TouchBox;

		public SphereCollider TouchSphere;

		public GameObject FX;

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

		public ArenaInteractiveEnvironmant temp;

		public string Message = "Temp";

		private int _baseLayer;

		protected override void OnCreate()
		{
			base.OnCreate();
			_baseLayer = gameObject.layer;
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
			if ((bool)_glow)
			{
				_glow.enabled = true;
			}
			if ((bool)_glow2)
			{
				_glow2.SetActive(true);
			}
			if (temp != null)
			{
				temp.IsReady = true;
			}
			gameObject.layer = _baseLayer;
		}

		public void Respawn_NotAnim()
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
			if ((bool)_glow)
			{
				_glow.enabled = true;
			}
			if ((bool)_glow2)
			{
				_glow2.SetActive(true);
			}
			if (temp != null)
			{
				temp.IsReady = true;
			}
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
			if (_model.Health <= 0f)
			{
				if ((bool)_glow)
				{
					_glow.enabled = false;
				}
				if ((bool)_glow2)
				{
					_glow2.SetActive(false);
				}
				if ((bool)TouchBox)
				{
					TouchBox.enabled = false;
				}
				if ((bool)TouchSphere)
				{
					TouchSphere.enabled = false;
				}
				gameObject.layer = LayerMask.NameToLayer("Default");
				if (temp != null)
				{
					temp.IsReady = false;
				}
				transform.GetComponent<Animation>().CrossFade("error_weit");
				if ((bool)mostController)
				{
					mostController.SendMessage(Message);
				}
				PlayExplosionSound();
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
