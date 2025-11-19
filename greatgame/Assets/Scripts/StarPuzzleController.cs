using UnityEngine;

public class StarPuzzleController : MonoBehaviour
{
    // Set the order you want: BL, Top, BR, ML, MR (assuming these map to indices 0-4)
    public int[] correctOrder = { 0, 1, 2, 3, 4 };

    // Drag all 5 switches here in the Inspector
    public StarSwitch[] switches;

    public GameObject doorToOpen;

    // --- NEW: For Chest Spawning ---
    public GameObject chestPrefab;
    // Spawn position for the chest. Default X=-30, Y=20, Z=0.
    public Vector3 chestSpawnPoint = new Vector3(-30f, 20f, 0f);
    // ---------------------------------

    private int progress = 0;
    private bool puzzleSolved = false;

    // --- NEW: Public accessor for the solved status ---
    public bool IsSolved => puzzleSolved;
    // ---------------------------------------------------

    public void HitSwitch(StarSwitch sw)
    {
        if (puzzleSolved) return;

        int expectedIndex = correctOrder[progress];

        if (sw.switchIndex == expectedIndex)
        {
            // ... (existing progress logic)
            sw.SetOn(true);
            progress++;

            if (progress >= correctOrder.Length)
            {
                puzzleSolved = true;
                OnPuzzleSolved();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        Debug.Log("Wrong switch, resetting puzzle");
        progress = 0;
        foreach (var s in switches)
        {
            s.ResetSwitch();
        }
    }

    private void OnPuzzleSolved()
    {
        Debug.Log("Star puzzle solved!");

        if (doorToOpen != null)
        {
            // Assuming setting it inactive "opens" the door
            doorToOpen.SetActive(false); 
        }

        // --- NEW: Spawn the Chest ---
        if (chestPrefab != null)
        {
            // Instantiate the chest at the designated position (Z=0) with default rotation
            Instantiate(chestPrefab, chestSpawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Chest Prefab is not assigned in the Inspector.");
        }
        // -----------------------------
    }
}