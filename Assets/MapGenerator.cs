using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public GameObject WallPrefab;
    public List<Vector3Int> WallPositions;
    public int MaxSize;
    public GameObject GroundPrefab;

    public int MinSize;
    public int MaxWallParts;
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
            WallPart.transform.position = WallPositions[i];
        }

    }
}