using System.Collections;
using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("2_3_QuietLawn@yoga", "", false, false)]
	public class QuietLawn_2_3_yoga : SlugterraSceneController
	{
		private void Start()
		{
			StartCoroutine(TargetMakrerTurnOff());
		}

		private IEnumerator TargetMakrerTurnOff()
		{
			yield return new WaitForSeconds(2f);
		}
	}
}
