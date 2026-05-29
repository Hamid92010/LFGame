using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedDepositUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SeedMechanicManager seedMechanicManager;
    [SerializeField] private PlayerSphereController playerController;

    [Header("Deposit Configuration")]
    [SerializeField] private SeedDepositType depositType;

    [Header("Station UI")]
    [SerializeField] private GameObject stationCanvas;
    [SerializeField] private TMP_Text selectedSeedsText;

    [Header("Buttons")]
    [SerializeField] private Button addSeedButton;
    [SerializeField] private Button subtractSeedButton;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button exitButton;

    private int selectedSeeds = 0;

    private void Start()
    {
        if (stationCanvas != null)
            stationCanvas.SetActive(false);

        ConfigureButtons();
        ResetSelectedSeeds();
        UpdateStationUI();
    }

    private void OnDestroy()
    {
        RemoveButtonListeners();
    }

    private void ConfigureButtons()
    {
        if (addSeedButton != null)
            addSeedButton.onClick.AddListener(AddSeed);

        if (subtractSeedButton != null)
            subtractSeedButton.onClick.AddListener(SubtractSeed);

        if (acceptButton != null)
            acceptButton.onClick.AddListener(AcceptDeposit);

        if (exitButton != null)
            exitButton.onClick.AddListener(CloseStationUI);
    }

    private void RemoveButtonListeners()
    {
        if (addSeedButton != null)
            addSeedButton.onClick.RemoveListener(AddSeed);

        if (subtractSeedButton != null)
            subtractSeedButton.onClick.RemoveListener(SubtractSeed);

        if (acceptButton != null)
            acceptButton.onClick.RemoveListener(AcceptDeposit);

        if (exitButton != null)
            exitButton.onClick.RemoveListener(CloseStationUI);
    }

    public void OpenStationUI()
    {
        if (seedMechanicManager != null && seedMechanicManager.GameEnded)
            return;

        if (stationCanvas != null)
            stationCanvas.SetActive(true);

        if (playerController != null)
            playerController.SetMovementEnabled(false);

        ResetSelectedSeeds();
        UpdateStationUI();
    }

    public void CloseStationUI()
    {
        if (stationCanvas != null)
            stationCanvas.SetActive(false);

        if (playerController != null)
            playerController.SetMovementEnabled(true);

        ResetSelectedSeeds();
        UpdateStationUI();
    }

    private void AddSeed()
    {
        if (seedMechanicManager == null)
            return;

        if (seedMechanicManager.GameEnded)
            return;

        if (selectedSeeds >= seedMechanicManager.AvailableSeeds)
            return;

        selectedSeeds++;
        UpdateStationUI();
    }

    private void SubtractSeed()
    {
        if (seedMechanicManager != null && seedMechanicManager.GameEnded)
            return;

        if (selectedSeeds <= 0)
            return;

        selectedSeeds--;
        UpdateStationUI();
    }

    private void AcceptDeposit()
    {
        if (seedMechanicManager == null)
            return;

        if (seedMechanicManager.GameEnded)
            return;

        if (!seedMechanicManager.CanDepositSeeds(selectedSeeds))
            return;

        seedMechanicManager.DepositSeeds(depositType, selectedSeeds);

        CloseStationUI();
    }

    private void ResetSelectedSeeds()
    {
        selectedSeeds = 0;
    }

    private void UpdateStationUI()
    {
        if (selectedSeedsText != null)
            selectedSeedsText.text = selectedSeeds.ToString();

        if (subtractSeedButton != null)
            subtractSeedButton.interactable = selectedSeeds > 0;

        if (acceptButton != null)
            acceptButton.interactable = selectedSeeds > 0;

        if (addSeedButton != null && seedMechanicManager != null)
        {
            addSeedButton.interactable =
                selectedSeeds < seedMechanicManager.AvailableSeeds &&
                !seedMechanicManager.GameEnded;
        }
    }
}
