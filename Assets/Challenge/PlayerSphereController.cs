using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSphereController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Camera")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private Rigidbody rb;
    private Vector2 movementInput;

    private float horizontalRotation;
    private float verticalRotation;

    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        horizontalRotation = transform.eulerAngles.y;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (!canMove)
            return;

        ReadMovementInput();
        ReadMouseInput();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            StopPlayerMovement();
            return;
        }

        MovePlayer();
    }

    private void ReadMovementInput()
    {
        movementInput = Vector2.zero;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            movementInput.y = 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            movementInput.y = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            movementInput.x = 1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            movementInput.x = -1f;

        movementInput = movementInput.normalized;
    }

    private void ReadMouseInput()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        horizontalRotation += mouseDelta.x * mouseSensitivity;

        verticalRotation -= mouseDelta.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void MovePlayer()
    {
        Vector3 direction =
            transform.right * movementInput.x +
            transform.forward * movementInput.y;

        rb.velocity = new Vector3(
            direction.x * speed,
            rb.velocity.y,
            direction.z * speed
        );
    }

    private void StopPlayerMovement()
    {
        movementInput = Vector2.zero;

        rb.velocity = new Vector3(
            0f,
            rb.velocity.y,
            0f
        );
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        canMove = isEnabled;

        if (canMove)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
            StopPlayerMovement();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}