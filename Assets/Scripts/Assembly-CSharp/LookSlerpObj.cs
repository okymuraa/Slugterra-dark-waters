using UnityEngine;

[ExecuteInEditMode]
public class LookSlerpObj : MonoBehaviour
{
	public GameObject Target;

	public bool smooth = true;

	public float damping = 6f;

	private Transform transformObj;

	private Quaternion rotation;

	public Transform TextTransform;

	public Transform textMesh;

	public Vector3 offset;

	private void Start()
	{
		transformObj = transform;
		transform.LookAt(Target.transform);
	}

	private void LateUpdate()
	{
		if ((bool)Target)
		{
			if (smooth)
			{
				rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			}
			else
			{
				transform.LookAt(Target.transform);
			}
		}
		textMesh.transform.position = TextTransform.position + offset;
		if (textMesh.transform.eulerAngles.x != Target.transform.eulerAngles.x)
		{
			textMesh.transform.eulerAngles = Vector3.left * (360f - Target.transform.eulerAngles.x);
		}
	}

	public void NewTarget(GameObject target)
	{
		Target = target;
	}

	public void NewText(string _text)
	{
		textMesh.GetComponent<TextMesh>().text = _text;
	}
}
