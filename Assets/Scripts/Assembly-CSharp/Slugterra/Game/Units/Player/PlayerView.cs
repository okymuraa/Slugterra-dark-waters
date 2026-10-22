using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Game.Units.Player
{
	public class PlayerView : MonoBehaviour
	{
		[SerializeField]
		private Renderer _playerRenderer;

		public Renderer PlayerRenderer
		{
			get
			{
				return _playerRenderer;
			}
		}

		public virtual void SetArmor(ArmorType type)
		{
		}
	}
}
