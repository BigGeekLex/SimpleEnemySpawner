using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSFD
{
    public class SpawnPlacer : MonoBehaviour
    {
        public List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] private bool isAutoSetPoints;
        [SerializeField]
        string[] registeredTagsInEnemyManager;

        void Awake()
        {
            if (isAutoSetPoints)
            {
                spawnPoints.Clear();
                foreach (Transform s in transform)
                {
                  spawnPoints.Add(s);     
                }
            }
        }
        public void SpawnUnit(UnitCounter enemyUnit, Transform spawnPoint)
        {
            GameObject unit = PoolManager.instance.Instantiate(enemyUnit.enemyUnit.prefab, spawnPoint.position, spawnPoint.rotation, false);//Instantiate<GameObject>(enemyUnit.enemyUnit.prefab);
            unit.transform.SetParent(spawnPoint);
            if (AuxiliarySystem.CompareTags(unit.tag, registeredTagsInEnemyManager))
            {
                EnemyManager.instance.OnUnitSpawn(unit);
            }
            unit.SetActive(true);
            //aatttt            
        }

        public IEnumerator SpawnSquad(Squad squad, Transform spawnPoint)
        {
            while (squad.enemies.Count > 0)
            {
                yield return new WaitForSeconds(squad.delayBetweenUnits);
                int ind = Random.Range(0, squad.enemies.Count);
                SpawnUnit(squad.enemies[ind], spawnPoint);
                squad.enemies[ind].num -= 1;
                if (squad.enemies[ind].num <= 0)
                {
                    squad.enemies.RemoveAt(ind);
                }
            }
        }       
        
        public IEnumerator SpawnGroup(List<Squad> squads)
        {
            yield return StartCoroutine(Spawner(squads));
        }
        IEnumerator Spawner(List<Squad> squads)
        {
            for (int i = 0; i < squads.Count; i++)
            {
                Transform position = null;
                switch (squads[i].squadSpawnStyle)
                {
                    case GroupSpawnStyle.onePoint:
                        {
                            position = spawnPoints[Random.Range(0, spawnPoints.Count)];
                            break;
                        }
                    case GroupSpawnStyle.divideInAllPoints:
                    {
                            //position = spawnPoints[SetDividedPosition(i, squads.Count, spawnPoints.Count)];
                        break;
                    }
                    default:
                        {
                            position = spawnPoints[Random.Range(0, spawnPoints.Count)];
                            break;
                        }
                }
                yield return StartCoroutine(SpawnSquad(squads[i], position));
            }
            //yield break;
        }

        /*
        int currentPointInd = 0;
        int SetDividedPosition(int currentSquadIndex, int squadsCount, int positionCount)
        {
            int squadsPerPoint = squadsCount / positionCount;

            if (currentSquadIndex <= squadsPerPoint)
            {
                return currentPointInd;
            }
            else
            {
                for(int i )
            }
                int ind = squadsCount / positionCount  currentSquadIndex;
            }
        }*/

        //public float GetGroupsSpawnProgress()

    }
}