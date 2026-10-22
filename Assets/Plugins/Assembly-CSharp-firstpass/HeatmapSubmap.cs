using System.Collections.Generic;
using UnityAnalyticsHeatmap;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class HeatmapSubmap : MonoBehaviour
{
	public List<HeatPoint> m_PointData;

	public int m_TrianglesPerShape;

	private void Start()
	{
		GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
		GetComponent<MeshRenderer>().receiveShadows = false;
		GetComponent<MeshRenderer>().lightProbeUsage = LightProbeUsage.Off;
		GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
	}
}
