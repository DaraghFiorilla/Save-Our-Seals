using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnrichBlockDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // use left click input to select and drop in pool
    // on drop, use radius to detect seals nearby
    // create func in seal behaviour to go towards this obj
    // timer changing sprite based on time left
    // on timer = 0, enrich seals in radius, and set sanc_feedandenrich's enrichobjactive bool to false

    private GraphicRaycaster raycaster;
    private EventSystem ev;
    [SerializeField] private GameObject radiusDisplay;
    private Image radiusImage;
    private Vector2 startPos;
    public bool droppable;
    private Sanc_GameManager gameManager;

    void Awake()
    {
        raycaster = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GraphicRaycaster>();
        ev = FindFirstObjectByType<EventSystem>();
        radiusImage = radiusDisplay.GetComponent<Image>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        startPos = transform.position;
        droppable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        radiusDisplay.SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        radiusDisplay.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);
        bool isInsideSibling = RectTransformUtility.RectangleContainsScreenPoint(
            radiusImage.rectTransform,
            eventData.position,
            raycaster.eventCamera);
        int i = 0;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Seal_SancBehaviour>() && isInsideSibling)
            {
                Debug.Log("hit seal " + result.gameObject.name);
                i++;
            }
        }
        if (i == 0) { transform.position = startPos; }
        else { droppable = false; } // other behaviours here
        radiusDisplay.SetActive(false);*/
        Camera cam = raycaster.eventCamera;
        RectTransform radiusRect = radiusImage.rectTransform;
        int hitCount = 0;
        
        foreach (Seal_SancBehaviour seal in gameManager.seals)
        {
            RectTransform sealRect = seal.GetComponent<RectTransform>();

            Vector3 sealScreenPos = RectTransformUtility.WorldToScreenPoint(cam, sealRect.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(radiusRect, sealScreenPos, cam))
            {
                Debug.Log("Hit seal: " + seal.gameObject.name);
                hitCount++;
            }
        }

        if (hitCount == 0)
        {
            transform.position = startPos;
        }
        else
        {
            droppable = false;
        }
        radiusDisplay.SetActive(false);
    }

}
