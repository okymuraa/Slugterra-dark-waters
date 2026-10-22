using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class AvatarInfoController : UIController<AvatarInfoView>, IObserver
	{
		private Unit _model;

		[SerializeField]
		private ProgressBarController _lifeBar;

		public void OnApplyModel(Unit model)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = model;
			_model.AddObserver(this);
			_model.CommitAndNotify();
			View.ApplyModel(_model);
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Humanoid)
			{
				OnUnitChanged(observable as Humanoid);
			}
		}

		private void OnUnitChanged(Humanoid unit)
		{
			_lifeBar.Progress = unit.Health / unit.MaxHealth;
		}
	}
}
