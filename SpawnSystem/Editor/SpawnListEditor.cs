using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MSFD
{
    [CustomEditor(typeof(SpawnList))]
    public class SpawnListEditor : Editor
    {
        public float stringFieldWidth = 140;
        public float intFieldWidth = 50;
        public float space = 20;
        public float vertSpace = 1;
        public float labelWidth = 80;
        public float buttonWidth = 20f;

        public float squadFieldWidth = 115;
        public float squadTypeFieldWidth = 20;

        SpawnList slist;

        private void OnEnable()
        {
            //hideFlags = HideFlags.None;
            slist = (SpawnList)target;
            if (slist.squads == null || slist.waves == null)
            {
                slist.Initializate();
            }
        }
        private void OnDisable()
        {
            SaveChanges();
        }
        public void SaveChanges()
        {
            EditorUtility.SetDirty(target);
            Debug.Log("Save" + target.name);
        }
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TotalWeight", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(slist.ReturnTotalWeight().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TotalTime", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(slist.ReturnTotalTime().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Totalpower", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(slist.ReturnTotalPower().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("AddSquad", GUILayout.Width(stringFieldWidth)))
            {
                AddSquad();
                return;
            }
            if (GUILayout.Button("RemoveSquad", GUILayout.Width(stringFieldWidth)))
            {
                RemoveSquad();
                return;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("AddWave", GUILayout.Width(stringFieldWidth)))
            {
                AddWave();
                return;
            }
            if (GUILayout.Button("RemoveWave", GUILayout.Width(stringFieldWidth)))
            {
                RemoveWave();
                return;
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Squads", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                GUILayout.Label((i + 1).ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            for (int j = 0; j < slist.squads.Count; j++)
            {
                GUILayout.BeginHorizontal();
                slist.squads[j] = (SquadData)EditorGUILayout.ObjectField("", slist.squads[j], typeof(SquadData),
                    false, GUILayout.Width(squadFieldWidth));


                if (slist.squads[j] != null)
                {
                    string str = "";
                    if (slist.squads[j].squadType == SquadType.Separate)
                    {
                        str += "S";
                    }
                    else if (slist.squads[j].squadType == SquadType.Monolit)
                    {
                        str += "M";
                    }
                    GUILayout.Label(str, GUILayout.Width(squadTypeFieldWidth));
                    //GUILayout.Label(spawner.squads[j].name, GUILayout.Width(stringFieldWidth));
                }
                else
                {
                    GUILayout.Label("", GUILayout.Width(squadTypeFieldWidth));
                    //GUILayout.Label("Empty", GUILayout.Width(stringFieldWidth));
                }
                for (int i = 0; i < slist.waves.Count; i++)
                {
                    if (slist.waves[i] != null && j < slist.waves[i].unitsNum.Count)
                    {
                        slist.waves[i].unitsNum[j] = EditorGUILayout.IntField(slist.waves[i].unitsNum[j], GUILayout.Width(intFieldWidth));
                    }
                    else
                    {
                        EditorGUILayout.IntField(-1, GUILayout.Width(intFieldWidth));
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(space);

            GUILayout.BeginHorizontal();
            GUILayout.Label("WaveWeight", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                GUILayout.Label(slist.waves[i].ReturnWaveWeight().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            GUILayout.Label("WavePower", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                GUILayout.Label(slist.waves[i].ReturnWavePower().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("WaveTime", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                GUILayout.Label(slist.waves[i].ReturnWaveTime().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GroupsInWave", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                slist.waves[i].groupsInWave = EditorGUILayout.IntField(slist.waves[i].groupsInWave, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GroupsDelay", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                slist.waves[i].groupsDelay = EditorGUILayout.FloatField(slist.waves[i].groupsDelay, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("UnitsDelay", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                slist.waves[i].UnitsDelay = EditorGUILayout.FloatField(slist.waves[i].UnitsDelay, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("NextWaveTime", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                if (i == slist.waves.Count - 1)
                {
                    GUILayout.Label("stop", GUILayout.Width(intFieldWidth));
                }
                else
                {
                    if (slist.waves[i].nextWaveCondition == NextWaveCondition.waitTimeOrUnitsDead)
                    {
                        slist.waves[i].nextWaveStartTime = EditorGUILayout.FloatField(slist.waves[i].nextWaveStartTime, GUILayout.Width(intFieldWidth));
                    }
                    else
                    {
                        GUILayout.Label("close", GUILayout.Width(intFieldWidth));
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("NextWaveCondition", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                if (i == slist.waves.Count - 1)
                {
                    GUILayout.Label("Undefine", GUILayout.Width(intFieldWidth));
                }
                else
                {
                    slist.waves[i].nextWaveCondition = (NextWaveCondition)EditorGUILayout.EnumPopup(slist.waves[i].nextWaveCondition, GUILayout.Width(intFieldWidth));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("SpawnStyle", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < slist.waves.Count; i++)
            {
                slist.waves[i].groupSpawnStyle = (GroupSpawnStyle)EditorGUILayout.EnumPopup(slist.waves[i].groupSpawnStyle, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SaveChanges", GUILayout.Width(stringFieldWidth * 2)))
            {
                SaveChanges();
                return;
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            base.OnInspectorGUI();
        }
        public void RemoveSquad()
        {
            slist.RemoveSquad();
        }
        public void AddSquad()
        {
            slist.AddSquadPlace();
        }
        public void AddWave()
        {
            slist.AddWave();
        }
        public void RemoveWave()
        {
            slist.RemoveWave();
        }
    }
}