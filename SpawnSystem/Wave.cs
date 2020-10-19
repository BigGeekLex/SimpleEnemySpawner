using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSFD
{
    [System.Serializable]
    public class Wave
    {
        public List<int> unitsNum = new List<int>();

        public int groupsInWave = 1;
        public float groupsDelay = 0;
        public float UnitsDelay = 0;
        /* {
             get
             {
                 return _unitsDelay;
             }
             set
             {
                 //_unitsDelay = value;

                 if (value < GameValues.minDelayBetweenUnits)
                 {
                     _unitsDelay = GameValues.minDelayBetweenUnits;
                 }
                 else
                 {
                     _unitsDelay = value;
                 }
                 Debug.Log("Set UnitsDelay value" + _unitsDelay);
             }
         }


         float _unitsDelay;*/

        public float nextWaveStartTime = 10f; // Works with conditions allUnitsDead, WaitTime

        public NextWaveCondition nextWaveCondition = NextWaveCondition.allUnitsDead;
        public GroupSpawnStyle groupSpawnStyle = GroupSpawnStyle.onMap;

        public SpawnList slist;
        public static Wave CreateWaveCopy(Wave source)
        {
            Wave wave = new Wave();
            wave.groupsDelay = source.groupsDelay;
            wave.groupsInWave = source.groupsInWave;
            wave.groupSpawnStyle = source.groupSpawnStyle;
            wave.nextWaveCondition = source.nextWaveCondition;
            wave.nextWaveStartTime = source.nextWaveStartTime;
            wave.slist = source.slist;

            wave.UnitsDelay = source.UnitsDelay;
            for (int i = 0; i < source.unitsNum.Count; i++)
            {
                wave.unitsNum.Add(source.unitsNum[i]);
            }
            return wave;
        }
        public float ReturnWaveWeight()
        {
            float power = 0;
            for (int i = 0; i < unitsNum.Count; i++)
            {
                if (slist.squads[i] != null)
                {
                    power += slist.squads[i].ReturnSquadWeight() * unitsNum[i];
                }
                else
                {
                    continue;
                }
            }
            return power;
        }
        public float ReturnWaveTime()
        {
            float time = 0;
            for (int i = 0; i < unitsNum.Count; i++)
            {
                if (slist.squads[i] != null)
                {
                    if (slist.squads[i].squadType == SquadType.Separate)
                    {
                        time += UnitsDelay * unitsNum[i] * slist.squads[i].ReturnNumUnits();
                    }
                    else
                    {
                        time += unitsNum[i] * slist.squads[i].ReturnSquadSpawnTime();
                    }
                }
                else
                {
                    continue;
                }
            }
            time += (groupsDelay * (groupsInWave - 1));
            return time;
        }
        public float ReturnWavePower()
        {
            return ReturnWaveWeight() / ReturnWaveTime();
        }

        public List<GroupOfUnits> GetGroupsOfUnits()
        {
            Wave wave = Wave.CreateWaveCopy(this);

            List<GroupOfUnits> spawnGroups = new List<GroupOfUnits>();
            float totalWaveWeight = wave.ReturnWaveWeight();
            for (int i = 0; i < wave.groupsInWave; i++)
            {
                float targetGroupWeight = totalWaveWeight * slist.unitsPerGroup.unitsAllocation[wave.groupsInWave - 1].groupProcent[i];
                GroupOfUnits group = new GroupOfUnits();
                group.targetWeight = targetGroupWeight;
                spawnGroups.Add(group);
            }

            InsertSquadsInSpawnGroups(spawnGroups, wave);

            return spawnGroups;
        }
        void InsertSquadsInSpawnGroups(List<GroupOfUnits> spawnGroups, Wave wave)
        {
            List<Squad> squads = CutSquads(wave);
            int squadsCount = squads.Count;
            for (int i = 0; i < squadsCount; i++)
            {
                int index;
                Squad squad = Squad.FindMaxWeightSquad(squads, out index);
                squads.RemoveAt(index);

                int random = Random.Range(0, spawnGroups.Count);
                if (spawnGroups[random].GetDelta() >= squad.GetSquadWeight())
                {
                    spawnGroups[random].squads.Add(squad);
                    spawnGroups[random].currentWeight += squad.GetSquadWeight();
                }
                else
                {
                    int r = random;
                    do
                    {
                        r++;
                        r = r % spawnGroups.Count;
                        if (spawnGroups[r].GetDelta() >= squad.GetSquadWeight())
                        {
                            spawnGroups[r].squads.Add(squad);
                            spawnGroups[r].currentWeight += squad.GetSquadWeight();
                            break;
                        }
                    } while (r != random);
                    if (r == random)
                    {
                        //Debug.LogError("Insert squad Error");
                        spawnGroups[r].squads.Add(squad);
                        spawnGroups[r].currentWeight += squad.GetSquadWeight();
                    }
                }
            }
        }
        List<Squad> CutSquads(Wave wave)
        {
            List<Squad> squads = new List<Squad>();
            for (int i = 0; i < wave.unitsNum.Count; i++)
            {
                if (wave.unitsNum[i] > 0)
                {
                    List<Squad> oneTypeSquads = slist.squads[i].GetSquads(wave.unitsNum[i]);
                    wave.unitsNum[i] = 0;
                    if (oneTypeSquads == null)
                    {
                        continue;
                    }
                    if (slist.squads[i].squadType == SquadType.Separate)
                    {
                        foreach (Squad x in oneTypeSquads)
                        {
                            x.delayBetweenUnits = wave.UnitsDelay;
                            x.squadSpawnStyle = wave.groupSpawnStyle;
                        }
                    }
                    squads.AddRange(oneTypeSquads);
                }
            }
            return squads;
        }
        List<Squad> CutSquadsDefinedType(Wave wave, SquadType squadType)
        {
            List<Squad> squads = new List<Squad>();
            for (int i = 0; i < wave.unitsNum.Count; i++)
            {
                if (wave.unitsNum[i] > 0)
                {
                    if (slist.squads[i].squadType == squadType)
                    {
                        squads = slist.squads[i].GetSquads(wave.unitsNum[i]);
                        wave.unitsNum[i] = 0;
                    }
                }
            }
            return squads;
        }
    }
}