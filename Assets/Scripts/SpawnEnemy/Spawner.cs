using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private List<CharacterBase> spawnedCharacters = new List<CharacterBase>();
    private bool hasSpawned;
    private bool allSpawnedAreDead = true;
    public UnityEvent OnAllSpawnedCharacterElininated;

    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointArray);
    }

    private void Update()
    {
        if(!hasSpawned || spawnedCharacters.Count == 0) return;

        allSpawnedAreDead = true;
        foreach(CharacterBase character in spawnedCharacters)
        {
            if(character.currentState != CharacterBase.CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }
        if (allSpawnedAreDead)
        {
            if(OnAllSpawnedCharacterElininated != null)
            {
                OnAllSpawnedCharacterElininated?.Invoke();
            }
            spawnedCharacters.Clear();
        }
    }


    public void SpawnCharacters()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        foreach(SpawnPoint point in spawnPointList)
        {
            if(point.enemyToSpawn != null)
            {
                GameObject spawnedGameObject = Instantiate(point.enemyToSpawn, point.transform.
                    position, point.transform.rotation);
                spawnedCharacters.Add(spawnedGameObject.GetComponent<CharacterBase>());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SpawnCharacters();
        }
    }
}
