using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Tracks")]
    [SerializeField] private AudioClip introTrack;      // intothemud.mp3
    [SerializeField] private AudioClip loopTrack;       // intothemud Loop.mp3

    private AudioSource audioSource;
    private bool hasPlayedIntro = false;
    bool stop = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // If no AudioSource exists, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start playing the intro
        PlayIntro();
    }

    void Update()
    {
        if (stop)
        {
            return;
        }
        // Check if intro has finished and switch to loop
        if (!hasPlayedIntro && !audioSource.isPlaying)
        {
            hasPlayedIntro = true;
            PlayLoop();
        }
    }

    void PlayIntro()
    {
        if (introTrack != null)
        {
            audioSource.clip = introTrack;
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            // If no intro, go straight to loop
            hasPlayedIntro = true;
            PlayLoop();
        }
    }

    void PlayLoop()
    {
        if (loopTrack != null)
        {
            audioSource.clip = loopTrack;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        stop = true;
    }
}