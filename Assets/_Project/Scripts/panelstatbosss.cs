using TMPro;
using UnityEngine;

public class panelstatbosss : MonoBehaviour
{
    public TextMeshProUGUI statsbosssword, statsbossshield, statsbossmagic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        statsbosssword.text = "Sword Stats : " + PlayerPrefs.GetInt("Sword").ToString();
        statsbossshield.text = "Shield Stats : " + PlayerPrefs.GetInt("Shield").ToString();
        statsbossmagic.text = "Magic Stats : " + PlayerPrefs.GetInt("Magic").ToString();
    }
}
