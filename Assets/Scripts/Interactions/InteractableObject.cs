using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

/// <summary>
/// This is the script attached to the NPC to initiate the interaction between the NPC and player, while also starting the cutscene related to this scenario
/// </summary>
namespace StarterAssets.Interactions
{
    /// <summary>
    /// This class implements InteractionInterface, also contains get and set
    /// </summary>
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private PlayableDirector cutsceneDirector; // This is where the Unity Timeline is attached, to play the cutscene
        private bool cutscenePlayed = false; // Ensures the cutscene plays once

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
        }
    /// <summary>
    /// Checks if the cutscene has already been played, if not will play it and disables the ThirdPersonCameraController logic temporarily as the cutscene uses the main camera itself
    /// </summary>
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
    /// <summary>
    /// Pretty much self-explanatory, but it  plays the cutscene and once done will set the flag to true
    /// </summary>
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

    /// <summary>
    /// When the cutscene finishes, it reenables the camera logic
    /// </summary>
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