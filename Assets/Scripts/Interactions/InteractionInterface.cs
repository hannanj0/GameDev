using UnityEngine.Events;

/// <summary>
/// This script is implemented by the InteractableObject script, used to handle interactions with the object
/// </summary>

namespace StarterAssets.Interactions
{
    public interface InteractionInterface
    {
        public UnityEvent onInteract { get; protected set; }
        public void Interact();
    }
}
