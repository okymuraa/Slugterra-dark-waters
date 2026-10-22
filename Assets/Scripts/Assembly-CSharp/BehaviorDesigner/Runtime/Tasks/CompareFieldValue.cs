using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskCategory("Reflection")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=151")]
	[TaskDescription("Compares the field value to the value specified. Returns success if the values are the same.")]
	public class CompareFieldValue : Conditional
	{
		[Tooltip("The GameObject to compare the field on")]
		public SharedGameObject targetGameObject;

		[Tooltip("The component to compare the field on")]
		public SharedString componentName;

		[Tooltip("The name of the field")]
		public SharedString fieldName;

		[Tooltip("The value to compare to")]
		public SharedVariable compareValue;

		public override TaskStatus OnUpdate()
		{
			if (compareValue == null)
			{
				Debug.LogWarning("Unable to compare field - compare value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to compare field - type is null");
				return TaskStatus.Failure;
			}
			Component component = GetDefaultGameObject(targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to compare the field with component " + componentName.Value);
				return TaskStatus.Failure;
			}
			FieldInfo field = component.GetType().GetField(fieldName.Value);
			object value = field.GetValue(component);
			if (value == null && compareValue.GetValue() == null)
			{
				return TaskStatus.Success;
			}
			return (!value.Equals(compareValue.GetValue())) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			componentName = null;
			fieldName = null;
			compareValue = null;
		}
	}
}
