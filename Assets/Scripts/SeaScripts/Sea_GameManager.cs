using UnityEngine;
using TMPro;

public class Sea_GameManager : MonoBehaviour
{
    public int totalSeals = 11;
    public int totalTrash = 12;
    public int sealsRemaining;
    public int trashRemaining;
    public bool paused;
    [SerializeField] private TextMeshProUGUI sealText;
    [SerializeField] private TextMeshProUGUI trashText;
    [SerializeField] private GameObject popupObj;

    private void Start()
    {
        sealsRemaining = CalculateUncollectedSeals();
        UpdateSealText();
        trashRemaining = CalculateUncollectedTrash();
        UpdateTrashText();

        if (sealsRemaining <= 0 && trashRemaining <= 0)
        {
            popupObj.SetActive(true);
        }
    }

    public void UpdateSealNo(bool isPos)
    {
        if (isPos) { sealsRemaining++; }
        else { sealsRemaining--; }
        UpdateSealText();
        if (sealsRemaining <= 0 && trashRemaining <= 0)
        {
            popupObj.SetActive(true);
        }
    }

    public void UpdateTrashNo(bool isPos)
    {
        if (isPos) { trashRemaining++; }
        else { trashRemaining--; }
        UpdateTrashText();
        if (trashRemaining <= 0)
        {
            if (sealsRemaining <= 0) { popupObj.SetActive(true); }
            PlayerPrefs.SetInt("TrashComplete_", 1);
            PlayerPrefs.Save();
        }
    }

    private void UpdateSealText()
    {
        sealText.text = "Seals remaining: " + sealsRemaining.ToString();
    }

    private void UpdateTrashText()
    {
        trashText.text = "Trash remaining: " + trashRemaining.ToString();
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

    private int CalculateUncollectedTrash()
    {
        int uncollected = 0;
        for (int i = 0; i < totalTrash; i++)
        {
            if (PlayerPrefs.GetInt("TrashCollected_" + i, 0) == 0)
                uncollected++;
        }
        return uncollected;
    }
}
