using AppsMinistry.Core;
using Slugterra.Domain.Game.Mission;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ComeBackObjectiveTarget : ObjectiveTarget
	{
		[SerializeField]
		private float _timer = 10f;

		[SerializeField]
		private Transform _detectObject;

		private float _startTime;

		private bool _inTrigger;

		private bool _isTimerOn;

		private GameController _gameController;

		private InGameView _inGameView;

		private CountdownWarningMessageView _countdownMessage;

		[SerializeField]
		protected string _descriptionKeyMessage;

		private string _message = string.Empty;

		private bool IsTimerOn
		{
			get
			{
				return _isTimerOn;
			}
			set
			{
				if (_isTimerOn != value)
				{
					_isTimerOn = value;
					_countdownMessage.Show(_isTimerOn);
					if (_isTimerOn)
					{
						_startTime = Time.time;
					}
				}
			}
		}

		public void Reset()
		{
			if (_isTimerOn)
			{
				_startTime = Time.time;
				_countdownMessage.Show(true);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_inGameView = Object.FindObjectOfType<InGameView>();
			_countdownMessage = _inGameView.CountdownWarningMessage;
			_gameController = Object.FindObjectOfType<GameController>();
			_message = MonoSingleton<Localization>.Instance[_descriptionKeyMessage];
		}

		protected override void OnTriggerEnter(Collider item)
		{
			if (item.transform == _detectObject)
			{
				_inTrigger = true;
				if (IsTimerOn)
				{
					IsTimerOn = false;
				}
			}
		}

		protected override void OnTriggerExit(Collider item)
		{
			if (!(item.transform != _detectObject))
			{
				_inTrigger = false;
				if (Model.IsActive)
				{
					IsTimerOn = true;
				}
			}
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
			if ((observable as Objective).Equals(Model) && Model.IsActive && !_inTrigger && !IsTimerOn)
			{
				IsTimerOn = true;
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (IsTimerOn && Model.IsActive)
			{
				if (_descriptionKey != null)
				{
					_countdownMessage.Description = _message;
				}
				float num = Time.time - _startTime;
				_countdownMessage.Timer.text = string.Format("{0}", (int)(_timer - num + 1f));
				if (_timer <= num)
				{
					_gameController.OnFinishGame(false);
					_countdownMessage.Show(false);
				}
			}
		}
	}
}
