using Interactive;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;

    public virtual bool Interact(HandInteraction getHandInteractionState, IHands hands)
    {
        return false;
    }

    public virtual void SetFocus(bool focused = true)
    {
        _spriteRenderer.color = focused ? Color.green : Color.white;
    }
}