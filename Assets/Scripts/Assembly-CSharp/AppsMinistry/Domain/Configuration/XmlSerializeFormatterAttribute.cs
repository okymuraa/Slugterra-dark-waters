using System;
using System.IO;
using System.Xml.Serialization;
using AppsMinistry.Core;
using UnityEngine;

namespace AppsMinistry.Domain.Configuration
{
	public sealed class XmlSerializeFormatterAttribute : SerializeFormatterAttribute
	{
		public override void Save(object ob, string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(ob.GetType());
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				xmlSerializer.Serialize(fileStream, ob);
				fileStream.Close();
			}
		}

		public override object LoadFile(Type type, string path)
		{
			object result = null;
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			try
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open))
				{
					result = xmlSerializer.Deserialize(fileStream);
					fileStream.Close();
				}
			}
			catch (Exception ex)
			{
				MonoLog.LogWarning(MonoLog.Channel.Core, string.Format("XmlSerializeFormatter: cannot load file {0}. (Log: {1})", path, ex.Message));
			}
			return result;
		}

		public override object LoadTextAsset(Type type, string path)
		{
			object result = null;
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			TextAsset textAsset = Resources.Load(path) as TextAsset;
			try
			{
				using (TextReader textReader = new StringReader(textAsset.text))
				{
					result = xmlSerializer.Deserialize(textReader);
					textReader.Close();
				}
			}
			catch (Exception ex)
			{
				MonoLog.LogWarning(MonoLog.Channel.Core, string.Format("XmlSerializeFormatter: cannot load TextAsset {0}. (Log: {1})", path, ex.Message));
			}
			return result;
		}
	}
}
