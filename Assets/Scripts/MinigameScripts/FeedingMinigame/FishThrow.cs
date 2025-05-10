using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FishThrow : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private Vector2 dragStartPos;
    private float dragStartTime;
    private RectTransform rt;
    private Sanc_GameManager gameManager;
    private GraphicRaycaster raycaster;
    private EventSystem ev;
    public float mass;

    public Vector2 gravity = new Vector2(0, -800f);

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        raycaster = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GraphicRaycaster>();
        ev = FindFirstObjectByType<EventSystem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = rt.anchoredPosition;
        dragStartTime = Time.time;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 releaseScreen = eventData.position;
        Vector2 releaseLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rt.parent as RectTransform,
            releaseScreen,
            null,
            out releaseLocal
        );

        float dt = Mathf.Max(Time.time - dragStartTime, 0.0f);
        Vector2 initialVelocity = (releaseLocal - dragStartPos) / dt;
        StartCoroutine(AnimateThrow(dragStartPos, initialVelocity));
    }

    IEnumerator AnimateThrow(Vector2 StartPos, Vector2 initialVel)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;
            Vector2 pos = StartPos + initialVel * t + mass * gravity * t * t;
            rt.anchoredPosition = pos;

            PointerEventData pd = new PointerEventData(ev);
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rt.position);
            pd.position = screenPoint;
            var results = new List<RaycastResult>();
            raycaster.Raycast(pd, results);
            foreach (var r in results)
            {
                var seal = r.gameObject.GetComponent<Seal_SancBehaviour>();
                if (seal != null)
                {
                    if (seal.canFeed)
                    {
                        seal.StartCoroutine(seal.FeedingCoroutine(seal.feedingCooldown - 5, 5, 10));
                        Destroy(gameObject);
                        yield break;
                    }
                }
            }

            if (pos.y < -(rt.parent as RectTransform).rect.height / 2 - 100)
                break;

            yield return null;
        }

        Destroy(gameObject);
    }
}
