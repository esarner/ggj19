using System.Collections.Generic;
using Interactive;
using UnityEngine;

public class Container : Interactable
{
    private bool _isOpen;
    private readonly int isOpen = Animator.StringToHash("isOpen");

    [SerializeField] private List<Pickup> _content;

    [SerializeField]
    private SpriteRenderer _doorSpriteRenderer;
    [SerializeField]
    private SpriteRenderer _containerSpriteRenderer;
    [SerializeField]
    private Animator _animator;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void SetFocus(bool focused = true)
    {
        if (focused)
        {
            _containerSpriteRenderer.color = Color.green;
            _doorSpriteRenderer.color = Color.green;
        }
        else
        {
            _containerSpriteRenderer.color = Color.white;
            _doorSpriteRenderer.color = Color.white;
        }
    }


    public override bool Interact(HandInteraction getHandInteractionState, IHands hands)
    {
        if (_isOpen && _content.Count > 0)
        {
            var pickup = _content[0];
            _content.RemoveAt(0);

            var contentInstance = Instantiate(pickup, transform);

            PickUp(getHandInteractionState, hands, contentInstance);
        }
        else
        {
            _isOpen = !_isOpen;
        }

        _animator.SetBool(isOpen, _isOpen);

        return true;
    }

    private void PickUp(HandInteraction getHandInteractionState, IHands hands, Pickup pickup)
    {
        if (getHandInteractionState == HandInteraction.Left)
        {
            hands.GrabWithLeftHand(pickup);
        }
        if (getHandInteractionState == HandInteraction.Right || getHandInteractionState == HandInteraction.Both)
        {
            hands.GrabWithRightHand(pickup);
        }

    }
}