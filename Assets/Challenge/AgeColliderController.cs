using UnityEngine;

public class AgeColliderController : MonoBehaviour
{
    [Header("Colliders to disable for 6 to 9 age range")]
    [SerializeField] private SphereCollider firstSphereCollider;
    [SerializeField] private GameObject textSiembra;
    [SerializeField] private SphereCollider secondSphereCollider;
    [SerializeField] private GameObject textCombustible;

    [Header("Colliders to activate for 6 to 9 age range")]
    [SerializeField] private SphereCollider ThirthSphereCollider;
    [SerializeField] private GameObject textPreguntas;
    [SerializeField] private GameObject textSemillas;
    [SerializeField] private GameObject textTiempo;

    [SerializeField] private GameObject textUIdificil;
    [SerializeField] private GameObject textUIMedio;
    [SerializeField] private GameObject CanvasInforme;


    private void Start()
    {
        ApplyColliderRulesByAge();
    }

    private void ApplyColliderRulesByAge()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("No existe GameManager en la escena.");
            return;
        }

        switch (GameManager.Instance.SelectedAgeRange)
        {
            case AgeRange.SixToSeven:
                DisableSelectedColliders();
                ThirthSphereCollider.enabled = true;
                textPreguntas.SetActive(true);
                break;

            case AgeRange.EightToNine:
                EnableSelectedColliders();
                ThirthSphereCollider.enabled = false;
                textSiembra.SetActive(true);
                textCombustible.SetActive(true);
                textSemillas.SetActive(true);
                textUIMedio.SetActive(true);
                textUIdificil.SetActive(false);
                CanvasInforme.SetActive(true);
                break;

            case AgeRange.TenToSixteen:
                EnableSelectedColliders();
                ThirthSphereCollider.enabled = false;
                textCombustible.SetActive(true);
                textSiembra.SetActive(true);
                textSemillas.SetActive(true);
                textTiempo.SetActive(true);
                textUIMedio.SetActive(false);
                textUIdificil.SetActive(true);
                CanvasInforme.SetActive(true);
                break;

            default:
                Debug.LogWarning("No se seleccionó ningún rango de edad.");
                break;
        }
    }

    private void DisableSelectedColliders()
    {
        if (firstSphereCollider != null)
            firstSphereCollider.enabled = false;

        if (secondSphereCollider != null)
            secondSphereCollider.enabled = false;
    }

    private void EnableSelectedColliders()
    {
        if (firstSphereCollider != null)
            firstSphereCollider.enabled = true;

        if (secondSphereCollider != null)
            secondSphereCollider.enabled = true;
    }
}
