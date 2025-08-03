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
        SceneManager.LoadSceneAsync(nameofscene);
    }
}
