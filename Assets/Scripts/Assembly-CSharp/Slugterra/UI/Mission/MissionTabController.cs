using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class MissionTabController : UIController<MissionTabView>
	{
		[SerializeField]
		private TabIconController _tabIconController;

		protected object _tabModel;

		protected override void _OnEnable()
		{
			if (_tabIconController != null)
			{
				_tabIconController.Active = true;
			}
		}

		protected override void _OnDisable()
		{
			if (_tabIconController != null)
			{
				_tabIconController.Active = false;
			}
		}

		public virtual void ApplyModel(object model)
		{
			_tabModel = model;
			View.ApplyModel(model);
		}
	}
}
