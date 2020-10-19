using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace MSFD
{
    [CustomEditor(typeof(SquadData))]
    public class SquadDataEditor : Editor
    {
        public float stringFieldWidth = 140;
        public float intFieldWidth = 50;
        public float space = 20;
        public float vertSpace = 1;
        public float labelWidth = 80;
        public float buttonWidth = 20f;

        SquadData squad;
        private void OnEnable()
        {
            //hideFlags = HideFlags.None;
            squad = (SquadData)target;
            if (squad.units == null)
            {
                squad.Initializate();
            }

            Refresh();

        }
        private void OnDisable()
        {
            SaveChanges();
        }
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("AddUnit", GUILayout.Width(stringFieldWidth)))
            {
                AddUnit();
                return;
            }
            if (GUILayout.Button("RemoveUnit", GUILayout.Width(stringFieldWidth)))
            {
                RemoveUnit();
                return;
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Units", GUILayout.Width(stringFieldWidth));
            GUILayout.Label("MinNum", GUILayout.Width(intFieldWidth));
            GUILayout.Label("MaxNum", GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();


            foreach (UnitStats unitStats in squad.units)
            {
                DisplayUnitStats(unitStats);
            }

            GUILayout.Space(space);

            GUILayout.BeginHorizontal();
            GUILayout.Label("SquadWeight", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(squad.ReturnSquadWeight().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("NumUnits", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(squad.ReturnNumUnits().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();
            GUILayout.Space(space);

            if (squad.squadType == SquadType.Monolit)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("SquadPower", GUILayout.Width(stringFieldWidth));
                GUILayout.Label(squad.ReturnSquadPower().ToString(), GUILayout.Width(intFieldWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("SquadSpawnTime", GUILayout.Width(stringFieldWidth));
                GUILayout.Label(squad.ReturnSquadSpawnTime().ToString(), GUILayout.Width(intFieldWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("UnitsDelay", GUILayout.Width(stringFieldWidth));
                squad.DelayBetweenUnits = EditorGUILayout.FloatField(squad.DelayBetweenUnits, GUILayout.Width(intFieldWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("SquadSpawnStyle", GUILayout.Width(stringFieldWidth));
                squad.SquadSpawnStyle = (GroupSpawnStyle)EditorGUILayout.EnumPopup(squad.SquadSpawnStyle, GUILayout.Width(stringFieldWidth));
                GUILayout.EndHorizontal();
            }


            GUILayout.BeginHorizontal();
            GUILayout.Label("SquadType", GUILayout.Width(stringFieldWidth));
            squad.squadType = (SquadType)EditorGUILayout.EnumPopup(squad.squadType, GUILayout.Width(stringFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.Space(space);
            if (GUILayout.Button("SaveChanges", GUILayout.Width(stringFieldWidth * 2)))
            {
                SaveChanges();
                return;
            }

            GUILayout.EndVertical();
            GUILayout.Space(space);
            GUILayout.FlexibleSpace();
            base.OnInspectorGUI();
        }
        public void DisplayUnitStats(UnitStats unitStats)
        {
            GUILayout.BeginHorizontal();
            unitStats.unitData = (UnitData)EditorGUILayout.ObjectField("", unitStats.unitData, typeof(UnitData),
                false, GUILayout.Width(stringFieldWidth));
            unitStats.MinNum = EditorGUILayout.IntField(unitStats.MinNum, GUILayout.Width(intFieldWidth));
            unitStats.MaxNum = EditorGUILayout.IntField(unitStats.MaxNum, GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();
        }

        public void Refresh()
        {

        }
        public void AddUnit()
        {
            squad.AddUnit();
        }
        public void RemoveUnit()
        {
            squad.RemoveUnit();
        }
        public void SaveChanges()
        {
            EditorUtility.SetDirty(target);
            Debug.Log("Save" + target.name);
        }

    }
}