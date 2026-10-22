namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskIcon("{SkinColor}ReturnSuccessIcon.png")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=39")]
	[TaskDescription("The return success task will always return success except when the child task is running.")]
	public class ReturnSuccess : Decorator
	{
		private TaskStatus executionStatus;

		public override bool CanExecute()
		{
			return executionStatus == TaskStatus.Inactive || executionStatus == TaskStatus.Running;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			executionStatus = childStatus;
		}

		public override TaskStatus Decorate(TaskStatus status)
		{
			if (status == TaskStatus.Failure)
			{
				return TaskStatus.Success;
			}
			return status;
		}

		public override void OnEnd()
		{
			executionStatus = TaskStatus.Inactive;
		}
	}
}
