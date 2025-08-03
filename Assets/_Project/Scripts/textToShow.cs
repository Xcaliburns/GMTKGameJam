using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class textToShow : MonoBehaviour
{
    public string text;
    public TextMeshProUGUI textMeshPro;
    public string loadsceneretry;
    public string loadscenecontinue;
    public string loadscenemainmenu;
    public Button[] buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
 
    }

    public void ShowText()
    {
        textMeshPro.text = text;
        textMeshPro.gameObject.SetActive(true);
        foreach (var item in buttons)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void Loadscenee(string nameofscene)
    {
        var ezezsssssss = Object.FindFirstObjectByType<ShiFuMi>();
        var player = Object.FindFirstObjectByType<PlayerController>();
        if (loadscenecontinue == "Save")
        {
            
                PlayerPrefs.SetInt("Sword", player.nbrSword);
            
            
                PlayerPrefs.SetInt("Magic", player.nbrMagic);
            
                PlayerPrefs.SetInt("Shield", player.nbrShield);
            
        }
           
        var dddd = Object.FindFirstObjectByType<DontDestroyCanva>();
        Destroy(dddd.gameObject);
        SceneManager.LoadSceneAsync(nameofscene);
    }
}
