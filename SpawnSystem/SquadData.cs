using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSFD
{
    [CreateAssetMenu(fileName = "SquadData", menuName = "Data/SquadData")]
    [System.Serializable]
    public class SquadData : ScriptableObject
    {
        public List<UnitStats> units;
        public SquadType squadType = SquadType.Separate;
        public float DelayBetweenUnits
        {
            get
            {
                if (squadType == SquadType.Separate)
                {
                    Debug.LogError("Error. Try to get _delayBetweenUnits in Separate squad");
                }
                return _delayBetweenUnits;
            }
            set
            {
                if (value < GameValues.minDelayBetweenUnits)
                {
                    _delayBetweenUnits = GameValues.minDelayBetweenUnits;
                }
                else
                {
                    _delayBetweenUnits = value;
                }
            }
        }
        public GroupSpawnStyle SquadSpawnStyle
        {
            get
            {
                if (squadType == SquadType.Separate)
                {
                    Debug.LogError("Error. Try to get _squadSpawnStyle in Separate squad");
                }
                return squadSpawnStyle;
            }
            set
            {
                squadSpawnStyle = value;
            }
        }


        GroupSpawnStyle squadSpawnStyle;
        float _delayBetweenUnits = 0;

        public List<Squad> GetSquads(int value)//value - value of this squad in this wave
        {
            List<Squad> squads = new List<Squad>();
            for (int i = 0; i < value; i++)
            {
                Squad squad = new Squad();
                squad.squadType = squadType;
                if (squadType == SquadType.Monolit)
                {
                    squad.delayBetweenUnits = DelayBetweenUnits;
                    squad.squadSpawnStyle = SquadSpawnStyle;
                }

                squad.enemies = GetOneSquadUnits();
                if (squad.enemies == null)
                {
                    continue;
                }
                else
                {
                    squads.Add(squad);
                }
            }
            if (squads.Count > 0)
            {
                return squads;
            }
            else
            {
                return null;
            }
        }

        //Decrement of OneSquadUnit
        List<UnitCounter> GetOneSquadUnits()//value - value of this squad in this wave
        {
            List<UnitCounter> unitList = new List<UnitCounter>();
            for (int i = 0; i < units.Count; i++)
            {
                UnitCounter unitData = new UnitCounter();
                int rand = Random.Range(units[i].MinNum, units[i].MaxNum + 1);
                if (rand == 0)
                {
                    continue;
                }
                else
                {
                    unitData.enemyUnit = units[i].unitData;
                    unitData.num = (int)rand;
                    unitList.Add(unitData);
                }
            }
            if (unitList.Count > 0)
            {
                return unitList;
            }
            else
            {
                return null;
            }
        }

        public float ReturnSquadWeight()
        {
            float weight = 0;
            foreach (UnitStats x in units)
            {
                if (x == null || x.unitData == null)
                {
                    continue;
                }
                weight += x.unitData.balanceWeight * ((x.MaxNum + x.MinNum) / 2f);
            }
            return weight;
        }
        public float ReturnSquadPower()
        {
            float power = 0;
            power = ReturnSquadWeight() / ReturnSquadSpawnTime();
            return power;
        }
        public float ReturnSquadSpawnTime()
        {
            if (squadType == SquadType.Separate)
            {
                Debug.LogError("Error. Try to get SquadSpawnTime in Separate squad");
            }
            float spawnTime = ReturnNumUnits() * DelayBetweenUnits;
            return spawnTime;
        }

        public float ReturnNumUnits()
        {
            float numUnits = 0;
            foreach (UnitStats x in units)
            {
                if (x == null || x.unitData == null)
                {
                    continue;
                }
                numUnits += ((x.MaxNum + x.MinNum) / 2f);
            }
            return numUnits;
        }

        public void Initializate()
        {
            units = new List<UnitStats>();
        }

        public void AddUnit()
        {
            units.Add(null);
        }
        public void RemoveUnit()
        {
            units.RemoveAt(units.Count - 1);
        }
    }

    [System.Serializable]
    public class UnitStats
    {
        public UnitData unitData;
        public int MinNum
        {
            get
            {
                return _minNum;
            }
            set
            {
                if (value < 0)
                {
                    _minNum = 0;
                }
                else if (value > _maxNum)
                {
                    _minNum = value;
                    _maxNum = value;
                }
                else
                {
                    _minNum = value;
                }
            }
        }
        public int MaxNum
        {
            get
            {
                return _maxNum;
            }
            set
            {
                if (value < 0)
                {
                    _maxNum = 0;
                }
                else if (value < _minNum)
                {
                    _minNum = value;
                    _maxNum = value;
                }
                else
                {
                    _maxNum = value;
                }
            }
        }

        int _minNum = 1;
        int _maxNum = 1;



    }
    public enum SquadType { Separate, Monolit };
}
