using UnityEngine;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/AnchorTextAbove")]
	public class AnchorTextAbove : MonoBehaviour
	{
		public string Text;

		private TextMesh textTM;

		private Camera mainCamera;

		private void Start()
		{
			mainCamera = Camera.main;
			textTM = GetComponent<TextMesh>();
			textTM.text = Text;
		}

		private void Update()
		{
			textTM.transform.forward = mainCamera.transform.forward;
		}
	}
}
