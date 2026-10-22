using AppsMinistry.Core;
using AppsMinistry.Scene;
using Slugterra.Game.Units.Ally;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("2_2_QuietLawn", "", false, false)]
	public class QuietLawn_2_2 : SlugterraSceneController
	{
		private EliController _eliController;

		private void Start()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQDA");
		}

		protected override void OnLoad()
		{
			base.OnLoad();
			_eliController = Object.FindObjectOfType<EliController>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_eliController != null)
			{
				_eliController.CanAttack = false;
			}
		}
	}
}
