using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    [SerializeField] private Vector2 _leftHandPosition;
    [SerializeField] private Vector2 _rightHandPosition;
    [SerializeField] private Vector2 _twoHandPosition;

    [SerializeField] private FocusObject _focus;
    private Animator _animator;

    private Pickup _leftHand;
    private Pickup _rightHand;
    private static readonly int OccupiedHands = Animator.StringToHash("occupiedHands");
    private bool _leftHandActive;
    private bool _rightHandActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _leftHandActive = Input.GetButton("InteractLeft");
        _rightHandActive = Input.GetButton("InteractRight");

        if (_leftHandActive && Input.GetButtonDown("InteractRight") || _rightHandActive && Input.GetButtonDown("InteractLeft"))
        {
            var pickup = _focus.GetFocusedPickup();

            if (_leftHand && _rightHand)
            {
                _leftHand.transform.SetParent(null);
                _rightHand = null;
                _leftHand = null;
            }
            else if (pickup && pickup.Hands == 2)
            {
                _leftHand = pickup;
                _rightHand = pickup;
                var pickupTransform = pickup.transform;

                pickupTransform.SetParent(transform);
                pickupTransform.localPosition = _twoHandPosition;
            }
        }
        else if (Input.GetButtonDown("InteractLeft"))
        {
            var pickup = _focus.GetFocusedPickup();

            if (_leftHand)
            {
                _leftHand.transform.SetParent(null);
                _leftHand = null;
            }
            else if (pickup && pickup.Hands <= 1)
            {
                _leftHand = pickup;
                var pickupTransform = pickup.transform;

                pickupTransform.SetParent(transform);
                pickupTransform.localPosition = _leftHandPosition;
            }
        }

        else if (Input.GetButtonDown("InteractRight"))
        {
            var pickup = _focus.GetFocusedPickup();

            if (_rightHand)
            {
                _rightHand.transform.SetParent(null);
                _rightHand = null;
                return;
            }

            if (pickup && pickup.Hands <= 1)
            {
                _rightHand = pickup;
                var pickupTransform = pickup.transform;

                pickupTransform.SetParent(transform);
                pickupTransform.localPosition = _rightHandPosition;                
            }
        }
        
        Animate();
    }

    private void Animate()
    {
        var animationNumber = 0;
        if (_leftHand || _leftHandActive)
        {
            animationNumber += 1;
        }

        if (_rightHand || _rightHandActive)
        {
            animationNumber += 2;
        }
        
        _animator.SetInteger(OccupiedHands, animationNumber);
    }
}