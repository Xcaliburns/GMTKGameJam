using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

public class NextTilemap : MonoBehaviour
{
    public List<GameObject> tilePrefs = new List<GameObject>();
    public List<string> nextTiles;
    public string nextTileChoosed;
    public enum TypeOfTile {DoorUp,DoorDown,DoorRightUp,DoorUpAndDown,DoorRightUpDown,DoorLeftUpDown, DoorLeft,DoorRight,LeftNoIssue,RightNoIssue
            ,UpNoIssue,DownNoIssue,DoorLeftAndRight, DoorUpAndRight, DoorUpAndLeft,CampFire,Begin,DoorLeftAndDown, DoorRightAndDown,DoorUpLeftRight,DoorDownLeftRight,DoorAllDirections};
    public TypeOfTile typeOfTile;
    public GameObject lastTile;
   [HideInInspector] public List<GameObject> tilesChoosed;
    public int RandomTileMap;
    public GameObject tileToSpawn;
    public List<GameObject> tilesNoIssue = new List<GameObject>();
    public enum ancientDoor { no,left,right, top, bottom };
    public ancientDoor ancientdoor;
    public LayerMask layerTilemap,layerWallTileMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
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
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                { // agamar t es ko
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight 
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft|| aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(+18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight || 
                    aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections
                   );
                    await testBeforeSpawnTilemap(-18, 0, new List<GameObject>(tilesChoosed));
                }


                break;

             case TypeOfTile.DoorRightUpDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(+18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, 10, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, -10, new List<GameObject>(tilesChoosed));
                }
                
               
                break;
             case TypeOfTile.DoorRightAndDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(+18, 0, new List<GameObject>(tilesChoosed));
                }
                
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, -10, new List<GameObject>(tilesChoosed));
                }
                
               
                break;
             case TypeOfTile.DoorLeftUpDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight ||
                     aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(-18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, 10, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, -10, new List<GameObject>(tilesChoosed));
                }
                
               
                break;
             case TypeOfTile.DoorLeftAndDown:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight ||
                     aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(-18, 0, new List<GameObject>(tilesChoosed));
                }
               
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, -10, new List<GameObject>(tilesChoosed));
                }
                
               
                break;

            case TypeOfTile.DoorUpAndRight:
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                  || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                  || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, 10, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18f, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                     || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(18, 0, new List<GameObject>(tilesChoosed));
                }
                break;

            case TypeOfTile.DoorRightUp:
                
                break;

                case TypeOfTile.DoorUpAndLeft:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown
                  || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, 10, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18f, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight ||
                  aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight 
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(-18f, 0, new List<GameObject>(tilesChoosed));
                }

                break;

                case TypeOfTile.LeftNoIssue:

                break;
                case TypeOfTile.RightNoIssue:

                break;

                case TypeOfTile.Begin:
                await spawnfirstTilemaps();

                break;

                case TypeOfTile.CampFire:

                break;

                case TypeOfTile.DoorUpAndDown:
                
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz =>  aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(0, 10,new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                   await testBeforeSpawnTilemap(0,-10, new List<GameObject>(tilesChoosed));
                }
                break;
                case TypeOfTile.DoorUpLeftRight:
                
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz =>  aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                 || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight);
                    await testBeforeSpawnTilemap(0, 10,new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x+18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                   await testBeforeSpawnTilemap(18,0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x-18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                   await testBeforeSpawnTilemap(-18,0, new List<GameObject>(tilesChoosed));
                }
                break;

                case TypeOfTile.DoorDownLeftRight:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(-18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                   await testBeforeSpawnTilemap(0,-10, new List<GameObject>(tilesChoosed));
                }
                break;
                case TypeOfTile.DoorAllDirections:

                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                    await testBeforeSpawnTilemap(-18, 0, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                      || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUp
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
                   await testBeforeSpawnTilemap(0,-10, new List<GameObject>(tilesChoosed));
                }
                if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1f, layerTilemap))
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown
              || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown
                || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections
                || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight);
                   await testBeforeSpawnTilemap(0,-10, new List<GameObject>(tilesChoosed));
                }
                break;

        
        }
    }

    public async Task spawnfirstTilemaps()
    {
        tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeft || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);

        RandomTileMap = Random.Range(0, tilesChoosed.Count);
        tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x + 18f, transform.position.y), Quaternion.identity);
        tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
        tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;

        tileToSpawn.GetComponent<NextTilemap>().enabled = true;
        LevelGenerator.Instance.currentNumberOfTiles++;
        LevelGenerator.Instance.nextTilemapsSpawned.Add(tileToSpawn.GetComponent<NextTilemap>());
        tilesChoosed.Clear();

        

        tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRight
       || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight
       || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight
                    || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight
                   || aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorAllDirections);
        RandomTileMap = Random.Range(0, tilesChoosed.Count);
        tileToSpawn = Instantiate(tilesChoosed[RandomTileMap], new Vector2(transform.position.x - 18f, transform.position.y), Quaternion.identity);
        tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.left;
        tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
        tileToSpawn.GetComponent<NextTilemap>().enabled = true;
        LevelGenerator.Instance.currentNumberOfTiles++;
    }
    public async Task testBeforeSpawnTilemap(float x, float y, List<GameObject> localTilesChoosed)
    {
      

        bool foundValidTile = false;
        int randomIndex = 0;
        while (!foundValidTile )
        {
            
             randomIndex = Random.Range(0, localTilesChoosed.Count);
            GameObject tile = localTilesChoosed[randomIndex];
            NextTilemap tileScript = tile.GetComponent<NextTilemap>();

            Vector2 origin = new Vector2(transform.position.x + x, transform.position.y + y);
            bool canSpawn = false;

            switch (tileScript.typeOfTile)
            {
                case TypeOfTile.DoorLeftUpDown:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap);
                    break;

                case TypeOfTile.DoorRightUpDown:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap);
                    break;

                case TypeOfTile.DoorUpAndDown:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap);
                    break;

                case TypeOfTile.DoorLeftAndRight:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;

                case TypeOfTile.DoorUpAndLeft:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap);
                    break;

                case TypeOfTile.DoorUpAndRight:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;
                case TypeOfTile.DoorRightAndDown:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;

                case TypeOfTile.DoorLeftAndDown:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap);
                    break;
                case TypeOfTile.DoorDownLeftRight:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;
                case TypeOfTile.DoorUpLeftRight:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;
                case TypeOfTile.DoorAllDirections:
                    canSpawn = !Physics2D.Raycast(origin, Vector2.up, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.down, 7, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.left, 10, layerWallTileMap) &&
                               !Physics2D.Raycast(origin, Vector2.right, 10, layerWallTileMap);
                    break;

                // Ajoute ici d'autres cas si tu en as besoin

                default:
                    break;
            }

            if (canSpawn)
            {
                foundValidTile = true;
            }

            await Task.Yield(); // attend une frame
        }
        if(LevelGenerator.Instance.currentNumberOfTiles >= LevelGenerator.Instance.numberOfTilesTarget)
        {
            return;
        }

        // Spawn final après avoir trouvé une tile valide
        tileToSpawn = Instantiate(localTilesChoosed[randomIndex], new Vector2(transform.position.x + x, transform.position.y + y), Quaternion.identity);
        tileToSpawn.GetComponent<NextTilemap>().lastTile = this.gameObject;
        tileToSpawn.GetComponent<NextTilemap>().ancientdoor = ancientDoor.right;
        tileToSpawn.GetComponent<NextTilemap>().enabled = true;
        LevelGenerator.Instance.currentNumberOfTiles++;
        LevelGenerator.Instance.nextTilemapsSpawned.Add(tileToSpawn.GetComponent<NextTilemap>());

    }

    public async Task VerifyIfHaveWall()
    {
        switch (typeOfTile)
        {
            case TypeOfTile.DoorAllDirections:
               bool left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
               bool right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                bool up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
               bool down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (left && up && down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && up && !down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && !down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && !down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && !up && down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpLeftRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && up && !down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && !down && !right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightUpDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && !up && down && right)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && up && !down && right)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && !up && !down && right)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftUpDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && !down && right)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && !down && right)
                {
                    Debug.Log("Replace to DoorLeftAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && down && right)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && down && right)
                {
                    Debug.Log("Replace to DoorLeftAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorDownLeftRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                
                break;
            case TypeOfTile.DoorLeftAndDown:
                 left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                 down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (!left  && down)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left  && !down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(left && down)
                {
                    Destroy(gameObject);
                }
                break;
            case TypeOfTile.DoorLeftUpDown:
                 left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                 up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                 down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if(!left && !up && down)
                {
                    Debug.Log("Replace to DoorUpAndLeft");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(left && up && !down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && !down)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && !down)
                {
                    Debug.Log("Replace to DoorLeftAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!left && up && down)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up && down)
                {
                    Debug.Log("Replace to DoorLeftAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
               if(left && up && down)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorRightUpDown:
                 right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                 up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                 down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (right && !up && !down)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && !up && down)
                {
                    Debug.Log("Replace to DoorUpAndRight");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && up && !down)
                {
                    Debug.Log("Replace to DoorRightAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && up && !down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && !up && down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && up && down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(right && up && down)
                {
                    Destroy(gameObject);
                }
                break;
            case TypeOfTile.DoorRightAndDown:
                 right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                 
                 down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (right  && !down)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && down)
                {
                    Debug.Log("Replace to DoorUpAndRight");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(right && down)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorUpAndDown:
                up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (!up && down)
                {
                    Debug.Log("Replace to UpNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (up && !down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (up && down)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorLeftAndRight:
                left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                if (!left && right)
                {
                    Debug.Log("Replace to LeftNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !right)
                {
                    Debug.Log("Replace to RightNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(left && right)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorUpAndLeft:

                 left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                 up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                if (!left && up )
                {
                    Debug.Log("Replace to LeftNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && !up)
                {
                    Debug.Log("Replace to UpNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (left && up)
                {
                    Destroy(gameObject);
                }   
                break;

            case TypeOfTile.DoorUpAndRight:
                right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                if (!right && up)
                {
                    Debug.Log("Replace to LeftNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && !up)
                {
                    Debug.Log("Replace to UpNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(right && up)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorDownLeftRight:
                 right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);

                if (right && !left && !down)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && !left && down)
                {
                    Debug.Log("Replace to DoorUpAndRight");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && left && !down)
                {
                    Debug.Log("Replace to DoorRightAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && left && !down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && !left && down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && left && down)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(right && left && down)
                {
                    Destroy(gameObject);
                }
                break;

            case TypeOfTile.DoorUpLeftRight:
                right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);

                if (right && !left && !up)
                {
                    Debug.Log("Replace to DoorUpAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorUpAndLeft);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && !left && up)
                {
                    Debug.Log("Replace to DoorUpAndRight");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorLeftAndRight);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && left && !up)
                {
                    Debug.Log("Replace to DoorRightAndDown");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DoorRightAndDown);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && left && !up)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (right && !left && up)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if (!right && left && up)
                {
                    Debug.Log("Replace to DownNoIssue");
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                if(right && left && up)
                {
                    Destroy(gameObject);
                }
                break;
            // Ajoute ici d'autres cas si tu en as besoin
            case TypeOfTile.LeftNoIssue:
                left = Physics2D.Raycast(transform.position, Vector2.left, 10, layerWallTileMap);
                if (left)
                { 
                    Destroy(gameObject);
                }
                break;
            case TypeOfTile.RightNoIssue:
                right = Physics2D.Raycast(transform.position, Vector2.right, 10, layerWallTileMap);
                if (right)
                { 
                    Destroy(gameObject);
                }
                break;
            case TypeOfTile.UpNoIssue:
                up = Physics2D.Raycast(transform.position, Vector2.up, 7, layerWallTileMap);
                if (up)
                { 
                    Destroy(gameObject);
                }

                break;
            case TypeOfTile.DownNoIssue:
                down = Physics2D.Raycast(transform.position, Vector2.down, 7, layerWallTileMap);
                if (down)
                { 
                    Destroy(gameObject);
                }

                break;
            default:
                break;
        }
        await Task.Yield();
    }
    bool left = false;
    bool up = false;
    bool down = false;
    bool right = false;
    public async Task spawnNoIssues()
    {
        print("yyoyoyoyoyyo");
        switch (typeOfTile)
        {
            case TypeOfTile.DoorAllDirections:
                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);
                up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);
                if (left && up && down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                if (left && up && !down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y-10), Quaternion.identity);
                }
                if (left && !up && down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                }
                if (!left && up && down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (!left && !up && down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                }
                if (left && !up && !down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y-10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                }
                if (left && !up && !down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y-10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                }
                if (!left && up && !down && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                     ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y-10), Quaternion.identity);
                }
                if (!left && !up && !down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y-10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                }
              
                //possibilités sans right
                if (!left && !up && down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (left && up && !down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);

                }
                if (left && !up && !down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (!left && up && !down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (!left && up && down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !up && down && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;
            case TypeOfTile.DoorLeftUpDown:
                 left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                 up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                 down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);

                if (!left && !up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18,transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (left && up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    
                }
                if (left && !up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                     ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (!left && up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y -10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                     ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x -18, transform.position.y), Quaternion.identity);
                }
                if (!left && up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x -18, transform.position.y), Quaternion.identity);
                }
                if (left && !up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorRightUpDown:
                 right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);
                 up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                 down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);

                if (right && !up && !down)
                {
                    
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x , transform.position.y -10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x , transform.position.y+10 ), Quaternion.identity);
                   
                }
                if (!right && !up && down)
                {
                    
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y+10), Quaternion.identity);
                    
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                     ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);

                }
                if (!right && up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                     ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                }
                if (!right && up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                if (right && up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                }
                if (right && !up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorUpAndDown:
                 up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                 down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);

                if (!up && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (up && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorLeftAndRight:
                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);
                if (!left && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorUpAndLeft:

                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                if (!left && up)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !up)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorUpAndRight:
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);
                up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                if (!right && up)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                if (right && !up)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorLeftAndDown:
                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);
                if (!left && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                }
                break;
            case TypeOfTile.DoorRightAndDown:
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);
                down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);
                if (!right && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                if (right && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                }
                break;
            case TypeOfTile.DoorDownLeftRight:
                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                down = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 10), 1, layerTilemap);
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x+18, transform.position.y), 1, layerTilemap);

                if (!left && !right && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x +18, transform.position.y), Quaternion.identity);
                }
                if (left && right && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);

                }
                if (left && !right && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x +18, transform.position.y), Quaternion.identity);
                }
                if (!left && right && !down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.UpNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y - 10), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (!left && right && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !right && down)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                }
                break;

            case TypeOfTile.DoorUpLeftRight:
                left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 18, transform.position.y), 1, layerTilemap);
                up = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 10), 1, layerTilemap);
                right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 18, transform.position.y), 1, layerTilemap);

                if (!left && !up && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (left && up && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x +18, transform.position.y), Quaternion.identity);

                }
                if (left && !up && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                if (!left && up && !right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.LeftNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x + 18, transform.position.y), Quaternion.identity);
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (!left && up && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.RightNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x - 18, transform.position.y), Quaternion.identity);
                }
                if (left && !up && right)
                {
                    tilesChoosed = tilePrefs.FindAll(aaz => aaz.GetComponent<NextTilemap>().typeOfTile == TypeOfTile.DownNoIssue);
                    var ee = Instantiate(tilesChoosed[0], new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
                }
                break;
            // Ajoute ici d'autres cas si tu en as besoin

            default:
                break;
        }
        await Task.Yield();
    }

    public async Task NoIssueProblem()
    {

        if(typeOfTile == TypeOfTile.RightNoIssue)
        {
            if(Physics2D.Raycast(transform.position, Vector2.right,10,layerWallTileMap))
            {
                Destroy(gameObject);
            }
        }
        if(typeOfTile == TypeOfTile.LeftNoIssue)
        {
            if(Physics2D.Raycast(transform.position, Vector2.left,10,layerWallTileMap))
            {
                Destroy(gameObject);
            }
        }
        if(typeOfTile == TypeOfTile.UpNoIssue)
        {
            if(Physics2D.Raycast(transform.position, Vector2.up,7,layerWallTileMap))
            {
                Destroy(gameObject);
            }
        }
        if(typeOfTile == TypeOfTile.DownNoIssue)
        {
            if(Physics2D.Raycast(transform.position, Vector2.down,7,layerWallTileMap))
            {
                Destroy(gameObject);
            }
        }
        
        await VerifyIfHaveWall();
        await Task.Yield();
    }
}
