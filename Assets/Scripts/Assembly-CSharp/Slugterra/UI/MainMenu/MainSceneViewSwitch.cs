using UnityEngine;

namespace Slugterra.UI.MainMenu
{
	public class MainSceneViewSwitch : MonoBehaviour
	{
		[SerializeField]
		private Camera[] _sceneCameras;

		private void Start()
		{
			EnableRandomCamera();
		}

		private int EnableRandomCamera()
		{
			if (_sceneCameras != null && _sceneCameras.Length > 0)
			{
				int num = Random.Range(0, _sceneCameras.Length);
				for (int i = 0; i < _sceneCameras.Length; i++)
				{
					_sceneCameras[i].enabled = num == i;
				}
				return num;
			}
			return -1;
		}
	}
}
