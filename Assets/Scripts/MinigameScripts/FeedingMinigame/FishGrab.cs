using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FishGrab : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
   
    void Awake()
    {
        m_Raycaster = FindFirstObjectByType<GraphicRaycaster>();
        m_EventSystem = FindFirstObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Seal_SancBehaviour>() != null)
            {
                Debug.Log("Fed seal!");
                result.gameObject.GetComponent<Seal_SancBehaviour>().AdjustHunger(5);
                Destroy(gameObject);
            }
        }
    }

}
