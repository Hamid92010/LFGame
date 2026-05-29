using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private bool invertDirection = false;

    private void LateUpdate()
    {
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        if (target == null)
            return;

        Vector3 directionToTarget = target.position - transform.position;

        if (directionToTarget == Vector3.zero)
            return;

        if (invertDirection)
            directionToTarget *= -1f;

        transform.rotation = Quaternion.LookRotation(directionToTarget);
    }
}
