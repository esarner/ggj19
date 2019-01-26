using UnityEngine;

namespace Interactive
{
    public class InnerDoor : Interactable
    {
        private bool _isOpen;
        private readonly int isOpen = Animator.StringToHash("isOpen");
      
        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override bool Interact(HandInteraction getHandInteractionState, IHands hands)
        {
            _isOpen = !_isOpen;

            _animator.SetBool(isOpen, _isOpen);

            return true;
        }
    }
}