using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEditor;

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
    [SerializeField] private TextMeshProUGUI releasedSealText;
    [SerializeField] private GameObject gameEndPrefab;
    public AudioClip[] audioFiles;

    [Header("Seal references")]
    public Seal_SancBehaviour selectedSeal;
    public List<Seal_SancBehaviour> seals = new List<Seal_SancBehaviour>();
    public int noOfEachSealType = 6;
    public int releasedSeals;
    public int releasedSealsEndTarget;
    public string[] names = new string[12];

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
        if (!PlayerPrefs.HasKey("Released_")) { SaveRescueNo(); }
        else { releasedSeals = PlayerPrefs.GetInt("Released_"); }
        releasedSealText.text = "Seals released: " + releasedSeals.ToString() + " / " + releasedSealsEndTarget.ToString();
        if (!PlayerPrefs.HasKey("GameComplete_"))
        {
            PlayerPrefs.SetInt("GameComplete_", 0);
            PlayerPrefs.Save();
        }
    }

    void LoadCollectedSeals()
    {
        //Debug.Log("Loading collected seals");
        HashSet<int> existingIDs = new HashSet<int>();
        foreach (Seal_SancBehaviour seal in seals)
        {
            existingIDs.Add(seal.myID);
        }
        /*foreach (int id in existingIDs)
        {
            Debug.Log("Existing ID found: " + id);
        }*/

        const int maxSeaSeals = 100;
        for (int id = 0; id < maxSeaSeals; id++)
        {
            if (PlayerPrefs.GetInt("SealCollected_" + id + "_", 0) == 1 && !existingIDs.Contains(id))
            {
                //Debug.Log("Found seal");
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
                sealDisplayParent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedSeal.myName;
                sealDisplayParent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Type: " + selectedSeal.sealType;
                healthDisplay.text = selectedSeal.health.ToString() + "%";
                hungerDisplay.text = selectedSeal.hunger.ToString() + "%";
                enrichDisplay.text = selectedSeal.enrichment.ToString() + "%";
                selectedSeal.selectionOutline.SetActive(true);
            }
        }
    }

    public void UpdateRescueNo()
    {
        //Debug.Log("Updating rescue no");
        releasedSeals++;
        releasedSealText.text = "Seals released: " + releasedSeals.ToString() + "/" + releasedSealsEndTarget.ToString();
        SaveRescueNo();
        if (releasedSeals >= releasedSealsEndTarget)
        {
            Instantiate(gameEndPrefab, canvas);
        }
    }

    public void EndGame() // button ref 
    {
        PlayerPrefs.SetInt("GameComplete_", 1);
        PlayerPrefs.Save();
    }

    void SaveRescueNo()
    {
        PlayerPrefs.SetInt("Released_", releasedSeals);
        PlayerPrefs.Save();
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
        //else { Debug.LogWarning("Enrich obj already active"); }
    }

    public void SwitchPools(bool toGreyPool)
    {
        //Debug.Log("run switchpools");
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

    public void SaveAndExit() // button ref
    {
        SaveRescueNo();
        foreach (Seal_SancBehaviour seal in seals)
        {
            seal.SaveAsCollected();
        }
        //EditorApplication.ExitPlaymode();
        Application.Quit();
    }

    public bool TrashComplete()
    {
        if (PlayerPrefs.GetInt("TrashComplete_") == 1)
        {
            //Debug.Log("TrashComplete_ == 1");
            return true;
        }
        else { /*Debug.Log("TrashComplete) != 1");*/ return false; }
    }
}
