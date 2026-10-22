using UnityEngine;

namespace Slugterra.UI.Game.Camera.PostEffect
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class MinimapCameraPostEffect : MonoBehaviour
	{
		[SerializeField]
		private Material _postEffect;

		private void Start()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, _postEffect);
		}
	}
}
