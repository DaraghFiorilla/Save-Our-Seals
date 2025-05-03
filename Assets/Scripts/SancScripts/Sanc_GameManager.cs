using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sanc_GameManager : MonoBehaviour
{
    private TimeManager timeManager;
    public Seal_SancBehaviour[] seals;
    public Seal_SancBehaviour selectedSeal;
    //private Vector3 mousePos;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    [SerializeField] private Transform sealDisplayParent;

    private void Awake()
    {
        timeManager = GetComponent<TimeManager>();
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindFirstObjectByType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindFirstObjectByType<EventSystem>();
    }

    private void Update()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            /*Collider2D targetObj = Physics2D.OverlapPoint(mousePos);
            if (targetObj != null)
            {
                selectedSeal = targetObj.gameObject.GetComponent<Seal_SancBehaviour>();
                Debug.Log("raycast sent, received collider from" + targetObj);
            }
            else { Debug.Log("No collider found at position " + Input.mousePosition); }*/

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count <= 0) { ClearSelect(); }

            foreach (RaycastResult result in results)
            {
                //Debug.Log("Hit " + result.gameObject.name);
                if (result.gameObject.GetComponent<Seal_SancBehaviour>() != null)
                {
                    selectedSeal = result.gameObject.GetComponent<Seal_SancBehaviour>();
                    ClearSelect();
                    selectedSeal.selected = true;
                    sealDisplayParent.GetChild(0).gameObject.SetActive(true);
                    sealDisplayParent.GetChild(0).GetComponent<Image>().sprite = selectedSeal.fullSprite;
                    sealDisplayParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Health: " + selectedSeal.health + "%";
                    sealDisplayParent.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Hunger: " + selectedSeal.hunger + "%";
                    sealDisplayParent.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Enrichment: " + selectedSeal.enrichment + "%";
                }
                /*else
                {
                    ClearSelect();
                }*/
            }
        }
    }

    void ClearSelect()
    {
        foreach (Seal_SancBehaviour seal in seals) { seal.selected = false; }
        sealDisplayParent.GetChild(0).GetComponent<Image>().sprite = null;
        sealDisplayParent.GetChild(0).gameObject.SetActive(false);
        sealDisplayParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.GetChild(2).GetComponent<TextMeshProUGUI>().text = null;
        sealDisplayParent.GetChild(3).GetComponent<TextMeshProUGUI>().text = null;
    }
}
