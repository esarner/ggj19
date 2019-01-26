using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private List<Pickup> _objectsInDropZone = new List<Pickup>();
    [SerializeField] private AudioClip _dropClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<Pickup>();


        if (pickup != null)
        {
            PlayDropSound();
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

    public List<Pickup> GetObjectsInDropZone()
    {
        return _objectsInDropZone;
    }

    private void PlayDropSound()
    {
        _audioSource.clip = _dropClip;
        _audioSource.Play();
    }
}
