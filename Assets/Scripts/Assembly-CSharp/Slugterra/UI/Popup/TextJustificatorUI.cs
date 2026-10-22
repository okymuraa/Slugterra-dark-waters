using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class TextJustificatorUI : MonoBehaviour
	{
		[SerializeField]
		private TextMesh text;

		[SerializeField]
		private float offsetFactor = 2.25f;

		[SerializeField]
		private int initTextLength = 4;

		private void Start()
		{
		}

		public void Justify()
		{
			float num = text.characterSize * (float)(text.text.Length - initTextLength) * offsetFactor;
			transform.localPosition += Vector3.left * num;
			initTextLength = text.text.Length;
		}
	}
}
