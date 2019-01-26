using System;
using System.Collections;
using System.Collections.Generic;
using Interactive;
using TMPro;
using UnityEngine;

public class Pickup : Interactable, IPickup
{
    private Collider2D _collider;

    [SerializeField] private int _hands;
    public int Hands => _hands;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public override bool Interact(HandInteraction getHandInteractionState, IHands hands)
    {
        switch (getHandInteractionState)
        {
            case HandInteraction.NoHands:
                return false;
            case HandInteraction.Left:
                if (_hands < 2)
                    hands.GrabWithLeftHand(this);
                break;
            case HandInteraction.Right:
                if (_hands < 2)
                    hands.GrabWithRightHand(this);
                break;
            case HandInteraction.Both:
                hands.GrabWithBothHands(this);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getHandInteractionState), getHandInteractionState, null);
        }

        return true;
    }

    public void OnPickup()
    {
        EnableCollision(false);
    }

    public void OnDrop()
    {
        EnableCollision();
    }

    private void EnableCollision(bool enable = true)
    {
        _collider.enabled = enable;
    }
}

public interface IPickup
{
    void OnPickup();
    void OnDrop();
}