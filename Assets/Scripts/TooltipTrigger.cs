using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // on hover, move tooltip to desired pos
    // on hover leave, return tooltip to starting pos
    [SerializeField] Tooltip tooltip;

    /*private void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnMouseOver");
        tooltip.MoveToTarget();
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnMouseOver");
        tooltip.MoveToTarget();
    }

    /*private void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnMouseExit");
        tooltip.MoveToStart();
    }*/

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnMouseExit");
        tooltip.MoveToStart();
    }
}
