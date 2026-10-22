using UnityEngine;

public class Trigger_ProjectorEvent : MonoBehaviour
{
	public enum TriggerAction
	{
		EnableTarget = 0,
		DisableTarget = 1
	}

	[SerializeField]
	private ProjectorMaterialSetting _TargetObject;

	private void OnTriggerEnter(Collider _Collider)
	{
		if (_Collider.CompareTag("Player"))
		{
			_TargetObject.ReplaseProjTexture(_Collider.transform);
		}
	}

	private void OnTriggerExit(Collider _Collider)
	{
		if (_Collider.CompareTag("Player"))
		{
			_TargetObject.ReplaseProjTextureDefault(_Collider.transform);
		}
	}
}
