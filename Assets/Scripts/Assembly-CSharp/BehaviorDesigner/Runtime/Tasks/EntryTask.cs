namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskIcon("{SkinColor}EntryIcon.png")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=23")]
	public class EntryTask : ParentTask
	{
		public override int MaxChildren()
		{
			return 1;
		}
	}
}
