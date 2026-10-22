using System.Collections;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Game.Triggers;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class ArenaInteractiveEnvironmant : InteractiveEnvironmentController
	{
		public float RespawnTime = 20f;

		[HideInInspector]
		public bool IsReady = true;

		public float Range = 1f;

		public SlugType slugTypeActive;

		public MeshRenderer _glow;

		public SkinnedMeshRenderer _glow2;

		private ParticleSystem _star;

		public BoxCollider TouchBox;

		public SphereCollider TouchSphere;

		public BoxCollider NeutralLayerBox;

		public SphereCollider NeutralLayerSphere;

		public GameObject InteractiveFX;

		public bool AttachFxParent;

		private GameObject cloneFx;

		private OnTriggerEventsSender _interactiveTrigger;

		[SerializeField]
		private GameObject _triggerHolder;

		public bool AlwaysActiveTrigger;

		[SerializeField]
		private float _interactiveTime;

		private float interactiveTimer;

		public Transform spawnPoint;

		public Animation ObjectAnimation;

		public AnimationClip open;

		public AnimationClip close;

		public AnimationClip start;

		public AnimationClip stop;

		public GameObject UseFx;

		public Transform UsePoint;

		private GameController _gameController;

		private EliController _eliController;

		private AssistantController _assistant;

		private int _baseLayer;

		private byte scanStep;

		protected override void OnCreate()
		{
			base.OnCreate();
			_baseLayer = base.gameObject.layer;
			if (!_disposable && (TouchBox != null || TouchSphere != null))
			{
				_gameController = Object.FindObjectOfType<GameController>();
				if (_gameController != null && _gameController.LevelType == LevelType.Action)
				{
					_eliController = Object.FindObjectOfType<EliController>();
					_assistant = Object.FindObjectOfType<AssistantController>();
				}
			}
			if (UseFx != null && UsePoint != null && _gameController != null && _gameController.LevelType == LevelType.Action)
			{
				GameObject gameObject = Object.Instantiate(UseFx, UsePoint.position, Quaternion.identity);
				_star = gameObject.GetComponent<ParticleSystem>();
				_star.transform.parent = UsePoint.transform;
			}
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
				if ((bool)_star)
				{
					_star.maxParticles = 0;
				}
				if ((bool)TouchBox)
				{
					TouchBox.enabled = false;
				}
				if ((bool)TouchSphere)
				{
					TouchSphere.enabled = false;
				}
				if ((bool)NeutralLayerBox)
				{
					NeutralLayerBox.enabled = false;
				}
				if ((bool)NeutralLayerSphere)
				{
					NeutralLayerSphere.enabled = false;
				}
				IsReady = false;
				StartCoroutine(Respawn(Random.Range(start.length, start.length + 5f)));
			}
			if (_triggerHolder != null)
			{
				_interactiveTrigger = _triggerHolder.GetComponent<OnTriggerEventsSender>();
				if (_interactiveTrigger != null && !AlwaysActiveTrigger)
				{
					_interactiveTrigger.Trigger.enabled = false;
				}
			}
			if (_interactiveTime > 0f)
			{
				interactiveTimer = _interactiveTime;
			}
		}

		private void OnEnable()
		{
			if (_interactiveTrigger != null)
			{
				_interactiveTrigger.OnTriggerEnterEvent += OnInteractive;
			}
		}

		private void OnDisable()
		{
			if (_interactiveTrigger != null)
			{
				_interactiveTrigger.OnTriggerEnterEvent -= OnInteractive;
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
			exploideAfterDead = false;
			Model.Health = Model.MaxHealth;
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
			if ((bool)NeutralLayerBox)
			{
				NeutralLayerBox.enabled = true;
			}
			if ((bool)NeutralLayerSphere)
			{
				NeutralLayerSphere.enabled = true;
			}
			if ((bool)_glow)
			{
				_glow.enabled = true;
			}
			if ((bool)_glow2)
			{
				_glow2.enabled = true;
			}
			if ((bool)_star)
			{
				_star.maxParticles = 1;
			}
			IsReady = true;
			gameObject.layer = _baseLayer;
		}

		private IEnumerator RespawnAnim(float t, string clip)
		{
			yield return new WaitForSeconds(t);
			if (ObjectAnimation == null)
			{
				transform.GetComponent<Animation>().Play(clip);
			}
			else
			{
				ObjectAnimation.Play(clip);
			}
		}

		public override void OnDamage(float damage, Attacker parent = null)
		{
			if (_model.IsDead() || Model == null || (parent != null && parent.damageType == DamageType.Splash))
			{
				return;
			}
			if (slugTypeActive != SlugType.None)
			{
				if (slugTypeActive != parent.slugType && slugTypeActive != SlugType.Size)
				{
					ClearTarget();
					return;
				}
				if (slugTypeActive == SlugType.Size && parent.slugType == SlugType.Flopper)
				{
					ClearTarget();
					return;
				}
			}
			if (Model.Health == 1f && _interactiveTrigger == null)
			{
				base.OnDamage(1f, parent);
			}
			Model.Health--;
			if (_model.Health <= 0f)
			{
				if (RespawnTime != 0f && !_disposable)
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
				if ((bool)_star)
				{
					_star.maxParticles = 0;
				}
				if ((bool)TouchBox)
				{
					TouchBox.enabled = false;
				}
				if ((bool)TouchSphere)
				{
					TouchSphere.enabled = false;
				}
				if ((bool)NeutralLayerBox)
				{
					NeutralLayerBox.enabled = false;
				}
				if ((bool)NeutralLayerSphere)
				{
					NeutralLayerSphere.enabled = false;
				}
				IsReady = false;
				if (parent != null && parent.transform != null)
				{
					UnitController component = parent.transform.GetComponent<UnitController>();
					if (component != null && component.AttackTarget == transform)
					{
						component.AttackTarget = null;
					}
				}
				gameObject.layer = LayerMask.NameToLayer("Default");
				PlayExplosionSound();
				if ((bool)stop)
				{
					if (ObjectAnimation == null)
					{
						transform.GetComponent<Animation>().Play(stop.name);
					}
					else
					{
						ObjectAnimation.Play(stop.name);
					}
				}
				if ((bool)open)
				{
					if (ObjectAnimation == null)
					{
						transform.GetComponent<Animation>().Play(open.name);
					}
					else
					{
						ObjectAnimation.Play(open.name);
					}
				}
				if (_createProgressBar)
				{
					Object.Destroy(_progressBar.gameObject, 1f);
					Object.Destroy(_progressBar, 1f);
				}
				if (_interactiveTrigger != null)
				{
					_interactiveTrigger.Trigger.enabled = true;
					StartCoroutine(DisableInterectiveTrigger(_interactiveTime));
				}
				if (spawnPoint == null)
				{
					if ((bool)InteractiveFX)
					{
						Object.Instantiate(InteractiveFX, transform.position, Quaternion.identity);
					}
				}
				else if ((bool)InteractiveFX)
				{
					cloneFx = Object.Instantiate(InteractiveFX, spawnPoint.position, spawnPoint.rotation);
					if (AttachFxParent)
					{
						cloneFx.transform.parent = spawnPoint;
					}
				}
			}
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
		}

		private IEnumerator DisableInterectiveTrigger(float time)
		{
			yield return new WaitForSeconds(time);
			_interactiveTrigger.Trigger.enabled = false;
		}

		private void FixedUpdate()
		{
			if (!AlwaysActiveTrigger || !(_triggerHolder != null))
			{
				return;
			}
			interactiveTimer -= Time.deltaTime;
			if (!_disposable)
			{
				scanStep++;
				if (scanStep > 3)
				{
					scanStep = 0;
					if (_interactiveTrigger.Trigger.gameObject.activeSelf)
					{
						_interactiveTrigger.Trigger.gameObject.SetActive(false);
					}
					else
					{
						_interactiveTrigger.Trigger.gameObject.SetActive(true);
					}
				}
			}
			if (_interactiveTime > 0f && interactiveTimer < 0f)
			{
				_interactiveTrigger.Trigger.gameObject.SetActive(false);
				AlwaysActiveTrigger = false;
			}
		}

		private void ClearTarget()
		{
			if (!_disposable && (!(TouchBox == null) || !(TouchSphere == null)))
			{
				if (_eliController != null && _eliController.AttackTarget != null && _eliController.AttackTarget == transform)
				{
					_eliController.AttackTarget = null;
				}
				if (_assistant != null && _assistant.AttackTarget == transform)
				{
					_assistant.AttackTarget = null;
				}
			}
		}

		private void OnInteractive(Collider collider)
		{
			UnitController component = collider.GetComponent<UnitController>();
			if (component != null)
			{
				UseAbilities(component, null);
				if (_triggerHolder != null && _disposable && AlwaysActiveTrigger)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}
}
