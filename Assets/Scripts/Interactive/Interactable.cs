using Interactive;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected SpriteRenderer _sprite;

    public virtual bool Interact(HandInteraction getHandInteractionState, IHands hands)
    {
        return false;
    }

    public virtual void SetFocus(bool focused = true)
    {
        _sprite.color = focused ? Color.green : Color.white;
    }
}