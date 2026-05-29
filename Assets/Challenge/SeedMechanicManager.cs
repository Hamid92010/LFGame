using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SeedDepositType
{
    Fuel,
    Garden
}

public class SeedMechanicManager : MonoBehaviour
{
    [Header("Mechanic Availability")]
    [SerializeField] private AgeRange requiredAgeRange = AgeRange.EightToNine;
    [SerializeField] private GameObject mechanicRoot;

    [Header("Initial Values")]
    [SerializeField] private int initialSeeds = 6;
    [SerializeField] private int targetKilometers = 10;

    [Header("Base UI")]
    [SerializeField] private TMP_Text availableSeedsText;
    [SerializeField] private TMP_Text traveledKilometersText;
    [SerializeField] private TMP_Text plantedSeedsText;
    [SerializeField] private TMP_Text objectiveStatusText;

    [Header("End Game UI")]
    [SerializeField] private GameObject objectiveCompletedCanvas;
    [SerializeField] private GameObject objectiveFailedCanvas;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float endMessageDuration = 5f;
    [SerializeField] private float fadeDuration = 4f;
    [SerializeField] private string startSceneName = "Menu";

    private int availableSeeds;
    private int traveledKilometers;
    private int plantedSeeds;

    private bool gameEnded = false;

    public int AvailableSeeds => availableSeeds;
    public int TraveledKilometers => traveledKilometers;
    public int PlantedSeeds => plantedSeeds;
    public bool GameEnded => gameEnded;

    private void Start()
    {
        HideEndGameUI();
        ResetFade();

        if (!CanUseMechanic())
        {
            DisableMechanic();
            return;
        }

        EnableMechanic();

        availableSeeds = initialSeeds;
        traveledKilometers = 0;
        plantedSeeds = 0;

        UpdateBaseUI();
    }

    private bool CanUseMechanic()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("No existe GameManager. No se puede validar el rango de edad.");
            return false;
        }

        return GameManager.Instance.SelectedAgeRange == requiredAgeRange;
    }

    private void EnableMechanic()
    {
        if (mechanicRoot != null)
            mechanicRoot.SetActive(true);
    }

    private void DisableMechanic()
    {
        if (mechanicRoot != null)
            mechanicRoot.SetActive(false);

        Debug.Log("Mecánica de semillas desactivada. El rango seleccionado no es EightToNine.");
    }

    public bool CanDepositSeeds(int amount)
    {
        if (gameEnded)
            return false;

        return amount > 0 && amount <= availableSeeds;
    }

    public void DepositSeeds(SeedDepositType depositType, int amount)
    {
        if (gameEnded)
            return;

        if (!CanDepositSeeds(amount))
        {
            Debug.LogWarning("Cantidad inválida de semillas.");
            return;
        }

        switch (depositType)
        {
            case SeedDepositType.Fuel:
                DepositSeedsAsFuel(amount);
                break;

            case SeedDepositType.Garden:
                DepositSeedsInGarden(amount);
                break;
        }

        UpdateBaseUI();
        CheckEndGameCondition();
    }

    private void DepositSeedsAsFuel(int amount)
    {
        availableSeeds -= amount;

        // Cada semilla usada como combustible equivale a 1 km.
        traveledKilometers += amount;
    }

    private void DepositSeedsInGarden(int amount)
    {
        availableSeeds -= amount;

        // Se registra cuántas semillas plantó el jugador.
        plantedSeeds += amount;

        // Cada semilla plantada devuelve 2 semillas.
        int generatedSeeds = amount * 2;
        availableSeeds += generatedSeeds;
    }

    private void CheckEndGameCondition()
    {
        if (traveledKilometers >= targetKilometers)
        {
            StartCoroutine(EndGame(objectiveCompletedCanvas));
            return;
        }

        if (availableSeeds <= 0 && traveledKilometers < targetKilometers)
        {
            StartCoroutine(EndGame(objectiveFailedCanvas));
        }
    }

    private IEnumerator EndGame(GameObject endCanvas)
    {
        gameEnded = true;

        if (endCanvas != null)
            endCanvas.SetActive(true);

        yield return new WaitForSeconds(endMessageDuration);

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(startSceneName);
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null)
            yield break;

        float elapsedTime = 0f;
        Color startColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            fadeImage.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );

            yield return null;
        }
    }

    private void UpdateBaseUI()
    {
        if (availableSeedsText != null)
            availableSeedsText.text = availableSeeds + " Semillas ";

        if (traveledKilometersText != null)
            traveledKilometersText.text = "" + traveledKilometers + " km";

        if (plantedSeedsText != null)
            plantedSeedsText.text = "Semillas plantadas: " + plantedSeeds;

        if (objectiveStatusText != null)
        {
            objectiveStatusText.text = traveledKilometers >= targetKilometers
                ? "Objetivo: cumplido"
                : "Objetivo: fallido";
        }
    }

    private void HideEndGameUI()
    {
        if (objectiveCompletedCanvas != null)
            objectiveCompletedCanvas.SetActive(false);

        if (objectiveFailedCanvas != null)
            objectiveFailedCanvas.SetActive(false);
    }

    private void ResetFade()
    {
        if (fadeImage == null)
            return;

        Color color = fadeImage.color;
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}