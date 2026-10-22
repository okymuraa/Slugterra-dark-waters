using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Game.Units;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class InventoryItemController : UIController<InventoryItemView>, IObserver
	{
		private ConsumableItem _model;

		private InventoryItemState _state;

		private float _duration;

		private float _durationEndTime;

		public ConsumableItem Model
		{
			get
			{
				return _model;
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is ConsumableItem)
			{
				View.ApplyModel(observable as ConsumableItem);
			}
		}

		protected override void _OnEnable()
		{
			View.OnUseEvent += OnUse;
		}

		protected override void _OnDisable()
		{
			View.OnUseEvent -= OnUse;
		}

		private void OnUse()
		{
			if (BoostConfigs.UseItem(_model) && _duration > 0f)
			{
				_durationEndTime = Time.time + _duration;
			}
		}

		protected override void OnUpdate()
		{
			if (_model != null)
			{
				if (_durationEndTime > Time.time)
				{
					SetState(InventoryItemState.Duration);
					float durationProgress = 1f - (_durationEndTime - Time.time) / _duration;
					View.SetDurationProgress(durationProgress);
				}
				else if (_model.Count > 0)
				{
					SetState(InventoryItemState.Enabled);
				}
				else
				{
					SetState(InventoryItemState.Disabled);
				}
			}
			else
			{
				SetState(InventoryItemState.Disabled);
			}
		}

		private void SetState(InventoryItemState state)
		{
			if (_state != state)
			{
				_state = state;
				View.SetState(state);
			}
		}

		public void ApplyModel(ConsumableItem model)
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			_model = model;
			GameController gameController = Object.FindObjectOfType<GameController>();
			if (gameController != null)
			{
				_model.SetMultiplayerGame(gameController.IsMultiplayer);
				_model.UpdateNetCount();
			}
			_model.AddObserver(this);
			ConsumableProperties consumableProperties = MonoSingleton<GameConfiguration>.Instance.ConsumableConfigurationManager.GetConsumableProperties(_model.Type);
			_duration = Mathf.Max(consumableProperties.AttackDuration, Mathf.Max(consumableProperties.DamageDuration, Mathf.Max(consumableProperties.HealDuration, consumableProperties.SpeedDuration)));
			View.ApplyModel(_model);
		}

		protected override void OnReleaseResource()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnReleaseResource();
		}
	}
}
