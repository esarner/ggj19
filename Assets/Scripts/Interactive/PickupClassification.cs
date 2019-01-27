using System;
using UnityEngine;

[Serializable]
public struct PickupClassification
{
    public PickupType Type;
    public int Points;

}

public enum PickupType
{
    Edible,
    Drinkable,
    Sittable,
    Watchable,
    Entertainment,
    Cleaning,
    Tools,
    Fruit,
}
