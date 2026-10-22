using System.Collections.Generic;
using System.Linq;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain;
using Slugterra.UI.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppsMinistry.Core.Input.Strategy
{
	internal class ETJoysticInputStrategy : BaseJoysticInputStrategy, IObserver
	{
		public ETCJoystick Joystick_movement;

		public ETCJoystick Joystick_rotation;

		public ETCButton Button_Back;

		private GameController _gameController;

		public ETJoysticInputStrategy()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
		{
			_gameController = Object.FindObjectOfType<GameController>();
			List<ETCJoystick> list = Object.FindObjectsOfType<ETCJoystick>().ToList();
			Joystick_movement = list.Find((ETCJoystick j) => j.gameObject.name == "Joystick_movement");
			Joystick_rotation = list.Find((ETCJoystick j) => j.gameObject.name == "Joystick_rotation");
			List<ETCButton> list2 = Object.FindObjectsOfType<ETCButton>().ToList();
			Button_Back = list2.Find((ETCButton j) => j.gameObject.name == "Button_Back");
			if (Joystick_movement != null && Joystick_rotation != null)
			{
				Activate();
			}
		}

		public override void Activate()
		{
			if (!(Joystick_movement == null))
			{
				Joystick_movement.onMove.AddListener(Use_OnMoveEvent_movement);
				Joystick_movement.onMoveEnd.AddListener(Use_OnMoveEndEvent_movement);
				Joystick_rotation.onMove.AddListener(Use_OnMoveEvent_rotation);
				Joystick_rotation.onMoveEnd.AddListener(Use_OnMoveEndEvent_rotation);
				Button_Back.onDown.AddListener(Use_OnRollEvent);
				_gameController.Model.AddObserver(this);
			}
		}

		public override void Deactivate()
		{
			if (!(Joystick_movement == null))
			{
				Joystick_movement.onMove.RemoveListener(Use_OnMoveEvent_movement);
				Joystick_movement.onMoveEnd.RemoveListener(Use_OnMoveEndEvent_movement);
				Joystick_rotation.onMove.RemoveListener(Use_OnMoveEvent_rotation);
				Joystick_rotation.onMoveEnd.RemoveListener(Use_OnMoveEndEvent_rotation);
				Button_Back.onDown.RemoveListener(Use_OnRollEvent);
				_gameController.Model.RemoveObserver(this);
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is GameModel)
			{
				GameModel gameModel = observable as GameModel;
				if (gameModel.State != GameState.Play && gameModel.State != GameState.Survival)
				{
					Joystick_movement.activated = false;
					Joystick_rotation.activated = false;
				}
				else
				{
					Joystick_movement.activated = true;
					Joystick_rotation.activated = true;
				}
			}
		}
	}
}
