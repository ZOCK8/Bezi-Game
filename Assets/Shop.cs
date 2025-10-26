using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    public Button TaschenLampe;
    public Button Energy;
    public Button Health;
    public PlayerManager playerManager;
    public Light light;
    void Start()
    {
        TaschenLampe.onClick.AddListener(() => Lampe());
        Energy.onClick.AddListener(() => EnergyAdd());
        Health.onClick.AddListener(() => HealthAdd());
    }

    // Update is called once per frame
    void Lampe()
    {
        light.intensity += 0.5f;
    }
    void EnergyAdd()
    {
        if (playerManager.Money > 1)
        {
            playerManager.Money -= 1;
            playerManager.Energy += 10;
        }
    }
    void HealthAdd()
    {
        if (playerManager.Money > 1)
        {
            playerManager.Money -= 1;
            playerManager.Health += 1;
        }

    }
}
