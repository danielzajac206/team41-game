using UnityEngine;

public class StarPuzzleController : MonoBehaviour
{
    public int[] correctOrder = { 0, 1, 2, 3, 4 };

    public StarSwitch[] switches;

    public GameObject doorToOpen;

    public GameObject chestPrefab;
    public Vector3 chestSpawnPoint = new Vector3(-30f, 20f, 0f);

    private int progress = 0;
    private bool puzzleSolved = false;

    public bool IsSolved => puzzleSolved;

    public void HitSwitch(StarSwitch sw)
    {
        if (puzzleSolved) return;

        int expectedIndex = correctOrder[progress];

        if (sw.switchIndex == expectedIndex)
        {
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
        progress = 0;
        foreach (var s in switches)
        {
            s.ResetSwitch();
        }
    }

    private void OnPuzzleSolved()
    {
        if (doorToOpen != null)
        {
            doorToOpen.SetActive(false); 
        }

        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, chestSpawnPoint, Quaternion.identity);
        }
    }
}