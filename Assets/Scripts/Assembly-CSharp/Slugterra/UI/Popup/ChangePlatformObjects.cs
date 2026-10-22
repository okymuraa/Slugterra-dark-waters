using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class ChangePlatformObjects : MonoBehaviour
	{
		public GameObject[] _objectTVOS;

		public GameObject[] _objectDefault;

		public ChangePlatformObjects(GameObject[] objectTVOS, GameObject[] objectDefault)
		{
			_objectTVOS = objectTVOS;
			_objectDefault = objectDefault;
		}

		public void SetAcriveObjects()
		{
			bool flag = false;
			flag = false;
			if (_objectTVOS != null && _objectTVOS.Length > 0)
			{
				for (int i = 0; i < _objectTVOS.Length; i++)
				{
					if (_objectTVOS[i] != null)
					{
						_objectTVOS[i].SetActive(flag);
					}
				}
			}
			if (_objectDefault == null || _objectDefault.Length <= 0)
			{
				return;
			}
			for (int j = 0; j < _objectDefault.Length; j++)
			{
				if (_objectDefault[j] != null)
				{
					_objectDefault[j].SetActive(!flag);
				}
			}
		}

		private void Start()
		{
			SetAcriveObjects();
		}
	}
}
