using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class UnitConfiguration
	{
		public const int MAX_ASSISTANT_LEVEL = 5;

		public const int MAX_ENEMY_LEVEL = 10;

		private Dictionary<AnimalType, string> _animalResourcesPath = new Dictionary<AnimalType, string>
		{
			{
				AnimalType.DinoRat,
				"Configs/Units/Enemy/Animals/DinoRatConfigs/DinoRatConfig"
			},
			{
				AnimalType.SlugHound,
				"Configs/Units/Enemy/Animals/SlugHoundConfigs/SlugHoundConfig"
			},
			{
				AnimalType.SlugScorpion,
				"Configs/Units/Enemy/Animals/SlugScorpionConfigs/SlugScorpionConfig"
			}
		};

		private Dictionary<AllyType, string> _alliesResourcesPath = new Dictionary<AllyType, string>
		{
			{
				AllyType.Eli,
				"Configs/Units/Ally/EliConfigs/EliConfig"
			},
			{
				AllyType.Follower,
				"Configs/Units/Ally/FollowerConfigs/FollowerConfig"
			},
			{
				AllyType.Kord,
				"Configs/Units/Ally/KordConfigs/KordConfig"
			},
			{
				AllyType.Pronto,
				"Configs/Units/Ally/ProntoConfigs/ProntoConfig"
			},
			{
				AllyType.Trixie,
				"Configs/Units/Ally/TrixieConfigs/TrixieConfig"
			}
		};

		private Dictionary<EnemyType, string> _enemyResourcesPath = new Dictionary<EnemyType, string>
		{
			{
				EnemyType.Billy,
				"Configs/Units/Enemy/Humanoids/BillyConfigs/BillyConfig"
			},
			{
				EnemyType.DeadWeed,
				"Configs/Units/Enemy/Humanoids/DeadWeedConfigs/DeadWeedConfig"
			},
			{
				EnemyType.Glasses,
				"Configs/Units/Enemy/Humanoids/GlassesConfigs/GlassesConfig"
			},
			{
				EnemyType.Lock,
				"Configs/Units/Enemy/Humanoids/LockConfigs/LockConfig"
			},
			{
				EnemyType.Lode,
				"Configs/Units/Enemy/Humanoids/LodeConfigs/LodeConfig"
			},
			{
				EnemyType.Nacho,
				"Configs/Units/Enemy/Humanoids/NachoConfigs/NachoConfig"
			},
			{
				EnemyType.ScrapHeap,
				"Configs/Units/Enemy/Humanoids/ScrapHeapConfigs/ScrapHeapConfig"
			},
			{
				EnemyType.Sentinel,
				"Configs/Units/Enemy/Humanoids/SentinelConfigs/SentinelConfig"
			},
			{
				EnemyType.Shorty,
				"Configs/Units/Enemy/Humanoids/ShortyConfigs/ShortyConfig"
			},
			{
				EnemyType.Twist,
				"Configs/Units/Enemy/Humanoids/TwistConfigs/TwistConfig"
			}
		};

		private Dictionary<EnemyType, string> _bossResourcesPath = new Dictionary<EnemyType, string>
		{
			{
				EnemyType.BossBilly,
				"Configs/Units/Enemy/Boss/BossBillyConfigs/BossBillyConfig"
			},
			{
				EnemyType.BossGlasses,
				"Configs/Units/Enemy/Boss/BossGlassesConfigs/BossGlassesConfig"
			},
			{
				EnemyType.BossKord,
				"Configs/Units/Enemy/Boss/BossKordConfigs/BossKordConfig"
			},
			{
				EnemyType.BossLock,
				"Configs/Units/Enemy/Boss/BossLockConfigs/BossLockConfig"
			},
			{
				EnemyType.BossLode,
				"Configs/Units/Enemy/Boss/BossLodeConfigs/BossLodeConfig"
			},
			{
				EnemyType.BossNacho,
				"Configs/Units/Enemy/Boss/BossNachoConfigs/BossNachoConfig"
			},
			{
				EnemyType.BossPronto,
				"Configs/Units/Enemy/Boss/BossProntoConfigs/BossProntoConfig"
			},
			{
				EnemyType.BossSentinelBig,
				"Configs/Units/Enemy/Boss/BossSentinelBigConfigs/BossSentinelBigConfig"
			},
			{
				EnemyType.BossShorty,
				"Configs/Units/Enemy/Boss/BossShortyConfigs/BossShortyConfig"
			},
			{
				EnemyType.BossTrixie,
				"Configs/Units/Enemy/Boss/BossTrixieConfigs/BossTrixieConfig"
			},
			{
				EnemyType.BossTwist,
				"Configs/Units/Enemy/Boss/BossTwistConfigs/BossTwistConfig"
			}
		};

		private UnitDictCollection _unitCollection;

		private Dictionary<string, AllyType> assistantTypeByName = new Dictionary<string, AllyType>();

		private Dictionary<string, EnemyType> enemyTypeByName = new Dictionary<string, EnemyType>();

		public void Initialize()
		{
			_unitCollection = new UnitDictCollection();
			foreach (AllyType key in _alliesResourcesPath.Keys)
			{
				HumanoidConfig value = LoadConfig<HumanoidConfig>(_alliesResourcesPath[key]);
				_unitCollection.alliesConfigs.Add(key, value);
			}
			for (AllyType allyType = AllyType.None; allyType < AllyType.Size; allyType++)
			{
				if (!assistantTypeByName.ContainsKey(allyType.ToString()))
				{
					assistantTypeByName.Add(allyType.ToString(), allyType);
				}
			}
			for (EnemyType enemyType = EnemyType.None; enemyType < EnemyType.Size; enemyType++)
			{
				if (!enemyTypeByName.ContainsKey(enemyType.ToString()))
				{
					enemyTypeByName.Add(enemyType.ToString(), enemyType);
				}
			}
			foreach (AnimalType key2 in _animalResourcesPath.Keys)
			{
				AnimalConfig value2 = LoadConfig<AnimalConfig>(_animalResourcesPath[key2]);
				_unitCollection.animalsConfigs.Add(key2, value2);
			}
			foreach (EnemyType key3 in _enemyResourcesPath.Keys)
			{
				HumanoidConfig value3 = LoadConfig<HumanoidConfig>(_enemyResourcesPath[key3]);
				_unitCollection.enemyConfigs.Add(key3, value3);
			}
			foreach (EnemyType key4 in _bossResourcesPath.Keys)
			{
				FoeConfig value4 = LoadConfig<FoeConfig>(_bossResourcesPath[key4]);
				_unitCollection.bossConfigs.Add(key4, value4);
			}
		}

		public List<HumanoidConfig> GetAllies()
		{
			return GetConfig(ref _unitCollection.alliesConfigs);
		}

		public List<HumanoidConfig> GetEnemies()
		{
			return GetConfig(ref _unitCollection.enemyConfigs);
		}

		public List<FoeConfig> GetBosses()
		{
			return GetConfig(ref _unitCollection.bossConfigs);
		}

		public List<AnimalConfig> GetAnimals()
		{
			return GetConfig(ref _unitCollection.animalsConfigs);
		}

		public HumanoidConfig GetAlliesConfig(AllyType type)
		{
			if (!_unitCollection.alliesConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetAlliesConfig AlliesConfiguration: {0} type doesn't exist", type.ToString());
				return new HumanoidConfig();
			}
			return _unitCollection.alliesConfigs[type];
		}

		public FoeConfig GetFoesConfig(EnemyType type)
		{
			if (!_unitCollection.bossConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetFoesConfig AlliesConfiguration: {0} type doesn't exist", type.ToString());
				return new FoeConfig();
			}
			return _unitCollection.bossConfigs[type];
		}

		public HumanoidLevels GetAlliesConfigurationLevel(AllyType type, int level)
		{
			if (!_unitCollection.alliesConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetAlliesConfigurationLevel AlliesConfiguration: {0} type doesn't exist", type.ToString());
				return new HumanoidLevels();
			}
			return _unitCollection.alliesConfigs[type].levels[level];
		}

		public List<HumanoidLevels> GetAssistantConfigurationsLevels(int maxLevel)
		{
			List<HumanoidLevels> list = new List<HumanoidLevels>();
			HumanoidConfig humanoidConfig = _unitCollection.alliesConfigs[AllyType.Pronto];
			HumanoidConfig humanoidConfig2 = _unitCollection.alliesConfigs[AllyType.Trixie];
			HumanoidConfig humanoidConfig3 = _unitCollection.alliesConfigs[AllyType.Kord];
			for (int i = 0; i <= maxLevel; i++)
			{
				list.Add(humanoidConfig.levels[i]);
				list.Add(humanoidConfig2.levels[i]);
				list.Add(humanoidConfig3.levels[i]);
			}
			return list;
		}

		public AllyType GetAssistantTypeByName(string allyName)
		{
			if (assistantTypeByName != null && assistantTypeByName.Count > 0 && !string.IsNullOrEmpty(allyName) && assistantTypeByName.ContainsKey(allyName))
			{
				return assistantTypeByName[allyName];
			}
			return AllyType.None;
		}

		public EnemyType GetEnemyTypeByName(string name)
		{
			if (enemyTypeByName != null && enemyTypeByName.Count > 0 && !string.IsNullOrEmpty(name) && enemyTypeByName.ContainsKey(name))
			{
				return enemyTypeByName[name];
			}
			return EnemyType.None;
		}

		public AnimalLevels GetAnimalsConfigurationLevel(AnimalType type, int level)
		{
			if (!_unitCollection.animalsConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetAnimalsConfigurationLevel AnimalsConfiguration: {0} type doesn't exist", type.ToString());
				return new AnimalLevels();
			}
			return _unitCollection.animalsConfigs[type].levels[level];
		}

		public HumanoidLevels GetEnemiesConfigurationLevel(EnemyType type, int level)
		{
			if (!_unitCollection.enemyConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetEnemiesConfigurationLevel EnemiesConfiguration: {0} type doesn't exist", type.ToString());
				return new HumanoidLevels();
			}
			return _unitCollection.enemyConfigs[type].levels[level];
		}

		public List<FoeLevels> GetFoesConfigurationsLevels(int maxLevel)
		{
			List<FoeLevels> list = new List<FoeLevels>();
			foreach (FoeConfig value in _unitCollection.bossConfigs.Values)
			{
				for (int i = 0; i <= maxLevel; i++)
				{
					list.Add(value.levels[i]);
				}
			}
			return list;
		}

		public FoeLevels GetFoeConfigurationLevel(EnemyType type, int level)
		{
			if (!_unitCollection.bossConfigs.ContainsKey(type))
			{
				Debug.LogWarningFormat("UnitConfiguration::GetFoeConfigurationLevel BossesConfiguration: {0} type doesn't exist", type.ToString());
				return new FoeLevels();
			}
			return _unitCollection.bossConfigs[type].levels[level];
		}

		private T LoadConfig<T>(string path) where T : BaseConfig
		{
			T val = (T)null;
			TextAsset textAsset = Resources.Load<TextAsset>(path);
			if (textAsset != null)
			{
				return JsonConvert.DeserializeObject<T>(textAsset.text);
			}
			return (T)null;
		}

		private List<V> GetConfig<K, V>(ref Dictionary<K, V> configDict) where V : BaseConfig
		{
			List<V> list = new List<V>();
			foreach (V value in configDict.Values)
			{
				list.Add(value);
			}
			return list;
		}
	}
}
