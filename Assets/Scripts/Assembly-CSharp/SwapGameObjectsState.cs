using UnityEngine;

public class SwapGameObjectsState : MonoBehaviour
{
	[SerializeField]
	private GameObject[] firstGameObjectsGroup;

	[SerializeField]
	private GameObject[] secondGameObjectsGroup;

	public void Swap(bool enablefirstGroup = true)
	{
		for (int i = 0; i < firstGameObjectsGroup.Length; i++)
		{
			if (firstGameObjectsGroup[i] != null)
			{
				firstGameObjectsGroup[i].SetActive(enablefirstGroup);
			}
		}
		for (int j = 0; j < secondGameObjectsGroup.Length; j++)
		{
			if (secondGameObjectsGroup[j] != null)
			{
				secondGameObjectsGroup[j].SetActive(!enablefirstGroup);
			}
		}
	}
}
