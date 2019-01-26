using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : Interactable
{
    private bool _isOpen;
    private Animator _animator;
    private readonly int open = Animator.StringToHash("open");

    [SerializeField] private List<Pickup> _content;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }



    public override void Interact()
    {
        if (_isOpen && _content.Count > 0)
        {
            var pickup = _content[0];
            _content.RemoveAt(0);

            var contentInstance = Instantiate(pickup, transform);
            var contentInstanceTransform = contentInstance.transform;

            contentInstanceTransform.localPosition = new Vector3(0, -0.5f, 0);
            contentInstanceTransform.SetParent(null);
        }
        else
        {
            _isOpen = !_isOpen;
        }

        _animator.SetBool(open, _isOpen);
    }
}