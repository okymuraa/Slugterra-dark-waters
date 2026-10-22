using UnityEngine;

public class CutSceneEventManager : MonoBehaviour
{
	public GameObject[] _CutSceneIDObject;

	private void Start()
	{
		for (int i = 0; i < _CutSceneIDObject.Length; i++)
		{
			if (_CutSceneIDObject[i] != null)
			{
				_CutSceneIDObject[i].SetActive(false);
			}
			else
			{
				Debug.Log("! Empty cut scene object in CutSceneEventManager list:" + i);
			}
		}
	}

	private void Update()
	{
	}

	public void Event_TriggerEnter(GameObject _Trigger)
	{
		int v_CutSceneID = _Trigger.GetComponent<Trigger_CollisionEvent>().v_CutSceneID;
		if (_CutSceneIDObject.Length < 1 && v_CutSceneID > _CutSceneIDObject.Length && _CutSceneIDObject[v_CutSceneID] == null)
		{
			Debug.Log("No  cut_scenes gameobject holder in _CutSceneIDObject! OR wrong v_CutSceneID on Trigger object ");
		}
		else
		{
			_CutSceneIDObject[v_CutSceneID].SetActive(true);
		}
	}
}
