using Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float vSpeed;
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private Transform GFXTransform = null;
    [SerializeField] private Animator animator = null;

    private Vector2 previousInput;
    private Vector3 moveDirection = Vector3.zero;

    private PlayerInputActions controls;
    private PlayerInputActions Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new PlayerInputActions();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [ClientCallback]
    private void Update() => Move();

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void Move()
    {
        // Use input up and down for direction, multiplied by speed
        moveDirection = new Vector3(-previousInput.y, 0, previousInput.x);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= movementSpeed;

        // Apply gravity manually.
        //Calculate vertical movement and gravity
        if (controller.isGrounded)
        {
            vSpeed = 0;
        }

        vSpeed -= gravity * Time.deltaTime;
        moveDirection.y = vSpeed;

        // Move Character Controller
        controller.Move(moveDirection * Time.deltaTime);

        if (previousInput.y != 0 || previousInput.x != 0)
        {
            var rotation = Quaternion.LookRotation(moveDirection);
            GFXTransform.rotation = Quaternion.RotateTowards(GFXTransform.rotation, rotation, 15f);
        }

        animator.SetBool("moving", previousInput.y != 0 || previousInput.x != 0);
    }
}
