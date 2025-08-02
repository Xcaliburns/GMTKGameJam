using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Liste des salles possibles de ce type")]
    public GameObject[] prefabs;

    [Header("Options")]
    public float spawnDelay = 0.5f;
    public float destroyDelay = 5f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        int randomIndex = Random.Range(0, prefabs.Length - 1);
        GameObject prefabToSpawn = prefabs[randomIndex];

        Instantiate(prefabToSpawn, transform.position, transform.rotation);

        yield return new WaitForSeconds(destroyDelay);

        //Debug.Log(gameObject.name + " détruit.");
        Destroy(gameObject);
    }
}
