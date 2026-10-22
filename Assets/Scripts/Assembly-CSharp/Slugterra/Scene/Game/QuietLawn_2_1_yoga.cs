using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("2_1_QuietLawn@yoga", "", false, false)]
	public class QuietLawn_2_1_yoga : SlugterraSceneController
	{
		[SerializeField]
		private GameObject Arrow;

		[SerializeField]
		private GameObject Hand;

		[SerializeField]
		private GameObject Fence;

		private QuietLawn_2_1 Self;

		[SerializeField]
		private tk2dButton _infurnus;

		private void Start()
		{
			Self = gameObject.GetComponent<QuietLawn_2_1>();
		}

		private void Update()
		{
			if (Fence == null)
			{
				Arrow.SetActive(false);
				_infurnus.targetObject = null;
				_infurnus.messageName = null;
				Self.enabled = false;
			}
		}

		private void ArrowTurnOn()
		{
			Arrow.SetActive(true);
			Hand.SetActive(false);
		}
	}
}
