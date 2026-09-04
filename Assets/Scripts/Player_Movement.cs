using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private PlayerControls controls;
    public Rigidbody2D rb;
    private float moveInput;
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float gravity = 9.81f;
    public Transform groundCheck;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    void Awake()
    {
        controls = new PlayerControls();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Jump();
        Gravity();
    }

    private void Gravity()
    {
        if (IsGrounded())
        {
            if (rb.velocity.y < 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }
        else
        {
            
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - gravity * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
    }

    private void PlayerMovement()
    {
        moveInput = controls.Player.Move.ReadValue<float>();
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (IsGrounded() && controls.Player.Jump.triggered)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * 2f * gravity));
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
