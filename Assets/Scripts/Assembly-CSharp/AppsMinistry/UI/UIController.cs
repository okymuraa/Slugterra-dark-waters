using UnityEngine;

namespace AppsMinistry.UI
{
	public class UIController<TView> : MonoBehaviour where TView : UIView
	{
		[SerializeField]
		private TView _view;

		public virtual TView View
		{
			get
			{
				return _view;
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			OnCreate();
		}

		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			OnStart();
		}

		protected virtual void _OnEnable()
		{
		}

		private void OnEnable()
		{
			_OnEnable();
		}

		protected virtual void _OnDisable()
		{
		}

		private void OnDisable()
		{
			_OnDisable();
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			OnUpdate();
		}

		protected virtual void OnReleaseResource()
		{
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}
	}
}
