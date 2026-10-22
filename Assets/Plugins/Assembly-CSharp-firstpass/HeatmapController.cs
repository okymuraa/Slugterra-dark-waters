using System;
using System.Collections.Generic;
using UnityAnalyticsHeatmap;
using UnityEngine;

[RequireComponent(typeof(HeatmapMeshRenderer))]
public class HeatmapController : MonoBehaviour
{
	public string dataPath = string.Empty;

	public string[] options;

	public int optionIndex;

	private int oldOptionIndex = -1;

	public float pointSize = 10f;

	private float oldPointSize = -99f;

	private HeatmapDataParser m_DataParser = new HeatmapDataParser();

	private Dictionary<string, HeatPoint[]> m_Data;

	private Gradient gradient;

	private float m_MaxDensity;

	private float m_MaxTime;

	private Vector3 m_LowSpace = Vector3.zero;

	private Vector3 m_HighSpace = Vector3.zero;

	private void Start()
	{
		gradient = GetComponent<GradientContainer>().ColorGradient;
		if (!string.IsNullOrEmpty(dataPath))
		{
			LoadData();
		}
	}

	private void LoadData()
	{
		m_DataParser.LoadData(dataPath, parseHandler, true);
	}

	private void parseHandler(Dictionary<string, HeatPoint[]> heatData, string[] options)
	{
		m_Data = heatData;
		this.options = options;
		Render();
	}

	private void Render()
	{
		if (m_Data != null)
		{
			SetLimits(m_Data[options[optionIndex]]);
			IHeatmapRenderer component = gameObject.GetComponent<IHeatmapRenderer>();
			component.allowRender = true;
			component.pointSize = pointSize;
			component.UpdateGradient(gradient);
			component.UpdateTimeLimits(0f, m_MaxTime);
			component.UpdateRenderMask(m_LowSpace.x, m_HighSpace.x, m_LowSpace.y, m_HighSpace.y, m_LowSpace.z, m_HighSpace.z);
			component.UpdateRenderStyle(RenderShape.Triangle, RenderDirection.YZ);
			component.UpdatePointData(m_Data[options[optionIndex]], m_MaxDensity);
			component.RenderHeatmap();
		}
	}

	private void Update()
	{
		if (optionIndex != oldOptionIndex || pointSize != oldPointSize)
		{
			if (options == null)
			{
				optionIndex = 0;
			}
			else
			{
				optionIndex = Math.Max(0, optionIndex);
				optionIndex = Math.Min(optionIndex, options.Length - 1);
				Render();
				oldOptionIndex = optionIndex;
			}
			oldPointSize = pointSize;
		}
	}

	private void SetLimits(HeatPoint[] points)
	{
		float a = 0f;
		m_MaxTime = 0f;
		m_LowSpace = default(Vector3);
		m_HighSpace = default(Vector3);
		for (int i = 0; i < points.Length; i++)
		{
			a = Mathf.Max(a, points[i].density);
			m_MaxTime = Mathf.Max(m_MaxTime, points[i].time);
			m_LowSpace = Vector3.Min(m_LowSpace, points[i].position);
			m_HighSpace = Vector3.Max(m_HighSpace, points[i].position);
		}
	}
}
