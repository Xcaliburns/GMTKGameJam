using UnityEngine;

public class MusicMenuManager : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[] musiquesMenu;
    public int numberOfMusicMenu;
    public static MusicMenuManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
           instance  = this;
        }
        if (musiquesMenu.Length == 1)
        {
            audiosource.loop = true;
        }
        audiosource.resource = musiquesMenu[numberOfMusicMenu];
        audiosource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (audiosource.time == audiosource.clip.length)
        {
            audiosource.Stop();
          
                if (numberOfMusicMenu == musiquesMenu.Length - 1)
                {
                    numberOfMusicMenu = 0;
                    audiosource.resource = musiquesMenu[numberOfMusicMenu];
                    audiosource.Play();
                    return;
                }
                else
                {
                    numberOfMusicMenu++;
                    audiosource.resource = musiquesMenu[numberOfMusicMenu];
                    audiosource.Play();
                    return;
                }
        }
    }

 
}
