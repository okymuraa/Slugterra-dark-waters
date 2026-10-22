using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Enemy;
using UnityEngine;

namespace Slugterra.UI.Menu
{
	public class ChoseBossMenuView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Type> OnChoseBossEvent__BackingField;

		private Rect _playRect;

		private Rect _backRect;

		private Rect _billyRect;

		private Rect _glassesRect;

		private Rect _lockRect;

		private Rect _lodeRect;

		private Rect _nachoRect;

		private Rect _shortyRect;

		private Rect _twistRect;

		private IDictionary<Type, GameObject> _bossObjects;

		private GameObject _currentBoss;

		private Vector3 _baseBossPosition;

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Type> OnChoseBossEvent
		{
			add
			{
				Action<Type> action = OnChoseBossEvent__BackingField;
				Action<Type> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChoseBossEvent__BackingField, (Action<Type>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Type> action = OnChoseBossEvent__BackingField;
				Action<Type> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnChoseBossEvent__BackingField, (Action<Type>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_playRect = ScreenUtil.NormalizeRect(new Rect(0.8f, 0.9f, 0.15f, 0.07f));
			_backRect = ScreenUtil.NormalizeRect(new Rect(0.05f, 0.9f, 0.15f, 0.07f));
			_billyRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.22f, 0.15f, 0.07f));
			_glassesRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.3f, 0.15f, 0.07f));
			_lockRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.38f, 0.15f, 0.07f));
			_lodeRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.46f, 0.15f, 0.07f));
			_nachoRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.54f, 0.15f, 0.07f));
			_shortyRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.62f, 0.15f, 0.07f));
			_twistRect = ScreenUtil.NormalizeRect(new Rect(0.1f, 0.7f, 0.15f, 0.07f));
			_bossObjects = new Dictionary<Type, GameObject>();
			GameObject original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossBillyPrefab") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossBillyController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossGlassesPrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossGlassesController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossLockPrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossLockController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossLodePrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossLodeController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossNachoPrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossNachoController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossShortyPrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossShortyController), gameObject);
			original = Resources.Load("Prefabs/Game/Units/Humanoids/Enemy/Boss/BossTwistPrefab") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, Vector3.one * 1000f, Quaternion.identity);
			gameObject.GetComponentInChildren<EnemyController>().enabled = false;
			gameObject.GetComponentInChildren<UnitPathFinder>().enabled = false;
			gameObject.transform.localScale = Vector3.one * 0.5f;
			gameObject.SetActive(false);
			_bossObjects.Add(typeof(BossTwistController), gameObject);
			_baseBossPosition = new Vector3(0.3f, 0.45f, -1f);
		}

		private void OnGUI()
		{
			if (GUI.Button(_playRect, "Play"))
			{
				OnPlay();
			}
			else if (GUI.Button(_backRect, "Back"))
			{
				OnBack();
			}
			else if (GUI.Button(_billyRect, "Billy"))
			{
				OnChoseBoss(typeof(BossBillyController));
			}
			else if (GUI.Button(_glassesRect, "Glasses"))
			{
				OnChoseBoss(typeof(BossGlassesController));
			}
			else if (GUI.Button(_lockRect, "Lock"))
			{
				OnChoseBoss(typeof(BossLockController));
			}
			else if (GUI.Button(_lodeRect, "Lode"))
			{
				OnChoseBoss(typeof(BossLodeController));
			}
			else if (GUI.Button(_nachoRect, "Nacho"))
			{
				OnChoseBoss(typeof(BossNachoController));
			}
			else if (GUI.Button(_shortyRect, "Shorty"))
			{
				OnChoseBoss(typeof(BossShortyController));
			}
			else if (GUI.Button(_twistRect, "Twist"))
			{
				OnChoseBoss(typeof(BossTwistController));
			}
		}

		protected override void OnApplyModel(object model)
		{
		}

		private void OnPlay()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBack()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnChoseBoss(Type bossController)
		{
			if (OnChoseBossEvent__BackingField != null)
			{
				OnChoseBossEvent__BackingField(bossController);
			}
		}

		private void OnBossChanged(Type boss)
		{
			if (_currentBoss != null)
			{
				_currentBoss.transform.position = Vector3.one * 1000f;
				_currentBoss.SetActive(false);
			}
			_currentBoss = _bossObjects[boss];
			_currentBoss.transform.position = _baseBossPosition;
			_currentBoss.transform.eulerAngles = Vector3.up * 180f;
			_currentBoss.SetActive(true);
		}
	}
}
