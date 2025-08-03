using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class sceneChangeScipt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(string nameOfScene)
    {
        DavidUIManager.Instance.Retry();
        SceneManager.LoadSceneAsync(nameOfScene);
        
    }
}
