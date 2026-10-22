using System;
using AppsMinistry.Domain.Observer;
using Newtonsoft.Json;

namespace AppsMinistry.Domain
{
	public class DomainObject : Observable
	{
		private string _id;

		[JsonIgnore]
		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public DomainObject()
		{
			_id = Guid.NewGuid().ToString();
		}
	}
}
