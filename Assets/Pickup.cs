using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private SpriteRenderer _sprite;

    [SerializeField] private int _hands;

    public int Hands
    {
        get { return _hands; }
    }

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void SetFocus(bool focused = true)
    {
        _sprite.color = focused ? Color.green : Color.white;
    }
}