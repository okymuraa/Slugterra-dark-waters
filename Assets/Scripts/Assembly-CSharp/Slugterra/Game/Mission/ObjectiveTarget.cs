using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Sound;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.Game.Mission;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ObjectiveTarget : MonoBehaviour, IObserver
	{
		private const float WAIT_TIME = 1f;

		private const float UPDATE_TIME = 0.1f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnObjectiveCompleteEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnObjectiveSetActiveEvent__BackingField;

		private GameController gameController;

		[SerializeField]
		private bool _isAutoComplete;

		[SerializeField]
		private bool _isActive;

		[SerializeField]
		private ObjectiveType _type = ObjectiveType.Auxiliary;

		[SerializeField]
		private Collider _trigger;

		[SerializeField]
		private GameObject _radarMarker;

		[SerializeField]
		private List<ObjectiveTarget> _prevObjectives = new List<ObjectiveTarget>();

		[SerializeField]
		private SoundContainer _objectiveSound;

		[SerializeField]
		private bool _playSound = true;

		private Objective _model;

		[SerializeField]
		private bool _showMarker = true;

		[SerializeField]
		private bool _showTargetMarker = true;

		private GameObject _targetMarker;

		[SerializeField]
		private string _tagForTrigger = "Player";

		private Transform _transform;

		private Camera _mainCamera;

		private float _nextUpdate;

		[SerializeField]
		private bool _showCompleteMessage;

		[SerializeField]
		protected string _descriptionKey;

		protected ObjectiveCompleteMessageView _objectiveCompleteMessage;

		protected ObjectiveCheckboxController _objectiveCheckbox;

		[SerializeField]
		private DoAfter _doAfterComplite;

		[SerializeField]
		private GameObject _someObject;

		private LevelItem _currentLevelItem;

		public GameObject RadarMarker
		{
			get
			{
				return _radarMarker;
			}
		}

		public SoundContainer ObjectiveSound
		{
			get
			{
				if (_playSound)
				{
					return _objectiveSound;
				}
				return null;
			}
		}

		public bool Active
		{
			get
			{
				return _model.IsActive;
			}
			set
			{
				if (!_model.IsActive && value)
				{
					OnActivate();
				}
				if (OnObjectiveSetActiveEvent__BackingField != null)
				{
					OnObjectiveSetActiveEvent__BackingField();
				}
				_isActive = value;
				_model.IsActive = value;
			}
		}

		public bool Completed
		{
			get
			{
				return _model.IsComplete;
			}
			set
			{
				ObjectiveMessageState state = ObjectiveMessageState.Fail;
				if (gameController == null)
				{
					gameController = UnityEngine.Object.FindObjectOfType<GameController>();
				}
				if (!_model.IsComplete && value)
				{
					if (OnObjectiveCompleteEvent__BackingField != null)
					{
						OnObjectiveCompleteEvent__BackingField();
					}
					Active = false;
					LevelItem.SetNewObjectiveStatus((int)_type, 1);
					if (_someObject != null)
					{
						if (_doAfterComplite == DoAfter.Disable)
						{
							_someObject.SetActive(false);
						}
						else if (_doAfterComplite == DoAfter.Enable)
						{
							_someObject.SetActive(true);
						}
					}
					state = ObjectiveMessageState.Complete;
					if (_type == ObjectiveType.Main && gameController.Model != null)
					{
						gameController.Model.Eli.Invulnerability = true;
					}
				}
				else if (!value)
				{
					Active = false;
					LevelItem.SetNewObjectiveStatus((int)_type, 2);
					state = ObjectiveMessageState.Fail;
				}
				if (_showCompleteMessage)
				{
					string objectiveDescription = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(_currentLevelItem.Scene.Name, (int)_type);
					_objectiveCompleteMessage.Description = MonoSingleton<Localization>.Instance.GetValueByKey(objectiveDescription);
					_objectiveCompleteMessage.SetState(state);
					_objectiveCompleteMessage.Show(true);
					StartCoroutine(HideMessage(5f));
				}
				_model.IsComplete = value;
			}
		}

		public Objective Model
		{
			get
			{
				return _model;
			}
		}

		public bool ShowMarker
		{
			get
			{
				return _showMarker;
			}
		}

		public event Action OnObjectiveCompleteEvent
		{
			add
			{
				Action action = OnObjectiveCompleteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectiveCompleteEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnObjectiveCompleteEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectiveCompleteEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnObjectiveSetActiveEvent
		{
			add
			{
				Action action = OnObjectiveSetActiveEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectiveSetActiveEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnObjectiveSetActiveEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectiveSetActiveEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private IEnumerator HideMessage(float time)
		{
			yield return new WaitForSeconds(time);
			if (gameController.IsPlayCutScene)
			{
				while (gameController.IsPlayCutScene)
				{
					yield return new WaitForSeconds(Time.fixedDeltaTime);
				}
				yield return new WaitForSeconds(2.5f);
			}
			_objectiveCompleteMessage.Show(false);
		}

		protected virtual void OnActivate()
		{
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			_transform = transform;
			SlugterraSceneController slugterraSceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			if (slugterraSceneController != null)
			{
				_currentLevelItem = model.GetLevelItem(slugterraSceneController.GetType());
			}
			else
			{
				_currentLevelItem = model.GetLevelItem(model.MissionSetting.Mission);
			}
			_model = new Objective(_type, _isActive);
			_model.AddObserver(this);
			if (_isAutoComplete && _trigger == null)
			{
				Collider component = gameObject.GetComponent<Collider>();
				if (component == null)
				{
					UnityEngine.Debug.LogWarning(string.Format("{0}: {1}", GetType(), " Couldn't found trigger for auto complete target"));
				}
				else if (!component.isTrigger)
				{
					UnityEngine.Debug.LogWarning(string.Format("{0}: {1}", GetType(), " Auto complete target is not trigger"));
				}
			}
			CreateObjectiveMarker();
			if (_radarMarker == null)
			{
				_radarMarker = Resources.Load("Prefabs/UI/Games/RadarItem") as GameObject;
			}
			OnCreate();
			_nextUpdate = Time.time + 1f;
			InGameView inGameView = UnityEngine.Object.FindObjectOfType<InGameView>();
			if (inGameView != null)
			{
				_objectiveCompleteMessage = inGameView.ObjectiveCompleteMessage;
			}
			ObjectivesView objectivesView = UnityEngine.Object.FindObjectOfType<ObjectivesView>();
			if (objectivesView != null)
			{
				switch (_type)
				{
				case ObjectiveType.Main:
					_objectiveCheckbox = objectivesView.MainObjective;
					break;
				case ObjectiveType.BonusFirst:
					_objectiveCheckbox = objectivesView.BonusObjectiveFirst;
					break;
				case ObjectiveType.BonusSecond:
					_objectiveCheckbox = objectivesView.BonusObjectiveSecond;
					break;
				}
			}
		}

		private void CreateObjectiveMarker()
		{
			GameObject original = Resources.Load("Prefabs/UI/ObjectiveTargetMarker") as GameObject;
			_targetMarker = UnityEngine.Object.Instantiate(original, _transform.position, Quaternion.identity);
			_targetMarker.transform.parent = _transform;
			_targetMarker.transform.localPosition = Vector3.zero + Vector3.up * 0.01f;
			EnableTargetMarker();
		}

		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			foreach (ObjectiveTarget prevObjective in _prevObjectives)
			{
				if (prevObjective != null)
				{
					prevObjective.Model.AddObserver(this);
				}
			}
			_mainCamera = Camera.main;
			if (_objectiveCheckbox != null)
			{
				string objectiveDescription = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetObjectiveDescription(_currentLevelItem.Scene.Name, (int)_type);
				_objectiveCheckbox.View.SetMessage(objectiveDescription);
				_objectiveCheckbox.State = ObjectiveCheckboxState.Unset;
			}
			OnStart();
		}

		public virtual void OnObjectChanged(object observable)
		{
			if (!(observable is Objective))
			{
				return;
			}
			if ((observable as Objective).Equals(_model))
			{
				EnableTargetMarker();
				return;
			}
			foreach (ObjectiveTarget prevObjective in _prevObjectives)
			{
				if (prevObjective != null && !prevObjective.Model.IsComplete)
				{
					return;
				}
			}
			if (!Active)
			{
				Active = true;
			}
		}

		private void EnableTargetMarker()
		{
			if (!_showTargetMarker || !_model.IsActive)
			{
				_targetMarker.SetActive(false);
			}
			else if (_model.IsVisible)
			{
				_targetMarker.SetActive(true);
			}
			else
			{
				_targetMarker.SetActive(false);
			}
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			if (_mainCamera == null)
			{
				_mainCamera = Camera.main;
			}
			if (_model == null || _model.IsComplete)
			{
				return;
			}
			if (_mainCamera != null)
			{
				Vector3 vector = _mainCamera.WorldToViewportPoint(_transform.position);
				if (vector.x > 1f || vector.x < 0f || vector.y > 1f || vector.y < 0f)
				{
					OnVisible(false);
				}
				else
				{
					OnVisible(true);
				}
			}
			if (_nextUpdate <= Time.time)
			{
				OnUpdate();
				_nextUpdate = Time.time + 0.1f;
			}
		}

		protected virtual void OnTriggerEnter(Collider item)
		{
			if (item.gameObject.tag == _tagForTrigger && !Model.IsComplete && Active)
			{
				Completed = true;
			}
		}

		protected virtual void OnTriggerExit(Collider item)
		{
		}

		protected virtual void OnDestroy()
		{
			_model.RemoveObserver(this);
			StopAllCoroutines();
			foreach (ObjectiveTarget prevObjective in _prevObjectives)
			{
				if (prevObjective != null)
				{
					prevObjective.Model.RemoveObserver(this);
				}
			}
		}

		private void OnVisible(bool visible)
		{
			if ((_showTargetMarker || !visible) && Model.IsVisible != visible)
			{
				Model.IsVisible = visible;
			}
		}
	}
}
