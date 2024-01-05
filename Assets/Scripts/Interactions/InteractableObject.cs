using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace StarterAssets.Interactions
{
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private PlayableDirector cutsceneDirector;
        private bool cutscenePlayed = false;

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
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