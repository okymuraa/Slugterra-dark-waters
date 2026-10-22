using System;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.Domain.App.Settings
{
	public class DuelSetting : DomainObject, IObserver
	{
		private FoeItem _foe;

		private float _rank;

		public FoeItem Foe
		{
			get
			{
				if (_foe == null)
				{
					SetDefaultFoe();
				}
				return _foe;
			}
			set
			{
				_foe = value;
				CommitAndNotify();
			}
		}

		public float Rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
				if (_rank < 0f)
				{
					_rank = 0f;
				}
				CommitAndNotify();
			}
		}

		public DuelSetting()
		{
			SetDefaultFoe();
			SetDefaultRank();
		}

		public void SetDefaultFoe()
		{
			Foe = new FoeItem(EnemyType.None.ToString(), 1, true, true);
		}

		public void SetDefaultRank()
		{
			Rank = 100f;
		}

		public BaseUnitConfig GetBossConfig()
		{
			EnemyType type = (EnemyType)Enum.Parse(typeof(EnemyType), _foe.Type);
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetFoeConfigurationLevel(type, 0);
		}

		public void OnObjectChanged(object observable)
		{
		}

		public static void Serialize(DuelSetting duelSettings)
		{
			string stringToSave = JsonConvert.SerializeObject(duelSettings);
			IOSystem.SaveStrings(Application.persistentDataPath, "Duel.cfg", stringToSave, false, true);
		}

		public static DuelSetting Deserialize()
		{
			DuelSetting duelSetting = null;
			if (IOSystem.IsFileExist(Application.persistentDataPath, "Duel.cfg"))
			{
				string value = IOSystem.ReadStrings(Application.persistentDataPath, "Duel.cfg");
				return JsonConvert.DeserializeObject<DuelSetting>(value);
			}
			return new DuelSetting();
		}
	}
}
