using UnityEngine;

public class StarPuzzleController : MonoBehaviour
{
    // Set the order you want: BL, Top, BR, ML, MR
    public int[] correctOrder = { 0, 1, 2, 3, 4 };

    // Drag all 5 switches here in the Inspector
    public StarSwitch[] switches;

    public GameObject doorToOpen;

    private int progress = 0;
    private bool puzzleSolved = false;

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
            doorToOpen.SetActive(false); // “open” the door
        }
    }
}