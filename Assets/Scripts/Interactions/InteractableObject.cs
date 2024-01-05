using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace StarterAssets.Interactions
{
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private PlayableDirector cutsceneDirector;

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
        }

        private bool cutscenePlayed = false;

        private void Update()
        {
            // Check for player input to initiate the cutscene
            if (Input.GetKeyDown(KeyCode.E) && !cutscenePlayed)
            {
                Interact();
            }
        }

        public void Interact()
        {
            _onInteract.Invoke();

            ThirdPersonCameraController cameraController = Camera.main.GetComponent<ThirdPersonCameraController>();

            if (cameraController != null)
            {
                cameraController.EnableCameraLogic(false);
            }

            PlayCutscene();
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
            // Cutscene finished, re-enable the camera logic
            ThirdPersonCameraController cameraController = Camera.main.GetComponent<ThirdPersonCameraController>();

            if (cameraController != null)
            {
                cameraController.EnableCameraLogic(true);
            }

            // Remove the event listener to prevent memory leaks
            cutsceneDirector.stopped -= OnCutsceneFinished;
        }
    }
}
