using EasyInput.Core;
using UnityEngine;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/ChangeColor")]
	public class ChangeColor : MonoBehaviour
	{
		private MeshRenderer myRenderer;

		private Material myMaterial;

		private Color originalColor;

		private void Start()
		{
			myRenderer = GetComponent<MeshRenderer>();
			myMaterial = myRenderer.material;
			originalColor = myMaterial.color;
		}

		public void changeColor(ButtonClick button)
		{
			myMaterial.color = Color.green;
		}

		public void changeColorBack(ButtonClick button)
		{
			myMaterial.color = originalColor;
		}

		public void toggleColor()
		{
			if (myMaterial.color == originalColor)
			{
				myMaterial.color = Color.green;
			}
			else
			{
				myMaterial.color = originalColor;
			}
		}
	}
}
