using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class FoesPanelView : UIView
	{
		[SerializeField]
		private tk2dUIScrollableArea _scrollArea;

		[SerializeField]
		private tk2dUIScrollableArea[] _scrollAreas;

		private tk2dUIScrollableArea _selectedScrollArea;

		[SerializeField]
		private TabIconController[] tabButtons;

		public void AddMatchItem(MatchItemController item)
		{
			int childCount = _scrollArea.contentContainer.transform.childCount;
			item.transform.parent = _scrollArea.contentContainer.transform;
			item.transform.localPosition = new Vector3(0f, (float)childCount * -1.29f, 0f);
			_scrollArea.ContentLength = (float)(childCount + 1) * 1.29f;
		}

		public void OnTabActivate(LobbyTabEnum tab)
		{
			tabButtons[(int)tab].Active = true;
			_scrollArea = _scrollAreas[(int)tab];
			_scrollArea.gameObject.SetActive(true);
		}

		public void OnTabDisactivate(LobbyTabEnum tab)
		{
			tabButtons[(int)tab].Active = false;
			_scrollArea = _scrollAreas[(int)tab];
			_scrollArea.gameObject.SetActive(false);
		}
	}
}
