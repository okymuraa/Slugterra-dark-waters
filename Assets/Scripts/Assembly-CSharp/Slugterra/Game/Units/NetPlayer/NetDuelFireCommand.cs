using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Game.Duel;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Slugs;
using Slugterra.Game.Units.Slugs.Network;
using UnityEngine;

namespace Slugterra.Game.Units.NetPlayer
{
	public class NetDuelFireCommand : DuelFireCommand
	{
		protected override void Fire()
		{
			NetPlayerController netPlayerController = _controller as NetPlayerController;
			if (netPlayerController.IsLocal)
			{
				LocalFire();
			}
			else
			{
				RemoteFire();
			}
		}

		private void LocalFire()
		{
			if (_controller.AttackTarget != null)
			{
				GameObject gameObject = SpawnSlug(_slug.Type, GetShotPoint(), Quaternion.identity);
				gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
				gameObject.transform.LookAt(MonoSingleton<DuelManager>.Instance.BossTransform.position);
				SlugController component = gameObject.GetComponent<SlugController>();
				component.SetSlug(_slug);
				int num = LayerMask.NameToLayer("EnemyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				component.SetStrategy(new NetLocalDuelSlugStrategy(component, new int[2] { num, num2 }, MonoSingleton<DuelManager>.Instance.BossTransform));
				component.Parent = _controller;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				_controller.SetNextSlug(SlugType.None);
				MonoSingleton<DuelManager>.Instance.StateMachine.ChangeState<SlugFlyState>(new object[1] { component });
				MonoSingleton<DuelManager>.Instance.SlugSmoothFollow.SetTarget(gameObject.transform);
				MonoSingleton<DuelManager>.Instance.AddAllySlug(component);
				_slug.Available = false;
				NetPlayerController netPlayerController = _controller as NetPlayerController;
				netPlayerController.RiseFireEvent(_slug.Type);
			}
		}

		private void RemoteFire()
		{
			_controller.Animator.SetTrigger("OnFire");
			_controller.PlaySlugShotSound(_slug.Type);
		}

		protected override GameObject SpawnSlug(SlugType slugType, Vector3 position, Quaternion rotation)
		{
			NetPlayerController netPlayerController = _controller as NetPlayerController;
			int viewID = netPlayerController.NetworkRoot.photonView.viewID;
			return PhotonNetwork.Instantiate(slugType.ToString(), position, rotation, 0, new object[2]
			{
				viewID,
				(int)slugType
			});
		}
	}
}
