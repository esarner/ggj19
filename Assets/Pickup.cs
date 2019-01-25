using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private SpriteRenderer _sprite;
  
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    
    public void SetFocus(bool focused = true)
    {
        _sprite.color = focused ? Color.green : Color.white;
    }
}