using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private readonly List<Interactable> _reachableObjects = new List<Interactable>();
    private Interactable _focusedInteractable;

    private void Update()
    {
        var interactable = _reachableObjects.FirstOrDefault();
        if (interactable)
        {
            _focusedInteractable = interactable;
            _focusedInteractable.SetFocus();
        }
        else
        {
            _focusedInteractable = null;
        }
    }

    public Interactable GetFocusedInteractable()
    {
        return _focusedInteractable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            _reachableObjects.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();

        if (interactable != null && _reachableObjects.Contains(interactable))
        {
            interactable.SetFocus(false);
            _reachableObjects.Remove(interactable);
        }
    }
}