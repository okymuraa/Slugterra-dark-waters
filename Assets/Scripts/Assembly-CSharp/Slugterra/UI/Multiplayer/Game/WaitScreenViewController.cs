using AppsMinistry.UI;
using Slugterra.Multiplayer.Events;
using UnityEngine;

namespace Slugterra.UI.Multiplayer.Game
{
	public class WaitScreenViewController : UIView
	{
		[SerializeField]
		private GameObject _messageRoot;

		[SerializeField]
		private GameObject _timerRoot;

		[SerializeField]
		private TextMesh _timer;

		private bool _started;

		private bool _countdown;

		private double _startTime;

		protected override void OnStart()
		{
			NetworkEventsDispatcher.OnDuelStartEvent += OnDuelStartEvent;
		}

		private void OnDuelStartEvent(double startTime)
		{
			_startTime = startTime;
			_countdown = true;
			_messageRoot.SetActive(false);
			_timerRoot.SetActive(true);
		}

		private void Update()
		{
			if (!_started && _countdown)
			{
				int num = (int)(_startTime - PhotonNetwork.time);
				if (num <= 0)
				{
					_started = true;
					gameObject.SetActive(false);
				}
				else
				{
					int num2 = num % 60;
					_timer.text = string.Format("00:{0:00}", num2);
				}
			}
		}
	}
}
