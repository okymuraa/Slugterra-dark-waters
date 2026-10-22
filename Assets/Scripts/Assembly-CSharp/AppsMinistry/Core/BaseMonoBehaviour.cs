using System;
using UnityEngine;

namespace AppsMinistry.Core
{
	[Serializable]
	public abstract class BaseMonoBehaviour : MonoBehaviour
	{
		private GameObject _gameObject;

		private Transform _transform;

		public GameObject GameObject
		{
			get
			{
				return _gameObject;
			}
		}

		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		protected virtual void OnCreate()
		{
		}

		protected virtual void OnEnabled()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnDisabled()
		{
		}

		protected virtual void OnDestroyed()
		{
		}

		private void Awake()
		{
			_gameObject = gameObject;
			_transform = transform;
			OnCreate();
		}

		private void OnEnable()
		{
			OnEnabled();
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
		}

		private void OnDisable()
		{
			OnDisabled();
		}

		private void OnDestroy()
		{
			OnDestroyed();
		}
	}
}
