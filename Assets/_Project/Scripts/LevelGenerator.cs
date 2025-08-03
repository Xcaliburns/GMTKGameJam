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
    public GameObject uiLoadingScreen;
    public GameObject player;
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

            for (int i = 0; i < 10; i++)
            {
                await VerifySuperpositionsTilemap();
                await verifytilemap();
                await verifyNoIssues();

            }
            await DetectNoIssueFarRoom();
            await SpawnEnemysInRooms();
            player.GetComponent<PlayerController>().blockinputs = false;
            Destroy(uiLoadingScreen);
            SpawnIsFinish = true;
            isSpawning = false; // optionnel ici, car ça ne doit plus rerentrer de toute façon
        }
    }

    public async Task SpawnEnemysInRooms()
    {

        var tilemapsspawned = Object.FindObjectsByType<RandomSpawner>(FindObjectsSortMode.None).ToList();
        foreach (var t in tilemapsspawned)
        {
           await t.SpawnRoutine();
        }
        await Task.Yield();
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
    public async Task DetectNoIssueFarRoom()
    {
        spawnLastRoomBoss = true;
        List<NextTilemap> transformList = Object.FindObjectsByType<NextTilemap>(FindObjectsSortMode.None).ToList();
       
        listtilesnoissue = transformList.FindAll(t => t.typeOfTile == NextTilemap.TypeOfTile.LeftNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.RightNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.UpNoIssue
        || t.typeOfTile == NextTilemap.TypeOfTile.DownNoIssue);

        if (listtilesnoissue.Count == 0)
        {
            Debug.LogWarning("Aucune salle NoIssue trouvée pour placer la séquence du boss");
            await Task.Yield();
            return;
        }

        listtilesnoissue.Sort((a, b) =>
            Vector2.Distance(beginRoom.position, a.transform.position)
            .CompareTo(Vector2.Distance(beginRoom.position, b.transform.position)));

        var lasttilenoissue = listtilesnoissue.Last();
        if (lasttilenoissue == null)
        {
            Debug.LogError("La dernière salle NoIssue est null");
            await Task.Yield();
            return;
        }

        Debug.Log($"Salle boss: tentative de placement à partir de {lasttilenoissue.name} de type {lasttilenoissue.typeOfTile}");
        
        // Sauvegarde de la position et du type pour l'utiliser après la destruction
        Vector3 tilePosition = lasttilenoissue.transform.position;
        NextTilemap.TypeOfTile tileType = lasttilenoissue.typeOfTile;
        bool roomDestroyed = false;
        
        // Vérification vers la droite
        if (!Physics2D.OverlapCircle(new Vector2(tilePosition.x + 18+9, tilePosition.y), 1, tilemapLayer))
        {
            if(tileType == NextTilemap.TypeOfTile.UpNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x + 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(tilePosition.x + 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à droite (UpNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.DownNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorRightAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x + 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(tilePosition.x + 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à droite (DownNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x + 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(tilePosition.x + 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à droite (LeftNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.RightNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x + 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossLeft, new Vector2(tilePosition.x + 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à droite (RightNoIssue)");
                return;
            }
        }
        
        // Vérification vers la gauche
        if (!Physics2D.OverlapCircle(new Vector2(tilePosition.x - 18-9, tilePosition.y), 1, tilemapLayer))
        {
            if (tileType == NextTilemap.TypeOfTile.UpNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndLeft);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x - 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(tilePosition.x - 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à gauche (UpNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.DownNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x - 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(tilePosition.x - 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à gauche (DownNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x - 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(tilePosition.x - 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à gauche (LeftNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.RightNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampLeft, new Vector2(tilePosition.x - 18, tilePosition.y), Quaternion.identity);
                ee = Instantiate(prefBossRight, new Vector2(tilePosition.x - 18*2, tilePosition.y), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée à gauche (RightNoIssue)");
                return;
            }
        }
        
        // Vérification vers le haut
        if (!Physics2D.OverlapCircle(new Vector2(tilePosition.x, tilePosition.y + 10 + 5), 1, tilemapLayer))
        {
            if (tileType == NextTilemap.TypeOfTile.UpNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(tilePosition.x, tilePosition.y + 10 + 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en haut (UpNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.DownNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(tilePosition.x, tilePosition.y + 10 + 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en haut (DownNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndLeft);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(tilePosition.x, tilePosition.y + 10 + 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en haut (LeftNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.RightNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndRight);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y + 10), Quaternion.identity);
                ee = Instantiate(prefBossDown, new Vector2(tilePosition.x, tilePosition.y + 10 + 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en haut (RightNoIssue)");
                return;
            }
        }
        
        // Vérification vers le bas
        if (!Physics2D.OverlapCircle(new Vector2(tilePosition.x, tilePosition.y - 10 - 5), 1, tilemapLayer))
        {
            if (tileType == NextTilemap.TypeOfTile.UpNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(tilePosition.x, tilePosition.y - 10 - 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en bas (UpNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.DownNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorUpAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(tilePosition.x, tilePosition.y - 10 - 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en bas (DownNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.LeftNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorLeftAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(tilePosition.x, tilePosition.y - 10 - 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en bas (LeftNoIssue)");
                return;
            }
            if (tileType == NextTilemap.TypeOfTile.RightNoIssue)
            {
                DestroyImmediate(lasttilenoissue.gameObject);
                roomDestroyed = true;
                
                var dssd = tilePrefs.FindAll(t => t.GetComponent<NextTilemap>().typeOfTile == NextTilemap.TypeOfTile.DoorRightAndDown);
                var ee = Instantiate(dssd[0], tilePosition, Quaternion.identity);
                Destroy(ee.GetComponent<NextTilemap>());
                ee = Instantiate(prefFireCampUp, new Vector2(tilePosition.x, tilePosition.y - 10), Quaternion.identity);
                ee = Instantiate(prefBossUp, new Vector2(tilePosition.x, tilePosition.y - 10 - 10), Quaternion.identity);
                
                Debug.Log("Séquence du boss créée en bas (RightNoIssue)");
                return;
            }
        }
        
        // Si aucune condition n'a été satisfaite mais que la salle existe toujours
        if (!roomDestroyed && lasttilenoissue != null)
        {
            Debug.LogWarning("Impossible de placer la séquence du boss dans une direction, destruction de la salle NoIssue");
            DestroyImmediate(lasttilenoissue.gameObject);
        }
        
        await Task.Yield();
    }
    private void OnDrawGizmos()
    {
        
    }
}
