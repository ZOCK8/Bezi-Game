using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [CustomEditor(typeof(MapGenerator))]

    void Start()
    {
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
        SpawnWalls();
    }
    void SpawnWalls()
    {
        for (int i = 0; i < WallPositions.Count; i++)
        {
            GameObject WallPart = Instantiate(WallPrefab);
            WallPart.transform.SetParent(WallContainer.transform);
            WallPrefab.transform.SetParent(WallContainer.transform);
            WallPart.transform.position = WallPositions[i];
        }
        ItemPlacment();
    }
    void ItemPlacment()
    {
        int s = Random.Range(10, 100);
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
        }
        BlockPlacer();
    }
    void BlockPlacer()
    {
        int BlockCount = Random.Range(50, 250);
        for (int i = 0; i < BlockCount; i++)
        {
            int x = Random.Range(MinSize, MaxSize);
            int z = Random.Range(MinSize, MaxSize);
            Vector3 Pos = new Vector3(x, 0, z);
            GameObject GroundPrefab = Instantiate(GroundObj);
            GroundPrefab.transform.SetParent(GroundStableContainer.transform);
            GroundPrefab.transform.position = Pos;

        }
    }
}
