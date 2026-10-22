using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using MiniJSON;

namespace AppsMinistry.Utils
{
	public class JSonRPCService
	{
		public static string GetJsonRpc(string methodName, string data, string id)
		{
			return "{\"jsonrpc\": \"2.0\", \"method\":\"" + methodName + "\", \"params\":" + data + ", \"id\":\"" + id + "\"}";
		}

		public static void ProceedRpcRequest(string apiUrl, string jsonRpcData, string webKey, Action<object> OnSucceess, Action<object> OnError, Action<object> OnFail, bool useThread = true)
		{
			string input = jsonRpcData + webKey;
			MD5 md5Hash = MD5.Create();
			string cipher = GetMd5Hash(md5Hash, input);
			if (useThread)
			{
				ThreadPool.QueueUserWorkItem((object obj) =>
				{
					SendRpcRequest(apiUrl, jsonRpcData, cipher, OnSucceess, OnError, OnFail);
				});
			}
			else
			{
				SendRpcRequest(apiUrl, jsonRpcData, cipher, OnSucceess, OnError, OnFail);
			}
		}

		private static void SendRpcRequest(string apiUrl, string jsonRpcData, string cipher, Action<object> OnSucceess, Action<object> OnError, Action<object> OnFail)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(apiUrl);
				webRequest.Method = "POST";
				byte[] bytes = Encoding.UTF8.GetBytes(jsonRpcData);
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.ContentLength = bytes.Length;
				webRequest.Headers.Add("X-Sign", cipher);
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				WebResponse response = webRequest.GetResponse();
				requestStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(requestStream);
				string text = streamReader.ReadToEnd();
				try
				{
					Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
					if (dictionary.ContainsKey("error"))
					{
						if (OnError != null)
						{
							OnError(text);
						}
					}
					else if (OnSucceess != null)
					{
						OnSucceess(text);
					}
				}
				catch (Exception ex)
				{
					if (OnFail != null)
					{
						OnFail(ex.ToString());
					}
				}
				streamReader.Close();
				requestStream.Close();
				response.Close();
			}
			catch (Exception ex2)
			{
				if (OnFail != null)
				{
					OnFail(ex2.ToString());
				}
			}
		}

		private static string GetMd5Hash(MD5 md5Hash, string input)
		{
			byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
	}
}
