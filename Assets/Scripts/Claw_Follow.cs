using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw_Follow : MonoBehaviour
{/*
    public Transform cameraTransform;
    private PlayerControls playerControls;
    private Vector2 mousePosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = playerControls.Player.MousePosition.ReadValue<Vector2>();

        // Convert mouse position to world position
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        // Set the position of the claw to follow the mouse position
        transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, transform.position.z);

        // Apply a rotational force to the claw based on the mouse position
        // Vector3 direction = worldMousePosition - transform.position;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // rb.angularVelocity = angle * 5f; // Adjust the multiplier for desired rotation speed

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
*/}
