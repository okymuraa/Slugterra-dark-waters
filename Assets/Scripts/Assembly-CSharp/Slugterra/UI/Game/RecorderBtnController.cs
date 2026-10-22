using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RecorderBtnController : UIController<RecorderBtnView>
	{
		private bool recordStateBtn;

		protected override void _OnEnable()
		{
		}

		private void Record()
		{
			recordStateBtn = !recordStateBtn;
			if (recordStateBtn)
			{
				PlayerPrefs.SetInt("isReplayKitEnabled", 1);
				View.RecordBtnIcon.SetSprite("icon_stop");
			}
			else
			{
				PlayerPrefs.SetInt("isReplayKitEnabled", 0);
				View.RecordBtnIcon.SetSprite("icon_record");
			}
		}
	}
}
