using UnityEngine;
using UnityEngine.Events;

namespace StarterAssets.Interactions
{
    public class InteractableObject : MonoBehaviour, InteractionInterface
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private Conversation _conversation;

        UnityEvent InteractionInterface.onInteract
        {
            get => _onInteract;
            set => _onInteract = value;
        }

        public Conversation Conversation
        {
            get => _conversation;
        }

        private void Start()
        {
            // Ensure the conversation is disabled initially
            if (_conversation != null)
            {
                _conversation.gameObject.SetActive(false);
            }
        }

        public void Interact()
        {
            // Trigger the onInteract event
            _onInteract.Invoke();
        }
    }
}
