using System;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ActivationObjectiveTarget : ObjectiveTarget
	{
		[SerializeField]
		private UnityEngine.Object[] _objects;

		[SerializeField]
		private bool _isEnable;

		[SerializeField]
		private bool _checkEliHealth;

		private bool _activated;

		private GameController _gameController;

		protected override void OnCreate()
		{
			base.OnCreate();
			_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		}

		protected override void OnActivate()
		{
			_activated = true;
			if (!_checkEliHealth)
			{
				Activete();
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_checkEliHealth && _activated && !_gameController.Model.Eli.IsDead())
			{
				Activete();
			}
		}

		private void Activete()
		{
			UnityEngine.Object[] objects = _objects;
			foreach (UnityEngine.Object obj in objects)
			{
				if (obj != null)
				{
					if (obj is GameObject)
					{
						(obj as GameObject).SetActive(_isEnable);
					}
					else if (obj is MonoBehaviour)
					{
						(obj as MonoBehaviour).enabled = _isEnable;
					}
				}
				else
				{
					Debug.LogException(new NullReferenceException("ActivationObjectiveTarget::OnActive unityObject is Null " + StackTraceUtility.ExtractStackTrace()));
				}
			}
			Completed = true;
		}
	}
}
