using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class CheckFallTargetState : Conditional
	{
		public BehaviorDataHolderSharedVariable dataHolder;

		private bool isRndGenerate;

		private PlayerController _playerController;

		private bool isSubscribed;

		public override void OnStart()
		{
			if (!isSubscribed)
			{
				dataHolder.Value.Me.ViewAreaTrigger.OnTriggerEnterEvent += OnViewTriggerEnter;
				dataHolder.Value.Me.ViewAreaTrigger.OnTriggerExitEvent += OnViewTriggerExit;
				isSubscribed = true;
			}
		}

		private void OnViewTriggerEnter(Transform tr)
		{
			if (!(tr == null))
			{
				_playerController = tr.GetComponent<PlayerController>();
				if (_playerController != null && _playerController.GetType() != typeof(EliController))
				{
					_playerController = null;
					dataHolder.Value.Enemy.IsReloadSlug = false;
					isRndGenerate = false;
				}
			}
		}

		private void OnViewTriggerExit(Transform tr)
		{
			if (!(tr == null))
			{
				_playerController = null;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (_playerController != null)
			{
				dataHolder.Value.Enemy.IsFallDown = _playerController.IsFallDown;
				return (!dataHolder.Value.Enemy.IsFallDown) ? TaskStatus.Failure : TaskStatus.Success;
			}
			dataHolder.Value.Enemy.IsFallDown = false;
			return TaskStatus.Failure;
		}
	}
}
