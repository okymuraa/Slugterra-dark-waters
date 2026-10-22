using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace AssetBundles
{
	public class LoadedAssetBundle
	{
		public AssetBundle m_AssetBundle;

		public int m_ReferencedCount;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action unload__BackingField;

		internal event Action unload
		{
			add
			{
				Action action = unload__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref unload__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = unload__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref unload__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			m_AssetBundle = assetBundle;
			m_ReferencedCount = 1;
		}

		internal void OnUnload()
		{
			m_AssetBundle.Unload(false);
			if (unload__BackingField != null)
			{
				unload__BackingField();
			}
		}
	}
}
