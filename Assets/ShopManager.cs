using Unity.Mathematics;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameObject Shop;
    public PlayerManager playerManager;
    public GameObject SpeachBubbel;
    public MapGenerator mapGenerator;
    public GameObject UIShop;
    void Start()
    {
        UIShop.SetActive(false);
        Shop = gameObject;
        GameObject GroundPrefab = Instantiate(mapGenerator.GroundObj);
        GroundPrefab.transform.SetParent(mapGenerator.GroundStableContainer.transform);
        Vector3 Pos = new Vector3Int((int)Shop.transform.position.x, 0, (int)Shop.transform.position.z);
        GroundPrefab.transform.position = Pos;
    }

    // Update is called once per frame
    void Update()
    {
        float Distance = Vector3.Distance(playerManager.PlayerPosition.position, Shop.transform.position);
        if (Distance < 3)
        {
            SpeachBubbel.SetActive(true);
            Quaternion rotation = Quaternion.Euler(0, playerManager.PlayerPosition.rotation.eulerAngles.y - 180f, 0);
            Shop.transform.rotation = rotation;
            if (Input.GetKeyDown(KeyCode.E) && UIShop.activeSelf)
            {
                UIShop.SetActive(false);
                Debug.Log("Activated UI");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                UIShop.SetActive(true);
            }
        }
        else
        {
            SpeachBubbel.SetActive(false);
        }
    }
}
