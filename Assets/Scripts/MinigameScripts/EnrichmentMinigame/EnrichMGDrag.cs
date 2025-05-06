using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnrichMGDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Tooltip("Valid tags: IceCube, Fish, Toy, Seaweed, Tray")]public string type;
    [HideInInspector] public bool? isInPlace;
    private GraphicRaycaster m_Raycaster;
    //private Image myImage;
    private EnrichmentMinigame minigameManager;

    void Awake()
    {
        minigameManager = FindFirstObjectByType<EnrichmentMinigame>();
        m_Raycaster = FindFirstObjectByType<GraphicRaycaster>();
        if (type == "IceCube")
        {
            isInPlace = null;
        }
        else
        {
            isInPlace = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (type == "Tray")
        {
            if (minigameManager.slotsFilled >= 3) { gameObject.transform.position = Input.mousePosition; }
        }
        else if (isInPlace != true) { gameObject.transform.position = Input.mousePosition; }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (type == "IceCube")
        {
            // Check to see if this obj is within ice box boundaries
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "IceBox")
                {
                    return;
                }
            }
            Destroy(gameObject);
        }
        else if (type != "Tray")
        {
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "IceTray")
                {
                    // play click audio
                    isInPlace = true;
                    transform.SetParent(result.gameObject.transform.Find(type + "BG"));
                    // fish offset due to its sprite shape
                    if (type == "Fish") { transform.localPosition = new Vector3(13, 0, 0); }
                    else { transform.localPosition = Vector3.zero; }
                    minigameManager.FillSlot();
                }
            }
        }
        else
        {
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Freezer>() != null)
                {
                    minigameManager.tutorialArrow.SetActive(false);
                    result.gameObject.GetComponent<Freezer>().StartFreezer();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
