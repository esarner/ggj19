using System;
using UnityEngine;

[Serializable]
public struct PickupClassification
{
    public PickupType Type;
    public uint Points;

}

public enum PickupType
{
    Edible,
    Drinkable,
    Sittable,
    Watchable,
    Entertainment,
    Cleaning,
    Tools
}
