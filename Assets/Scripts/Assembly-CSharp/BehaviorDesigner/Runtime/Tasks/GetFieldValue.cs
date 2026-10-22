using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	[TaskCategory("Reflection")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=147")]
	[TaskDescription("Gets the value from the field specified. Returns success if the field was retrieved.")]
	public class GetFieldValue : Action
	{
		[Tooltip("The GameObject to get the field on")]
		public SharedGameObject targetGameObject;

		[Tooltip("The component to get the field on")]
		public SharedString componentName;

		[Tooltip("The name of the field")]
		public SharedString fieldName;

		[RequiredField]
		[Tooltip("The value of the field")]
		public SharedVariable fieldValue;

		public override TaskStatus OnUpdate()
		{
			if (fieldValue == null)
			{
				Debug.LogWarning("Unable to get field - field value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(componentName.Value);
			if (typeWithinAssembly == null)
			{
				Debug.LogWarning("Unable to get field - type is null");
				return TaskStatus.Failure;
			}
			Component component = GetDefaultGameObject(targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				Debug.LogWarning("Unable to get the field with component " + componentName.Value);
				return TaskStatus.Failure;
			}
			FieldInfo field = component.GetType().GetField(fieldName.Value);
			fieldValue.SetValue(field.GetValue(component));
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			componentName = null;
			fieldName = null;
			fieldValue = null;
		}
	}
}
