using UnityEngine;

namespace Oculus.Interaction
{
    /// <summary>
    /// Manages the interaction with towels and updates the shared towel UI icon.
    /// Resets the icon's remaining active time to the configured duration if already active.
    /// </summary>
    public class Towel1 : MonoBehaviour
    {
        [Tooltip("The interactable to monitor for ray interactions.")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView;
        private bool _started = false;

        [Tooltip("The shared towel UI icon.")]
        public GameObject sharedTowelIconUI; // Shared UI icon for all towels

        [Tooltip("Duration for which the icon will remain visible after interaction.")]
        public float iconDisplayDuration = 20f; // Default is 20 seconds

        private Coroutine _iconTimerCoroutine;
        private float _remainingTime; // Tracks the remaining active time of the icon

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
            this.AssertField(sharedTowelIconUI, nameof(sharedTowelIconUI));
            _remainingTime = 0; // Initialize remaining time
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += HandleStateChange;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= HandleStateChange;
            }
        }

        private void HandleStateChange(InteractableStateChangeArgs args)
        {
            if (args.NewState == InteractableState.Select)
            {
                HandleTowelPickup();
            }
        }

        private void HandleTowelPickup()
        {
            Destroy(gameObject); // Remove the towel object from the scene
            ResetOrStartTowelIconTimer();
        }

        private void ResetOrStartTowelIconTimer()
        {
            if (sharedTowelIconUI == null)
                return;

            if (_iconTimerCoroutine != null)
            {
                StopCoroutine(_iconTimerCoroutine); // 기존 코루틴 종료
            }

            // 타이머 초기화 및 코루틴 시작
            _remainingTime = iconDisplayDuration;
            sharedTowelIconUI.SetActive(true); // UI 활성화
            _iconTimerCoroutine = StartCoroutine(UpdateIconTimer());
        }


        private System.Collections.IEnumerator UpdateIconTimer()
        {
            while (_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime; // 시간 감소
                yield return null; // 다음 프레임 대기
            }

            _remainingTime = 0; // 종료 시 시간 보정
            sharedTowelIconUI.SetActive(false); // UI 비활성화
            _iconTimerCoroutine = null; // 코루틴 참조 초기화
        }

        #region Inject

        public void InjectAllTowelInteractions(IInteractableView interactableView, GameObject uiIcon, float duration)
        {
            InjectInteractableView(interactableView);
            InjectTowelIcon(uiIcon);
            iconDisplayDuration = duration;
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as UnityEngine.Object;
            InteractableView = interactableView;
        }

        public void InjectTowelIcon(GameObject uiIcon)
        {
            sharedTowelIconUI = uiIcon;
        }

        #endregion
    }
}
