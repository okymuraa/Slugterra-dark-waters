using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.EditorTools
{
	/// <summary>
	/// Exportador profesional de escenarios limpios para Blender.
	/// Genera archivo .OBJ y .MTL con texturas automáticas (.png) para que en Blender se vea a todo color.
	/// </summary>
	public static class ExportMeshToObj
	{
		[MenuItem("Tools/Slugterra/1. Exportar SOLO el Terreno Limpio (Montañas y Suelo a OBJ)")]
		public static void ExportOnlyTerrain()
		{
			UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			if (!scene.isLoaded)
			{
				EditorUtility.DisplayDialog("Error", "No hay ninguna escena activa abierta.", "OK");
				return;
			}

			GameObject terrainGo = GameObject.Find("Terrain");
			if (terrainGo == null) terrainGo = GameObject.Find("2_1_TerrainMesh");

			List<GameObject> targets = new List<GameObject>();
			if (terrainGo != null)
			{
				targets.Add(terrainGo);
			}
			else
			{
				foreach (var root in scene.GetRootGameObjects())
				{
					foreach (var tr in root.GetComponentsInChildren<Transform>(true))
					{
						if (tr.gameObject.layer == 11 || tr.gameObject.name.Contains("TerrainMesh"))
						{
							targets.Add(tr.gameObject);
						}
					}
				}
			}

			if (targets.Count == 0)
			{
				EditorUtility.DisplayDialog("Aviso", "No se encontró el objeto 'Terrain' o '2_1_TerrainMesh' en la escena.", "OK");
				return;
			}

			string defaultName = scene.name + "_SOLO_TERRENO.obj";
			string path = EditorUtility.SaveFilePanel("Guardar SOLO Terreno para Blender", "", defaultName, "obj");
			if (string.IsNullOrEmpty(path)) return;

			ExportCleanHierarchy(targets.ToArray(), scene.name + "_Terreno", path, true);
		}

		[MenuItem("Tools/Slugterra/2. Exportar Escenario 3D Completo (Terreno + Casas + Árboles con Texturas)")]
		public static void ExportCleanEnvironment()
		{
			UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			if (!scene.isLoaded)
			{
				EditorUtility.DisplayDialog("Error", "No hay ninguna escena activa abierta.", "OK");
				return;
			}

			GameObject staticGeo = GameObject.Find("StaticGeometry");
			GameObject[] roots = staticGeo != null ? new[] { staticGeo } : scene.GetRootGameObjects();

			string defaultName = scene.name + "_ESCENARIO_CON_TEXTURAS.obj";
			string path = EditorUtility.SaveFilePanel("Guardar Escenario 3D para Blender", "", defaultName, "obj");
			if (string.IsNullOrEmpty(path)) return;

			ExportCleanHierarchy(roots, scene.name + "_Escenario", path, false);
		}

		[MenuItem("GameObject/Slugterra/Exportar este objeto a OBJ (Con Texturas)", false, 19)]
		public static void ExportSelectedHierarchyClean()
		{
			GameObject selected = Selection.activeGameObject;
			if (selected == null)
			{
				EditorUtility.DisplayDialog("Exportador", "Selecciona un objeto en la jerarquía (por ejemplo 'Terrain' o 'StaticGeometry').", "OK");
				return;
			}

			string defaultName = selected.name + "_Limpio.obj";
			string path = EditorUtility.SaveFilePanel("Guardar Objeto Limpio para Blender", "", defaultName, "obj");
			if (string.IsNullOrEmpty(path)) return;

			ExportCleanHierarchy(new[] { selected }, selected.name, path, false);
		}

		private static void ExportCleanHierarchy(GameObject[] roots, string exportName, string outputPath, bool onlyTerrain)
		{
			var renderers = new List<MeshRenderer>();
			foreach (var root in roots)
			{
				renderers.AddRange(root.GetComponentsInChildren<MeshRenderer>(true));
			}

			if (renderers.Count == 0)
			{
				EditorUtility.DisplayDialog("Exportador", "No se encontraron MeshRenderers en la selección.", "OK");
				return;
			}

			string mtlFileName = Path.GetFileNameWithoutExtension(outputPath) + ".mtl";
			string mtlFilePath = Path.Combine(Path.GetDirectoryName(outputPath), mtlFileName);

			StringBuilder sbObj = new StringBuilder();
			StringBuilder sbMtl = new StringBuilder();

			sbObj.AppendLine("# Slugterra Revival - Exportador Limpio con Texturas");
			sbObj.AppendLine($"# Export: {exportName}");
			sbObj.AppendLine($"mtllib {mtlFileName}");

			sbMtl.AppendLine("# Slugterra Revival - Archivo de Materiales para Blender");

			Dictionary<string, string> registeredMaterials = new Dictionary<string, string>();

			int globalVertexOffset = 0;
			int globalNormalOffset = 0;
			int globalUvOffset = 0;
			int objectsExported = 0;

			for (int r = 0; r < renderers.Count; r++)
			{
				MeshRenderer mr = renderers[r];
				if (mr == null || !mr.enabled) continue;

				GameObject go = mr.gameObject;
				if (!go.activeInHierarchy) continue;

				// Filtrado de basura y UI 2D
				if (go.layer == 5 || go.layer == 25 || go.layer == 20) continue;

				string goNameLower = go.name.ToLower();
				if (goNameLower.Contains("icon") || goNameLower.Contains("btn") || goNameLower.Contains("lbl") ||
					goNameLower.Contains("glow") || goNameLower.Contains("fade") || goNameLower.Contains("minimap") ||
					goNameLower.Contains("camera") || goNameLower.Contains("sky_cylinder") || goNameLower.Contains("shadow"))
				{
					continue;
				}

				if (onlyTerrain && go.layer != 11 && !go.name.Contains("TerrainMesh"))
				{
					continue;
				}

				MeshFilter mf = go.GetComponent<MeshFilter>();
				if (mf == null || mf.sharedMesh == null) continue;

				Mesh mesh = mf.sharedMesh;
				if (mesh.vertexCount == 0) continue;

				// Obtener material y textura correspondiente
				Material mat = mr.sharedMaterial;
				string matName = "Mat_Default";
				if (mat != null)
				{
					matName = mat.name.Replace(":", "_").Replace("/", "_").Replace(" ", "_");
					if (!registeredMaterials.ContainsKey(matName))
					{
						Texture tex = GetMainTextureFromMaterial(mat);
						string texLine = "";
						if (tex != null)
						{
							string assetPath = AssetDatabase.GetAssetPath(tex);
							if (!string.IsNullOrEmpty(assetPath))
							{
								string fullTexPath = Path.GetFullPath(assetPath);
								texLine = $"map_Kd {fullTexPath}\n";
							}
						}

						sbMtl.AppendLine($"newmtl {matName}");
						sbMtl.AppendLine("Ka 1.000 1.000 1.000");
						sbMtl.AppendLine("Kd 1.000 1.000 1.000");
						sbMtl.AppendLine("Ks 0.000 0.000 0.000");
						sbMtl.AppendLine("d 1.0");
						sbMtl.AppendLine("illum 1");
						if (!string.IsNullOrEmpty(texLine))
						{
							sbMtl.Append(texLine);
						}
						sbMtl.AppendLine();

						registeredMaterials[matName] = matName;
					}
				}

				// Manejo de Static Batching
				int firstSubMesh = 0;
				int subMeshCount = mesh.subMeshCount;
				bool isBatched = false;

				SerializedObject so = new SerializedObject(mr);
				SerializedProperty sbi = so.FindProperty("m_StaticBatchInfo");
				if (sbi != null)
				{
					var firstProp = sbi.FindPropertyRelative("firstSubMesh");
					var countProp = sbi.FindPropertyRelative("subMeshCount");
					if (firstProp != null && countProp != null && countProp.intValue > 0)
					{
						firstSubMesh = firstProp.intValue;
						subMeshCount = countProp.intValue;
						isBatched = true;
					}
				}

				if (firstSubMesh >= mesh.subMeshCount) continue;
				if (firstSubMesh + subMeshCount > mesh.subMeshCount)
				{
					subMeshCount = mesh.subMeshCount - firstSubMesh;
				}

				EditorUtility.DisplayProgressBar(
					"Exportando con Texturas",
					$"Procesando {go.name} ({r + 1}/{renderers.Count})...",
					(float)r / renderers.Count);

				string cleanObjName = go.name.Replace(":", "_").Replace("/", "_").Replace(" ", "_");
				sbObj.AppendLine($"\no {cleanObjName}_{objectsExported}");

				Vector3[] allVertices = mesh.vertices;
				Vector3[] allNormals = mesh.normals;
				Vector2[] allUvs = mesh.uv;
				bool hasNormals = allNormals != null && allNormals.Length > 0;
				bool hasUvs = allUvs != null && allUvs.Length > 0;

				for (int sm = 0; sm < subMeshCount; sm++)
				{
					int targetSubmeshIndex = firstSubMesh + sm;
					int[] triangles = mesh.GetTriangles(targetSubmeshIndex);
					if (triangles == null || triangles.Length == 0) continue;

					Dictionary<int, int> vertexRemap = new Dictionary<int, int>();
					List<int> usedVertIndices = new List<int>();

					for (int t = 0; t < triangles.Length; t++)
					{
						int origIdx = triangles[t];
						if (!vertexRemap.ContainsKey(origIdx))
						{
							vertexRemap[origIdx] = usedVertIndices.Count;
							usedVertIndices.Add(origIdx);
						}
					}

					foreach (int origIdx in usedVertIndices)
					{
						Vector3 v = allVertices[origIdx];
						Vector3 worldV = isBatched ? v : mf.transform.TransformPoint(v);
						sbObj.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "v {0:F6} {1:F6} {2:F6}", -worldV.x, worldV.y, worldV.z));

						if (hasNormals && origIdx < allNormals.Length)
						{
							Vector3 n = allNormals[origIdx];
							Vector3 worldN = isBatched ? n : mf.transform.TransformDirection(n).normalized;
							sbObj.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "vn {0:F6} {1:F6} {2:F6}", -worldN.x, worldN.y, worldN.z));
						}

						if (hasUvs && origIdx < allUvs.Length)
						{
							Vector2 uv = allUvs[origIdx];
							sbObj.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "vt {0:F6} {1:F6}", uv.x, uv.y));
						}
					}

					sbObj.AppendLine($"g {cleanObjName}_Submesh_{targetSubmeshIndex}");
					sbObj.AppendLine($"usemtl {matName}");

					for (int t = 0; t < triangles.Length; t += 3)
					{
						int i0 = vertexRemap[triangles[t]] + 1 + globalVertexOffset;
						int i1 = vertexRemap[triangles[t + 2]] + 1 + globalVertexOffset;
						int i2 = vertexRemap[triangles[t + 1]] + 1 + globalVertexOffset;

						if (hasNormals && hasUvs)
						{
							sbObj.AppendLine($"f {i0}/{i0}/{i0} {i1}/{i1}/{i1} {i2}/{i2}/{i2}");
						}
						else if (hasNormals)
						{
							sbObj.AppendLine($"f {i0}//{i0} {i1}//{i1} {i2}//{i2}");
						}
						else if (hasUvs)
						{
							sbObj.AppendLine($"f {i0}/{i0} {i1}/{i1} {i2}/{i2}");
						}
						else
						{
							sbObj.AppendLine($"f {i0} {i1} {i2}");
						}
					}

					globalVertexOffset += usedVertIndices.Count;
					if (hasNormals) globalNormalOffset += usedVertIndices.Count;
					if (hasUvs) globalUvOffset += usedVertIndices.Count;
				}

				objectsExported++;
			}

			EditorUtility.ClearProgressBar();

			if (objectsExported == 0)
			{
				EditorUtility.DisplayDialog("Aviso", "No se encontraron mallas válidas para exportar.", "OK");
				return;
			}

			File.WriteAllText(outputPath, sbObj.ToString(), Encoding.UTF8);
			File.WriteAllText(mtlFilePath, sbMtl.ToString(), Encoding.UTF8);

			EditorUtility.DisplayDialog(
				"Exportación Exitosa con Texturas",
				$"Se exportó exitosamente el archivo .OBJ y sus materiales .MTL:\n\n{outputPath}\n\n" +
				"Ahora al importarlo en Blender (File > Import > Wavefront .obj), " +
				"Blender conectará automáticamente las imágenes .PNG de las texturas.",
				"¡Excelente!");
		}

		private static Texture GetMainTextureFromMaterial(Material mat)
		{
			if (mat == null) return null;
			if (mat.HasProperty("_MainTex") && mat.GetTexture("_MainTex") != null) return mat.GetTexture("_MainTex");
			if (mat.HasProperty("_Splat0") && mat.GetTexture("_Splat0") != null) return mat.GetTexture("_Splat0");
			if (mat.HasProperty("_Control") && mat.GetTexture("_Control") != null) return mat.GetTexture("_Control");
			return mat.mainTexture;
		}
	}
}
