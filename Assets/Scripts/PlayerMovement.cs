using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    private PlayerControls controls;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    private void Awake() {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    private void FixedUpdate() {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.linearVelocity = movement * moveSpeed;
    }
}
