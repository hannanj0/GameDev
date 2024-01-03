using UnityEngine.Events;

namespace StarterAssets.Interactions
{
    public interface InteractionInterface
    {
        public UnityEvent onInteract { get; protected set; }
        public void Interact();
    }
}
