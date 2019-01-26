using System.Collections;
using System.Collections.Generic;
using Interactive;
using UnityEngine;

public class Hands : MonoBehaviour, IHands
{
    [SerializeField] private Vector2 _leftHandPosition;
    [SerializeField] private Vector2 _rightHandPosition;
    [SerializeField] private Vector2 _twoHandPosition;
    [SerializeField] private AudioClip _pickupAudioClip;
    [SerializeField] private AudioClip _dropSound;

    [SerializeField] private FocusObject _focus;
    private Animator _animator;

    private Pickup _leftHand;
    private Pickup _rightHand;
    private static readonly int OccupiedHands = Animator.StringToHash("occupiedHands");
    private bool _leftHandActive;
    private bool _rightHandActive;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void PlayPickUpSound()
    {
        _audioSource.clip = _pickupAudioClip;
        _audioSource.Play();
    }


    void PlayDropSound()
    {
        _audioSource.clip = _dropSound;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _leftHandActive = Input.GetButton("InteractLeft");
        _rightHandActive = Input.GetButton("InteractRight");

        if (_leftHandActive && Input.GetButtonDown("InteractRight") || _rightHandActive && Input.GetButtonDown("InteractLeft"))
        {
            var handInteractionState = GetHandInteractionState();

            var interactable = _focus.GetFocusedInteractable();

            if (interactable)
            {
                if (interactable.Interact(handInteractionState, this) == false)
                {
                    if (_leftHand)
                    {
                        _leftHand.OnDrop();
                        _leftHand.transform.SetParent(null);
                        _rightHand = null;
                        _leftHand = null;
                        PlayDropSound();
                    }
                }
            }
            else if (handInteractionState == HandInteraction.NoHands)
            {
                if (_leftHand)
                {
                    _leftHand.OnDrop();
                    _leftHand.transform.SetParent(null);
                    _rightHand = null;
                    _leftHand = null;
                    PlayDropSound();
                }
            }
        }
        else if (Input.GetButtonDown("InteractLeft"))
        {
            var handInteractionState = GetHandInteractionState();

            var interactable = _focus.GetFocusedInteractable();

            if (interactable)
            {
                if (interactable.Interact(handInteractionState, this) == false)
                {
                    _leftHand.OnDrop();
                    _leftHand.transform.SetParent(null);
                    _leftHand = null;
                    PlayDropSound();
                }
            }
            else if (handInteractionState == HandInteraction.NoHands)
            {
                _leftHand.OnDrop();
                _leftHand.transform.SetParent(null);
                _leftHand = null;
                PlayDropSound();
            }
        }
        else if (Input.GetButtonDown("InteractRight"))
        {
            var handInteractionState = GetHandInteractionState();

            var interactable = _focus.GetFocusedInteractable();

            if (interactable)
            {
                if (interactable.Interact(handInteractionState, this) == false)
                {
                    _rightHand.OnDrop();
                    _rightHand.transform.SetParent(null);
                    _rightHand = null;
                    PlayDropSound();
                }
            }
            else if (handInteractionState == HandInteraction.NoHands)
            {
                _rightHand.OnDrop();
                _rightHand.transform.SetParent(null);
                _rightHand = null;
                PlayDropSound();
            }
        }

        Animate();
    }

    private HandInteraction GetHandInteractionState()
    {
        if (_rightHandActive && !_leftHandActive && !_rightHand)
            return HandInteraction.Right;

        if (_leftHandActive && !_rightHandActive && !_leftHand)
            return HandInteraction.Left;

        if (_leftHandActive && _rightHandActive && !_leftHand && !_rightHand)
            return HandInteraction.Both;

        return HandInteraction.NoHands;
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

    public void GrabWithLeftHand(Pickup pickup)
    {
        _leftHand = pickup;
        pickup.OnPickup();
        var pickupTransform = pickup.transform;

        pickupTransform.SetParent(transform);
        pickupTransform.localPosition = _leftHandPosition;

        PlayPickUpSound();
    }

    public void GrabWithRightHand(Pickup pickup)
    {
        _rightHand = pickup;
        pickup.OnPickup();
        var pickupTransform = pickup.transform;

        pickupTransform.SetParent(transform);
        pickupTransform.localPosition = _rightHandPosition;

        PlayPickUpSound();
    }

    public void GrabWithBothHands(Pickup pickup)
    {
        _leftHand = pickup;
        _rightHand = pickup;
        pickup.OnPickup();
        var pickupTransform = pickup.transform;

        pickupTransform.SetParent(transform);
        pickupTransform.localPosition = _twoHandPosition;

        PlayPickUpSound();
    }
}