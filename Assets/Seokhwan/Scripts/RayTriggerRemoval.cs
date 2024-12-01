using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction
{
    /// <summary>
    /// Removes the game object when it is hit by a ray and the trigger button is pressed.
    /// </summary>
    public class RayTriggerRemoval : MonoBehaviour
    {
        [Tooltip("The interactable to monitor for ray interactions.")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;

        [Tooltip("The input used to confirm the removal action.")]
        [SerializeField]
        private string _triggerButton = "Fire1";

        private IInteractableView InteractableView;
        private bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
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
            if (args.NewState == InteractableState.Hover && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                RemoveObject();
            }
        }

        private void RemoveObject()
        {
            Destroy(gameObject);
        }

        #region Inject

        public void InjectAllRayTriggerRemoval(IInteractableView interactableView)
        {
            InjectInteractableView(interactableView);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as UnityEngine.Object;
            InteractableView = interactableView;
        }

        #endregion
    }
}
