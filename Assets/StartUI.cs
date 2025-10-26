using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public GameObject StartUIGameObject;
    [Header("Buttons")]
    public Button StartButton;
    public Button StartLevel;
    public Button CloseLevelSelect;
    public Button ZOCK;
    public Button Laura;
    public Button Hihidu;
    public Button AboutUS;
    public Button AboutUsClose;
    [Header("Random")]
    public GameObject LevelButtonContainer;
    public List<GameObject> LevelButtons;
    public List<Sprite> LevelLocked;
    public List<Sprite> LevelUnLocked;
    [Header("Scripts")]
    public PlayerManager playerManager;
    public MapGenerator mapGenerator;
    [Header("UI Parts")]
    public GameObject LevelSelector;
    public GameObject AboutUsUI;
    public Slider slider;
    public TextMeshProUGUI MousSensitivy;
    public GameObject GhostRotate;

    void Start()
    {
        // 1. Level-Buttons sammeln (wie im Original-Skript)
        for (int i = 0; i < LevelButtonContainer.transform.childCount; i++)
        {
            LevelButtons.Add(LevelButtonContainer.transform.GetChild(i).gameObject);
        }

        // 2. Allgemeine Listener hinzufügen
        StartLevel.onClick.AddListener(() => StartCoroutine(Starting()));
        StartButton.onClick.AddListener(() => LevelSelector.SetActive(true));
        CloseLevelSelect.onClick.AddListener(() => LevelSelector.SetActive(false));

        ZOCK.onClick.AddListener(() => Application.OpenURL("https://github.com/ZOCK8"));
        Laura.onClick.AddListener(() => Application.OpenURL("https://040"));
        Hihidu.onClick.AddListener(() => Application.OpenURL("https://040"));
        AboutUS.onClick.AddListener(() => AboutUsUI.SetActive(true));
        AboutUsClose.onClick.AddListener(() => AboutUsUI.SetActive(false));

        // 3. Level-Button-Listener HIER (in Start) hinzufügen - NUR EINMAL
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            // Wichtig: Verwenden Sie eine lokale Variable im Loop, um i zu "fangen"
            int index = i; 
            LevelButtons[index].GetComponent<Button>().onClick.AddListener(() => 
            { 
                SelectLevel(LevelButtons[index]); 
                Debug.Log(LevelButtons[index].name); 
            });
        }
    }

    IEnumerator Starting()
    {
        yield return new WaitForSeconds(0.7f);
        mapGenerator.StartLevel();
        StartUIGameObject.SetActive(false);
    }

    void SelectLevel(GameObject LevelButton)
    {
        // Überprüfen Sie immer, ob das Level freigeschaltet ist
        if (LevelButton.name.StartsWith("UnLocked"))
        {
            switch (LevelButton.name)
            {
                case "UnLocked0":
                    mapGenerator.MaxSize = 50;
                    mapGenerator.MaxWallParts = 100;
                    mapGenerator.BlockCount = 100;
                    mapGenerator.SpawnEnemy1 = true;
                    mapGenerator.SpawnEnemy2 = false;
                    mapGenerator.MaxEnemys = 1;
                    break;
                case "UnLocked1":
                    mapGenerator.MaxSize = 125;
                    mapGenerator.MaxWallParts = 225;
                    mapGenerator.BlockCount = 110;
                    mapGenerator.SpawnEnemy1 = true;
                    mapGenerator.SpawnEnemy2 = false;
                    mapGenerator.MaxEnemys = 3;
                    break;
                case "UnLocked2":
                    mapGenerator.MaxSize = 175;
                    mapGenerator.MaxWallParts = 140;
                    mapGenerator.BlockCount = 115;
                    mapGenerator.SpawnEnemy1 = true;
                    mapGenerator.SpawnEnemy2 = true;
                    mapGenerator.MaxEnemys = 4;
                    break;
                case "UnLocked3":
                    mapGenerator.MaxSize = 200;
                    mapGenerator.MaxWallParts = 160;
                    mapGenerator.BlockCount = 125;
                    mapGenerator.SpawnEnemy1 = true;
                    mapGenerator.SpawnEnemy2 = true;
                    mapGenerator.MaxEnemys = 6;
                    break;
            }
        }
        else
        {
            Debug.Log("Level ist noch gesperrt!");
            // Optional: Visuelles oder akustisches Feedback geben
        }
    }

    // Update wird einmal pro Frame ausgeführt
    void Update()
    {
        // ✅ UI-Aktualisierungen für Slider/Sensitivität
        float value = slider.value / 4f; // Korrekte Gleitkomma-Division
        float cleanValue = Mathf.Round(value * 100f) / 100f; // Bessere Methode zur "Bereinigung"
        MousSensitivy.text = (value * 100f).ToString("F0") + "%"; // Zeigt gerundet den Prozentwert
        playerManager.mouseSensitivity = slider.value;


        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * playerManager.mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * playerManager.mouseSensitivity * Time.deltaTime;
            Vector3 currentEuler = GhostRotate.transform.localEulerAngles;

            float newYRotation = currentEuler.y - mouseY;
            float newXRotation = currentEuler.x;

            GhostRotate.transform.localRotation = Quaternion.Euler(newXRotation, newYRotation, currentEuler.z);
            GhostRotate.transform.Rotate(Vector3.up * mouseX, Space.World);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        for (int i = 0; i < LevelButtons.Count; i++)
        {
            if (i < LevelUnLocked.Count && i < LevelLocked.Count)
            {
                if (i <= playerManager.Levels) 
                {
                    LevelButtons[i].GetComponent<Image>().sprite = LevelUnLocked[i];
                    LevelButtons[i].name = "UnLocked" + i;
                    LevelButtons[i].transform.GetChild(0).GetComponent<Text>().text = "Level " + (i + 1);
                    LevelButtons[i].GetComponent<Button>().interactable = true;
                }
                else 
                {
                    LevelButtons[i].GetComponent<Image>().sprite = LevelLocked[i];
                    LevelButtons[i].name = "Locked" + i;
                    LevelButtons[i].transform.GetChild(0).GetComponent<Text>().text = "Locked";
                    LevelButtons[i].GetComponent<Button>().interactable = false; 
                }
            }
        }

    }
}