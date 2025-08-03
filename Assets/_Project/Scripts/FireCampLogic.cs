using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireCampLogic : MonoBehaviour
{
    public int tradesLeft;
    public PlayerController player;
    public Button buttonSword;
    public Button buttonMagic;
    public Button buttonShield;
    public TextMeshProUGUI texttradesleft;
    public GameObject panel;
    public static FireCampLogic instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        buttonSword.onClick.AddListener(() => Trade("sword"));
        buttonMagic.onClick.AddListener(() => Trade("magic"));
        buttonShield.onClick.AddListener(() => Trade("shield"));
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        texttradesleft.text = $"{tradesLeft}/3";
    }

    public void Trade(string nameofStat)
    {
        if (tradesLeft == 0) { return; }

        if(nameofStat == "sword" && player.nbrSword > 0)
        {
            player.nbrMagic++;
            player.nbrSword--;
            tradesLeft--;
            return;
        }
        if(nameofStat == "magic"&& player.nbrMagic > 0)
        {
            player.nbrShield++;
            player.nbrMagic--;
            tradesLeft--;
            return;
        }
        if(nameofStat == "shield"&& player.nbrShield > 0)
        {
            player.nbrSword++;
            player.nbrShield--;
            tradesLeft--;
            return;
        }
        
    }

   


}
