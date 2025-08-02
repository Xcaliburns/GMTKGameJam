using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject creditsText;
    private Vector2 creditsTextBasePos;
    public Slider slidervolume;
    public string nameOfScenePlay;
    public Button buttonPlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonPlay.onClick.AddListener(() => LoadScenePlay(nameOfScenePlay));
        if(!PlayerPrefs.HasKey("GlobalVolume"))
        {
            PlayerPrefs.SetFloat("GlobalVolume", 1);
        }
        slidervolume.value = PlayerPrefs.GetFloat("GlobalVolume");

        creditsTextBasePos = (Vector2)creditsText.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume");

        if(creditsPanel.activeInHierarchy)
        {
            creditsText.GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(creditsText.transform.localPosition, new Vector2(creditsText.transform.localPosition.x, 1259), 50 * Time.deltaTime);
        }
        else
        {
            creditsText.transform.position = creditsTextBasePos;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeSliderValue()
    {
        PlayerPrefs.SetFloat("GlobalVolume", slidervolume.value);
    }

    public void OpenSettingsMenu()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void OpenCreditsMenu()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void BackToMainMenu()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void LoadScenePlay(string nameOfScene)
    {
        SceneManager.LoadSceneAsync(nameOfScene);
    }
}
