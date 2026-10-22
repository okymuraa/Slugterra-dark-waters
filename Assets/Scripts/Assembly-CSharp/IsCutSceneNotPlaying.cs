using BehaviorDesigner.Runtime.Tasks;
using Slugterra.UI.Game;
using UnityEngine;

[TaskCategory("Slugterra/CutScene")]
public class IsCutSceneNotPlaying : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private GameController gameController;

	public override TaskStatus OnUpdate()
	{
		if (gameController == null)
		{
			gameController = Object.FindObjectOfType<GameController>();
		}
		if (gameController.IsPlayCutScene)
		{
			return (!dataHolder.Value.Me.UnitSettings.CutSceneUnlock) ? TaskStatus.Failure : TaskStatus.Success;
		}
		return TaskStatus.Success;
	}
}
