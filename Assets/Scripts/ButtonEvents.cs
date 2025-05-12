using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    [SerializeField] private GameObject creditsWindowPrefab;

    public void OutOfFuel(GameObject objToDisable)
    {
        objToDisable.SetActive(false);
        SwitchScene(1);
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadCredits()
    {
        if (creditsWindowPrefab) { Instantiate(creditsWindowPrefab); }
    }

    public void VisitSRI()
    {
        Application.OpenURL("https://www.sealrescueireland.org/");
    }
}
