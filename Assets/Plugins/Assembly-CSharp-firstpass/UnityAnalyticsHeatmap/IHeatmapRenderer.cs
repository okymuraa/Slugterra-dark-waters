using System;
using UnityEngine;

namespace UnityAnalyticsHeatmap
{
	public interface IHeatmapRenderer
	{
		float pointSize { get; set; }

		bool allowRender { get; set; }

		bool activateTips { get; set; }

		int currentPoints { get; }

		int totalPoints { get; }

		void UpdatePointData(HeatPoint[] data, float maxDensity);

		[Obsolete]
		void UpdateColors(Color[] colors);

		void UpdateGradient(Gradient gradient);

		[Obsolete]
		void UpdateThresholds(float[] thresholds);

		void UpdateRenderMask(float lowX, float highX, float lowY, float highY, float lowZ, float highZ);

		void UpdateTimeLimits(float startTime, float endTime);

		void RenderHeatmap();

		void UpdateRenderStyle(RenderShape style, RenderDirection direction);
	}
}
