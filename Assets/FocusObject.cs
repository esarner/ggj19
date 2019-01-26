using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private readonly List<Pickup> _reachableObjects = new List<Pickup>();
    private Pickup _focusedPickup;

    private void Update()
    {
        var pickup = _reachableObjects.FirstOrDefault();
        if (pickup)
        {
            _focusedPickup = pickup;
            _focusedPickup.SetFocus();

          
        }
    }

    public Pickup GetFocusedPickup()
    {
        return _focusedPickup;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<Pickup>();

        if (pickup != null)
        {
            _reachableObjects.Add(pickup);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pickup = other.GetComponent<Pickup>();

        if (pickup != null && _reachableObjects.Contains(pickup))
        {
            pickup.SetFocus(false);
            _reachableObjects.Remove(pickup);
        }
    }
}