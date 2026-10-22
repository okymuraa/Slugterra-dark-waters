using EasyInput.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyInput.Misc
{
	[AddComponentMenu("EasyInputHelper/Miscellaneous/ExitToMaster")]
	public class ExitToMaster : MonoBehaviour
	{
		private void OnEnable()
		{
			EasyInputHelper.On_ClickStart += localClickStart;
		}

		private void OnDestroy()
		{
			EasyInputHelper.On_Click -= localClickStart;
		}

		private void localClickStart(ButtonClick button)
		{
			if (button.button == EasyInputConstants.CONTROLLER_BUTTON.MenuButton)
			{
				SceneManager.LoadScene("MasterExample");
			}
		}
	}
}
