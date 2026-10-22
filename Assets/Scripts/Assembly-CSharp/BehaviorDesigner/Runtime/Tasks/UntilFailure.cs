namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskIcon("{SkinColor}UntilFailureIcon.png")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=41")]
	[TaskDescription("The until failure task will keep executing its child task until the child task returns failure.")]
	public class UntilFailure : Decorator
	{
		private TaskStatus executionStatus;

		public override bool CanExecute()
		{
			return executionStatus == TaskStatus.Success || executionStatus == TaskStatus.Inactive;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			executionStatus = childStatus;
		}

		public override void OnEnd()
		{
			executionStatus = TaskStatus.Inactive;
		}
	}
}
