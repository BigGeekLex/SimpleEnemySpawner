#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBalance", menuName = "UnitBalanceWeight")]
public class UnitBalanceWeight : ScriptableObject {
    
    [SerializeField]
    public List<string> unitNames;
    [SerializeField]
    public List<float> unitWeights;

    private void OnEnable()
    {
        this.hideFlags = HideFlags.None;
    }

    public float GetBalanceWeight(string name)
    {
        if (unitNames.Contains(name))
        {
            return unitWeights[unitNames.IndexOf(name)];
        }
        else
        {
            Debug.LogError("Заданный юнит отсутствует");
            return 0;
        }
    }
}
#endif