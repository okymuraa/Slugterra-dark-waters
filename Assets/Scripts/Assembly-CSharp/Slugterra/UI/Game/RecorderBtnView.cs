using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RecorderBtnView : UIView
	{
		[SerializeField]
		private tk2dSprite _recordBtnIcon;

		public tk2dSprite RecordBtnIcon
		{
			get
			{
				return _recordBtnIcon;
			}
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}
	}
}
