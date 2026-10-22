using System.Collections;
using AppsMinistry.Core.Sound;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class InteractiveObjType_1 : EnvironmentController
	{
		public float RespawnTime = 20f;

		public MeshRenderer _glow;

		public SkinnedMeshRenderer _glow2;

		public BoxCollider TouchBox;

		public SphereCollider TouchSphere;

		public Transform spawnPoint;

		public GameObject InteractiveFX;

		public GameObject Interactive_Trigger;

		public AnimationClip open;

		public AnimationClip close;

		public AnimationClip start;

		public AnimationClip stop;

		private float maxHealth;

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

		protected override void OnCreate()
		{
			base.OnCreate();
			maxHealth = _model.Health;
			if ((bool)start && open == null)
			{
				if ((bool)_glow)
				{
					_glow.enabled = false;
				}
				if ((bool)_glow2)
				{
					_glow2.enabled = false;
				}
				if ((bool)TouchBox)
				{
					TouchBox.enabled = false;
				}
				if ((bool)TouchSphere)
				{
					TouchSphere.enabled = false;
				}
				StartCoroutine(Respawn(Random.Range(start.length, start.length + 5f)));
			}
			if (_createProgressBar)
			{
				progressBarPrefab = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
				CreateProgressBar();
			}
		}

		private IEnumerator Respawn(float t)
		{
			if ((bool)start)
			{
				StartCoroutine(RespawnAnim(t - start.length, start.name));
			}
			if ((bool)close)
			{
				StartCoroutine(RespawnAnim(t - close.length, close.name));
			}
			yield return new WaitForSeconds(t);
			if (_model.Health != maxHealth)
			{
				_model.Health = maxHealth;
			}
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
				_glow2.enabled = true;
			}
		}

		private IEnumerator RespawnAnim(float t, string clip)
		{
			yield return new WaitForSeconds(t);
			transform.GetComponent<Animation>().Play(clip);
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
			if (RespawnTime == 0f)
			{
				base.OnDamage(damage, parent);
			}
			else
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
					if (RespawnTime != 0f)
					{
						StartCoroutine(Respawn(RespawnTime));
					}
					if ((bool)_glow)
					{
						_glow.enabled = false;
					}
					if ((bool)_glow2)
					{
						_glow2.enabled = false;
					}
					if ((bool)TouchBox)
					{
						TouchBox.enabled = false;
					}
					if ((bool)TouchSphere)
					{
						TouchSphere.enabled = false;
					}
					if ((bool)stop)
					{
						transform.GetComponent<Animation>().Play(stop.name);
					}
					if ((bool)open)
					{
						transform.GetComponent<Animation>().Play(open.name);
					}
					if (_createProgressBar)
					{
						Object.Destroy(_progressBar.gameObject, 1f);
						Object.Destroy(_progressBar, 1f);
					}
					if (spawnPoint == null)
					{
						if ((bool)InteractiveFX)
						{
							Object.Instantiate(InteractiveFX, transform.position, Quaternion.identity);
						}
						if ((bool)Interactive_Trigger)
						{
							Object.Instantiate(Interactive_Trigger, transform.position, Quaternion.identity);
						}
					}
					else
					{
						if ((bool)InteractiveFX)
						{
							Object.Instantiate(InteractiveFX, spawnPoint.position, spawnPoint.rotation);
						}
						if ((bool)Interactive_Trigger)
						{
							Object.Instantiate(Interactive_Trigger, spawnPoint.position, spawnPoint.rotation);
						}
					}
				}
				if (_progressBar != null)
				{
					_progressBar.Progress = _model.Health / _model.MaxHealth;
				}
			}
		}

		public SoundItem PlayExplosionSound()
		{
			return (!(_explosionSound != null)) ? null : _explosionSound.Play();
		}
	}
}
