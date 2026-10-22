using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public class NetworkService
	{
		public const int DOWNLOAD_WINDOW = 4096;

		public static void ProceedSimpleRequest(string apiUrl, Action<string> OnSuccess, Action<string> OnFail)
		{
			ThreadPool.QueueUserWorkItem((object obj) =>
			{
				SendPostRequestString(apiUrl, OnSuccess, OnFail);
			});
		}

		public static void DownloadFileWithContinue(string urlToFile, string folderToSave, string fileName, Action<string> OnSuccess, Action<string> OnFail, Action<long, long> OnProgressChanged)
		{
			ThreadPool.QueueUserWorkItem((object obj) =>
			{
				DownloadFile(urlToFile, folderToSave, fileName, OnSuccess, OnFail, OnProgressChanged);
			});
		}

		private static void SendPostRequestString(string apiUrl, Action<string> OnSucceess, Action<string> OnFail)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(apiUrl);
				webRequest.Method = "POST";
				Stream stream = null;
				WebResponse response = webRequest.GetResponse();
				stream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(stream);
				string obj = streamReader.ReadToEnd();
				if (OnSucceess != null)
				{
					OnSucceess(obj);
				}
				streamReader.Close();
				stream.Close();
				response.Close();
			}
			catch (Exception ex)
			{
				if (OnFail != null)
				{
					OnFail(ex.ToString());
				}
			}
		}

		private static void DownloadFile(string urlToFile, string folderToSave, string fileName, Action<string> OnSuccess, Action<string> OnFail, Action<long, long> OnProgressChanged)
		{
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.Create(urlToFile) as HttpWebRequest;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				long contentLength = httpWebResponse.ContentLength;
				long num = 0L;
				int num2 = 0;
				FileStream fileStream = null;
				if (IOSystem.IsFileExist(folderToSave, fileName))
				{
					fileStream = new FileStream(folderToSave + "/" + fileName, FileMode.Append);
				}
				else
				{
					IOSystem.CreateDirectory(folderToSave);
					fileStream = new FileStream(folderToSave + "/" + fileName, FileMode.Create);
				}
				if (fileStream.Length != 0)
				{
					num2 = Convert.ToInt32(fileStream.Length);
				}
				num += num2;
				if (fileStream.Length >= contentLength)
				{
					Debug.Log("File " + urlToFile + " alreadyDownloaded and Saved to " + folderToSave + "/" + fileName);
					if (OnSuccess != null)
					{
						OnSuccess(folderToSave + "/" + fileName);
					}
					return;
				}
				HttpWebRequest httpWebRequest2 = WebRequest.Create(urlToFile) as HttpWebRequest;
				httpWebRequest2.AddRange(num2);
				Stream stream = null;
				HttpWebResponse httpWebResponse2 = httpWebRequest2.GetResponse() as HttpWebResponse;
				stream = httpWebResponse2.GetResponseStream();
				long contentLength2 = httpWebResponse2.ContentLength;
				Debug.Log(contentLength2 + " " + num2);
				byte[] buffer = new byte[4096];
				int num3 = stream.Read(buffer, 0, 4096);
				while (num3 > 0)
				{
					fileStream.Write(buffer, 0, num3);
					num += num3;
					num3 = stream.Read(buffer, 0, 4096);
					if (OnProgressChanged != null)
					{
						OnProgressChanged(num, contentLength);
					}
				}
				fileStream.Close();
				stream.Close();
				httpWebResponse2.Close();
				Debug.Log("File " + urlToFile + " alreadyDownloaded and Saved to " + folderToSave + "/" + fileName);
				if (OnSuccess != null)
				{
					OnSuccess(folderToSave + "/" + fileName);
				}
			}
			catch
			{
			}
		}

		public static string GetUniqueID()
		{
			string key = "ID";
			System.Random random = new System.Random();
			DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
			double totalSeconds = (DateTime.UtcNow - dateTime).TotalSeconds;
			string text = string.Concat(Application.systemLanguage, "-", string.Format("{0:X}", Convert.ToInt32(totalSeconds)), "-", string.Format("{0:X}", Convert.ToInt32(Time.time * 1000000f)), "-", string.Format("{0:X}", random.Next(1000000000)));
			Debug.Log("Generated Unique ID: " + text);
			if (PlayerPrefsSystem.HasKey(key))
			{
				text = PlayerPrefs.GetString(key);
			}
			else
			{
				PlayerPrefs.SetString(key, text);
				PlayerPrefs.Save();
			}
			return text;
		}
	}
}
