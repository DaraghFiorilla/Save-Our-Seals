using UnityEngine;
using TMPro;

public class Sea_GameManager : MonoBehaviour
{
    public int sealsRemaining;
    [SerializeField] private TextMeshProUGUI sealText;

    public void UpdateSealNo(bool isPos)
    {
        if (isPos) { sealsRemaining++; }
        else { sealsRemaining--; }
        sealText.text = "Seals remaining: " + sealsRemaining.ToString();
    }
}
