using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSFD
{
    [CreateAssetMenu(fileName = "UnitsPerGroupAllocation", menuName = "Spawn/UnitsPerGroupAllocation")]
    [System.Serializable]
    public class UnitsPerGroupAllocation : ScriptableObject
    {
        public List<UnitsAllocation> unitsAllocation;
        
    }
    [System.Serializable]
    public class UnitsAllocation
    {
        public List<float> groupProcent;
    }
}