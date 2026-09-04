using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Puzzle_Completion : MonoBehaviour
{
    public GameObject pieceParent;
    public GameObject puzzleGoals;
    private GameObject[] puzzlePieces;
    private BoxCollider2D[] puzzleBoundaries;
    private bool allPiecesInBounds = false;
    private bool densityGoalsMet = false;
    public bool puzzleCompleted = false;
    private bool optionalDensityGoalsMet = false;
    public bool optionalCompletionCondition = false;
    private Transform[] highDensityAreas;
    private Transform[] lowDensityAreas;
    private Transform[] optionalHighDensityAreas;
    private Transform[] optionalLowDensityAreas;
    public GameObject puzzleCompletedDisplay;
    public GameObject optionalPuzzleCompletedDisplay;
    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get all puzzle pieces from the pieceParent
        puzzlePieces = new GameObject[pieceParent.transform.childCount];
        for (int i = 0; i < pieceParent.transform.childCount; i++)
        {
            puzzlePieces[i] = pieceParent.transform.GetChild(i).gameObject;
        }

        // Get all puzzle boundaries from the current GameObject
        puzzleBoundaries = GetComponents<BoxCollider2D>();

        // Get all high and low density areas from the puzzleGoals child objects
        // Find the "High Density" and "Low Density" child objects of puzzleGoals and get their children
        Transform densityParent = puzzleGoals.transform.Find("High Density");
        List<Transform> densityAreasList = new List<Transform>();
        foreach (Transform child in densityParent)
        {
            densityAreasList.Add(child);
        }
        highDensityAreas = densityAreasList.ToArray();

        densityParent = puzzleGoals.transform.Find("Low Density");
        densityAreasList.Clear();
        foreach (Transform child in densityParent)
        {
            densityAreasList.Add(child);
        }
        lowDensityAreas = densityAreasList.ToArray();

        densityParent = puzzleGoals.transform.Find("Optional High Density");
        densityAreasList.Clear();
        foreach (Transform child in densityParent)
        {
            densityAreasList.Add(child);
        }
        optionalHighDensityAreas = densityAreasList.ToArray();

        densityParent = puzzleGoals.transform.Find("Optional Low Density");
        densityAreasList.Clear();
        foreach (Transform child in densityParent)
        {
            densityAreasList.Add(child);
        }
        optionalLowDensityAreas = densityAreasList.ToArray();
        if (debugMode)
        {
            Debug.Log("Puzzle pieces: " + puzzlePieces.Length);
            Debug.Log("Puzzle boundaries: " + puzzleBoundaries.Length);
            Debug.Log("High density areas: " + highDensityAreas.Length);
            Debug.Log("Low density areas: " + lowDensityAreas.Length);
            Debug.Log("Optional high density areas: " + optionalHighDensityAreas.Length);
            Debug.Log("Optional low density areas: " + optionalLowDensityAreas.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        allPiecesInBounds = CheckPiecesInBounds();
        densityGoalsMet = CheckDensityGoals();
        if (debugMode)
        {
            Debug.Log("All pieces in bounds: " + allPiecesInBounds);
            Debug.Log("Density goals met: " + densityGoalsMet);
        }
        puzzleCompleted = allPiecesInBounds && densityGoalsMet;

        if (puzzleCompleted)
        {
            puzzleCompletedDisplay.SetActive(true);
        }
        else
        {
            puzzleCompletedDisplay.SetActive(false);
        }

        optionalDensityGoalsMet = CheckOptionalDensityGoals();
        if (debugMode)
        {
            Debug.Log("Optional density goals met: " + optionalDensityGoalsMet);
        }
        optionalCompletionCondition = puzzleCompleted && optionalDensityGoalsMet;

        if (optionalCompletionCondition)
        {
            optionalPuzzleCompletedDisplay.SetActive(true);
        }
        else
        {
            optionalPuzzleCompletedDisplay.SetActive(false);
        }
    }

    private bool CheckPiecesInBounds()
    {
        // check if anything on the boxes layer touches a boundary
        foreach (BoxCollider2D boundary in puzzleBoundaries)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(boundary.bounds.center, boundary.bounds.size, 0f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Boxes"))
                {
                    return false;
                }
            }
        }
        return true;

        // //check no puzzle pieces touch a boundary
        // foreach (GameObject piece in puzzlePieces)
        // {
        //     foreach (BoxCollider2D boundary in puzzleBoundaries)
        //     {
        //         // check for overlap between the boundary collider and the piece's child colliders
        //         Collider2D[] pieceColliders = piece.GetComponentsInChildren<Collider2D>();
        //         foreach (Collider2D pieceCollider in pieceColliders)
        //         {
        //             if (boundary.bounds.Intersects(pieceCollider.bounds))
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        // }
        // return true;
    }

    private bool CheckDensityGoals()
    {
        if (highDensityAreas.Length > 0) {
            for (int i = 0; i < highDensityAreas.Length; i++)
            {
                Transform highDensityArea = highDensityAreas[i];
                // find all pieces on the "Boxes" layer that are within the surrounding 2x2 area
                Collider2D[] colliders = Physics2D.OverlapBoxAll(highDensityArea.position, new Vector2(2f, 2f), 0f);
                int boxCount = 0;
                List<int> countedPieceIDs = new List<int>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Boxes"))
                    {
                        int pieceID = collider.gameObject.transform.parent.GetInstanceID();
                        if (!countedPieceIDs.Contains(pieceID))
                        {
                            countedPieceIDs.Add(pieceID);
                            boxCount++;
                        }
                    }
                }
                if (boxCount < 4)
                {
                    return false;
                }
            }
        }

        if (lowDensityAreas.Length > 0) {
            for (int i = 0; i < lowDensityAreas.Length; i++)
            {
                Transform lowDensityArea = lowDensityAreas[i];
                // find all pieces on the "Boxes" layer that are within the surrounding 2x2 area
                Collider2D[] colliders = Physics2D.OverlapBoxAll(lowDensityArea.position, new Vector2(2f, 2f), 0f);
                int boxCount = 0;
                List<int> countedPieceIDs = new List<int>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Boxes"))
                    {
                        int pieceID = collider.gameObject.transform.parent.GetInstanceID();
                        if (!countedPieceIDs.Contains(pieceID))
                        {
                            countedPieceIDs.Add(pieceID);
                            boxCount++;
                        }
                    }
                }
                Debug.Log("Low density area " + i + " box count: " + boxCount);
                if (boxCount > 2)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool CheckOptionalDensityGoals()
    {
        for (int i = 0; i < optionalHighDensityAreas.Length; i++)
        {
            Transform optionalHighDensityArea = optionalHighDensityAreas[i];
            // find all pieces on the "Boxes" layer that are within the surrounding 2x2 area
            Collider2D[] colliders = Physics2D.OverlapBoxAll(optionalHighDensityArea.position, new Vector2(2f, 2f), 0f);
            int boxCount = 0;
            List<int> countedPieceIDs = new List<int>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Boxes"))
                {
                    int pieceID = collider.gameObject.transform.parent.GetInstanceID();
                    if (!countedPieceIDs.Contains(pieceID))
                    {
                        countedPieceIDs.Add(pieceID);
                        boxCount++;
                    }
                }
            }
            if (boxCount < 4)
            {
                return false;
            }
        }

        for (int i = 0; i < optionalLowDensityAreas.Length; i++)
        {
            Transform optionalLowDensityArea = optionalLowDensityAreas[i];
            // find all pieces on the "Boxes" layer that are within the surrounding 2x2 area
            Collider2D[] colliders = Physics2D.OverlapBoxAll(optionalLowDensityArea.position, new Vector2(2f, 2f), 0f);
            int boxCount = 0;
            List<int> countedPieceIDs = new List<int>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Boxes"))
                {
                    int pieceID = collider.gameObject.transform.parent.GetInstanceID();
                    if (!countedPieceIDs.Contains(pieceID))
                    {
                        countedPieceIDs.Add(pieceID);
                        boxCount++;
                    }
                }
            }
            if (debugMode)
            {
                Debug.Log("Optional low density area " + i + " box count: " + boxCount);
            }
            if (boxCount > 2)
            {
                return false;
            }
        }

        return true;
    }
}
