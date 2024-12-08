using System.Collections;
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

        [Tooltip("Audio clip for towel effect.")]
        public AudioClip towelEffectClip; // 타월 효과음 클립

        private AudioSource towelAudioSource; // 타월 전용 AudioSource

        private Coroutine _iconTimerCoroutine;
        private float _remainingTime; // Tracks the remaining active time of the icon
        public PlayerMovement playerMovement;
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

            // 타월 전용 AudioSource 생성 및 초기화
            towelAudioSource = gameObject.AddComponent<AudioSource>();
            towelAudioSource.playOnAwake = false;

            playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();  // "Player"는 Player 오브젝트의 이름

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
            // 타월을 즉시 안보이게 만듦
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // 타이머 시작
            ResetOrStartTowelIconTimer();

            // 효과음 재생
            PlayTowelSound();

            if (playerMovement != null)
            {
                Debug.Log("towel job");
                playerMovement.ActivateTowelEffect();
            }

            // 타이머 종료 후 Destroy 실행
            StartCoroutine(DestroyAfterTimer());
        }

        private IEnumerator DestroyAfterTimer()
        {
            // 타이머가 종료될 때까지 대기
            yield return new WaitForSeconds(iconDisplayDuration);
            playerMovement.DeactivateTowelEffect();
            // 타월 오브젝트 완전 제거
            Destroy(gameObject);
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

        private IEnumerator UpdateIconTimer()
        {
            Debug.Log($"Icon Timer Started. Duration: {_remainingTime}");
            while (_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime; // 시간 감소
                Debug.Log($"Duration: {_remainingTime}");

                if (_remainingTime <= 5f) // 남은 시간이 5초 이하일 경우
                {
                    // UI가 서서히 흐려졌다가 진해지는 효과 적용
                    float alpha = Mathf.PingPong(Time.time, 1f); // 0에서 1 사이의 값 반복
                    SetIconAlpha(alpha);
                }

                yield return null; // 다음 프레임 대기
            }
            Debug.Log($"Icon Timer Ended.");

            _remainingTime = 0; // 종료 시 시간 보정
            sharedTowelIconUI.SetActive(false); // UI 비활성화
            _iconTimerCoroutine = null; // 코루틴 참조 초기화
        }

        private void SetIconAlpha(float alpha)
        {
            // Icon의 투명도 업데이트
            CanvasGroup canvasGroup = sharedTowelIconUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }
        }

        private void PlayTowelSound()
        {
            if (towelEffectClip != null)
            {
                towelAudioSource.clip = towelEffectClip;
                towelAudioSource.Play();
            }
        }

        #region Inject

        public void InjectAllTowelInteractions(IInteractableView interactableView, GameObject uiIcon, float duration, AudioClip effectClip)
        {
            InjectInteractableView(interactableView);
            InjectTowelIcon(uiIcon);
            iconDisplayDuration = duration;
            towelEffectClip = effectClip;
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
