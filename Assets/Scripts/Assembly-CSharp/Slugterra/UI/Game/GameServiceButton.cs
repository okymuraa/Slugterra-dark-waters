using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class GameServiceButton : MonoBehaviour
	{
		[SerializeField]
		private tk2dBaseSprite _gameCenterIcon;

		[SerializeField]
		private TextMesh _gameCenterText;

		[SerializeField]
		private TextMesh _playServiceText;

		private void Start()
		{
			_gameCenterIcon.SetSprite(SpriteNamesHelper.GetGameCenterIcon());
			if (_gameCenterText != null)
			{
				_gameCenterText.gameObject.SetActive(false);
			}
			if (_playServiceText != null)
			{
				_playServiceText.gameObject.SetActive(true);
			}
		}
	}
}
