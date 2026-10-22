using Slugterra.Game.Item;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class PickUpState : PlayerState
	{
		private Transform _pickUpObject;

		private Animator _animator;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_pickUpObject = (Transform)args[0];
			_animator = Controller.Animator;
			Controller.Animator.SetTrigger("PickUpTrigger");
		}

		protected override void OnUpdate()
		{
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName("Cover_Pistol_Idle"))
			{
				if (_pickUpObject != null)
				{
					ItemController component = _pickUpObject.GetComponent<ItemController>();
					component.PickUp(Controller.Model);
					_pickUpObject = null;
				}
			}
			else if (_pickUpObject == null && !_animator.IsInTransition(0) && !currentAnimatorStateInfo.IsName("Cover_Pistol_Enter") && !currentAnimatorStateInfo.IsName("Cover_Pistol_Exit"))
			{
				StateMachine.NextState<IdleState>(new object[0]);
				Finish();
			}
		}
	}
}
