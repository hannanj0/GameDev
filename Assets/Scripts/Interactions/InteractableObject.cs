using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.InputSystem; // Include the Input System namespace

namespace StarterAssets.Interactions
{
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private PlayableDirector cutsceneDirector;

        private PlayerControls controls; // Reference to PlayerControls
        private bool cutscenePlayed = false;

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
        }

        void Awake()
        {
            controls = new PlayerControls(); // Initialize the PlayerControls
            controls.Gameplay.Interact.performed += context => Interact();
        }

        void OnEnable()
        {
            controls.Gameplay.Interact.Enable(); // Enable the Interact action
        }

        void OnDisable()
        {
            controls.Gameplay.Interact.Disable(); // Disable the Interact action
        }

        public void Interact()
        {
            if (!cutscenePlayed)
            {
                _onInteract.Invoke();

                ThirdPersonCameraController cameraController = Camera.main.GetComponent<ThirdPersonCameraController>();
                if (cameraController != null)
                {
                    cameraController.EnableCameraLogic(false);
                }

                PlayCutscene();
            }
        }

        private void PlayCutscene()
        {
            if (cutsceneDirector != null)
            {
                cutsceneDirector.gameObject.SetActive(true);
                cutsceneDirector.stopped += OnCutsceneFinished;
                cutsceneDirector.Play();
                cutscenePlayed = true;
            }
        }

        private void OnCutsceneFinished(PlayableDirector director)
        {
            ThirdPersonCameraController cameraController = Camera.main.GetComponent<ThirdPersonCameraController>();
            if (cameraController != null)
            {
                cameraController.EnableCameraLogic(true);
            }

            cutsceneDirector.stopped -= OnCutsceneFinished;
        }
    }
}
