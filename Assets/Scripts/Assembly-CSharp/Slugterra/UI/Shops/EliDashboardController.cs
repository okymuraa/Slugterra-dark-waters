using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliDashboardController : ItemDashboardController, IObserver
	{
		[SerializeField]
		private EliSpecsController _specsController;

		private MissionSetting _model;

		private DuelSetting _duelModel;

		protected override void OnStart()
		{
			base.OnStart();
			ApplyModel(MonoSingleton<AppController>.Instance.Model.DuelSetting);
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				View.ApplyModel(observable as MissionSetting);
			}
			if (observable is DuelSetting)
			{
				View.ApplyModel(observable as DuelSetting);
			}
		}

		public override void ApplyModel(object model)
		{
			if (model is MissionSetting)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as MissionSetting;
				_model.AddObserver(this);
				View.ApplyModel(_model);
			}
			if (model is DuelSetting)
			{
				if (_duelModel != null)
				{
					_duelModel.RemoveObserver(this);
				}
				_duelModel = model as DuelSetting;
				_duelModel.AddObserver(this);
				View.ApplyModel(_duelModel);
			}
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			if (_duelModel != null)
			{
				_duelModel.RemoveObserver(this);
			}
		}
	}
}
