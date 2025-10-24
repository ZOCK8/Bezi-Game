using UnityEngine;

public class HitBoxScribt : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            GhostHunter enemyHunter = other.GetComponent<GhostHunter>();

            if (enemyHunter != null) 
            {
                if (!enemyHunter.InPlayerRange)
                {
                    enemyHunter.InPlayerRange = true;
                    Debug.Log("ACHTUNG: Enemy (" + other.gameObject.name + ") hat den Bereich betreten!");
                }
            }
            else
            {
                Debug.LogWarning("Enemy-Objekt hat den Tag 'Enemy', aber NICHT das GhostHunter-Skript!");
            }
        }
    }
}