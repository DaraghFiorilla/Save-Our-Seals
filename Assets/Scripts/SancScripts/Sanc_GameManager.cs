using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sanc_GameManager : MonoBehaviour
{
    public List<Seal_SancBehaviour> seals = new List<Seal_SancBehaviour>();

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public Transform sealDisplayParent;
    [SerializeField] private GameObject sealPrefab;
    [SerializeField] private Vector2[] spawnLocations;
    [SerializeField] private Slider hungerSlider, enrichmentSlider;
    public Seal_SancBehaviour selectedSeal;
    private Transform canvas;

    public int availableFish;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindFirstObjectByType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindFirstObjectByType<EventSystem>();

        LoadCollectedSeals();
    }

    void LoadCollectedSeals()
    {
        HashSet<int> existingIDs = new HashSet<int>();
        foreach (Seal_SancBehaviour seal in seals)
        {
            existingIDs.Add(seal.myID);
        }

        const int maxSeaSeals = 100;
        for (int id = 0; id < maxSeaSeals; id++)
        {
            if (PlayerPrefs.GetInt("SealCollected_" + id, 0) == 1 && !existingIDs.Contains(id))
            {
                string key = "SealCollected_" + id;

                string sealType = PlayerPrefs.GetString(key + "_Type", "Grey");
                int age = PlayerPrefs.GetInt("SealCollected_" + id + "_Age", 0);

                // instantiate
                GameObject newSealObj = Instantiate(sealPrefab, canvas);
                Seal_SancBehaviour seal = newSealObj.GetComponent<Seal_SancBehaviour>();
                RectTransform sealRect = newSealObj.GetComponent<RectTransform>();
                sealRect.anchoredPosition = spawnLocations[id];
                newSealObj.name = "Seal_In_Sanc_" + id;

                // setvalues
                seal.myID = id;
                seal.sealType = sealType;
                seal.age = age;

                if (!seals.Contains(seal))
                {
                    seals.Add(seal);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DisplaySeal();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelect();
        }
    }

    public void DisplaySeal()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Seal_SancBehaviour>() != null)
            {
                ClearSelect();
                selectedSeal = result.gameObject.GetComponent<Seal_SancBehaviour>();
                selectedSeal.selected = true;
                sealDisplayParent.GetChild(0).gameObject.SetActive(true);
                sealDisplayParent.GetChild(0).GetComponent<Image>().sprite = selectedSeal.fullSprite;
                sealDisplayParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Age: " + selectedSeal.age.ToString();
                sealDisplayParent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Type: " + selectedSeal.sealType[0].ToString();
                sealDisplayParent.Find("HealthBG/HealthInputText").GetComponent<TextMeshProUGUI>().text = selectedSeal.health.ToString() + "%";
                sealDisplayParent.Find("HungerBG/HungerInputText").GetComponent<TextMeshProUGUI>().text = selectedSeal.hunger.ToString() + "%";
                sealDisplayParent.Find("EnrichmentBG/EnrichmentInputText").GetComponent<TextMeshProUGUI>().text = selectedSeal.health.ToString() + "%";
                selectedSeal.selectionOutline.SetActive(true);
            }
        }
    }

    void ClearSelect()
    {
        foreach (Seal_SancBehaviour seal in seals)
        {
            seal.selected = false;
            seal.selectionOutline.SetActive(false);
        }
        sealDisplayParent.GetChild(0).GetComponent<Image>().sprite = null;
        sealDisplayParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.Find("HealthBG/HealthInputText").GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.Find("HungerBG/HungerInputText").GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.Find("EnrichmentBG/EnrichmentInputText").GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.GetChild(0).gameObject.SetActive(false);
        hungerSlider.value = 0;
        enrichmentSlider.value = 0;
    }
}
