using UnityEngine;

namespace Slugterra.UI.Wallet
{
	public class BackTVOSChange : MonoBehaviour
	{
		[SerializeField]
		private GameObject TVOSUIPrefab;

		[SerializeField]
		private GameObject OtherUIPrefab;

		private void Start()
		{
			if (TVOSUIPrefab != null)
			{
				TVOSUIPrefab.SetActive(false);
			}
			if (OtherUIPrefab != null)
			{
				OtherUIPrefab.SetActive(true);
			}
		}
	}
}
