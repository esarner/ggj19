using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private readonly List<Transform> _reachableObjects = new List<Transform>();

    private void Update()
    {
        var fisk = _reachableObjects.FirstOrDefault();
        if (fisk)
        {
            fisk.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            _reachableObjects.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_reachableObjects.Contains(other.transform))
        {
            _reachableObjects.Remove(other.transform);
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
