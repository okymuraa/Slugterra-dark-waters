using AppsMinistry.UI;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.UI.Loading
{
	public class LoadingScreenView : UIView
	{
		private const float SmoothSpeed = 1.5f;

		[SerializeField]
		private tk2dUIProgressBar _loadingProgress;

		private AsyncOperation _asyncOperation;

		protected override void OnStart()
		{
			base.OnStart();
			_asyncOperation = SceneLoader.CurrentAsyncOperation;
			if (_asyncOperation == null)
			{
				_loadingProgress.gameObject.SetActive(false);
			}
			else
			{
				_loadingProgress.Value = 0f;
			}
		}

		private void Update()
		{
			if (_asyncOperation != null)
			{
				_loadingProgress.Value = Mathf.Lerp(_loadingProgress.Value, _asyncOperation.progress, Time.unscaledDeltaTime * 1.5f);
			}
		}
	}
}
