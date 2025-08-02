using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;
    public List<GameObject> tilePrefs = new List<GameObject>();
    public int numberOfTilesTarget, currentNumberOfTiles;
    public bool SpawnIsFinish;
    [HideInInspector] public List<NextTilemap> nextTilemapsSpawned = new List<NextTilemap>();
    public Transform beginRoom;
    public List<NextTilemap> listtilesnoissue = new List<NextTilemap>();
    public GameObject prefFireCampLeft, prefFireCampUp;
    public GameObject prefBossLeft,prefBossRight,prefBossDown,prefBossUp;
    public LayerMask tilemapLayer;
    public bool spawnLastRoomBoss, isSpawning;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    async void Update()
    {
        if (Instance == null) Instance = this;

        if (currentNumberOfTiles >= numberOfTilesTarget && !SpawnIsFinish && !isSpawning)
        {
            isSpawning = true;

            for (int i = 0; i < 50; i++)
            {
                await VerifySuperpositionsTilemap();
                await verifytilemap();
                await verifyNoIssues();

            }

            SpawnIsFinish = true;
            isSpawning = false; // optionnel ici, car ça ne doit plus rerentrer de toute façon
        }
    }
    public async Task VerifySuperpositionsTilemap()
    {
        nextTilemapsSpawned.Clear();
        nextTilemapsSpawned = Object.FindObjectsByType<NextTilemap>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < nextTilemapsSpawned.Count; i++)
        {
            var a = nextTilemapsSpawned[i];
            for (int j = i + 1; j < nextTilemapsSpawned.Count; j++)
            {
                var b = nextTilemapsSpawned[j];
                if (Mathf.Approximately(a.transform.position.x, b.transform.position.x) &&
                    Mathf.Approximately(a.transform.position.y, b.transform.position.y))
                {
                    Destroy(b.gameObject);
                    nextTilemapsSpawned.RemoveAt(j);
                    j--; // car la liste a été raccourcie
                }
            }
        }
        await Task.Yield(); // Assure que la méthode est asynchrone
    }
    public async Task verifyNoIssues()
    {
        nextTilemapsSpawned.Clear();
        nextTilemapsSpawned = Object.FindObjectsByType<NextTilemap>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < nextTilemapsSpawned.Count; i++)
        {
            if (nextTilemapsSpawned[i] != null)
            {
                await nextTilemapsSpawned[i].spawnNoIssues();
            }

        }
    }
    public async Task verifytilemap()
    {
        nextTilemapsSpawned.Clear();
        nextTilemapsSpawned = Object.FindObjectsByType<NextTilemap>(FindObjectsSortMode.None).ToList();

        for (int i = 0; i < nextTilemapsSpawned.Count; i++)
        {
            if (nextTilemapsSpawned[i] != null)
            {
                await nextTilemapsSpawned[i].VerifyIfHaveWall();
            }

        }
        await Task.Yield();
    }
    public void DetectNoIssueFarRoom()
    {
        spawnLastRoomBoss = true;
        List<NextTilemap> transformList = Object.FindObjectsByType<NextTilemap>(FindObjectsSortMode.None).ToList();
       
        listtilesnoissue = transformList.FindAll(t => t.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue);

        listtilesnoissue.Sort((a, b) =>
    Vector2.Distance(beginRoom.position, a.transform.position)
    .CompareTo(Vector2.Distance(beginRoom.position, b.transform.position)));

        var lasttilenoissue = listtilesnoissue.Last();
        if (!Physics2D.OverlapCircle(new Vector2(lasttilenoissue.transform.position.x + 18+9, lasttilenoissue.transform.position.y),1, tilemapLayer))
        {
            if(lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndRight);
                var ee = Instantiate(dssd[0],lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x + 18, lasttilenoissue.transform.position.y),Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(lasttilenoissue.transform.position.x + 18*2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorRightAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x + 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(lasttilenoissue.transform.position.x + 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x + 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(lasttilenoissue.transform.position.x + 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x + 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(lasttilenoissue.transform.position.x + 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
        }
        if (!Physics2D.OverlapCircle(new Vector2(lasttilenoissue.transform.position.x - 18-9, lasttilenoissue.transform.position.y),1, tilemapLayer))
        {
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndLeft);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x - 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(lasttilenoissue.transform.position.x - 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x - 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(lasttilenoissue.transform.position.x - 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x - 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(lasttilenoissue.transform.position.x - 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(lasttilenoissue.transform.position.x - 18, lasttilenoissue.transform.position.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(lasttilenoissue.transform.position.x - 18 * 2, lasttilenoissue.transform.position.y), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f); ;
                
                return;
            }
        }
        if (!Physics2D.OverlapCircle(new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10 + 5),1, tilemapLayer))
        {
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y +10 ), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10 + 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10 + 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndLeft);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10 + 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndRight);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(lasttilenoissue.transform.position.x , lasttilenoissue.transform.position.y + 10 + 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
        }
        if (!Physics2D.OverlapCircle(new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y-10-5),1, tilemapLayer))
        {
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y -10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y-10-10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y - 10 - 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y-10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(lasttilenoissue.transform.position.x , lasttilenoissue.transform.position.y - 10 - 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject,0.1f);
                
                return;
            }
            if (lasttilenoissue.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue)
            {
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorRightAndDown);
                var ee = Instantiate(dssd[0], lasttilenoissue.transform.position, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(lasttilenoissue.transform.position.x, lasttilenoissue.transform.position.y - 10 - 10), Quaternion.identity);
                Destroy(lasttilenoissue.gameObject, 0.1f);

                return;
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        
    }
}
