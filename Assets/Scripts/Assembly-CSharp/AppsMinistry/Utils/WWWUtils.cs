using System;
using System.Collections;
using AppsMinistry.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace AppsMinistry.Utils
{
	[AppSingleton]
	public class WWWUtils : MonoSingleton<WWWUtils>
	{
		public class CountryCodeData
		{
			public string ip;

			public string country;

			public string coutry_name;
		}

		private string _countryCode = string.Empty;

		private bool isGetCountryCodeWork;

		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public string CountryCode
		{
			get
			{
				return _countryCode;
			}
			private set
			{
				_countryCode = value;
			}
		}

		public void InitializeCountryCode(Action callback = null)
		{
			if (!isGetCountryCodeWork)
			{
				StartCoroutine(GetCountryCode(callback));
			}
		}

		private IEnumerator GetCountryCode(Action callback)
		{
			isGetCountryCodeWork = true;
			WWW www = new WWW("http://appapi.neoline.biz/platform/helpers/ip.json");
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				CountryCodeData countryCodeData = JsonConvert.DeserializeObject<CountryCodeData>(www.text);
				CountryCode = countryCodeData.country.ToLower();
				if (callback != null)
				{
					callback();
				}
			}
			else
			{
				Debug.LogWarning("WWWUtils::GetCountryCode Error " + www.error);
			}
			isGetCountryCodeWork = false;
		}

		public static long ConvertToTimestamp(DateTime value)
		{
			return (long)(value - Epoch).TotalSeconds;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			return Epoch.AddSeconds(unixTimeStamp).ToUniversalTime();
		}
	}
}
