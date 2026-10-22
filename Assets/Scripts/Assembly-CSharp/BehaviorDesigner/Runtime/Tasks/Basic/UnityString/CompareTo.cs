namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskDescription("Compares the first string to the second string. Returns an int which indicates whether the first string precedes, matches, or follows the second string.")]
	[TaskCategory("Basic/String")]
	public class CompareTo : Action
	{
		[Tooltip("The string to compare")]
		public SharedString firstString;

		[Tooltip("The string to compare to")]
		public SharedString secondString;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedInt storeResult;

		public override TaskStatus OnUpdate()
		{
			storeResult.Value = firstString.Value.CompareTo(secondString.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			firstString = string.Empty;
			secondString = string.Empty;
			storeResult = 0;
		}
	}
}
