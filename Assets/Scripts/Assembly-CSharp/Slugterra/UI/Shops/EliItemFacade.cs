using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliItemFacade : ItemFacade
	{
		[SerializeField]
		private SlugBarViewController _slugBar;

		[SerializeField]
		private GameObject _boostBar;

		private MissionSetting _model;

		protected override void OnEnable()
		{
			base.OnEnable();
			_boostBar.SetActive(true);
			_slugBar.gameObject.SetActive(true);
			_slugBar.Diselect();
		}

		protected override void OnDisable()
		{
			_boostBar.SetActive(false);
			_slugBar.gameObject.SetActive(false);
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is MissionSetting)
			{
				_model = model as MissionSetting;
			}
		}
	}
}
