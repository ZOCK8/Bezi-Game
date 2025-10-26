using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject ItemConatainer;
    public List<BoxCollider> ItemColliders;
    public BoxCollider PlayerCollider;
    public PlayerManager playerManager;
    IEnumerator Start()
    {
        yield return new UnityEngine.WaitUntil(() => ItemConatainer.transform.childCount != 0);
        for (int i = 0; i < ItemConatainer.transform.childCount; i++)
        {
            ItemColliders.Add(ItemConatainer.transform.GetChild(i).GetComponent<BoxCollider>());
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pumpkin"))
        {
            BoxCollider collectedCollider = other.GetComponent<BoxCollider>();

            if (collectedCollider != null && ItemColliders.Contains(collectedCollider))
            {
                playerManager.Money += 1;
                ItemColliders.Remove(other.GetComponent<BoxCollider>());
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Energy"))
        {
            BoxCollider collectedCollider = other.GetComponent<BoxCollider>();

            if (collectedCollider != null && ItemColliders.Contains(collectedCollider))
            {
                playerManager.Energy += 15;
                ItemColliders.Remove(other.GetComponent<BoxCollider>());
                Destroy(other.gameObject);
            }
        }
    }
}
