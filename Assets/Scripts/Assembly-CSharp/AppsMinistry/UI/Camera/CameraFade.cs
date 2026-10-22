using UnityEngine;

namespace AppsMinistry.UI.Camera
{
	public class CameraFade : MonoBehaviour
	{
		private const int DrawDepth = -1000;

		private Color _overlayColor = Color.black;

		private Texture2D _overlayTexture;

		private float _fadeStart;

		private float _fadeInTime;

		private float _fadeWaitTime;

		private float _fadeOutTime;

		private bool _fade;

		private void Start()
		{
			_overlayTexture = new Texture2D(1, 1);
			_overlayTexture.SetPixel(0, 0, Color.white);
			_overlayTexture.wrapMode = TextureWrapMode.Repeat;
			_overlayTexture.Apply();
		}

		private void OnEnable()
		{
			_fade = false;
		}

		private void OnDisable()
		{
			_fade = false;
		}

		public void StartFade(Color overlay, float fadeWaitTime, float fadeInTime, float fadeOutTime)
		{
			_overlayColor = overlay;
			_fadeStart = Time.unscaledTime;
			_fadeInTime = Mathf.Clamp(fadeInTime, Mathf.Epsilon, float.PositiveInfinity);
			_fadeWaitTime = _fadeInTime + Mathf.Clamp(fadeWaitTime, Mathf.Epsilon, float.PositiveInfinity);
			_fadeOutTime = _fadeWaitTime + Mathf.Clamp(fadeOutTime, Mathf.Epsilon, float.PositiveInfinity);
			_fade = true;
		}

		public void StopFade()
		{
			_fade = false;
		}

		private float GetFadeAlpha(float time)
		{
			if (time <= _fadeInTime)
			{
				return Mathf.Lerp(0f, 1f, time / _fadeInTime);
			}
			if (time <= _fadeWaitTime)
			{
				return 1f;
			}
			if (time <= _fadeOutTime)
			{
				return Mathf.Lerp(1f, 0f, (time - _fadeWaitTime) / (_fadeOutTime - _fadeWaitTime));
			}
			return 0f;
		}

		private void OnGUI()
		{
			if (_fade)
			{
				float num = Time.unscaledTime - _fadeStart;
				GUI.depth = -1000;
				GUI.color = new Color(_overlayColor.r, _overlayColor.g, _overlayColor.b, GetFadeAlpha(num));
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _overlayTexture);
				_fade = num <= _fadeOutTime;
			}
		}
	}
}
