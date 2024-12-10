using UnityEngine;

namespace Oculus.Interaction
{
    /// <summary>
    /// Manages the interaction with a medical kit, including activating effects, playing sounds, and updating health gauge.
    /// </summary>
    public class MedicalKit : MonoBehaviour
    {
        [Tooltip("The interactable to monitor for ray interactions.")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView;
        private bool _started = false;

        [Tooltip("The effect to trigger upon interaction.")]
        public GameObject healingEffect; // Prefab or GameObject for the healing effect

        [Tooltip("Duration for which the item will remain visible before being destroyed.")]
        public float destructionDelay = 5f; // Delay before destroying the object

        [Tooltip("The health gauge manager or script.")]
        public GameObject healthGauge; // Reference to the health gauge (replace this with your implementation)

        [Tooltip("The sound to play when the item is picked up.")]
        public AudioClip pickupSound; // The sound effect to play

        private AudioSource _audioSource;
        public Smokebar smokebar;
        public Healthbar healthbar;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;

            // Add or get the AudioSource component
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
            this.AssertField(healingEffect, nameof(healingEffect));
            this.AssertField(healthGauge, nameof(healthGauge));
            this.AssertField(pickupSound, nameof(pickupSound));
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
                HandleMedicalKitPickup();
            }
        }

        private void HandleMedicalKitPickup()
        {
            // Disable the renderer and collider to "hide" the object
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            healthbar.AidBox();
            smokebar.AidBox();

            // Trigger the healing effect
            if (healingEffect != null)
            {
                Instantiate(healingEffect, transform.position, Quaternion.identity);
            }

            // Play the pickup sound
            PlayPickupSound();

            // TODO: Increase health gauge here
            // Example:
            // healthGauge.GetComponent<HealthManager>().IncreaseHealth(amount);

            // Destroy the object after the delay
            StartCoroutine(DestroyAfterDelay());
        }

        private void PlayPickupSound()
        {
            if (_audioSource != null && pickupSound != null)
            {
                _audioSource.PlayOneShot(pickupSound);
            }
        }

        private System.Collections.IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(destructionDelay);

            // Destroy the medical kit object
            Destroy(gameObject);
        }

        #region Inject

        public void InjectAllMedicalKit(IInteractableView interactableView, GameObject effect, GameObject healthManager, AudioClip sound, float delay)
        {
            InjectInteractableView(interactableView);
            InjectHealingEffect(effect);
            InjectHealthGauge(healthManager);
            InjectPickupSound(sound);
            destructionDelay = delay;
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as UnityEngine.Object;
            InteractableView = interactableView;
        }

        public void InjectHealingEffect(GameObject effect)
        {
            healingEffect = effect;
        }

        public void InjectHealthGauge(GameObject healthManager)
        {
            healthGauge = healthManager;
        }

        public void InjectPickupSound(AudioClip sound)
        {
            pickupSound = sound;
        }

        #endregion
    }
}
