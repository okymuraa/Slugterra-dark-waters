using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace TinyTeam.Debuger
{
	public class MemoryCounter
	{
		public const float MEMORY_DIVIDER = 1f / 1048576f;

		private float preTime = -10f;

		private float interval = 1f;

		public uint totalReserved;

		public uint allocated;

		public long monoMemory;

		public bool Update(int updateInterval)
		{
			if (Time.realtimeSinceStartup - preTime < interval)
			{
				return false;
			}
			preTime = Time.realtimeSinceStartup;
			totalReserved = Profiler.GetTotalReservedMemory();
			allocated = Profiler.GetTotalAllocatedMemory();
			monoMemory = GC.GetTotalMemory(false);
			return true;
		}

		public new string ToString()
		{
			return "MEM(TOTAL):" + (float)totalReserved * (1f / 1048576f) + "MB\nMEM(ALLOC):" + (float)allocated * (1f / 1048576f) + "MB\nMEM(MONO):" + (float)monoMemory * (1f / 1048576f) + "MB";
		}
	}
}
