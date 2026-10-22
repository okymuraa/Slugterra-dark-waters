using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	public class PlayerWaypoint : MonoBehaviour
	{
		[SerializeField]
		private Renderer _arrow;

		[SerializeField]
		private Renderer _sphere;

		private GameController _gameController;

		private void Awake()
		{
			_gameController = Object.FindObjectOfType<GameController>();
			if (_gameController != null)
			{
				_gameController.OnPlayCutSceneEvent += OnPlayCutScene;
			}
		}

		private void OnPlayCutScene(bool isPlaying)
		{
			_arrow.enabled = !isPlaying;
			_sphere.enabled = !isPlaying;
		}
	}
}
