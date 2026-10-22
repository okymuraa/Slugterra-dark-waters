using System;
using UnityEngine;

namespace AppsMinistry.Domain.Configuration
{
	public abstract class Config : DomainObject
	{
		public virtual void Save()
		{
			Save(GetPath(GetType()));
		}

		public void Save(string path)
		{
			GetSerializer(GetType()).Save(this, path);
		}

		public static T LoadFile<T>() where T : Config
		{
			return LoadFile<T>(GetPath(typeof(T)));
		}

		public static T LoadFile<T>(string path) where T : Config
		{
			return (T)LoadFile(typeof(T), path);
		}

		public static object LoadFile(Type type, string path)
		{
			return GetSerializer(type).LoadFile(type, path);
		}

		public static T LoadTextAsset<T>() where T : Config
		{
			return LoadTextAsset<T>(GetPath(typeof(T)));
		}

		public static T LoadTextAsset<T>(string path) where T : Config
		{
			return (T)LoadTextAsset(typeof(T), path);
		}

		public static object LoadTextAsset(Type type, string path)
		{
			return GetSerializer(type).LoadTextAsset(type, path);
		}

		private static string GetPath(Type type)
		{
			string text = null;
			Attribute[] customAttributes = Attribute.GetCustomAttributes(type);
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is ConfigPathAttribute)
				{
					text = (attribute as ConfigPathAttribute).Path;
				}
			}
			if (text == null)
			{
				Debug.LogWarning("Config: cann't find ConfigPathAttribute or bad path");
			}
			return text;
		}

		private static SerializeFormatterAttribute GetSerializer(Type type)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(type);
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is SerializeFormatterAttribute)
				{
					return attribute as SerializeFormatterAttribute;
				}
			}
			Debug.LogWarning("Config: cann't find SerializeFormatterAttribute or bad path");
			return null;
		}
	}
}
