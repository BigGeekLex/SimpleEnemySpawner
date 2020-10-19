using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*
namespace MSFD
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        public float stringFieldWidth = 140;
        public float intFieldWidth = 50;
        public float space = 20;
        public float vertSpace = 1;
        public float labelWidth = 80;
        public float buttonWidth = 20f;

        public float squadFieldWidth = 115;
        public float squadTypeFieldWidth = 20;

        Spawner spawner;

        private void OnEnable()
        {
            //hideFlags = HideFlags.None;
            spawner = (Spawner)target;
            if (spawner.squads == null || spawner.waves == null)
            {
                spawner.Initializate();
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

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TotalWeight", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(spawner.ReturnTotalWeight().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TotalTime", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(spawner.ReturnTotalTime().ToString(), GUILayout.Width(intFieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Totalpower", GUILayout.Width(stringFieldWidth));
            GUILayout.Label(spawner.ReturnTotalPower().ToString(), GUILayout.Width(intFieldWidth));
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
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                GUILayout.Label((i + 1).ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            for (int j = 0; j < spawner.squads.Count; j++)
            {
                GUILayout.BeginHorizontal();
                spawner.squads[j] = (SquadData)EditorGUILayout.ObjectField("", spawner.squads[j], typeof(SquadData),
                    false, GUILayout.Width(squadFieldWidth));


                if (spawner.squads[j] != null)
                {
                    string str = "";
                    if (spawner.squads[j].squadType == SquadType.Separate)
                    {
                        str += "S";
                    }
                    else if (spawner.squads[j].squadType == SquadType.Monolit)
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
                for (int i = 0; i < spawner.waves.Count; i++)
                {
                    if (spawner.waves[i] != null && j < spawner.waves[i].unitsNum.Count)
                    {
                        spawner.waves[i].unitsNum[j] = EditorGUILayout.IntField(spawner.waves[i].unitsNum[j], GUILayout.Width(intFieldWidth));
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
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                GUILayout.Label(spawner.waves[i].ReturnWaveWeight().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            GUILayout.Label("WavePower", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                GUILayout.Label(spawner.waves[i].ReturnWavePower().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("WaveTime", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                GUILayout.Label(spawner.waves[i].ReturnWaveTime().ToString(), GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GroupsInWave", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                spawner.waves[i].groupsInWave = EditorGUILayout.IntField(spawner.waves[i].groupsInWave, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GroupsDelay", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                spawner.waves[i].groupsDelay = EditorGUILayout.FloatField(spawner.waves[i].groupsDelay, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("UnitsDelay", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                spawner.waves[i].UnitsDelay = EditorGUILayout.FloatField(spawner.waves[i].UnitsDelay, GUILayout.Width(intFieldWidth));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("NextWaveTime", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                if (i == spawner.waves.Count - 1)
                {
                    GUILayout.Label("stop", GUILayout.Width(intFieldWidth));
                }
                else
                {
                    if (spawner.waves[i].nextWaveCondition == NextWaveCondition.waitTimeOrUnitsDead)
                    {
                        spawner.waves[i].nextWaveStartTime = EditorGUILayout.FloatField(spawner.waves[i].nextWaveStartTime, GUILayout.Width(intFieldWidth));
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
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                if (i == spawner.waves.Count - 1)
                {
                    GUILayout.Label("Undefine", GUILayout.Width(intFieldWidth));
                }
                else
                {
                    spawner.waves[i].nextWaveCondition = (NextWaveCondition)EditorGUILayout.EnumPopup(spawner.waves[i].nextWaveCondition, GUILayout.Width(intFieldWidth));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("SpawnStyle", GUILayout.Width(stringFieldWidth));
            for (int i = 0; i < spawner.waves.Count; i++)
            {
                spawner.waves[i].groupSpawnStyle = (GroupSpawnStyle)EditorGUILayout.EnumPopup(spawner.waves[i].groupSpawnStyle, GUILayout.Width(intFieldWidth));
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


        public void Refresh()
        {

        }
        public void SaveChanges()
        {
            EditorUtility.SetDirty(target);
            Debug.Log("Save" + target.name);
        }
        public void RemoveSquad()
        {
            spawner.RemoveSquad();
        }

        public void AddSquad()
        {
            spawner.AddSquadPlace();
        }

        public void AddWave()
        {
            spawner.AddWave();
        }
        public void RemoveWave()
        {
            spawner.RemoveWave();
        }
    }
}*/