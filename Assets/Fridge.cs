using System.Collections;
using System.Collections.Generic;
using Interactive;
using UnityEngine;

public class Fridge : Interactable
{
    private bool _isOpen;
    private Animator _animator;
    private readonly int open = Animator.StringToHash("open");

    [SerializeField] private List<Pickup> _content;

    [SerializeField]
    private SpriteRenderer _doorSprite;
    [SerializeField]
    private SpriteRenderer _fridgeSprite;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    }

    public override void SetFocus(bool focused = true)
    {
        if (focused)
        {
            _fridgeSprite.color = Color.green;
            _doorSprite.color = Color.green;
        }
        else
        {
            _fridgeSprite.color = Color.white;
            _doorSprite.color = Color.white;
        }
    }


    public override bool Interact(HandInteraction getHandInteractionState, IHands hands)
    {
        if (_isOpen && _content.Count > 0)
        {
            var pickup = _content[0];
            _content.RemoveAt(0);

            var contentInstance = Instantiate(pickup, transform);
            var contentInstanceTransform = contentInstance.transform;

            PickUp(getHandInteractionState, hands, contentInstance);
        }
        else
        {
            _isOpen = !_isOpen;

        }

        _animator.SetBool(open, _isOpen);

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