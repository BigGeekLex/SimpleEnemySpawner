using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitBalanceWeight))]
public class UnitBalanceWeightEditor : Editor
{
    float stringWidth = 100f;
    float intWidth = 50f;

    UnitBalanceWeight unitBalance;

    private void OnEnable()
    {
        unitBalance = (UnitBalanceWeight)target;


    }
    public override void OnInspectorGUI()
    {

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("+", GUILayout.Width(stringWidth)))
        {
            unitBalance.unitNames.Add(string.Empty);
            unitBalance.unitWeights.Add(0);

        }

        if (GUILayout.Button("-", GUILayout.Width(stringWidth)))
        {
            if(unitBalance.unitNames.Count == 0)
            {
                return;
            }

            unitBalance.unitNames.RemoveAt(unitBalance.unitNames.Count-1);
            unitBalance.unitWeights.RemoveAt(unitBalance.unitWeights.Count - 1);
        }

        GUILayout.EndHorizontal();

        for (int i = 0; i < unitBalance.unitNames.Count; i++)
        {
            GUILayout.BeginHorizontal();


            unitBalance.unitNames[i] = EditorGUILayout.TextField(unitBalance.unitNames[i], GUILayout.Width(stringWidth));
            unitBalance.unitWeights[i] = EditorGUILayout.FloatField(unitBalance.unitWeights[i], GUILayout.Width(stringWidth));

            GUILayout.EndHorizontal();
        }


        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        base.OnInspectorGUI();
    }
}