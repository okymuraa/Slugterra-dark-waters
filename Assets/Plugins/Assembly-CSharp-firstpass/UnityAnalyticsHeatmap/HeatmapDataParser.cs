using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

namespace UnityAnalyticsHeatmap
{
	public class HeatmapDataParser
	{
		public delegate void ParseHandler(Dictionary<string, HeatPoint[]> heatData, string[] options);

		private ParseHandler m_ParseHandler;

		public void LoadData(string path, ParseHandler handler, bool asResource = false)
		{
			m_ParseHandler = handler;
			if (!string.IsNullOrEmpty(path))
			{
				if (asResource)
				{
					LoadResource(path);
				}
				else
				{
					LoadStream(path);
				}
			}
		}

		protected void LoadStream(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			using (streamReader)
			{
				ConsumeHeatmapData(streamReader.ReadToEnd());
			}
		}

		protected void LoadResource(string path)
		{
			TextAsset textAsset = Resources.Load(path) as TextAsset;
			ConsumeHeatmapData(textAsset.text);
		}

		protected void ConsumeHeatmapData(string text)
		{
			Dictionary<string, HeatPoint[]> dictionary = new Dictionary<string, HeatPoint[]>();
			ArrayList arrayList = new ArrayList();
			float b = 0f;
			float b2 = 0f;
			Vector3 rhs = Vector3.zero;
			Vector3 rhs2 = Vector3.zero;
			Dictionary<string, object> dictionary2 = Json.Deserialize(text) as Dictionary<string, object>;
			foreach (KeyValuePair<string, object> item in dictionary2)
			{
				arrayList.Add(item.Key);
				List<object> list = item.Value as List<object>;
				HeatPoint[] array = new HeatPoint[list.Count];
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					array[i] = default(HeatPoint);
					float x = 0f;
					float y = 0f;
					float z = 0f;
					float time = 0f;
					float x2 = 0f;
					float y2 = 0f;
					float z2 = 0f;
					float x3 = 0f;
					float y3 = 0f;
					float z3 = 0f;
					float num = 0f;
					Dictionary<string, object> dictionary3 = list[i] as Dictionary<string, object>;
					foreach (KeyValuePair<string, object> item2 in dictionary3)
					{
						float num2 = (float)Convert.ToDouble(item2.Value);
						switch (item2.Key)
						{
						case "x":
							x = num2;
							break;
						case "y":
							y = num2;
							break;
						case "z":
							z = num2;
							break;
						case "t":
							time = num2;
							break;
						case "d":
							num = num2;
							break;
						case "rx":
							x2 = num2;
							break;
						case "ry":
							y2 = num2;
							break;
						case "rz":
							z2 = num2;
							break;
						case "dx":
							x3 = num2;
							break;
						case "dy":
							y3 = num2;
							break;
						case "dz":
							z3 = num2;
							break;
						}
					}
					array[i].position = new Vector3(x, y, z);
					array[i].rotation = new Vector3(x2, y2, z2);
					array[i].destination = new Vector3(x3, y3, z3);
					array[i].density = num;
					array[i].time = time;
					b = Mathf.Max(num, b);
					b2 = Mathf.Max(array[i].time, b2);
					if (i == 0)
					{
						rhs = (rhs2 = array[i].position);
						continue;
					}
					rhs = Vector3.Min(array[i].position, rhs);
					rhs2 = Vector3.Max(array[i].position, rhs2);
				}
				dictionary[item.Key] = array;
			}
			if (m_ParseHandler != null)
			{
				m_ParseHandler(dictionary, arrayList.ToArray(typeof(string)) as string[]);
			}
		}
	}
}
