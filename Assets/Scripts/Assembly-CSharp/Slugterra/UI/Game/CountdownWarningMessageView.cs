using UnityEngine;

namespace Slugterra.UI.Game
{
	public class CountdownWarningMessageView : MessageViewBase
	{
		[SerializeField]
		private TextMesh _timer;

		public TextMesh Timer
		{
			get
			{
				return _timer;
			}
		}
	}
}
