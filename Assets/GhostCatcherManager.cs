using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GhostCatcherManager : MonoBehaviour
{
    public NavMeshSurface navs;
    public List<NavMeshAgent> agents;
    public MapGenerator mapGenerator;
    public PlayerManager playerManager;
    void Start()
    {
        navs.BuildNavMesh();
        for (int i = 0; i < transform.childCount; i++)
        {
            agents.Add(transform.GetChild(i).GetComponent<NavMeshAgent>());
        }
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].gameObject.GetComponent<GhostHunter>().playerManager = playerManager;
            bool FoundPos = false;
            int x = Random.Range(mapGenerator.MinSize, mapGenerator.MaxSize);
            int y = 1;
            int z = Random.Range(mapGenerator.MinSize, mapGenerator.MaxSize);
            Vector3 Pos = new Vector3(x, y, z);
            Vector3 richtung = Vector3.down;
            RaycastHit hit;
            agents[i].destination = Pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i].remainingDistance < 1 && !agents[i].GetComponent<GhostHunter>().InPlayerRange)
            {
                int x = Random.Range(mapGenerator.MinSize, mapGenerator.MaxSize);
                int y = 1;
                int z = Random.Range(mapGenerator.MinSize, mapGenerator.MaxSize);
                Vector3 Pos = new Vector3(x, y, z);
                Vector3 richtung = Vector3.down;
                RaycastHit hit;
                if (!Physics.Raycast(Pos, richtung, out hit, 0.9f))
                {
                    agents[i].destination = Pos;
                }
            }
            if (agents[i].GetComponent<GhostHunter>().InPlayerRange)
            {
                agents[i].destination = playerManager.PlayerPosition.position;
            }
        }
    }
}
