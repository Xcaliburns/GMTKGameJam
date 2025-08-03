using UnityEngine;
using System.Collections;

public class princess : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            var obj = Object.FindFirstObjectByType<panelstatbosss>();
            obj.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var obj = Object.FindFirstObjectByType<panelstatbosss>();
            obj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
