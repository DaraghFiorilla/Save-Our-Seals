using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
    [SerializeField] private Sprite[] sprites;
    private Image myImage;
    public int enrichAmount;

    void Awake()
    {
        raycaster = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GraphicRaycaster>();
        ev = FindFirstObjectByType<EventSystem>();
        radiusImage = radiusDisplay.GetComponent<Image>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        startPos = transform.position;
        droppable = true;
        myImage = GetComponent<Image>();
        myImage.sprite = sprites[0];
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
        List<Seal_SancBehaviour> seals = new List<Seal_SancBehaviour>();

        foreach (Seal_SancBehaviour seal in gameManager.seals)
        {
            RectTransform sealRect = seal.GetComponent<RectTransform>();

            Vector3 sealScreenPos = RectTransformUtility.WorldToScreenPoint(cam, sealRect.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(radiusRect, sealScreenPos, cam))
            {
                Debug.Log("Hit seal: " + seal.gameObject.name);
                seals.Add(seal);
                hitCount++;
            }
        }
        radiusDisplay.SetActive(false);
        if (hitCount == 0)
        {
            transform.position = startPos;
            return;
        }
        else
        {
            droppable = false;
            foreach (Seal_SancBehaviour seal in seals)
            {
                StartCoroutine(seal.EnrichmentCoroutine(seal.enrichCooldown - 5, 5, enrichAmount));
            }
        }
        StartCoroutine(BreakIce(16));
    }

    IEnumerator BreakIce(float timer)
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            switch (timer)
            {
                case > 12.5f:
                    if (myImage.sprite != sprites[0]) { myImage.sprite = sprites[0]; }
                    break;

                case > 10f:
                    if (myImage.sprite != sprites[1]) { myImage.sprite = sprites[1]; }
                    break;

                case > 7.5f:
                    if (myImage.sprite != sprites[2]) { myImage.sprite = sprites[2]; }
                    break;

                case > 5f:
                    if (myImage.sprite != sprites[3]) { myImage.sprite = sprites[3]; }
                    break;

                default:
                    if (myImage.color.a != 0) { Color newColor = myImage.color; newColor.a = 0; myImage.color = newColor; }
                    break;
            }
            yield return null;
        }
        Destroy(gameObject.transform.parent.gameObject);
    }
}
