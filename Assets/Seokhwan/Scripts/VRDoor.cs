using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace Oculus.Interaction
{
    /// <summary>
    /// Removes the game object when it is hit by a ray and the trigger button is pressed.
    /// </summary>
    public class VRDoor : MonoBehaviour
    {
        [Tooltip("The interactable to monitor for ray interactions.")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView;
        private bool _started = false;
        public Animator openandclose;
		public bool open;
		public Transform Player;
        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
            this.EndStart(ref _started);
            open = false;
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
            if (Player) {
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 15)
					{
						if (open == false)
						{
							if (args.NewState == InteractableState.Select)
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (args.NewState == InteractableState.Select)
								{
									StartCoroutine(closing());
								}
						}

					}

				}
			}
        }
        IEnumerator opening()
		{
			print("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
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
