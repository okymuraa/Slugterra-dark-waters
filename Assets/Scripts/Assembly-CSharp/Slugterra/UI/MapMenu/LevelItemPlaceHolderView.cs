using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.MapMenu
{
	public class LevelItemPlaceHolderView : UIView
	{
		private const float HeightOffset = 0.65f;

		[SerializeField]
		private Vector3 _targetScale = Vector3.one * 2.3f;

		[SerializeField]
		private string _levelName;

		private LevelItemController _levelItemController;

		public string LevelName
		{
			get
			{
				return _levelName;
			}
			set
			{
				_levelName = value;
			}
		}

		public LevelItemController LevelItemController
		{
			get
			{
				return _levelItemController;
			}
			set
			{
				SetLevelItem(value);
			}
		}

		public void SetLevelItem(LevelItemController levelItemController)
		{
			_levelItemController = levelItemController;
			Vector3 position = transform.position;
			position.y += 0.65f;
			_levelItemController.transform.position = position;
			_levelItemController.transform.localScale = _targetScale;
			_levelItemController.transform.eulerAngles = new Vector3(60f, 0f, 0f);
		}
	}
}
