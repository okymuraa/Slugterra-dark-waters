using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.HealthRestoration;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopTabController : UIController<ShopTabView>
	{
		[SerializeField]
		private TabIconController _tabIconController;

		protected object _tabModel;

		protected int _toViewValue = -1;

		public int ToViewValue
		{
			get
			{
				return _toViewValue;
			}
			set
			{
				_toViewValue = value;
			}
		}

		protected RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		protected override void _OnEnable()
		{
			View.ShopItemsScrollScontroller.OnObjectSelected += OnObjectSelected;
			if (_tabIconController != null)
			{
				_tabIconController.Active = true;
			}
		}

		protected override void _OnDisable()
		{
			View.ShopItemsScrollScontroller.OnObjectSelected -= OnObjectSelected;
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

		protected virtual void OnObjectSelected(ShopItemController item)
		{
		}

		protected virtual int GetUnlockedItemsCount(object model)
		{
			return 0;
		}

		public virtual void UpdateCategory()
		{
		}

		public virtual bool SetScrollToIssue()
		{
			return false;
		}
	}
}
