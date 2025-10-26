using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.AI.Navigation;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject WallContainer;
    public List<Vector3Int> WallPositions;
    public int MaxSize;
    public List<GameObject> Items;
    public GameObject ItemContainer;
    public int MinSize;
    public int MaxWallParts;
    public GameObject GroundObj;
    public GameObject GroundStableContainer;
    public NavMeshSurface navMeshSurface;
    public GameObject GroundObjekt;
    public StartManager startManager;
    public int TimeLoding;
    public int BlockCount;
    public int Build;
    private int s;
    public List<Vector3> PositionsWall;
    public List<GameObject> Enemys;
    public bool SpawnEnemy1;
    public bool SpawnEnemy2;
    public int MaxEnemys;

    public void StartLevel()
    {
        TimeLoding = 1;
        startManager.MapIsDone = false;
        Delte();
    }
    public void Delte()
    {
        for (int i = 0; i < GroundStableContainer.transform.childCount; i++)
        {
            Destroy(GroundStableContainer.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < ItemContainer.transform.childCount; i++)
        {
            Destroy(ItemContainer.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < WallContainer.transform.childCount; i++)
        {
            Destroy(WallContainer.transform.GetChild(i).gameObject);
        }
        WallPositions.Clear();
        GetPositions();
    }

    public void GetPositions()
    {
        for (int M = 0; M < MaxWallParts; M++)
        {
            bool FoundPos = false;
            while (!FoundPos)
            {
                int X = Random.Range(MinSize, MaxSize);
                int Y = 0;
                int Z = Random.Range(MinSize, MaxSize);
                if (X == 0 || X == 1 || X == -1)
                {
                    X = 10;
                }
                if (Z == 0 || Z == 1 || Z == -1)
                {
                    Z = 10;
                }
                Vector3Int WallPos = new Vector3Int(X, Y, Z);
                if (!WallPositions.Contains(WallPos))
                {
                    WallPositions.Add(WallPos);
                    FoundPos = true;
                }
            }
        }
        StartCoroutine(SpawnWalls());
    }
    IEnumerator SpawnWalls()
    {
        for (int i = 0; i < WallPositions.Count; i++)
        {
            GameObject WallPart = Instantiate(WallPrefab);
            WallPart.transform.SetParent(WallContainer.transform);
            WallPart.transform.position = WallPositions[i];
            yield return null;
        }
        float start = -MaxSize;
        float end = MaxSize;

        // Untere und obere Linie
        for (int x = (int)start; x <= end; x++)
        {
            PositionsWall.Add(new Vector3(x, 0, start)); // unten
            PositionsWall.Add(new Vector3(x, 0, end));   // oben
            yield return null;
        }
        // Linke und rechte Linie
        for (int z = (int)start; z <= end; z++)
        {
            PositionsWall.Add(new Vector3(start, 0, z)); // links
            PositionsWall.Add(new Vector3(end, 0, z));   // rechts
            yield return null;
        }
        // Jetzt alle Wände spawnen
        foreach (Vector3 pos in PositionsWall)
        {
            GameObject wall = Instantiate(WallPrefab, pos, Quaternion.identity);
            wall.transform.SetParent(WallContainer.transform);
            yield return null;
        }
        StartCoroutine(ItemPlacment());
    }
    IEnumerator ItemPlacment()
    {
        s = Random.Range(10, 100);
        for (int i = 0; i < s; i++)
        {
            int n = Random.Range(0, Items.Count);
            GameObject Item = Instantiate(Items[n]);
            Item.transform.SetParent(ItemContainer.transform);
            bool FoundPos = false;
            while (!FoundPos)
            {
                int x = Random.Range(MinSize, MaxSize);
                int z = Random.Range(MinSize, MaxSize);
                Vector3Int Position = new Vector3Int(x, 2, z);
                if (!WallPositions.Contains(Position))
                {
                    Item.transform.position = Position;
                    FoundPos = true;
                }
            }
            yield return null;
        }
        StartCoroutine(BlockPlacer());
    }
    IEnumerator BlockPlacer()
    {
        BlockCount = Random.Range(50, 250);
        for (int i = 0; i < BlockCount; i++)
        {
            int x = Random.Range(MinSize, MaxSize);
            int z = Random.Range(MinSize, MaxSize);
            Vector3Int Pos = new Vector3Int(x, 0, z);
            if (!WallPositions.Contains(Pos))
            {
                GameObject GroundPrefab = Instantiate(GroundObj);
                GroundPrefab.transform.SetParent(GroundStableContainer.transform);
                GroundPrefab.transform.position = Pos;
            }
            else
            {
                BlockCount += 1;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        TimeLoding = 100;
        yield return new WaitForSeconds(0.4f);
        navMeshSurface.BuildNavMesh();
        GroundObjekt.SetActive(false);
        startManager.MapIsDone = true;
    }
    void Update()
    {
        float WallPosTime = (float)WallContainer.transform.childCount / 4 / MaxWallParts;
        float WallTime = (float)WallPositions.Count / MaxWallParts;
        float ItemTime = (float)ItemContainer.transform.childCount / Mathf.Max(s, 1);
        float BlockTime = (float)GroundStableContainer.transform.childCount / Mathf.Max(BlockCount, 1);

        float avg = (WallPosTime + WallTime + ItemTime + BlockTime) / 4f;

        int targetTime = Mathf.Clamp(Mathf.RoundToInt(avg * 100f), 0, 100);
        if (TimeLoding != 100)
        {
            TimeLoding = (int)Mathf.Lerp(TimeLoding, targetTime, Time.deltaTime * 5f);
        }
    }
}
