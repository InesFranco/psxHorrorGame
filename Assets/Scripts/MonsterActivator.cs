using System.Collections.Generic;
using UnityEngine;

public class MonsterActivator : MonoBehaviour
{
    private float TimeBetweenMonstersGen = 20f;
    public Monster monsterPrefab;
    private Transform[] positions;
    private Dictionary<Transform, bool> positionFilled;

    void Start()
    {
        positions = GetComponentsInChildren<Transform>();
        positionFilled = new Dictionary<Transform, bool>();
        InvokeRepeating("ActivateEnemy", 5f, TimeBetweenMonstersGen);
    }

    private void ActivateEnemy()
    {
        int randIdx = Random.Range(0, positions.Length);
        Transform posAtIndex = positions[randIdx];
        if(!positionFilled.ContainsKey(posAtIndex)){
            positionFilled.Add(posAtIndex, true);
            Monster mon = Instantiate(monsterPrefab, posAtIndex.position, posAtIndex.rotation);
            mon.destroyingMonster.AddListener(() => positionFilled[posAtIndex] = false); 
        }
        else {
            positionFilled.TryGetValue(posAtIndex, out bool val);
            if(!val){
                Monster mon = Instantiate(monsterPrefab, posAtIndex.position, posAtIndex.rotation);
                mon.destroyingMonster.AddListener(() => positionFilled[posAtIndex] = false); 
            }
        }
        
    }
}
