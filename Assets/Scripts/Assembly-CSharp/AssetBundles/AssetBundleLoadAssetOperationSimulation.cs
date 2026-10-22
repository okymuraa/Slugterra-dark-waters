using UnityEngine;

namespace AssetBundles
{
	public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
	{
		private Object m_SimulatedObject;

		public AssetBundleLoadAssetOperationSimulation(Object simulatedObject)
		{
			m_SimulatedObject = simulatedObject;
		}

		public override T GetAsset<T>()
		{
			return m_SimulatedObject as T;
		}

		public override bool Update()
		{
			return false;
		}

		public override bool HaveError()
		{
			return false;
		}

		public override string GetBundleName()
		{
			return string.Empty;
		}

		public override bool IsDone()
		{
			return true;
		}
	}
}
