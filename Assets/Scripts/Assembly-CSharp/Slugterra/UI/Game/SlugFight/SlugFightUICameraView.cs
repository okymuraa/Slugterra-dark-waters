using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.UI.Game.SlugFight
{
	public class SlugFightUICameraView : ManualMonoSingleton<SlugFightUICameraView>
	{
		[SerializeField]
		private SlugFightController _controller;

		private bool _isSlugFightActive;

		public SlugFightController FightController
		{
			get
			{
				return _controller;
			}
		}

		public bool IsSlugFightActive
		{
			get
			{
				return _isSlugFightActive;
			}
		}

		protected override void OnCreate()
		{
			Show(false);
		}

		public void Show(bool value)
		{
			gameObject.SetActive(value);
			_isSlugFightActive = value;
		}
	}
}
