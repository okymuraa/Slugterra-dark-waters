using System.Collections.Generic;
using System.Linq;
using UnityAnalyticsHeatmap;
using UnityEngine;

[RequireComponent(typeof(GradientContainer))]
public class HeatmapMeshRenderer : MonoBehaviour, IHeatmapRenderer
{
	private const int k_NotRendering = 0;

	private const int k_BeginRenderer = 1;

	private const int k_RenderInProgress = 2;

	private const int k_UpdateMaterials = 4;

	private const int k_VerticesPerMesh = 65000;

	private float m_HighThreshold;

	private float m_LowThreshold;

	private float m_StartTime;

	private float m_EndTime = 1f;

	[Range(0.1f, 100f)]
	private float m_ParticleSize = 1f;

	private bool m_Tips;

	private HeatPoint[] m_Data;

	private float m_MaxDensity;

	private float m_LowX;

	private float m_LowY;

	private float m_LowZ;

	private float m_HighX = 1f;

	private float m_HighY = 1f;

	private float m_HighZ = 1f;

	private RenderShape m_RenderStyle;

	private RenderDirection m_RenderDirection;

	private Shader m_Shader;

	public Material[] m_Materials;

	private Gradient m_Gradient;

	private int m_RenderState;

	private List<GameObject> m_GameObjects = new List<GameObject>();

	public float pointSize
	{
		get
		{
			return m_ParticleSize;
		}
		set
		{
			if (m_ParticleSize != value)
			{
				m_ParticleSize = value;
				m_RenderState = 1;
			}
		}
	}

	public bool activateTips
	{
		get
		{
			return m_Tips;
		}
		set
		{
			if (m_Tips == value)
			{
				return;
			}
			m_Tips = value;
			if (m_Tips)
			{
				m_RenderState = 1;
				return;
			}
			MeshCollider[] componentsInChildren = transform.GetComponentsInChildren<MeshCollider>();
			MeshCollider[] array = componentsInChildren;
			foreach (MeshCollider meshCollider in array)
			{
				Object.DestroyImmediate(meshCollider.gameObject.GetComponent<MeshCollider>());
			}
		}
	}

	public bool allowRender { get; set; }

	public int currentPoints { get; set; }

	public int totalPoints { get; set; }

	private void Start()
	{
		m_Shader = Shader.Find("Heatmaps/Particles/AlphaBlend");
	}

	public void UpdatePointData(HeatPoint[] newData, float newMaxDensity)
	{
		m_Data = newData;
		m_MaxDensity = newMaxDensity;
		m_RenderState = 1;
	}

	public void UpdateColors(Color[] colors)
	{
	}

	public void UpdateThresholds(float[] threshholds)
	{
	}

	public void UpdateGradient(Gradient gradient)
	{
		if (m_Materials == null || m_Materials.Length == 0)
		{
			m_Shader = Shader.Find("Heatmaps/Particles/AlphaBlend");
			m_Materials = new Material[1];
			m_Materials[0] = new Material(m_Shader);
		}
		if (gradient == null || !CompareGradients(gradient, m_Gradient))
		{
			m_Gradient = gradient;
			m_RenderState = 4;
		}
	}

	public void UpdateRenderMask(float lowX, float highX, float lowY, float highY, float lowZ, float highZ)
	{
		if (lowX != m_LowX)
		{
			m_LowX = lowX;
			m_RenderState = 1;
		}
		if (lowY != m_LowY)
		{
			m_LowY = lowY;
			m_RenderState = 1;
		}
		if (lowZ != m_LowZ)
		{
			m_LowZ = lowZ;
			m_RenderState = 1;
		}
		if (highX != m_HighX)
		{
			m_HighX = highX;
			m_RenderState = 1;
		}
		if (highY != m_HighY)
		{
			m_HighY = highY;
			m_RenderState = 1;
		}
		if (highZ != m_HighZ)
		{
			m_HighZ = highZ;
			m_RenderState = 1;
		}
	}

	public void UpdateTimeLimits(float startTime, float endTime)
	{
		if (m_StartTime != startTime || m_EndTime != endTime)
		{
			m_StartTime = startTime;
			m_EndTime = endTime;
			m_RenderState = 1;
		}
	}

	public void UpdateRenderStyle(RenderShape style, RenderDirection direction)
	{
		if (style != m_RenderStyle || direction != m_RenderDirection)
		{
			m_RenderDirection = direction;
			m_RenderStyle = style;
			m_RenderState = 1;
		}
	}

	public void RenderHeatmap()
	{
		if (!allowRender)
		{
			return;
		}
		switch (m_RenderState)
		{
		case 1:
			m_RenderState = 2;
			CreatePoints();
			break;
		case 2:
			if (!hasData())
			{
			}
			break;
		case 4:
		{
			if (!hasData())
			{
				break;
			}
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			int vecticesForShape = GetVecticesForShape();
			GameObject gameObject = null;
			Material[] array = null;
			for (int i = 0; i < m_Data.Length; i++)
			{
				if (!(m_Data[i].time >= m_StartTime) || !(m_Data[i].time <= m_EndTime))
				{
					continue;
				}
				num2 = num * vecticesForShape / 65000;
				if (num2 != num3)
				{
					if (gameObject != null && array != null)
					{
						gameObject.GetComponent<Renderer>().materials = array;
					}
					gameObject = m_GameObjects[num2];
					array = gameObject.GetComponent<Renderer>().sharedMaterials;
				}
				array = m_Materials;
				num3 = num2;
				num++;
			}
			if (gameObject != null && array != null)
			{
				gameObject.GetComponent<Renderer>().materials = array;
			}
			CreatePoints();
			break;
		}
		}
	}

	private void CreatePoints()
	{
		if (!hasData())
		{
			return;
		}
		totalPoints = m_Data.Length;
		currentPoints = 0;
		List<List<HeatPoint>> list = new List<List<HeatPoint>>();
		int num = 0;
		int vecticesForShape = GetVecticesForShape();
		for (int i = 0; i < m_Data.Length; i++)
		{
			HeatPoint item = m_Data[i];
			if (item.time >= m_StartTime && item.time <= m_EndTime && item.position.x >= m_LowX && item.position.x <= m_HighX && item.position.y >= m_LowY && item.position.y <= m_HighY && item.position.z >= m_LowZ && item.position.z <= m_HighZ)
			{
				currentPoints++;
				if (list.Count <= num)
				{
					list.Add(new List<HeatPoint>());
				}
				list[num].Add(item);
				num = currentPoints * vecticesForShape / 65000;
			}
		}
		if (currentPoints == 0)
		{
			m_RenderState = 0;
			return;
		}
		int count = list.Count;
		int count2 = m_GameObjects.Count;
		int num2 = count - count2;
		if (num2 > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				int num3 = count2 + j;
				GameObject gameObject = new GameObject("Submap" + num3);
				gameObject.AddComponent<HeatmapSubmap>();
				gameObject.GetComponent<MeshFilter>().sharedMesh = new Mesh();
				gameObject.transform.parent = base.gameObject.transform;
				m_GameObjects.Add(gameObject);
			}
		}
		else if (num2 < 0)
		{
			for (int k = count; k < count2; k++)
			{
				Transform transform = base.gameObject.transform.Find("Submap" + k);
				if (transform != null)
				{
					transform.parent = null;
					m_GameObjects.Remove(transform.gameObject);
					Object.DestroyImmediate(transform.gameObject);
				}
			}
		}
		for (int l = 0; l < m_GameObjects.Count; l++)
		{
			Mesh sharedMesh = m_GameObjects[l].GetComponent<MeshFilter>().sharedMesh;
			sharedMesh.Clear();
			sharedMesh.subMeshCount = list[l].Count;
			RenderSubmap(m_GameObjects[l], list[l]);
		}
		m_RenderState = 0;
	}

	private void RenderSubmap(GameObject go, List<HeatPoint> submap)
	{
		List<int[]> list = new List<int[]>();
		List<Vector3[]> list2 = new List<Vector3[]>();
		List<Color32[]> list3 = new List<Color32[]>();
		Vector3[] array = null;
		Material[] array2 = new Material[submap.Count];
		for (int i = 0; i < submap.Count; i++)
		{
			array2[i] = m_Materials[0];
			Vector3 position = submap[i].position;
			Vector3 rotation = submap[i].rotation;
			Vector3 destination = submap[i].destination;
			switch (m_RenderStyle)
			{
			case RenderShape.Cube:
				array = AddCubeVectorsToMesh(position.x, position.y, position.z);
				list2.Add(array);
				list.Add(AddCubeTrisToMesh(i * array.Length));
				break;
			case RenderShape.Arrow:
				array = AddArrowVectorsToMesh(position, rotation);
				list2.Add(array);
				list.Add(AddArrowTrisToMesh(i * array.Length));
				break;
			case RenderShape.Square:
				array = AddSquareVectorsToMesh(position.x, position.y, position.z);
				list2.Add(array);
				list.Add(AddSquareTrisToMesh(i * array.Length));
				break;
			case RenderShape.Triangle:
				array = AddTriVectorsToMesh(position.x, position.y, position.z);
				list2.Add(array);
				list.Add(AddTriTrisToMesh(i * array.Length));
				break;
			case RenderShape.PointToPoint:
				array = AddP2PVectorsToMesh(position, destination);
				list2.Add(array);
				list.Add(AddP2PTrisToMesh(i * array.Length));
				break;
			}
			list3.Add(AddColorsToMesh(array.Length, submap[i]));
		}
		Vector3[] vertices = list2.SelectMany((Vector3[] x) => x).ToArray();
		Mesh sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
		sharedMesh.vertices = vertices;
		sharedMesh.colors32 = list3.SelectMany((Color32[] x) => x).ToArray();
		for (int num = 0; num < list.Count; num++)
		{
			int[] triangles = list[num];
			sharedMesh.SetTriangles(triangles, num);
		}
		go.GetComponent<Renderer>().materials = array2;
		go.GetComponent<HeatmapSubmap>().m_PointData = submap;
		go.GetComponent<HeatmapSubmap>().m_TrianglesPerShape = GetTrianglesForShape();
		if (m_Tips)
		{
			if (go.GetComponent<MeshCollider>() == null)
			{
				go.AddComponent<MeshCollider>();
			}
			go.GetComponent<MeshCollider>().sharedMesh = sharedMesh;
		}
	}

	private Color32[] AddColorsToMesh(int count, HeatPoint pt)
	{
		Color32[] array = new Color32[count];
		float num = pt.density / m_MaxDensity;
		if (float.IsInfinity(num))
		{
			num = 0f;
		}
		Color color = PickGradientColor(num);
		for (int i = 0; i < count; i++)
		{
			array[i] = new Color(color.r, color.g, color.b, color.a);
		}
		return array;
	}

	private Vector3[] AddCubeVectorsToMesh(float x, float y, float z)
	{
		float num = m_ParticleSize / 2f;
		Vector3 vector = new Vector3(x - num, y - num, z - num);
		Vector3 vector2 = new Vector3(x + num, y - num, z - num);
		Vector3 vector3 = new Vector3(x + num, y + num, z - num);
		Vector3 vector4 = new Vector3(x - num, y + num, z - num);
		Vector3 vector5 = new Vector3(x - num, y - num, z + num);
		Vector3 vector6 = new Vector3(x + num, y - num, z + num);
		Vector3 vector7 = new Vector3(x + num, y + num, z + num);
		Vector3 vector8 = new Vector3(x - num, y + num, z + num);
		return new Vector3[8] { vector, vector2, vector3, vector4, vector5, vector6, vector7, vector8 };
	}

	private int[] AddCubeTrisToMesh(int offset)
	{
		int[] array = new int[36]
		{
			0, 1, 2, 0, 2, 3, 4, 6, 5, 4,
			7, 6, 1, 6, 2, 1, 5, 6, 3, 4,
			0, 3, 7, 4, 2, 7, 3, 2, 6, 7,
			0, 4, 5, 0, 5, 1
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i] += offset;
		}
		return array;
	}

	private Vector3[] AddArrowVectorsToMesh(Vector3 position, Vector3 rotation)
	{
		float num = m_ParticleSize / 3f;
		Vector3 vector = new Vector3(0f - num, 0f, 0f);
		Vector3 vector2 = new Vector3(0f - num, 0f, (0f - m_ParticleSize) * 2f);
		Vector3 vector3 = new Vector3(0f - m_ParticleSize, 0f, (0f - m_ParticleSize) * 2f);
		Vector3 vector4 = new Vector3(0f, 0f, (0f - m_ParticleSize) * 3f);
		Vector3 vector5 = new Vector3(m_ParticleSize, 0f, (0f - m_ParticleSize) * 2f);
		Vector3 vector6 = new Vector3(num, 0f, (0f - m_ParticleSize) * 2f);
		Vector3 vector7 = new Vector3(num, 0f, 0f);
		Vector3[] array = new Vector3[7] { vector, vector2, vector3, vector4, vector5, vector6, vector7 };
		Quaternion q = Quaternion.Euler(rotation);
		for (int i = 0; i < array.Length; i++)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, q, Vector3.one);
			array[i] = matrix4x.MultiplyPoint3x4(array[i]);
		}
		return array;
	}

	private int[] AddArrowTrisToMesh(int offset)
	{
		int[] array = new int[9] { 0, 1, 5, 6, 0, 5, 3, 4, 2 };
		for (int i = 0; i < array.Length; i++)
		{
			array[i] += offset;
		}
		return array;
	}

	private Vector3[] AddSquareVectorsToMesh(float x, float y, float z)
	{
		float num = m_ParticleSize / 2f;
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		Vector3 vector4;
		switch (m_RenderDirection)
		{
		case RenderDirection.YZ:
			vector = new Vector3(x, y - num, z - num);
			vector2 = new Vector3(x, y + num, z - num);
			vector3 = new Vector3(x, y + num, z + num);
			vector4 = new Vector3(x, y - num, z + num);
			break;
		case RenderDirection.XZ:
			vector = new Vector3(x - num, y, z - num);
			vector2 = new Vector3(x + num, y, z - num);
			vector3 = new Vector3(x + num, y, z + num);
			vector4 = new Vector3(x - num, y, z + num);
			break;
		default:
			vector = new Vector3(x - num, y - num, z);
			vector2 = new Vector3(x + num, y - num, z);
			vector3 = new Vector3(x + num, y + num, z);
			vector4 = new Vector3(x - num, y + num, z);
			break;
		}
		return new Vector3[4] { vector, vector2, vector3, vector4 };
	}

	private int[] AddSquareTrisToMesh(int offset)
	{
		return new int[6]
		{
			offset,
			offset + 2,
			offset + 1,
			offset,
			offset + 3,
			offset + 2
		};
	}

	private Vector3[] AddTriVectorsToMesh(float x, float y, float z)
	{
		float num = m_ParticleSize / 2f;
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		switch (m_RenderDirection)
		{
		case RenderDirection.YZ:
			vector = new Vector3(x, y - num, z - num);
			vector2 = new Vector3(x, y, z + num);
			vector3 = new Vector3(x, y + num, z - num);
			break;
		case RenderDirection.XZ:
			vector = new Vector3(x - num, y, z - num);
			vector2 = new Vector3(x, y, z + num);
			vector3 = new Vector3(x + num, y, z - num);
			break;
		default:
			vector = new Vector3(x - num, y - num, z);
			vector2 = new Vector3(x, y + num, z);
			vector3 = new Vector3(x + num, y - num, z);
			break;
		}
		return new Vector3[3] { vector, vector2, vector3 };
	}

	private int[] AddTriTrisToMesh(int offset)
	{
		return new int[3]
		{
			offset,
			offset + 2,
			offset + 1
		};
	}

	private Vector3[] AddP2PVectorsToMesh(Vector3 fromVector, Vector3 toVector)
	{
		Vector3 vector = toVector - fromVector;
		Quaternion q = ((!(vector == Vector3.zero)) ? Quaternion.LookRotation(vector) : Quaternion.identity);
		float num = Vector3.Distance(fromVector, toVector);
		float z = num - m_ParticleSize * 1.5f;
		float num2 = m_ParticleSize * 0.5f;
		Vector3 vector2 = new Vector3(0f - m_ParticleSize, 0f, 0f - num2);
		Vector3 vector3 = new Vector3(0f - m_ParticleSize, 0f, num2);
		Vector3 vector4 = new Vector3(m_ParticleSize, 0f, num2);
		Vector3 vector5 = new Vector3(m_ParticleSize, 0f, 0f - num2);
		Vector3 vector6 = new Vector3(-0.5f * m_ParticleSize, 0f, 0f - num2);
		Vector3 vector7 = new Vector3(-0.5f * m_ParticleSize, 0f, z);
		Vector3 vector8 = new Vector3(0.5f * m_ParticleSize, 0f, z);
		Vector3 vector9 = new Vector3(0.5f * m_ParticleSize, 0f, 0f - num2);
		Vector3 vector10 = new Vector3(0f - m_ParticleSize, 0f, z);
		Vector3 vector11 = new Vector3(0f, 0f, num);
		Vector3 vector12 = new Vector3(m_ParticleSize, 0f, z);
		Vector3 vector13 = new Vector3(0f - m_ParticleSize, 0f, num);
		Vector3 vector14 = new Vector3(0f - m_ParticleSize, 0f, num + num2);
		Vector3 vector15 = new Vector3(m_ParticleSize, 0f, num + num2);
		Vector3 vector16 = new Vector3(m_ParticleSize, 0f, num);
		Vector3[] array = new Vector3[15]
		{
			vector2, vector3, vector4, vector5, vector6, vector7, vector8, vector9, vector10, vector11,
			vector12, vector13, vector14, vector15, vector16
		};
		for (int i = 0; i < array.Length; i++)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(fromVector, q, Vector3.one);
			array[i] = matrix4x.MultiplyPoint3x4(array[i]);
		}
		return array;
	}

	private int[] AddP2PTrisToMesh(int offset)
	{
		return new int[21]
		{
			offset,
			offset + 1,
			offset + 2,
			offset,
			offset + 2,
			offset + 3,
			offset + 4,
			offset + 5,
			offset + 6,
			offset + 4,
			offset + 6,
			offset + 7,
			offset + 8,
			offset + 9,
			offset + 10,
			offset + 11,
			offset + 12,
			offset + 13,
			offset + 11,
			offset + 13,
			offset + 14
		};
	}

	private bool hasData()
	{
		return m_Data != null && m_Data.Length > 0;
	}

	private int GetVecticesForShape()
	{
		int result = 0;
		switch (m_RenderStyle)
		{
		case RenderShape.Cube:
			result = 8;
			break;
		case RenderShape.Arrow:
			result = 7;
			break;
		case RenderShape.Square:
			result = 4;
			break;
		case RenderShape.Triangle:
			result = 3;
			break;
		case RenderShape.PointToPoint:
			result = 15;
			break;
		}
		return result;
	}

	private int GetTrianglesForShape()
	{
		int result = 0;
		switch (m_RenderStyle)
		{
		case RenderShape.Cube:
			result = 12;
			break;
		case RenderShape.Arrow:
			result = 3;
			break;
		case RenderShape.Square:
			result = 2;
			break;
		case RenderShape.Triangle:
			result = 1;
			break;
		case RenderShape.PointToPoint:
			result = 7;
			break;
		}
		return result;
	}

	private Color PickGradientColor(float percent)
	{
		return m_Gradient.Evaluate(percent);
	}

	public static bool CompareGradients(Gradient gradient, Gradient otherGradient)
	{
		if (gradient == null || otherGradient == null)
		{
			return false;
		}
		if (gradient.colorKeys.Length != otherGradient.colorKeys.Length || gradient.alphaKeys.Length != otherGradient.alphaKeys.Length)
		{
			return false;
		}
		for (int i = 0; i < gradient.colorKeys.Length; i++)
		{
			GradientColorKey gradientColorKey = gradient.colorKeys[i];
			GradientColorKey gradientColorKey2 = otherGradient.colorKeys[i];
			if (gradientColorKey.color != gradientColorKey2.color || gradientColorKey.time != gradientColorKey2.time)
			{
				return false;
			}
		}
		for (int j = 0; j < gradient.alphaKeys.Length; j++)
		{
			GradientAlphaKey gradientAlphaKey = gradient.alphaKeys[j];
			GradientAlphaKey gradientAlphaKey2 = otherGradient.alphaKeys[j];
			if (gradientAlphaKey.alpha != gradientAlphaKey2.alpha || gradientAlphaKey.time != gradientAlphaKey2.time)
			{
				return false;
			}
		}
		return true;
	}
}
