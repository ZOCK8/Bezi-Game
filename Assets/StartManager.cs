using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public GameObject Map;
    public MapGenerator mapGenerator;
    public GameObject StartLodingMap;
    public TextMeshProUGUI PercentText;
    public List<GameObject> AktivateAfterMap;
    public bool MapIsDone;
    void Start()
    {
        for (int i = 0; i < AktivateAfterMap.Count; i++)
        {
            AktivateAfterMap[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        PercentText.text = mapGenerator.TimeLoding.ToString();
        if (MapIsDone)
        {
            for (int i = 0; i < AktivateAfterMap.Count; i++)
            {
                AktivateAfterMap[i].SetActive(true);
            }
            StartLodingMap.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
