using AppsMinistry.Core;
using Slugterra.Domain.Game.UI;
using Slugterra.Game.Duel;
using Slugterra.Game.Units.Player;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class HypnotismEffect : UnitEffect
	{
		private Attacker _attacker;

		private float _finishTime;

		private GameController _gameController;

		private UnitController _unitController;

		protected override void OnStart(object[] args)
		{
			_unitController = (UnitController)args[0];
			float num = (float)args[1];
			_finishTime = Time.time + num;
			_attacker = (Attacker)args[2];
			_gameController = Object.FindObjectOfType<GameController>();
			if (_gameController.LevelType == LevelType.Action)
			{
				HypnotizeAction();
				if (_attacker != null && _attacker.transform != null)
				{
					UnitController component = _attacker.transform.GetComponent<UnitController>();
					if (component.AttackTarget == _unitController.Transform)
					{
						component.AttackTarget = null;
					}
				}
			}
			else if (_gameController.LevelType == LevelType.Arena)
			{
				HypnotizeArena();
			}
		}

		private void Update()
		{
			if (_unitController == null || _unitController.Model.IsDead())
			{
				Finish();
			}
			else if (_finishTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_gameController.LevelType == LevelType.Action)
			{
				if (_unitController != null && !_unitController.Model.IsDead())
				{
					HypnotizeAction();
				}
			}
			else if (_gameController.LevelType == LevelType.Arena && _unitController != null && !_unitController.Model.IsDead())
			{
				DehypnotizeArena();
			}
		}

		private void HypnotizeAction()
		{
			if (_unitController.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
			{
				_unitController.ToAlly();
			}
			else if (_unitController.gameObject.layer == LayerMask.NameToLayer("AllyLayer"))
			{
				_unitController.ToEnemy();
			}
		}

		private void HypnotizeArena()
		{
			PlayerController playerController = _unitController as PlayerController;
			if (playerController != null)
			{
				playerController.DisableControl(GetType());
				playerController.enabled = false;
				playerController.AttackTarget = null;
			}
			else
			{
				_unitController.AttackTarget = null;
				_unitController.PathFinder.enabled = false;
				_unitController.enabled = false;
				_unitController.StateMachine.enabled = false;
			}
		}

		private void DehypnotizeArena()
		{
			PlayerController playerController = _unitController as PlayerController;
			if (playerController != null)
			{
				playerController.EnableControl(GetType());
				playerController.AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
				playerController.enabled = true;
			}
			else
			{
				_unitController.AttackTarget = MonoSingleton<DuelManager>.Instance.PlayerController.Transform;
				_unitController.PathFinder.enabled = true;
				_unitController.enabled = true;
				_unitController.StateMachine.enabled = true;
			}
		}
	}
}
