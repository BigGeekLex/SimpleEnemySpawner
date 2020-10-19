using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MSFD.Network;
namespace MSFD
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        SpawnPlacer spawnPlacer;
        [SerializeField]
        SpawnList slist;
        [SerializeField]
        [Header("Only for debug")]
        SpawnerState state = SpawnerState.disable;

        public UnityEvent onEndSpawn;
        int waveInd = 0;
        float waveSpawnProgress;

        private void Awake()
        {
            //Messenger.AddListener(GameEvents.START_BATTLE, StartSpawn);
            // Messenger.AddListener(GameEvents.ALL_UNITS_IN_WAVE_DIED, OnAllUnitsInWaveDied);
            Messenger.AddListener(GameEvents.ACTIVE_NUM_UNITS_CHANGED, OnNumLiveUnitsChanged);
        }
        private void OnDestroy()
        {
            // Messenger.RemoveListener(GameEvents.START_BATTLE, StartSpawn);
            // Messenger.RemoveListener(GameEvents.ALL_UNITS_IN_WAVE_DIED, OnAllUnitsInWaveDied);
            Messenger.RemoveListener(GameEvents.ACTIVE_NUM_UNITS_CHANGED, OnNumLiveUnitsChanged);
        }
        
        public void StartSpawn()
        {
            if (state == SpawnerState.disable)
            {
                waveInd = 0;
                StartCoroutine(Spawn());
            }
            else
            {
                Debug.LogError("Attention! Attempt to start spawn second time. Spawner is already working");
            }
        }
        public IEnumerator Spawn()
        {
            while (waveInd < slist.waves.Count)
            {
                Debug.Log("Spawn wave " + (waveInd + 1));
                Messenger<int>.Broadcast(GameEvents.SPAWN_WAVE, waveInd + 1, MessengerMode.DONT_REQUIRE_LISTENER);
                yield return StartCoroutine(SpawnWave(waveInd));
                waveInd++;
                waveSpawnProgress = 0;
                state = SpawnerState.waitBetweenWaves;
                OnNumLiveUnitsChanged();
                
                if (slist.waves[waveInd - 1].nextWaveCondition == NextWaveCondition.waitTimeOrUnitsDead)
                {
                    StartCoroutine(ContinueSpawnAfterTime(slist.waves[waveInd - 1].nextWaveStartTime + slist.defaultDelayBeforeNewWave));
                }
                yield break;
            }
            state = SpawnerState.endSpawn;
            onEndSpawn.Invoke();
        }


        IEnumerator SpawnWave(int ind)
        {
            state = SpawnerState.spawn;

            List<GroupOfUnits> spawnGroups = slist.waves[ind].GetGroupsOfUnits();

            for (int i = 0; i < spawnGroups.Count; i++)
            {
                yield return StartCoroutine(spawnPlacer.SpawnGroup(spawnGroups[i].squads));
                waveSpawnProgress = (float)(i+1) / spawnGroups.Count;
                if (slist.waves[ind].groupsInWave > 1)
                {
                    if (i == spawnGroups.Count - 1)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(slist.waves[ind].groupsDelay);
                }
            }
            yield break;
        }

        void OnAllUnitsInWaveDied()
        {
            Debug.Log("TryActivateWave On Units Died");
            switch (slist.waves[waveInd - 1].nextWaveCondition)
            {
                case NextWaveCondition.allUnitsDead:
                    {
                        //!!!!!!!!!!!!!!
                        StartCoroutine(ContinueSpawnAfterTime(slist.defaultDelayBeforeNewWave));
                        break;
                    }
                case NextWaveCondition.WaitForEvent:
                    {
                        return;
                    }
                case NextWaveCondition.waitTimeOrUnitsDead:
                    {
                        if (state != SpawnerState.spawn && state != SpawnerState.endSpawn)
                        {
                            StopCoroutine(ContinueSpawnAfterTime(slist.waves[waveInd - 1].nextWaveStartTime + slist.defaultDelayBeforeNewWave));
                            StartCoroutine(ContinueSpawnAfterTime(slist.defaultDelayBeforeNewWave));
                        }
                        break;
                    }
            }
        }
        public IEnumerator ContinueSpawnAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(Spawn());
        }

        public float GetCurrentSpawnProgress()
        {
            return ((float)waveInd + waveSpawnProgress) / (float)slist.waves.Count;       
        }
        public int GetWavesCount()
        {
            return slist.waves.Count; ;
        }
        public void OnNumLiveUnitsChanged()
        {
            if(EnemyManager.instance.NumLiveUnits <= 0 && state == SpawnerState.waitBetweenWaves)
            {
                OnAllUnitsInWaveDied();
            }
        }
        public SpawnerState GetSpawnerState()
        {
            return state;
        }
    }

    public enum GroupSpawnStyle
    {
        onMap,
        divideInAllPoints,
        onePoint,
        twoNearestPoints,
        threeNearestPoints,
        fourNearestPoints

    };
    public enum NextWaveCondition { allUnitsDead, waitTimeOrUnitsDead, WaitForEvent };
    public enum SpawnerState { disable, spawn, waitBetweenWaves, endSpawn };

    public class UnitCounter
    {
        public UnitData enemyUnit;
        public int num;

        public float ReturnWeight()
        {
            //Attention
            return num * enemyUnit.balanceWeight;
        }
    }
}
namespace MSFD
{
    public class GroupOfUnits
    {
        //???
        public List<Squad> squads = new List<Squad>();
        public float currentWeight = 0;
        public float targetWeight;

        public float GetDelta()
        {
            return targetWeight - currentWeight;
        }
        public void AddSquad(Squad squad)
        {
            squads.Add(squad);
            currentWeight += squad.GetSquadWeight();
        }
        public float ReturnCurrentWeight()
        {
            float weight = 0;
            for (int i = 0; i < squads.Count; i++)
            {
                weight += squads[i].GetSquadWeight();
            }
            return weight;
        }
    }

    public class Squad
    {
        public List<UnitCounter> enemies = new List<UnitCounter>();
        //Only for monolitSquad
        public SquadType squadType;
        public float delayBetweenUnits;
        public GroupSpawnStyle squadSpawnStyle;

        public float GetSquadWeight()
        {
            float weight = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                weight += enemies[i].enemyUnit.balanceWeight * enemies[i].num;
            }
            return weight;
        }
        public static Squad FindMaxWeightSquad(List<Squad> squads, out int ind)
        {
            int maxInd = 0;
            for (int i = 1; i < squads.Count; i++)
            {
                if (squads[i].GetSquadWeight() > squads[maxInd].GetSquadWeight())
                {
                    maxInd = i;
                }
            }
            ind = maxInd;
            return squads[maxInd];
        }
        public static Squad FindMaxWeightSquad(List<Squad> squads)
        {
            int maxInd = 0;
            for (int i = 1; i < squads.Count; i++)
            {
                if (squads[i].GetSquadWeight() > squads[maxInd].GetSquadWeight())
                {
                    maxInd = i;
                }
            }
            return squads[maxInd];
        }
    }   
}
