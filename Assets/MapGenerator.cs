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
    public float Time;
    public int BlockCount;
    public int Build;
    private int s;

    [CustomEditor(typeof(MapGenerator))]

    void Start()
    {
        Time = 1;
        startManager.MapIsDone = false;
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
            WallPrefab.transform.SetParent(WallContainer.transform);
            WallPart.transform.position = WallPositions[i];
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
        navMeshSurface.BuildNavMesh();
        GroundObjekt.SetActive(false);
        startManager.MapIsDone = true;
    }
    void Update()
    {
        float WallTime = MaxWallParts / WallContainer.transform.childCount;
        float ItemTime = s / ItemContainer.transform.childCount;
        float BlockTime = BlockCount / GroundStableContainer.transform.childCount;
        Time = ((WallTime / ItemTime) + (WallTime / BlockTime)) / 2f * 100f;

    }
}
