using System;
using AppsMinistry.Domain;
using Newtonsoft.Json;

namespace Slugterra.Domain.Game.HealthRestoration
{
	[Serializable]
	public abstract class AbstractHealthData : DomainObject
	{
		private DateTime _timeWhenObjectIsRestored = default(DateTime);

		public DateTime TimeWhenObjectIsRestored
		{
			get
			{
				return _timeWhenObjectIsRestored;
			}
			set
			{
				_timeWhenObjectIsRestored = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public bool IsFullyRestored
		{
			get
			{
				return true;
			}
		}

		[JsonIgnore]
		public TimeSpan TimeToRestore
		{
			get
			{
				return TimeSpan.Zero;
			}
		}

		public void Heal()
		{
			TimeWhenObjectIsRestored = default(DateTime);
		}
	}
}
