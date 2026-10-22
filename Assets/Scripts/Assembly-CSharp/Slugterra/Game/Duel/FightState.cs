using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Input;

namespace Slugterra.Game.Duel
{
	[PersistentCommand]
	public class FightState : DuelState
	{
		protected override void OnStart(object[] args)
		{
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.Zoom);
			Controller.DuelSmothFollow.enabled = true;
			Controller.PlayerController.EnableControl(GetType());
		}

		protected override void OnFinish()
		{
			if (DonDestroyMonoSingleton<InputManager>.Instance != null)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
			}
			Controller.PlayerController.DisableControl(GetType());
			if (Controller.DuelSmothFollow != null)
			{
				Controller.DuelSmothFollow.enabled = false;
			}
		}

		protected override void OnReleaseResources()
		{
		}
	}
}
