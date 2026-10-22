using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class FindTargetState : Action
	{
		public UnitControllerVar unitController;

		public ViewAreaTriggerVar viewAreaTrigger;

		public SharedBool isTargetFinded;

		public SharedBool isAlive;

		public SharedString enemyTag;

		private ViewAreaTrigger _viewArea;

		private TaskStatus _currentStatus;

		private UnitController _unitController;

		private bool isSubscribed;

		public override void OnStart()
		{
			_currentStatus = TaskStatus.Running;
			if (_viewArea == null)
			{
				_viewArea = viewAreaTrigger.Value;
			}
			if (_unitController == null)
			{
				_unitController = unitController.Value;
			}
			if (!isSubscribed)
			{
				Subscribe();
				isSubscribed = true;
			}
		}

		private void OnViewTriggerEnter(Transform tr)
		{
			if (tr.tag == enemyTag.Value)
			{
				isTargetFinded.Value = true;
				_unitController.Animator.SetBool("EnemyInSight", true);
			}
		}

		private void OnViewTriggerExit(Transform tr)
		{
			if (tr.tag == enemyTag.Value)
			{
				isTargetFinded.Value = false;
				_unitController.Animator.SetBool("EnemyInSight", false);
			}
		}

		public override TaskStatus OnUpdate()
		{
			return _currentStatus;
		}

		private void Subscribe()
		{
			_viewArea.OnTriggerEnterEvent += OnViewTriggerEnter;
			_viewArea.OnTriggerExitEvent += OnViewTriggerExit;
		}
	}
}
