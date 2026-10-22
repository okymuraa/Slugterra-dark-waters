using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public class IOSystem
	{
		public static bool IsFileExist(string path, string fileName)
		{
			string fileName2 = FullFileName(path, fileName);
			string text = FileName(path, fileName);
			FileInfo fileInfo = new FileInfo(fileName2);
			return fileInfo.Exists;
		}

		public static void SaveBytes(string path, string fileName, ref byte[] data, bool replaceIfExist = false, bool isNeedCrypt = false)
		{
			string path2 = FullFileName(path, fileName);
			string text = FileName(path, fileName);
			byte[] array = (isNeedCrypt ? ReverseBytesArray(ref data, true) : data);
			try
			{
				if (replaceIfExist)
				{
					File.Delete(path2);
				}
				using (FileStream fileStream = File.Create(path2))
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::SaveToFile Exception " + ex.ToString());
			}
		}

		public static void SaveStrings(string path, string fileName, string stringToSave, bool replaceIfExist = false, bool isNeedCrypt = false)
		{
			string path2 = FullFileName(path, fileName);
			string text = FileName(path, fileName);
			byte[] originalArray = Encoding.ASCII.GetBytes(stringToSave);
			byte[] array = (isNeedCrypt ? ReverseBytesArray(ref originalArray, true) : originalArray);
			try
			{
				if (replaceIfExist)
				{
					File.Delete(path2);
				}
				using (FileStream fileStream = File.Create(path2))
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::SaveToFile Exception " + ex.ToString());
			}
		}

		private static string FullFileName(string path, string fileName)
		{
			return (!string.IsNullOrEmpty(fileName)) ? string.Format("{0}/{1}", path, fileName) : path;
		}

		private static string FileName(string path, string fileName)
		{
			return Path.GetFileName((!string.IsNullOrEmpty(fileName)) ? string.Format("{0}/{1}", path, fileName) : path);
		}

		private static string RelativePath(string path, string fileName)
		{
			Uri uri = new Uri(FullFileName(path, fileName));
			Uri uri2 = new Uri(Application.persistentDataPath + "/");
			return uri2.MakeRelativeUri(uri).ToString();
		}

		public static byte[] ReadBytes(string path, string fileName)
		{
			string text = FullFileName(path, fileName);
			string text2 = FileName(path, fileName);
			byte[] originalArray = null;
			try
			{
				if (originalArray == null && new FileInfo(text).Exists)
				{
					using (FileStream fileStream = File.Open(text, FileMode.Open))
					{
						originalArray = new byte[fileStream.Length];
						fileStream.Read(originalArray, 0, (int)fileStream.Length);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::ReadFile(string path, string fileName) Exception " + ex.ToString());
				return null;
			}
			if (originalArray != null && originalArray.Length > 0 && (IsEncrypted(originalArray[0]) || IsReverse(originalArray[0])))
			{
				return ReverseBytesArray(ref originalArray, IsEncrypted(originalArray[0]));
			}
			return originalArray;
		}

		public static string ReadStrings(string path, string fileName)
		{
			string text = FullFileName(path, fileName);
			string text2 = FileName(path, fileName);
			byte[] originalArray = null;
			try
			{
				if (originalArray == null && new FileInfo(text).Exists)
				{
					using (FileStream fileStream = File.Open(text, FileMode.Open))
					{
						originalArray = new byte[fileStream.Length];
						fileStream.Read(originalArray, 0, (int)fileStream.Length);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::ReadFile(string path, string fileName) Exception " + ex.ToString());
				return null;
			}
			byte[] bytes = originalArray;
			if (originalArray != null && originalArray.Length > 0 && (IsEncrypted(originalArray[0]) || IsReverse(originalArray[0])))
			{
				bytes = ReverseBytesArray(ref originalArray, IsEncrypted(originalArray[0]));
			}
			return Encoding.ASCII.GetString(bytes);
		}

		public static void RenameFile(string path, string fileName, string newFileName)
		{
			string sourceFileName = path + "/" + fileName;
			string destFileName = path + "/" + newFileName;
			try
			{
				File.Move(sourceFileName, destFileName);
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::RenameFile Exception " + ex.ToString());
			}
		}

		public static void RemoveFile(string path, string fileName)
		{
			string text = path + "/" + fileName;
			try
			{
				Debug.Log("FileName: " + text);
				File.Delete(text);
			}
			catch (Exception ex)
			{
				Debug.LogError("IOSystem::RemoveFile Exception " + ex.ToString());
			}
		}

		public static void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public static bool DirectoryExist(string path)
		{
			return Directory.Exists(path);
		}

		private static bool IsEncrypted(byte ch)
		{
			if (Convert.ToByte('}') == ch)
			{
				return false;
			}
			if (Convert.ToByte('n') == ch || Convert.ToByte('N') == ch)
			{
				return true;
			}
			return false;
		}

		private static bool IsReverse(byte ch)
		{
			switch (ch)
			{
			case 125:
				return true;
			case 78:
			case 110:
				return true;
			default:
				return false;
			}
		}

		private static byte[] ReverseBytesArray(ref byte[] originalArray, bool needXor)
		{
			List<byte> list = new List<byte>();
			byte b = Convert.ToByte(51);
			for (int i = 0; i < originalArray.Length; i++)
			{
				if (needXor)
				{
					list.Add((byte)(originalArray[i] ^ b));
				}
				else
				{
					list.Add(originalArray[i]);
				}
			}
			list.Reverse();
			return list.ToArray();
		}
	}
}
