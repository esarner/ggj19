using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    private readonly List<Pickup> _objectsInDropZone = new List<Pickup>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<Pickup>();

        if (pickup != null)
        {
            _objectsInDropZone.Add(pickup);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pickup = other.GetComponent<Pickup>();

        if (pickup != null && _objectsInDropZone.Contains(pickup))
        {
            _objectsInDropZone.Remove(pickup);
        }
    }
}
