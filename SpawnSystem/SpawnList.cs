using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSFD
{
    [CreateAssetMenu(fileName = "SpawnList", menuName = "Data/SpawnList")]
    [System.Serializable]
    public class SpawnList : ScriptableObject
    {
        public List<SquadData> squads;
        public List<Wave> waves;
        public UnitsPerGroupAllocation unitsPerGroup;
        public float defaultDelayBeforeNewWave = 3f;

#if UNITY_EDITOR
        public void Initializate()
        {
            squads = new List<SquadData>();
            waves = new List<Wave>();
        }
        public void AddSquadPlace()
        {
            squads.Add(null);
            foreach (Wave x in waves)
            {
                x.unitsNum.Add(0);
            }
        }
        public void RemoveSquad()
        {
            squads.Remove(squads[squads.Count - 1]);
            foreach (Wave x in waves)
            {
                x.unitsNum.RemoveAt(x.unitsNum.Count - 1);
            }
        }
        public void AddWave()
        {
            Wave wave = new Wave();
            wave.slist = this;
            List<int> unitsNum = new List<int>();
            for (int i = 0; i < squads.Count; i++)
            {
                unitsNum.Add(0);
            }
            wave.unitsNum = unitsNum;
            waves.Add(wave);
        }
        public void RemoveWave()
        {
            waves.RemoveAt(waves.Count - 1);
        }

        public float ReturnTotalWeight()
        {
            float weight = 0;
            for (int i = 0; i < waves.Count; i++)
            {
                weight += waves[i].ReturnWaveWeight();
            }
            return weight;
        }
        public float ReturnTotalTime()
        {
            float time = 0;
            for (int i = 0; i < waves.Count; i++)
            {
                time += waves[i].ReturnWaveTime();
                if (i != waves.Count - 1)
                {
                    time += waves[i].nextWaveStartTime;
                }
            }
            return time;
        }
        public float ReturnTotalPower()
        {
            float power = ReturnTotalWeight() / ReturnTotalTime();

            return power;
        }
#endif
    }
}