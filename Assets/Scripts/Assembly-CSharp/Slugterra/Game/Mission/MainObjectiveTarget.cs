namespace Slugterra.Game.Mission
{
	internal class MainObjectiveTarget : ObjectiveTarget
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			Completed = true;
		}
	}
}
