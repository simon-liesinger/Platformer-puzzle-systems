using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Controller : MonoBehaviour
{
    private float targetPosition;
    // private Quaternion targetRotation;
    private float positionDelta;
    private float velocityDelta;
    // private float rotationDelta;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the piece is nearly aligned to the grid, snap it to the grid
        if (
            Mathf.Abs(transform.position.x + 0.5f - Mathf.Round(transform.position.x + 0.5f)) < 0.3f &&
            Mathf.Abs(transform.eulerAngles.z/90 - Mathf.Round(transform.eulerAngles.z/90)) < 0.1f &&
            Mathf.Abs(rb.velocity.x) < 0.1f &&
            Mathf.Abs(rb.angularVelocity) < 0.1f
        )
        {
            targetPosition = Mathf.Round(transform.position.x + 0.5f) - 0.5f;
            // targetRotation = Quaternion.Euler(
            //     transform.eulerAngles.x,
            //     transform.eulerAngles.y,
            //     Mathf.Round(transform.eulerAngles.z / 90) * 90
            // );

            positionDelta = targetPosition - transform.position.x;
            velocityDelta = positionDelta * 10f;
            // rotationDelta = Quaternion.Angle(transform.rotation, targetRotation);
            rb.velocity = new Vector2(velocityDelta, rb.velocity.y);
            // rb.angularVelocity = rotationDelta;
        }
    }
}
