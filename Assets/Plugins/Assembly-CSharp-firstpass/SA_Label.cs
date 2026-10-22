using UnityEngine;

public class SA_Label : MonoBehaviour
{
	public string text
	{
		get
		{
			if (gameObject == null)
			{
				return string.Empty;
			}
			TextMesh componentInChildren = gameObject.GetComponentInChildren<TextMesh>();
			if (componentInChildren != null)
			{
				return componentInChildren.text;
			}
			return string.Empty;
		}
		set
		{
			if (gameObject == null)
			{
				return;
			}
			TextMesh[] componentsInChildren = gameObject.GetComponentsInChildren<TextMesh>();
			TextMesh[] array = componentsInChildren;
			foreach (TextMesh textMesh in array)
			{
				if (textMesh != null)
				{
					textMesh.text = value;
				}
			}
		}
	}
}
