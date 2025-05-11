using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sanc_GameManager : MonoBehaviour
{
    [Header("Object references")]
    public Transform sealDisplayParent;
    [SerializeField] private GameObject greySealPrefab;
    [SerializeField] private GameObject commonSealPrefab;
    [SerializeField] private Vector2[] poolSpawnLocations;
    //[SerializeField] private Vector2[] commonSealSpawnLocations;
    [SerializeField] private GameObject greyPool, commonPool;
    [SerializeField] private Slider hungerSlider, enrichmentSlider;
    [SerializeField] private GameObject enrichmentMGPrefab;
    public TextMeshProUGUI healthDisplay, hungerDisplay, enrichDisplay;
    [SerializeField] private Vector2[] poolPositions;
    [SerializeField] private GameObject[] poolButtons;

    [Header("Seal references")]
    public Seal_SancBehaviour selectedSeal;
    public List<Seal_SancBehaviour> seals = new List<Seal_SancBehaviour>();
    public int noOfEachSealType = 6;

    private Transform canvas;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    private Sanc_FeedAndEnrich feedEnrichManager;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        m_Raycaster = FindFirstObjectByType<GraphicRaycaster>();
        m_EventSystem = FindFirstObjectByType<EventSystem>();
        feedEnrichManager = GetComponent<Sanc_FeedAndEnrich>();

        LoadCollectedSeals();
    }

    void LoadCollectedSeals()
    {
        Debug.Log("Loading collected seals");
        HashSet<int> existingIDs = new HashSet<int>();
        foreach (Seal_SancBehaviour seal in seals)
        {
            existingIDs.Add(seal.myID);
        }
        foreach (int id in existingIDs)
        {
            Debug.Log("Existing ID found: " + id);
        }

        const int maxSeaSeals = 100;
        for (int id = 0; id < maxSeaSeals; id++)
        {
            if (PlayerPrefs.GetInt("SealCollected_" + id + "_", 0) == 1 && !existingIDs.Contains(id))
            {
                Debug.Log("Found seal");
                string key = "SealCollected_" + id + "_";

                string sealType = PlayerPrefs.GetString(key + "Type_", "Grey");
                int age = PlayerPrefs.GetInt(key + "Age_", 0);

                // instantiate
                GameObject newSealObj;
                if (sealType.Equals("Grey")) { newSealObj = Instantiate(greySealPrefab, greyPool.transform); Debug.Log("instantiating grey: sealType contained grey"); }
                else { newSealObj = Instantiate(commonSealPrefab, commonPool.transform); Debug.Log("instantiating common: sealType didn't contain grey"); }
                Seal_SancBehaviour seal = newSealObj.GetComponent<Seal_SancBehaviour>();
                RectTransform sealRect = newSealObj.GetComponent<RectTransform>();
                if (seal.sealType == "Grey" || seal.sealType == "Gray") { sealRect.localPosition = poolSpawnLocations[id]; }
                else if (seal.sealType == "Common") { sealRect.localPosition = poolSpawnLocations[id - noOfEachSealType]; }
                else { Debug.Log("Invalid seal type"); }
                newSealObj.name = seal.sealType + "_Seal_In_Sanc_" + id;

                // setvalues
                seal.myID = id;
                seal.sealType = sealType;
                seal.age = age;

                if (!seals.Contains(seal))
                {
                    seals.Add(seal);
                }
                seal.SaveAsCollected();
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
        if (Input.GetMouseButtonDown(2))
        {
            LogAllPlayerPrefs();
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
                healthDisplay.text = selectedSeal.health.ToString() + "%";
                hungerDisplay.text = selectedSeal.hunger.ToString() + "%";
                enrichDisplay.text = selectedSeal.enrichment.ToString() + "%";
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

    public void SpawnEnrichPrefab()
    {
        if (!feedEnrichManager.enrichObjActive) { Instantiate(enrichmentMGPrefab, canvas); }
        else { Debug.LogWarning("Enrich obj already active"); }
    }

    public void SwitchPools(bool toGreyPool)
    {
        Debug.Log("run switchpools");
        if (toGreyPool)
        {
            // set to grey pool button inactive
            // set grey pool position to on screen
            // set common pool position to off screen
            poolButtons[0].SetActive(false);
            poolButtons[1].SetActive(true);
            greyPool.transform.localPosition = poolPositions[0];
            commonPool.transform.localPosition = poolPositions[1];
        }
        else
        {
            poolButtons[0].SetActive(true);
            poolButtons[1].SetActive(false);
            greyPool.transform.localPosition = poolPositions[1];
            commonPool.transform.localPosition = poolPositions[0];
        }
    }

    public void LogAllPlayerPrefs()
    {
        Debug.Log("----- PlayerPrefs Contents -----");
        for (int id = 0; id < 100; id++)
        {
            string baseKey = "SealCollected_" + id + "_";
            if (PlayerPrefs.HasKey(baseKey))
            {
                Debug.Log($"{baseKey} = {PlayerPrefs.GetInt(baseKey)}");

                string typeKey = baseKey + "_Type";
                string ageKey = baseKey + "_Age";

                if (PlayerPrefs.HasKey(typeKey))
                    Debug.Log($"{typeKey} = {PlayerPrefs.GetString(typeKey)}");

                if (PlayerPrefs.HasKey(ageKey))
                    Debug.Log($"{ageKey} = {PlayerPrefs.GetInt(ageKey)}");
            }
        }
        Debug.Log("-----  End of PlayerPrefs -----");
    }
}
