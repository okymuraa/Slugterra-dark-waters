using UnityEngine;

namespace TinyTeam.Debuger
{
	public class FPSCounter
	{
		public float current;

		public float updateInterval = 0.5f;

		private float accum;

		private int frames = 100;

		private float timeleft;

		private float delta;

		public FPSCounter()
		{
			timeleft = updateInterval;
		}

		public bool Update()
		{
			delta = Time.deltaTime;
			timeleft -= delta;
			accum += Time.timeScale / delta;
			frames++;
			if (timeleft <= 0f)
			{
				current = accum / (float)frames;
				timeleft = updateInterval;
				accum = 0f;
				frames = 0;
				return true;
			}
			return false;
		}
	}
}
