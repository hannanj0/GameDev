using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

/// <summary>
/// The InteractableObject script is used to keep track of the objects that can be interacted with.
/// </summary>

namespace StarterAssets.Interactions
{
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private PlayableDirector cutsceneDirector;
        private bool cutscenePlayed = false;

        public GameObject[] uiElementsToDisable; // New field to reference UI elements

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
        }

        public void Interact()
        {
            if (!cutscenePlayed)
            {
                // Disable UI elements before starting the cutscene
                DisableUIElements();

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
            // Re-enable UI elements after the cutscene finishes
            EnableUIElements();

            ThirdPersonCameraController cameraController = Camera.main.GetComponent<ThirdPersonCameraController>();
            if (cameraController != null)
            {
                cameraController.EnableCameraLogic(true);
            }

            cutsceneDirector.stopped -= OnCutsceneFinished;
        }

        private void DisableUIElements()
        {
            foreach (GameObject uiElement in uiElementsToDisable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(false);
                }
            }
        }

        private void EnableUIElements()
        {
            foreach (GameObject uiElement in uiElementsToDisable)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(true);
                }
            }
        }
    }
}
