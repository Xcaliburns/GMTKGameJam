using System.Collections.Generic;
using UnityEngine;

public class NextTilemap : MonoBehaviour
{
    public List<GameObject> tilePrefs = new List<GameObject>();
    public List<string> nextTiles;
    public string nextTileChoosed;
    public enum TypeOfTile {DoorUp,DoorDown,DoorRightUp,DoorUpAndDown,DoorRightUpDown,DoorLeftUpDown, DoorLeft,DoorRight,LeftNoIssue,RightNoIssue,UpNoIssue,DownNoIssue,DoorLeftAndRight, DoorUpAndRight, DoorUpAndLeft,CampFire,Begin };
    public TypeOfTile typeOfTile;
    public GameObject lastTile;
    public List<GameObject> tilesChoosed;
    public int RandomTileMap;
    public GameObject tileToSpawn;
    public List<GameObject> tilesNoIssue = new List<GameObject>();
    public enum ancientDoor { no,left,right, top, bottom };
    public ancientDoor ancientdoor;
    public LayerMask layerTilemap;
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
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   ||aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue
                   ||aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown);
                }
                else
                {
                    x = +18f;
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                }

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
            
                    
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + x, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();

                
                break;

             case TypeOfTile.DoorRightUpDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft 
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft|| aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + 18f, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown 
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y - 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                
               
                break;
             case TypeOfTile.DoorLeftUpDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft 
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight|| aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x - 18f, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown 
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y - 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                
               
                break;

            case TypeOfTile.DoorUpAndRight:
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18f, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => 
                     aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + 18f, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                break;

            case TypeOfTile.DoorRightUp:
                
                break;

                case TypeOfTile.DoorUpAndLeft:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18f, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x - 18f, transform.position.y), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }

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

                case TypeOfTile.DoorUpAndDown:
                
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y + 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight);

                    RandomTileMap = Random.Range(0, tilesChoosed.Count);
                    tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x, transform.position.y - 10f), Quaternion.identity);
                    tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
                    tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

                    tileToSpawn.GetComponent<NextTilemap>().enabled = true;
                    LevelGenerator.Instance.currentNumberOfTiles++;
                    tilesChoosed.Clear();
                }
                break;

        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
