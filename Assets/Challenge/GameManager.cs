using UnityEngine;

public enum AgeRange
{
    None,
    SixToSeven,
    EightToNine,
    TenToSixteen
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public AgeRange SelectedAgeRange { get; private set; } = AgeRange.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedAgeRange(AgeRange ageRange)
    {
        SelectedAgeRange = ageRange;
        Debug.Log("Edad seleccionada: " + SelectedAgeRange);
    }
}
