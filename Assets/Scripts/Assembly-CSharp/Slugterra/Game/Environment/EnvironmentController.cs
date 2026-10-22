using Slugterra.Domain.Game.Environments;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public abstract class EnvironmentController : MonoBehaviour, IDestructible
	{
		[SerializeField]
		private float _maxHealth = 5f;

		[SerializeField]
		private GameObject[] _fxObject;

		[SerializeField]
		private GameObject[] _spawnObjects;

		[SerializeField]
		protected bool _disposable = true;

		private HitMarker _hitMarker;

		protected Slugterra.Domain.Game.Environments.Environment _model;

		[SerializeField]
		private Renderer _Renderer;

		public Slugterra.Domain.Game.Environments.Environment Model
		{
			get
			{
				return _model;
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			_hitMarker = GetComponent<HitMarker>();
			if (_hitMarker == null)
			{
				_hitMarker = gameObject.AddComponent<HitMarker>();
				Transform transform = new GameObject("HitMarker").transform;
				transform.parent = base.transform;
				transform.localPosition = Vector3.up;
				_hitMarker.Markers.Add(transform);
			}
			_model = new Slugterra.Domain.Game.Environments.Environment(_maxHealth);
			OnCreate();
		}

		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			OnStart();
		}

		public virtual void OnDamage(float damage, Attacker parent = null)
		{
			if (_model != null && !_model.IsDead())
			{
				_model.Health -= damage;
				if (_model.IsDead() && _disposable)
				{
					OnDelete();
				}
			}
		}

		public Renderer GetRenderer()
		{
			return _Renderer;
		}

		private void OnDelete()
		{
			for (int i = 0; i < _fxObject.Length; i++)
			{
				if (_fxObject[i] != null)
				{
					GameObject gameObject = Object.Instantiate(_fxObject[i], transform.position, transform.rotation);
					gameObject.SetActive(true);
				}
			}
			if (_spawnObjects != null && _spawnObjects.Length > 0)
			{
				int num = Random.Range(0, _spawnObjects.Length);
				GameObject gameObject2 = _spawnObjects[num];
				if (gameObject2 != null)
				{
					Object.Instantiate(gameObject2, transform.position, transform.rotation);
				}
			}
			base.gameObject.SetActive(false);
		}

		protected virtual void OnApplyModel(Slugterra.Domain.Game.Environments.Environment model)
		{
		}
	}
}
