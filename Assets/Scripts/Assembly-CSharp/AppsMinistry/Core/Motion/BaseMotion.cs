using UnityEngine;

namespace AppsMinistry.Core.Motion
{
	public abstract class BaseMotion : BaseMonoBehaviour
	{
		[SerializeField]
		private float _time;

		[SerializeField]
		private bool _loop;

		[SerializeField]
		private bool _playOnce;

		private float _startTime;

		private bool _finished;

		public float Time
		{
			get
			{
				return _time;
			}
			set
			{
				_time = value;
			}
		}

		public bool Loop
		{
			get
			{
				return _loop;
			}
			set
			{
				_loop = value;
			}
		}

		protected bool PlayOnce
		{
			get
			{
				return _playOnce;
			}
		}

		protected float CurrentTime
		{
			get
			{
				return UnityEngine.Time.unscaledTime - _startTime - (float)Iteration * Time;
			}
		}

		protected int Iteration
		{
			get
			{
				return (int)((UnityEngine.Time.unscaledTime - _startTime) / Time);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_startTime = UnityEngine.Time.unscaledTime;
		}

		protected abstract void OnMotion();

		protected sealed override void OnUpdate()
		{
			base.OnUpdate();
			if (PlayOnce)
			{
				if (Loop && Iteration >= 2)
				{
					Finish();
				}
				else if (!Loop && Iteration >= 1)
				{
					Finish();
				}
			}
			if (!_finished)
			{
				OnMotion();
			}
		}

		public void Finish()
		{
			_finished = true;
			enabled = false;
		}
	}
}
