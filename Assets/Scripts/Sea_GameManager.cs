using UnityEngine;
using TMPro;

public class Sea_GameManager : MonoBehaviour
{
    public int sealsRemaining;
    [SerializeField] private TextMeshProUGUI sealText;
    [SerializeField] private GameObject popupObj;

    public void UpdateSealNo(bool isPos)
    {
        if (isPos) { sealsRemaining++; }
        else { sealsRemaining--; }
        sealText.text = "Seals remaining: " + sealsRemaining.ToString();
        if (sealsRemaining <= 0)
        {
            popupObj.SetActive(true);
        }
    }
}
