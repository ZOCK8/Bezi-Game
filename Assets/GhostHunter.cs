using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GhostHunter : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    public PlayerManager playerManager;
    public bool InPlayerRange;
    private Animator EnemyAniamtor;
    public GameObject GroundObj;
    public GameObject GroundContainer;
    private bool BlockWalk;
    private NavMeshAgent agent;
    void Start()
    {
        InPlayerRange = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
        BlockWalk = false;
        EnemyAniamtor = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }
    void Update()
    {
        float Distance = Vector3.Distance(transform.position, playerManager.PlayerPosition.position);
        if (Distance <= 20)
        {
            Debug.Log("InPlayerRange");
            InPlayerRange = true;
        }
        if (GetComponent<NavMeshAgent>().velocity.magnitude < 0.1f && !BlockWalk)
        {
            EnemyAniamtor.SetBool("Walk", true);
        }
        else
        {
            InPlayerRange = false;
        }

        if (Distance <= 3f)
        {
            StartCoroutine(Attack());
        }
        Vector3 richtung = Vector3.down;
        RaycastHit hit;
        Vector3Int Pos = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z);
        bool positionTaken = playerManager.Grounds.Any(ground => ground != null && ground.transform.position == Pos);
        if (!Physics.Raycast(Pos, richtung, out hit, 0.9f) && !positionTaken)
        {
            GameObject GroundPrefab = Instantiate(GroundObj);
            GroundPrefab.transform.SetParent(GroundContainer.transform);
            GroundPrefab.transform.position = Pos;
            playerManager.Grounds.Add(GroundPrefab);
        }
    }
    void LateUpdate()
    {
        Vector3 newAgentPosition = agent.nextPosition;

        transform.position = new Vector3(newAgentPosition.x, 1, newAgentPosition.z);

        agent.nextPosition = transform.position;
    }
    IEnumerator Attack()
    {
        Debug.Log("Attack");

        BlockWalk = true;
        EnemyAniamtor.SetBool("Walk", false);
        EnemyAniamtor.Play("Attacking");
        playerManager.CameraAnimator.Play("Shaking");
        yield return new WaitForSeconds(0.8f);
        Debug.Log("-1 Heart");
        BlockWalk = false;
        EnemyAniamtor.SetBool("Walk", true);
    }
}
