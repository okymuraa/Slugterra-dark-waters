using System;
using AppsMinistry.Core.State;
using UnityEngine;

namespace Slugterra.Game.Units.Animals.DinoRat
{
	public abstract class DinoRatState : MonoState
	{
		private bool _isPlay;

		private MecanimStateAttribute _mecanimState;

		public bool IsPlay
		{
			get
			{
				return _isPlay;
			}
		}

		protected MecanimStateAttribute MecanimState
		{
			get
			{
				return _mecanimState;
			}
		}

		public DinoRatController Controller
		{
			get
			{
				return StateMachine.Controller as DinoRatController;
			}
		}

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_isPlay = false;
			Attribute[] customAttributes = Attribute.GetCustomAttributes(GetType());
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is MecanimStateAttribute)
				{
					_mecanimState = attribute as MecanimStateAttribute;
				}
			}
		}

		protected override void OnUpdate()
		{
			if (_mecanimState == null || _isPlay)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = Controller.Animator.GetCurrentAnimatorStateInfo(0);
			string[] names = _mecanimState.Names;
			foreach (string text in names)
			{
				if (currentAnimatorStateInfo.IsName(text))
				{
					_isPlay = true;
					break;
				}
			}
		}
	}
}
