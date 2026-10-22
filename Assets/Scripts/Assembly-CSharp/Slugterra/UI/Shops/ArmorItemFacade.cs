using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Shops
{
	public class ArmorItemFacade : ItemFacade
	{
		private ArmorItem _model;

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
			if (model is ArmorItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as ArmorItem;
				_model.AddObserver(this);
			}
		}

		protected override void OnMissionSettingChanged(MissionSetting settings)
		{
		}

		private void OnWearOrRemove()
		{
			if (IsWearing(_model))
			{
				_missionSetting.SetDefaultArmor(MonoSingleton<AppController>.Instance.Model.ShopModel);
			}
			else
			{
				_missionSetting.Armor = _model;
			}
		}

		private bool IsWearing(ArmorItem armor)
		{
			if (armor != null && _missionSetting.Armor != null)
			{
				return armor.Type == _missionSetting.Armor.Type;
			}
			return false;
		}
	}
}
