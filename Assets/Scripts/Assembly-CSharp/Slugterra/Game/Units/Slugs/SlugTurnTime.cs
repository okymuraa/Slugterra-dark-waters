using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class SlugTurnTime : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		private List<float> _turnTime;

		public List<float> TurnTime
		{
			get
			{
				if (_turnTime == null || _turnTime.Count < 5)
				{
					Init(ref _turnTime);
				}
				return _turnTime;
			}
		}

		protected void Init<T>(ref List<T> _list)
		{
			_list = new List<T>();
			for (int i = 0; i < 5; i++)
			{
				_list.Add(default(T));
			}
		}
	}
}
