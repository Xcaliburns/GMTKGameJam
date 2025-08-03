using UnityEngine;

public class MusicMenuManager : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[] musiquesMenu;
    public int numberOfMusicMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audiosource.resource = musiquesMenu[numberOfMusicMenu];
        audiosource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(audiosource.time == audiosource.clip.length)
        {
            audiosource.Stop();
            if(musiquesMenu.Length == 1)
            {
                numberOfMusicMenu = 0;
                audiosource.resource = musiquesMenu[numberOfMusicMenu];
                audiosource.Play();
                return;

            }
            else
            {
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

 
}
