using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;

public class DisableAndActivateEnemys : MonoBehaviour
{
    public List <EnemyComponent> enemys = new List<EnemyComponent>();
    public CinemachineCamera cambrain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cambrain = GetComponentInChildren<CinemachineCamera>();
        var dddd = Object.FindObjectsByType<EnemyComponent>(FindObjectsSortMode.None).ToList();
        enemys = dddd.FindAll(t => t.transform.root == this.transform);
        disableEnemys();
    }

    // Update is called once per frame
    void Update()
    {
        if (cambrain.IsLive)
        {
            if(GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.CampFire)
            {
               MusicMenuManager.instance.audiosource.enabled = false;
            }
            else
            {
                MusicMenuManager.instance.audiosource.enabled = true;
                if(!MusicMenuManager.instance.audiosource.isPlaying)
                {
                    MusicMenuManager.instance.audiosource.Play();
                }
            }
            activateEnemys();
        }
        else
        {
            disableEnemys();
        }
    }
 
    public void disableEnemys()
    {
        
        foreach (EnemyComponent enemy in enemys)
        {
            if(enemy != null)
            {
                enemy.gameObject.SetActive(false);
            }
            
        }
    }

    public void activateEnemys()
    {
        foreach (EnemyComponent enemy in enemys)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
}
