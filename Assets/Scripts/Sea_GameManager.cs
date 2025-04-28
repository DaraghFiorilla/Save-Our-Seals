using UnityEngine;
using TMPro;

public class Sea_GameManager : MonoBehaviour
{
    public int totalSeals = 6;
    public int sealsRemaining;
    [SerializeField] private TextMeshProUGUI sealText;
    [SerializeField] private GameObject popupObj;

    private void Start()
    {
        sealsRemaining = CalculateUncollectedSeals();
        UpdateSealText();

        if (sealsRemaining <= 0)
        {
            popupObj.SetActive(true);
        }
    }

    public void UpdateSealNo(bool isPos)
    {
        if (isPos) { sealsRemaining++; }
        else { sealsRemaining--; }
        UpdateSealText();
        if (sealsRemaining <= 0)
        {
            popupObj.SetActive(true);
        }
    }

    private void UpdateSealText()
    {
        sealText.text = "Seals remaining: " + sealsRemaining.ToString();
    }

    private int CalculateUncollectedSeals()
    {
        int uncollected = 0;
        for (int i = 0; i < totalSeals; i++)
        {
            if (PlayerPrefs.GetInt("SealCollected_" + i, 0) == 0)
            {
                uncollected++;
            }
        }
        return uncollected;
    }
}
