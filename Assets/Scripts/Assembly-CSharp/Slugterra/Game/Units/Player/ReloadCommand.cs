using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class ReloadCommand : MonoCommand<ReloadCommand>
	{
		private PlayerController _controller;

		private SlugType _slugType;

		private float _reloadEndTime;

		private float _reloadTime;

		public SlugType SlugType
		{
			get
			{
				return _slugType;
			}
		}

		public float ReloadEndTime
		{
			get
			{
				return _reloadEndTime;
			}
		}

		public float ReloadTime
		{
			get
			{
				return _reloadTime;
			}
		}

		protected override void OnStart(object[] args)
		{
			_controller = (PlayerController)args[0];
			_slugType = (SlugType)args[1];
			_reloadTime = (_controller.Model as Humanoid).Blaster.SlugReloadTime;
			_reloadEndTime = Time.time + _reloadTime;
			_controller.Animator.SetFloat("ReloadSpeed", (_controller.Model as Humanoid).Blaster.SlugReloadTime);
			_controller.Animator.SetTrigger("ReloadTrigger");
			_controller.PlaySlugLoadSound();
		}

		private void Update()
		{
			if (_reloadEndTime <= Time.time)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			_controller.SetNextSlug(_slugType);
			if (!(_controller.Model as Humanoid).ReloadInMotion)
			{
				_controller.Model.RemoveSpeedCoeficient(GetType());
			}
		}
	}
}
