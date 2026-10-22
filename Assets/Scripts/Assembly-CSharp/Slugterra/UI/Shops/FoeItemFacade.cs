using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class FoeItemFacade : ItemFacade
	{
		private FoeItem _model;

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is FoeItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as FoeItem;
				_model.AddObserver(this);
			}
		}

		protected override void OnDuelSettingChanged(DuelSetting settings)
		{
		}

		private void OnHireOrFire()
		{
			if (IsCurrentFoe(_model))
			{
				_duelSetting.SetDefaultFoe();
			}
			else
			{
				_duelSetting.Foe = _model;
			}
		}

		private bool IsCurrentFoe(FoeItem foe)
		{
			if (foe != null && _duelSetting.Foe != null)
			{
				return foe.Type == _duelSetting.Foe.Type;
			}
			return false;
		}
	}
}
