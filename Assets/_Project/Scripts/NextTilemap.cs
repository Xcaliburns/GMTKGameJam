using System.Collections.Generic;
using UnityEngine;

public class NextTilemap : MonoBehaviour
{
    public List<GameObject> tilePrefs = new List<GameObject>();
    public List<string> nextTiles;
    public string nextTileChoosed;
    public enum TypeOfTile {DoorUp,DoorDown,DoorUpAndDown,DoorRightUpDown, DoorLeft,DoorRight,LeftNoIssue,RightNoIssue,UpNoIssue,DownNoIssue,DoorLeftAndRight, DoorUpAndRight, DoorUpAndLeft,CampFire,Begin };
    public TypeOfTile typeOfTile;
    public GameObject lastTile;
    public List<GameObject> tilesChoosed;
    public int RandomTileMap;
    public GameObject tileToSpawn;
    public List<GameObject> tilesNoIssue = new List<GameObject>();
    public enum ancientDoor { left,right, top, bottom };
    public ancientDoor ancientdoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (LevelGenerator.Instance.currentNumberOfTiles >= LevelGenerator.Instance.numberOfTilesTarget)
        {
            return;
        }
        switch(typeOfTile)
        {
            case TypeOfTile.DoorLeft:
                tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                break;

                case TypeOfTile.DoorRight:

                break;

                case TypeOfTile.DoorLeftAndRight:
                float x = 0;
                float y = 0;
                if (lastTile.transform.position.x > transform.position.x)
                {
                    x = -18f;
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight||aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                }
                else
                {
                    x = +18f;
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                }

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    while (tilesChoosed[RandomTileMap].GetComponent<NextTilemap>().typeOfTile == this.typeOfTile)
                    {
                        RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    }
                    
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + x, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();

                
                break;

             case TypeOfTile.DoorRightUpDown:
                // pour la porte du haut
                bool havenoissue = false;
                tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
               || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
               || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);

                RandomTileMap = Random.Range(0, tilesChoosed.Count);
                while (tilesChoosed[RandomTileMap].GetComponent<NextTilemap>().typeOfTile == this.typeOfTile)
                {
                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                }
                if(tilesChoosed[RandomTileMap].GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue)
                {
                    havenoissue = true;
                }
                tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                LevelGenerator.Instance.currentNumberOfTiles++;
                tilesChoosed.Clear();
                // pour la porte du bas
                
                tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
               || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
               || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                if(havenoissue == false)
                {
                    tilesNoIssue = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                }
                foreach(var tile in tilesNoIssue)
                {
                    tilesChoosed.Add(tile);
                }
                RandomTileMap = Random.Range(0, tilesChoosed.Count);
                while (tilesChoosed[RandomTileMap].GetComponent<NextTilemap>().typeOfTile == this.typeOfTile)
                {
                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                }

                tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                LevelGenerator.Instance.currentNumberOfTiles++;
                tilesChoosed.Clear();

                break;

            case TypeOfTile.DoorUpAndRight:
                
                break;

                case TypeOfTile.DoorUpAndLeft:

                break;

                case TypeOfTile.LeftNoIssue:

                break;
                case TypeOfTile.RightNoIssue:

                break;

                case TypeOfTile.Begin:
                tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                   
                     RandomTileMap = Random.Range(0, tilesChoosed.Count);
                tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + 18f,transform.position.y),Quaternion.identity);
                tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;
                
                tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                LevelGenerator.Instance.currentNumberOfTiles++;
                tilesChoosed.Clear();
                tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRight 
               || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight);
                RandomTileMap = Random.Range(0, tilesChoosed.Count);
                tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x - 18f, transform.position.y), Quaternion.identity);
                tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.left;
                tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                LevelGenerator.Instance.currentNumberOfTiles++;

                break;

                case TypeOfTile.CampFire:

                break;

              


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
