using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Liste des salles possibles de ce type")]
    public GameObject[] prefabs;

    [Header("Options")]
    public float spawnDelay = 0.5f;
    public float destroyDelay = 5f;

  
    public async Task SpawnRoutine()
    {
      

        int randomIndex = Random.Range(0, prefabs.Length - 1);
        GameObject prefabToSpawn = prefabs[randomIndex];

        Instantiate(prefabToSpawn, transform.position, transform.rotation);

       

        //Debug.Log(gameObject.name + " détruit.");
        Destroy(gameObject);
    }
}
