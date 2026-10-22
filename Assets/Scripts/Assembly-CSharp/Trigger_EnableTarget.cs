using UnityEngine;

public class Trigger_EnableTarget : MonoBehaviour
{
	[SerializeField]
	public GameObject v_Target;

	public bool b_TriggerOnce;

	private bool b_CheckParamsOk = true;

	private void Start()
	{
		if (v_Target != null)
		{
			v_Target.SetActive(false);
			return;
		}
		Debug.Log("!!!!  ВСТАЛЯЙТЕ ЦЕЛЕВЫЕ ГЕЙМ ОБЪЕКТЫ! ЗАИПЛИ:" + gameObject.name.ToString());
		b_CheckParamsOk = false;
	}

	private void OnTriggerEnter(Collider v_Collider)
	{
		if (v_Collider.tag == "Player" && b_CheckParamsOk)
		{
			v_Target.SetActive(true);
			if (b_TriggerOnce && v_Target.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
