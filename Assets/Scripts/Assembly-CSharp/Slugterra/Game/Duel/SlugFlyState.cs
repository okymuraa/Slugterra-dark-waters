using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Input;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Units.Slugs;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.Game.Duel
{
	[PersistentCommand]
	public class SlugFlyState : DuelState
	{
		private bool _isLeftArrow;

		private bool _isRightArrow;

		private InGameModel _inGameModel;

		private SlugController slug;

		private InGameModel InGameModel
		{
			get
			{
				if (_inGameModel == null)
				{
					InGameController inGameController = Object.FindObjectOfType<InGameController>();
					if (inGameController != null)
					{
						_inGameModel = inGameController.Model;
					}
				}
				return _inGameModel;
			}
		}

		protected override void OnStart(object[] args)
		{
			if (args != null && args.Length > 0 && args[0] != null)
			{
				slug = args[0] as SlugController;
			}
			if (!(slug == null) && !(slug.Animator == null))
			{
				slug.Animator.SetTrigger("OnFly");
				slug.ParticleLoop(true);
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvent(InputEvent.Press);
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvent(InputEvent.Move);
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvent(InputEvent.Release);
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvent(InputEvent.Acceleration);
				SlugSmoothFollow slugSmoothFollow = Controller.SlugSmoothFollow;
				slugSmoothFollow.transform.position = Controller.PlayerController.Transform.position + Vector3.up * 2f;
				slugSmoothFollow.transform.localEulerAngles = new Vector3(slugSmoothFollow.transform.localEulerAngles.x, 0f, 0f);
				slugSmoothFollow.transform.LookAt(Controller.BossTransform.position);
				if (InGameModel != null)
				{
					_isLeftArrow = InGameModel.IsLeftArrow;
					_isRightArrow = InGameModel.IsRightArrow;
					InGameModel.IsLeftArrow = true;
					InGameModel.IsRightArrow = true;
					InGameModel.CurrentDuelState = this;
				}
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (slug == null || slug.Animator == null)
			{
				StateMachine.NextState<FightState>(new object[0]);
				Finish();
			}
		}

		protected override void OnFinish()
		{
			SlugSmoothFollow slugSmoothFollow = Controller.SlugSmoothFollow;
			if (InGameModel != null)
			{
				InGameModel.IsLeftArrow = _isLeftArrow;
				InGameModel.IsRightArrow = _isRightArrow;
				InGameModel.CurrentDuelState = null;
			}
			_inGameModel = null;
		}
	}
}
