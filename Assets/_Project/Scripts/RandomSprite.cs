using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public SpriteRenderer spriterenderer;
    public Sprite[] spritesrandom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var eze = Random.Range(0,spritesrandom.Length);
        spriterenderer.sprite = spritesrandom[eze];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
