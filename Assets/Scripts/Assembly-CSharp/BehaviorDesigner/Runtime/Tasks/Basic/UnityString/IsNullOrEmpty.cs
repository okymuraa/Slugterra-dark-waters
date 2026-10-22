namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskDescription("Returns success if the string is null or empty")]
	[TaskCategory("Basic/String")]
	public class IsNullOrEmpty : Conditional
	{
		[Tooltip("The target string")]
		public SharedString targetString;

		public override TaskStatus OnUpdate()
		{
			return (!string.IsNullOrEmpty(targetString.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetString = string.Empty;
		}
	}
}
