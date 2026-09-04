using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox_Scalar : MonoBehaviour
{
    public float scaleFactor = 1.0f; // The factor by which to scale the hitbox
    public BoxCollider2D hitbox; // Reference to the BoxCollider2D component
    private Transform claw;
    public float minDistance = 3.0f; // Minimum distance to scale the hitbox
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent != null)
        {
            if (transform.parent.parent.parent != null)
            {
                claw = transform.parent.parent.parent.Find("Claw");
            }
        }
        if (hitbox == null)
        {
            hitbox = GetComponent<BoxCollider2D>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (claw != null)
        {
            float distance = Vector2.Distance(claw.position, transform.position);
            if (distance < minDistance)
            {
                scaleFactor = 0.8f; // Scale down the hitbox when the claw is close
            }
            else
            {
                scaleFactor = 0.95f; // Reset to original size when the claw is far
            }
            hitbox.size = Vector2.Lerp(hitbox.size, new Vector2(scaleFactor, scaleFactor), Time.deltaTime);
        }
    }
}
