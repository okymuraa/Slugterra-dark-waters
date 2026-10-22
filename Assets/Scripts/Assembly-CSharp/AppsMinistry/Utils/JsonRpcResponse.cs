using System;
using System.Collections.Generic;

namespace AppsMinistry.Utils
{
	[Serializable]
	public class JsonRpcResponse<T>
	{
		public string jsonrpc = "2.0";

		public string id = "1";

		public List<T> result;
	}
}
