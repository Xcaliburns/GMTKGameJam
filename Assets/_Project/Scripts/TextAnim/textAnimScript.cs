
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textAnimScript : MonoBehaviour
{
    public List<string> textLines = new List<string>();
    public TextMeshProUGUI textUI;
    public float timeBetweenLines = 1f;
    private float timer; 
    private int currentLineIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = textLines[currentLineIndex];
        if(timer < timeBetweenLines )
        {
            timer = Mathf.MoveTowards(timer, timeBetweenLines, 1f * Time.deltaTime);
        }
        else if( timer >= timeBetweenLines )
        {
            if(currentLineIndex < textLines.Count-1)
            {
                currentLineIndex++;
            }
            else if(currentLineIndex == textLines.Count -1)
            {
                currentLineIndex = 0;
            }
            
            timer = 0;
        }
    }
}
