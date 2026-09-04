using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw_V2 : MonoBehaviour
{

    private Vector3 mousePos;
    private Vector3 direction;
    protected Vector3 angleDirection;
    private float distance;
    private float angle;
    private Vector3 angleTargetPosition;
    public Rigidbody2D rb;
    public float angleSpeed = 5f;
    public float movementSpeed = 10f;
    private bool Grabbing = false;
    private GameObject GrabbedObject;
    private GameObject GrabbedPart;
    private Quaternion grabbedObjectAngle;
    private Vector3 offset;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;
    private Vector3 positionDelta;
    private Vector2 velocityDelta;
    private float rotationDelta;
    private float angularVelocityDelta;
    public float linearGrabStrength = 10f;
    public float angularGrabStrength = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveClaw();
        GrabObject();
    }

    void MoveClaw()
    {
        //get global mouse coords and log them
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // set the rotation to downwards, adjusted by the horizontal distance to the mouse position
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            angleTargetPosition = new Vector3(mousePos.x+2, mousePos.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            angleTargetPosition = new Vector3(mousePos.x-2, mousePos.y, transform.position.z);
        }
        else
        {
            angleTargetPosition = new Vector3(mousePos.x, mousePos.y-2, transform.position.z);
        }
        angleDirection = angleTargetPosition - transform.position;
        angle = Mathf.Atan2(angleDirection.y, angleDirection.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        if ((angle - transform.eulerAngles.z) > 180)
        {
            angle -= 360;
        }
        else if ((angle - transform.eulerAngles.z) < -180)
        {
            angle += 360;
        }
        rb.angularVelocity = (angle - transform.eulerAngles.z) * angleSpeed; // Adjust the multiplier for desired rotation speed

        //lerp the position to the target position
        direction = mousePos - transform.position;
        //transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, mousePos.y, transform.position.z), Time.deltaTime * 5f);
        rb.velocity = direction.normalized * movementSpeed; // Adjust the multiplier for desired movement speed
    }

    void GrabObject()
    {
        //grab while the mouse button is held down
        if (Input.GetMouseButton(0))
        {
            if (!Grabbing)
            {
                // find the nearest object on the boxes layer
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f, LayerMask.GetMask("Boxes"));
                if (colliders.Length > 0)
                {
                    Grabbing = true;
                    GrabbedPart = colliders[0].gameObject;
                    GrabbedObject = GrabbedPart.transform.parent.gameObject;
                    //find the angle of the grabbed object relative to the claw
                    grabbedObjectAngle = GrabbedObject.transform.rotation * Quaternion.Inverse(transform.rotation);
                    // Debug.Log("Grabbed object angle: " + grabbedObjectAngle);
                    //round the angle to the nearest 90 degrees
                    grabbedObjectAngle = Quaternion.Euler(
                        Mathf.Round(grabbedObjectAngle.eulerAngles.x / 90) * 90,
                        Mathf.Round(grabbedObjectAngle.eulerAngles.y / 90) * 90,
                        Mathf.Round(grabbedObjectAngle.eulerAngles.z / 90) * 90
                    );
                    GrabbedObject.GetComponent<Piece_Controller>().enabled = false;
                }
            }

            if (GrabbedObject != null && GrabbedPart != null)
            {
                MoveGrabbedObject();
            }
        }
        else if (Grabbing)
        {
            // GrabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GrabbedObject.GetComponent<Piece_Controller>().enabled = true;
            Grabbing = false;
            GrabbedObject = null;
            GrabbedPart = null;
        }
    }

    void MoveGrabbedObject()
    {
        //calculate where the grabbed object should be positioned based on the claw's position and rotation
        offset = GrabbedPart.transform.position - GrabbedObject.transform.position;
        desiredPosition = transform.position - offset;
        desiredRotation = transform.rotation * grabbedObjectAngle;

        //change the object's velocity and angular velocity to move it to the desired position and rotation
        //while preserving total momentum by moving the claw in the opposite direction
        positionDelta = desiredPosition - GrabbedObject.transform.position;
        rotationDelta = Mathf.DeltaAngle(GrabbedObject.transform.eulerAngles.z, desiredRotation.eulerAngles.z); //Quaternion.SignedAngle(GrabbedObject.transform.rotation, desiredRotation);
        // Debug.Log("Position delta: " + positionDelta + ", Rotation delta: " + rotationDelta);


        // /*
        velocityDelta = positionDelta * linearGrabStrength;
        angularVelocityDelta = rotationDelta * angularGrabStrength;
        rb.velocity -= velocityDelta;
        rb.angularVelocity -= angularVelocityDelta;

        GrabbedObject.GetComponent<Rigidbody2D>().velocity = velocityDelta;
        GrabbedObject.GetComponent<Rigidbody2D>().angularVelocity = angularVelocityDelta;
        // */
        /*
        //move the grabbed object to the claw's position
        GrabbedObject.transform.position = desiredPosition;
        //rotate the grabbed object to the claw's rotation, adjusted by the grabbed object's angle
        GrabbedObject.transform.rotation = desiredRotation;
        */
    }
}
