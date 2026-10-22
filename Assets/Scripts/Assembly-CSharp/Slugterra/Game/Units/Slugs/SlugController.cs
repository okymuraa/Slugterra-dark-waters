using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Sound;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Slugs.Network;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class SlugController : BaseMonoBehaviour
	{
		public const float DefaultLifeTime = 15f;

		public const float FlopperLifeTime = 5f;

		private GameController _gameController;

		[SerializeField]
		private GameObject _collisionParticle;

		[SerializeField]
		private GameObject _particle;

		[SerializeField]
		private GameObject _flare;

		[SerializeField]
		private int _ownerViewID;

		[SerializeField]
		private bool _isSplash;

		[SerializeField]
		private bool _isAutoAttack;

		private HumanoidController _parent;

		private Animator _animator;

		private Slug _model;

		protected ISlugStrategy _strategy;

		private Vector3 _velocity;

		private float _projectileSpeedCoef = 1f;

		[SerializeField]
		private SoundContainer _explosionSound;

		public int OwnerViewID
		{
			get
			{
				return _ownerViewID;
			}
			set
			{
				_ownerViewID = value;
			}
		}

		public bool IsSplash
		{
			get
			{
				return _isSplash;
			}
		}

		public bool IsAutoAttack
		{
			get
			{
				return _isAutoAttack;
			}
			set
			{
				_isAutoAttack = value;
			}
		}

		public HumanoidController Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		public Transform Target { get; set; }

		public Animator Animator
		{
			get
			{
				return _animator;
			}
		}

		public Slug Model
		{
			get
			{
				return _model;
			}
		}

		public virtual float Damage
		{
			get
			{
				return _model.Damage;
			}
		}

		public NetSlugController NetController { get; protected set; }

		public float ProjectileSpeedCoef
		{
			get
			{
				return _projectileSpeedCoef;
			}
			set
			{
				_projectileSpeedCoef = value;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			if (_flare != null)
			{
				_flare.SetActive(false);
			}
			_animator = Transform.GetComponentInChildren<Animator>();
			if ((bool)_animator)
			{
				_animator.SetTrigger("OnFly");
			}
			ParticleLoop(true);
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (_explosionSound != null)
			{
				_explosionSound.RegisterInSoundManager();
			}
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
		}

		public void ParticleLoop(bool _state)
		{
			if (_particle != null)
			{
				_particle.SetActive(_state);
			}
		}

		protected void OnTriggerEnter(Collider collider)
		{
			CheckCollision(collider);
		}

		public virtual void CheckCollision(Collider collider)
		{
			if (collider != null && (_strategy == null || _strategy.OnTriggerEnter(collider.transform)))
			{
				Target = null;
				SelfDestruction();
				if (_gameController.IsMultiplayer)
				{
					OnExplosion();
				}
			}
		}

		public void CheckTransform(Transform curTransform)
		{
			if (curTransform != null && (_strategy == null || _strategy.OnTriggerEnter(curTransform)))
			{
				Target = null;
				SelfDestruction(0.001f);
			}
		}

		protected override void OnUpdate()
		{
			if (_model != null)
			{
				if (_strategy != null)
				{
					_strategy.OnUpdate();
				}
				if (!_isAutoAttack)
				{
					Transform.Translate(_velocity * Time.deltaTime * _model.ProjectileSpeed * _projectileSpeedCoef, Space.World);
				}
			}
		}

		public void OnExplosion()
		{
			if (_collisionParticle != null)
			{
				UnityEngine.Object.Instantiate(_collisionParticle, gameObject.transform.position, Quaternion.identity);
			}
			PlayExplosionSound();
		}

		private SoundItem PlayExplosionSound()
		{
			return (!(_explosionSound != null)) ? null : _explosionSound.Play();
		}

		public void SetSlug(Slug bullet)
		{
			_model = bullet;
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Action)
			{
				if (_model.Type == SlugType.Flopper)
				{
					SelfDestruction(5f);
				}
				else
				{
					SelfDestruction(15f);
				}
			}
		}

		public void SetVelocity(Vector3 velocity)
		{
			_velocity = velocity;
		}

		protected virtual void OnSetSlugStrategy()
		{
		}

		public void SetStrategy(ISlugStrategy strategy)
		{
			_strategy = strategy;
			OnSetSlugStrategy();
		}

		public bool IsGoingToAttackOwner()
		{
			if (Model.IsGhoul && !Model.IsSatisfied)
			{
				return true;
			}
			return false;
		}

		protected override void OnDestroyed()
		{
			if (_model != null && _model.Type != SlugType.Flopper && _model != null && _parent != null)
			{
				_parent.ReloadSlug(_model);
			}
			if (_strategy != null)
			{
				_strategy.OnReleaseResource();
			}
			base.OnDestroyed();
		}

		public virtual void SelfDestruction(float timer = 0f)
		{
			PhotonView component = GetComponent<PhotonView>();
			if (component != null && PhotonNetwork.isMasterClient && timer == 0f)
			{
				PhotonNetwork.networkingPeer.SendDestroySlugEvent(component.viewID, true);
			}
			UnityEngine.Object.Destroy(gameObject, timer);
		}

		public void Disable()
		{
			gameObject.SetActive(false);
			OnDestroyed();
			_strategy = null;
			_model = null;
		}

		private IEnumerator SendDestroySlug(int viewID, float time)
		{
			yield return new WaitForSeconds(time);
			PhotonNetwork.networkingPeer.SendDestroySlugEvent(viewID, true);
		}
	}
}
