using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteRadial")]
public class tk2dRadialSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector2[] meshUvs;

	private Vector3[] meshVertices;

	private Color32[] meshColors;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private int[] meshIndices;

	[SerializeField]
	protected bool _createBoxCollider;

	private Vector3 boundsCenter = Vector3.zero;

	private Vector3 boundsExtents = Vector3.zero;

	public bool CreateBoxCollider
	{
		get
		{
			return _createBoxCollider;
		}
		set
		{
			if (_createBoxCollider != value)
			{
				_createBoxCollider = value;
				UpdateCollider();
			}
		}
	}

	private new void Awake()
	{
		base.Awake();
		mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		GetComponent<MeshFilter>().mesh = mesh;
		if (boxCollider == null)
		{
			boxCollider = GetComponent<BoxCollider>();
		}
		if (boxCollider2D == null)
		{
			boxCollider2D = GetComponent<BoxCollider2D>();
		}
		if ((bool)Collection)
		{
			if (_spriteId < 0 || _spriteId >= Collection.Count)
			{
				_spriteId = 0;
			}
			Build();
		}
	}

	protected void OnDestroy()
	{
		if ((bool)mesh)
		{
			UnityEngine.Object.Destroy(mesh);
		}
	}

	protected new void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 16, _color, collectionInst.premultipliedAlpha);
	}

	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = CurrentSprite;
		tk2dSpriteGeomGen.SetRadialSpriteGeom(meshVertices, meshUvs, meshNormals, meshTangents, 0, currentSprite, scale);
		if (meshNormals.Length > 0 || meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(meshVertices, meshVertices[0], meshVertices[15], currentSprite.normals, currentSprite.tangents, meshNormals, meshTangents);
		}
		if (currentSprite.positions.Length != 4 || currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	private void SetIndices()
	{
		meshIndices = new int[54];
		tk2dSpriteGeomGen.SetRadialSpriteindices(meshIndices, 0, 0, CurrentSprite);
	}

	private bool NearEnough(float value, float compValue, float scale)
	{
		float num = Mathf.Abs(value - compValue);
		return Mathf.Abs(num / scale) < 0.01f;
	}

	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = CurrentSprite;
		meshUvs = new Vector2[16];
		meshVertices = new Vector3[16];
		meshColors = new Color32[16];
		meshNormals = new Vector3[0];
		meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			meshNormals = new Vector3[16];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			meshTangents = new Vector4[16];
		}
		SetIndices();
		SetGeometry(meshVertices, meshUvs);
		SetColors(meshColors);
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			mesh.Clear();
		}
		mesh.vertices = meshVertices;
		mesh.colors32 = meshColors;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.triangles = meshIndices;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
		GetComponent<MeshFilter>().mesh = mesh;
		UpdateCollider();
		UpdateMaterial();
	}

	protected override void UpdateGeometry()
	{
		UpdateGeometryImpl();
	}

	protected override void UpdateColors()
	{
		UpdateColorsImpl();
	}

	protected override void UpdateVertices()
	{
		UpdateGeometryImpl();
	}

	private void UpdateIndices()
	{
		if (mesh != null)
		{
			SetIndices();
			mesh.triangles = meshIndices;
		}
	}

	protected void UpdateColorsImpl()
	{
		if (meshColors == null || meshColors.Length == 0)
		{
			Build();
			return;
		}
		SetColors(meshColors);
		mesh.colors32 = meshColors;
	}

	protected void UpdateGeometryImpl()
	{
		if (meshVertices == null || meshVertices.Length == 0)
		{
			Build();
			return;
		}
		mesh.vertices = meshVertices;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
		UpdateCollider();
	}

	protected override void UpdateCollider()
	{
		if (!CreateBoxCollider)
		{
			return;
		}
		if (CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			if (boxCollider != null)
			{
				boxCollider.size = 2f * boundsExtents;
				boxCollider.center = boundsCenter;
			}
		}
		else if (CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D && boxCollider2D != null)
		{
			boxCollider2D.size = 2f * boundsExtents;
			boxCollider2D.offset = boundsCenter;
		}
	}

	protected override void CreateCollider()
	{
		UpdateCollider();
	}

	protected override void UpdateMaterial()
	{
		if (GetComponent<Renderer>().sharedMaterial != collectionInst.spriteDefinitions[spriteId].materialInst)
		{
			GetComponent<Renderer>().material = collectionInst.spriteDefinitions[spriteId].materialInst;
		}
	}

	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	public void SetValue(float _value)
	{
		SetGeometry(meshVertices, meshUvs);
		float num = (float)Math.PI * 2f * _value;
		float num2 = Mathf.Min(Mathf.Abs(Mathf.Tan(num)), 1f) * Mathf.Sign(Mathf.Sin(num));
		float num3 = Mathf.Min(Mathf.Abs(Mathf.Tan(num + (float)Math.PI / 2f)), 1f) * Mathf.Sign(Mathf.Cos(num));
		float magnitude = (meshVertices[3] - meshVertices[0]).magnitude;
		float magnitude2 = (meshVertices[12] - meshVertices[0]).magnitude;
		Vector3 vector = default(Vector3);
		vector.x = meshVertices[5].x + num2 * magnitude * 0.5f;
		vector.y = meshVertices[5].y + num3 * magnitude2 * 0.5f;
		vector.z = meshVertices[5].z;
		if (_value <= 0.125f)
		{
			meshVertices[14] = vector;
			meshUvs[14] += (meshUvs[15] - meshUvs[14]) * num2;
		}
		else if (_value <= 0.25f)
		{
			meshVertices[15] = vector;
			meshVertices[14] = meshVertices[15];
			meshUvs[15] += (meshUvs[11] - meshUvs[15]) * (1f - num3);
			meshUvs[14] = meshUvs[15];
		}
		else if (_value <= 0.375f)
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[7] = vector;
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[7] += (meshUvs[3] - meshUvs[7]) * Mathf.Abs(num3);
		}
		else if (_value <= 0.5f)
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[3] = vector;
			meshVertices[7] = meshVertices[3];
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[3] += (meshUvs[2] - meshUvs[3]) * (1f - num2);
			meshUvs[7] = meshUvs[3];
		}
		else if (_value <= 0.625f)
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[7] = (meshVertices[3] = meshVertices[2]);
			meshVertices[1] = vector;
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[7] = (meshUvs[3] = meshUvs[2]);
			meshUvs[1] += (meshUvs[0] - meshUvs[1]) * Mathf.Abs(num2);
		}
		else if (_value <= 0.75f)
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[7] = (meshVertices[3] = meshVertices[2]);
			meshVertices[0] = vector;
			meshVertices[1] = meshVertices[0];
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[7] = (meshUvs[3] = meshUvs[2]);
			meshUvs[0] += (meshUvs[4] - meshUvs[0]) * (1f - Mathf.Abs(num3));
			meshUvs[1] = meshUvs[0];
		}
		else if (_value <= 0.875f)
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[7] = (meshVertices[3] = meshVertices[2]);
			meshVertices[1] = (meshVertices[0] = meshVertices[4]);
			meshVertices[8] = vector;
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[7] = (meshUvs[3] = meshUvs[2]);
			meshUvs[1] = (meshUvs[0] = meshUvs[4]);
			meshUvs[8] += (meshUvs[12] - meshUvs[8]) * num3;
		}
		else
		{
			meshVertices[14] = (meshVertices[15] = meshVertices[11]);
			meshVertices[7] = (meshVertices[3] = meshVertices[2]);
			meshVertices[1] = (meshVertices[0] = meshVertices[4]);
			meshVertices[12] = vector;
			meshVertices[8] = meshVertices[12];
			meshUvs[14] = (meshUvs[15] = meshUvs[11]);
			meshUvs[7] = (meshUvs[3] = meshUvs[2]);
			meshUvs[1] = (meshUvs[0] = meshUvs[4]);
			meshUvs[12] += (meshUvs[13] - meshUvs[12]) * (1f - Mathf.Abs(num2));
			meshUvs[8] = meshUvs[12];
		}
		UpdateGeometry();
	}
}
