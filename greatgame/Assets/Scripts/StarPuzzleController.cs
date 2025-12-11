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

    [SerializeField] AudioClip switchHit;
    [SerializeField] AudioClip solved;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HitSwitch(StarSwitch sw)
    {
        if (puzzleSolved) return;

        int expectedIndex = correctOrder[progress];

        if (sw.switchIndex == expectedIndex)
        {
            sw.SetOn(true);
            progress++;
            audioSource.clip = switchHit;
            audioSource.Play();

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
        audioSource.clip = solved;
        audioSource.Play();
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