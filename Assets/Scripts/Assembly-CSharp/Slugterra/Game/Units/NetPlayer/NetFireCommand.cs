using System.Collections.Generic;
using Slugterra.Configuration;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.NetPlayer
{
	public class NetFireCommand : FireCommand
	{
		protected override void Fire()
		{
			NetPlayerController netPlayerController = _controller as NetPlayerController;
			netPlayerController.Animator.GetComponent<IKControl>().ResetWeight();
			if (!(_controller.AttackTarget != null))
			{
				return;
			}
			Transform transform = null;
			HitMarkers currentVisibleMarkers = _controller.ViewArea.CurrentVisibleMarkers;
			if (currentVisibleMarkers.Markers.Count != 0)
			{
				transform = currentVisibleMarkers.GetRandomMarker();
				Vector3 forward = transform.position - _controller.ShotPoint.position;
				Quaternion rotation = Quaternion.LookRotation(forward);
				GameObject slugPrefab = GameFactory.GetSlugPrefab(_slug.Type);
				PhotonView component = netPlayerController.gameObject.GetComponent<PhotonView>();
				GameObject gameObject = SpawnNetSlug(_slug.Type, _controller.ShotPoint.position, rotation, forward.normalized, transform, component.viewID, _controller.BlasterType);
				gameObject.layer = LayerMask.NameToLayer((!netPlayerController.IsLocal) ? "EnemySlugLayer" : "AllySlugLayer");
				SlugController component2 = gameObject.GetComponent<SlugController>();
				component2.SetVelocity(forward.normalized);
				component2.SetSlug(_slug);
				int num = LayerMask.NameToLayer("EnemyLayer");
				int num2 = LayerMask.NameToLayer("NeutralLayer");
				int num3 = LayerMask.NameToLayer("PassiveLayer");
				if (netPlayerController.IsLocal)
				{
					component2.SetStrategy(new AllySlugStrategy(component2, new int[3] { num, num2, num3 }));
				}
				else
				{
					component2.SetStrategy(new EnemySlugStrategy(component2, new int[3] { num, num2, num3 }));
				}
				component2.Parent = _controller;
				component2.Target = transform;
				_controller.Animator.SetTrigger("OnFire");
				_controller.PlaySlugShotSound(_slug.Type);
				_controller.SetNextSlug(SlugType.None);
				_slug.Available = false;
				if (_slug.Type == SlugType.Flopper)
				{
					_controller.ReloadSlug(_slug);
				}
				_controller.ShootCounter--;
				_controller.UpdateNextFire();
			}
		}

		private GameObject SpawnNetSlug(SlugType slugType, Vector3 position, Quaternion rotation, Vector3 attackNormalPosition, Transform hitTransform, int viewID, BlasterType blasterType)
		{
			Dictionary<SlugType, string> dictionary = new Dictionary<SlugType, string>();
			dictionary[SlugType.Flopper] = "Prefabs/Multiplayer/Slugs/FlopperSlug";
			dictionary[SlugType.Geoshard] = "Prefabs/Multiplayer/Slugs/Special/GeoshardSlug";
			dictionary[SlugType.Arachnet] = "Prefabs/Multiplayer/Slugs/Control/ArachnetSlug";
			dictionary[SlugType.Infurnus] = "Prefabs/Multiplayer/Slugs/Attack/InfurnusSlug";
			GameObject gameObject = Controller.AttackTarget.gameObject;
			PhotonView photonView = null;
			if (gameObject != null)
			{
				photonView = gameObject.GetComponent<PhotonView>();
			}
			int num = -1;
			if (photonView != null)
			{
				num = photonView.viewID;
			}
			return PhotonNetwork.Instantiate(dictionary[slugType], position, rotation, 0, new object[8]
			{
				"Slug",
				slugType.ToString(),
				attackNormalPosition,
				hitTransform.position,
				hitTransform.rotation,
				viewID,
				blasterType.ToString(),
				num
			});
		}

		protected override GameObject SpawnSlug(SlugType slugType, Vector3 position, Quaternion rotation)
		{
			return null;
		}
	}
}
