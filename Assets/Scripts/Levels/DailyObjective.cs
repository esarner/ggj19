using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu]
    public class DailyObjective : ScriptableObject
    {
        public string ObjectiveDescription;
        public List<PickupTypeWithMultiplier> PickupTypesWithMultiplier;
    
        [Serializable]
        public class PickupTypeWithMultiplier
        {
            public PickupType PickupType;
            public int Multiplier = 1;
        }
    }
}
