using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform player;
    public Vector2 puzzleCenter;
    public Vector3 offset;
    public enum CameraState { FollowPlayer, Puzzle }
    public CameraState cameraState = CameraState.FollowPlayer;
    private bool caughtTarget = true;
    private Vector3 target;
    private Vector3 snapTarget;
    public float cameraSpeed = 5f;
    public float defaultCameraSize = 5f;
    private float targetCameraSize;
    private Vector3 playerVelocity;
    // Start is called before the first frame update
    void Start()
    {
        target = player.position + offset;
        transform.position = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraState == CameraState.FollowPlayer)
        {
            snapTarget = player.position + offset;

            // set velocityFactor so that the stable position trailing a moving object is centered on it
            // the velocity must be equal to cameraSpeed * velocity * velocityFactor, so velocityFactor = 1 / cameraSpeed
            playerVelocity = new Vector3(
                player.GetComponent<Rigidbody2D>().velocity.x,
                player.GetComponent<Rigidbody2D>().velocity.y,
                0
            );
            target = snapTarget + playerVelocity / cameraSpeed;
            targetCameraSize = defaultCameraSize;
        }
        else if (cameraState == CameraState.Puzzle)
        {
            target = new Vector3(puzzleCenter.x, puzzleCenter.y, 0) + offset;
        }

        if (caughtTarget)
        {
            transform.position = snapTarget;
        } 
        else
        {
        transform.position = Vector3.Lerp(transform.position, target, cameraSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, snapTarget) < 0.1f)
            {
                caughtTarget = true;
            }
        }
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetCameraSize, cameraSpeed * Time.deltaTime);
    }

    public void TargetPuzzle(Vector3 center, float cameraSize)
    {
        puzzleCenter = new Vector2(center.x, center.y);
        cameraState = CameraState.Puzzle;
        caughtTarget = false;
        targetCameraSize = cameraSize;
    }

    public void TargetPlayer()
    {
        cameraState = CameraState.FollowPlayer;
        caughtTarget = false;
    }
}
