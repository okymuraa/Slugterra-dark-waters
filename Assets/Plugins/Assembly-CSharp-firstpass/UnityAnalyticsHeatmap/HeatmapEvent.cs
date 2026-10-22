using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace UnityAnalyticsHeatmap
{
	public class HeatmapEvent
	{
		private static Dictionary<string, object> s_Dictionary = new Dictionary<string, object>();

		public static AnalyticsResult Send(string eventName, Vector3 v, Dictionary<string, object> options = null)
		{
			AddXY(v.x, v.y);
			AddZ(v.z);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Vector3 v, float time, Dictionary<string, object> options = null)
		{
			AddXY(v.x, v.y);
			AddZ(v.z);
			AddTime(time);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Vector3 v, float time, float rotation, Dictionary<string, object> options = null)
		{
			AddXY(v.x, v.y);
			AddZ(v.z);
			s_Dictionary["rx"] = rotation;
			AddTime(time);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Transform trans, Dictionary<string, object> options = null)
		{
			AddXY(trans.position.x, trans.position.y);
			AddZ(trans.position.z);
			AddRotation(trans.rotation.eulerAngles);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Transform trans, float time, Dictionary<string, object> options = null)
		{
			AddXY(trans.position.x, trans.position.y);
			AddZ(trans.position.z);
			AddRotation(trans.rotation.eulerAngles);
			AddTime(time);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Vector3 v, Vector3 v1, Dictionary<string, object> options = null)
		{
			AddXY(v.x, v.y);
			AddZ(v.z);
			AddDestination(v1);
			AddOptions(options);
			return Commit(eventName);
		}

		public static AnalyticsResult Send(string eventName, Vector3 v, Vector3 v1, float time, Dictionary<string, object> options = null)
		{
			AddXY(v.x, v.y);
			AddZ(v.z);
			AddDestination(v1);
			AddTime(time);
			AddOptions(options);
			return Commit(eventName);
		}

		protected static AnalyticsResult Commit(string eventName)
		{
			AnalyticsResult result = Analytics.CustomEvent("Heatmap." + eventName, s_Dictionary);
			s_Dictionary.Clear();
			return result;
		}

		protected static void AddXY(float x, float y)
		{
			s_Dictionary["x"] = x;
			s_Dictionary["y"] = y;
		}

		protected static void AddZ(float z)
		{
			s_Dictionary["z"] = z;
		}

		protected static void AddTime(float time)
		{
			s_Dictionary["t"] = time;
		}

		protected static void AddRotation(Vector3 r)
		{
			s_Dictionary["rx"] = r.x;
			s_Dictionary["ry"] = r.y;
			s_Dictionary["rz"] = r.z;
		}

		protected static void AddDestination(Vector3 v)
		{
			s_Dictionary["dx"] = v.x;
			s_Dictionary["dy"] = v.y;
			s_Dictionary["dz"] = v.z;
		}

		protected static void AddOptions(Dictionary<string, object> options)
		{
			if (options == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> option in options)
			{
				s_Dictionary[option.Key] = option.Value;
			}
		}
	}
}
