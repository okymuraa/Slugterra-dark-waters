namespace Slugterra.Game.Units
{
	public class FirstAidKit : BoostEffect
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			if (!controller.Model.IsDead())
			{
				controller.Model.Health += boost.Health;
			}
			Finish();
		}
	}
}
