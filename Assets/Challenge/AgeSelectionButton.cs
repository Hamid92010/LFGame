using UnityEngine;
using UnityEngine.SceneManagement;

public class AgeSelectionButton : MonoBehaviour
{
    [SerializeField] private AgeRange ageRange;
    [SerializeField] private string cinematicSceneName = "CinematicScene";

    public void SelectAge()
    {
        GameManager.Instance.SetSelectedAgeRange(ageRange);
        SceneManager.LoadScene(cinematicSceneName);
    }
}
