using Slugterra.Configuration;
using Slugterra.Multiplayer.Player;

namespace Slugterra.Multiplayer.Network
{
	public class NetworkPlayerEventsSync : NetworkSyncComponent
	{
		private NetworkPlayerRoot _root;

		private void Start()
		{
			_root = GetComponent<NetworkPlayerRoot>();
		}

		private void OnDamage(float damage)
		{
			if (PhotonNetwork.isMasterClient)
			{
				_root.photonView.RPC("RpcOnDamage", PhotonTargets.Others, damage);
			}
		}

		private void OnSlugShot(SlugType slugType)
		{
			if (_root.IsLocalPlayer)
			{
				_root.photonView.RPC("RpcSlugShot", PhotonTargets.Others, (int)slugType);
			}
		}

		[PunRPC]
		private void RpcOnDamage(float damage)
		{
			if (!PhotonNetwork.isMasterClient)
			{
				_root.Player.TakeDamage(damage);
			}
		}

		[PunRPC]
		private void RpcSlugShot(int slugType)
		{
			_root.Player.OnUseSlug((SlugType)slugType);
		}
	}
}
