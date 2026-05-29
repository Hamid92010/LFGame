using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionZone : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pressEMessage;
    [SerializeField] private GameObject interactionCanvas;

    private PlayerSphereController playerController;
    private bool playerIsInside;

    private void Start()
    {
        pressEMessage.SetActive(false);
        interactionCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!playerIsInside)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenCanvas();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerController = other.GetComponent<PlayerSphereController>();

        playerIsInside = true;
        pressEMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerIsInside = false;
        playerController = null;

        pressEMessage.SetActive(false);
    }

    private void OpenCanvas()
    {
        pressEMessage.SetActive(false);
        interactionCanvas.SetActive(true);

        if (playerController != null)
            playerController.SetMovementEnabled(false);
    }

    public void CloseCanvas()
    {
        interactionCanvas.SetActive(false);

        if (playerController != null)
            playerController.SetMovementEnabled(true);

        if (playerIsInside)
            pressEMessage.SetActive(true);
    }
}
