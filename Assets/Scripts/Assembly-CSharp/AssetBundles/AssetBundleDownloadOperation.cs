namespace AssetBundles
{
	public abstract class AssetBundleDownloadOperation : AssetBundleLoadOperation
	{
		private bool done;

		public string assetBundleName { get; private set; }

		public LoadedAssetBundle assetBundle { get; protected set; }

		public string error { get; protected set; }

		public abstract float downloadProgress { get; protected set; }

		protected abstract bool downloadIsDone { get; }

		public AssetBundleDownloadOperation(string assetBundleName)
		{
			this.assetBundleName = assetBundleName;
		}

		protected abstract void FinishDownload();

		public abstract void StopDownload();

		public override bool Update()
		{
			if (!done && downloadIsDone)
			{
				FinishDownload();
				done = true;
			}
			return !done;
		}

		public override bool HaveError()
		{
			return error != null;
		}

		public override string GetBundleName()
		{
			return assetBundleName;
		}

		public override bool IsDone()
		{
			return done;
		}

		public abstract string GetSourceURL();
	}
}
