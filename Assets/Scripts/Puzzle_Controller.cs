using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Puzzle_Controller : MonoBehaviour
{
    private Transform entryPoint;
    private Transform claw;
    private Transform playerCamera;
    private Transform player;
    private bool exitedPuzzle = false;
    private Collider2D[] hits;
    private BoxCollider2D entryPointCollider;
    public Vector3 center; // The center of the puzzle for camera targeting
    public float cameraSize = 10f; // The size of the camera when focusing on the puzzle
    private bool completedPuzzle;
    private bool completedBonus;
    public enum CompletionTrigger { None, ClearCells, SpawnItem, Both }
    public CompletionTrigger completionType = CompletionTrigger.ClearCells;
    public CompletionTrigger bonusType = CompletionTrigger.SpawnItem;
    public GameObject itemPrefab;
    public GameObject spawnedItem;
    public Vector3 spawnLocation;
    public Vector3 clearLocationStart;
    public Vector3 clearLocationEnd;
    private Check_Puzzle_Completion completionCheck;
    public Tilemap tiles;
    // Start is called before the first frame update
    void Start()
    {
        if (entryPoint == null)
        {
            entryPoint = transform.Find("Entry");
        }
        if (claw == null)
        {
            claw = transform.Find("Claw");
        }
        if (completionCheck == null)
        {
            completionCheck = transform.Find("Completion Checker").GetComponent<Check_Puzzle_Completion>();
        }
        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera_Follow>().transform;
        }
        if (player == null)
        {
            player = FindObjectOfType<Player_Movement>().transform;
        }
        entryPointCollider = entryPoint.GetComponent<BoxCollider2D>();
        if (clearLocationStart.x > clearLocationEnd.x)
        {
            (clearLocationStart.x, clearLocationEnd.x) = (clearLocationEnd.x, clearLocationStart.x);
        }
        if (clearLocationStart.y > clearLocationEnd.y)
        {
            (clearLocationStart.y, clearLocationEnd.y) = (clearLocationEnd.y, clearLocationStart.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandlePuzzleEntry();
        HandlePuzzleCompletion();
    }

    void HandlePuzzleEntry()
    {
        // check if the player is on the entry point using it's collider
        hits = Physics2D.OverlapBoxAll(entryPoint.position, entryPointCollider.size, 0f);
        bool playerOnEntryPoint = false;
        foreach (Collider2D hit in hits)
        {
            if (hit.name == "Player")
            {
                if (!exitedPuzzle)
                {
                    // set the camera to puzzle mode
                    playerCamera.GetComponent<Camera_Follow>().TargetPuzzle(transform.position + center, cameraSize);
                    // disable player movement
                    player.GetComponent<Player_Movement>().enabled = false;
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                playerOnEntryPoint = true;
                break;
            }
        }
        if (!playerOnEntryPoint)
        {
            exitedPuzzle = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            exitedPuzzle = true;
            // set the camera to follow the player
            playerCamera.GetComponent<Camera_Follow>().TargetPlayer();
            player.GetComponent<Player_Movement>().enabled = true;
        }
    }

    void HandlePuzzleCompletion()
    {
        if (completionCheck.puzzleCompleted && !completedPuzzle)
        {
            completedPuzzle = true;
            if (completionType == CompletionTrigger.ClearCells || completionType == CompletionTrigger.Both)
            {
                for (int x = (int)clearLocationStart.x; x < (int)clearLocationEnd.x; x++)
                {
                    for (int y = (int)clearLocationStart.y; y < (int)clearLocationEnd.y; y++)
                    {
                        tiles.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
            if (completionType == CompletionTrigger.SpawnItem || completionType == CompletionTrigger.Both)
            {
                if (itemPrefab != null)
                {
                    spawnedItem = Instantiate(itemPrefab);
                    spawnedItem.transform.position = spawnLocation;
                }
            }
        }
        if (completionCheck.optionalCompletionCondition && !completedBonus)
        {
            completedBonus = true;
            if (bonusType == CompletionTrigger.ClearCells || bonusType == CompletionTrigger.Both)
            {
                for (int x = (int)clearLocationStart.x; x < (int)clearLocationEnd.x; x++)
                {
                    for (int y = (int)clearLocationStart.y; y < (int)clearLocationEnd.y; y++)
                    {
                        tiles.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
                // tiles.BoxFill(Vector3Int.FloorToInt((clearLocationStart+clearLocationEnd)/2), null, (int)clearLocationStart.x, (int)clearLocationStart.y, (int)clearLocationEnd.x, (int)clearLocationEnd.y);
            }
            if (bonusType == CompletionTrigger.SpawnItem || bonusType == CompletionTrigger.Both)
            {
                if (itemPrefab != null)
                {
                    spawnedItem = Instantiate(itemPrefab);
                    spawnedItem.transform.position = spawnLocation;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (completionType == CompletionTrigger.ClearCells || completionType == CompletionTrigger.Both)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 1f);
            Gizmos.DrawCube(
                (clearLocationStart+clearLocationEnd)/2,
                new Vector3(
                    Mathf.Abs(clearLocationEnd.x-clearLocationStart.x),
                    Mathf.Abs(clearLocationEnd.y-clearLocationStart.y),
                    1
                )
            );
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            Gizmos.DrawWireSphere(spawnLocation, 0.75f);
        }
    }
}
