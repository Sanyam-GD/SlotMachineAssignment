using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance for global audio access
    public static AudioManager Instance;

    [Header("Audio Sources")]

    // Handles UI, win/lose and reel stop sounds
    [SerializeField] private AudioSource sfxSource;

    // Handles looping reel spinning audio
    [SerializeField] private AudioSource reelLoopSource;

    // Handles background music
    [SerializeField] private AudioSource musicSource;

    [Header("Button Sounds")]

    // Sound played when spin button is pressed
    [SerializeField] private AudioClip buttonClick;

    // Sound played when increasing/decreasing bet
    [SerializeField] private AudioClip betClick;

    [Header("Reel Sounds")]

    // Continuous reel spinning loop sound
    [SerializeField] private AudioClip reelSpin;

    // Reel stop tick sound
    [SerializeField] private AudioClip reelStop;

    [Header("Result Sounds")]

    // Winning sound effect
    [SerializeField] private AudioClip winSound;

    // Losing sound effect
    [SerializeField] private AudioClip loseSound;

    [Header("Background Music")]

    // Main casino background music
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        // Ensure only one AudioManager exists in scene
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start background music automatically
        PlayBackgroundMusic();
    }

    // Plays lever pull sound
    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClick);
    }

    // Plays bet button click sound
    public void PlayBetClick()
    {
        sfxSource.PlayOneShot(betClick);
    }

    // Starts looping reel spinning sound
    public void StartReelSpin()
    {
        // Prevent replay spam if already playing
        if (reelLoopSource.isPlaying)
            return;

        reelLoopSource.clip = reelSpin;

        reelLoopSource.loop = true;

        reelLoopSource.Play();
    }

    // Stops reel spinning loop sound
    public void StopReelSpin()
    {
        reelLoopSource.Stop();

        reelLoopSource.clip = null;
    }

    // Plays reel stop sound with slight random pitch to create a more natural casino feel
    public void PlayReelStop()
    {
        sfxSource.pitch = Random.Range(0.95f, 1.05f);

        sfxSource.PlayOneShot(reelStop);

        // Reset pitch back to default
        sfxSource.pitch = 1f;
    }

    // Plays winning sound effect
    public void PlayWin()
    {
        sfxSource.PlayOneShot(winSound);
    }

    // Plays losing sound effect
    public void PlayLose()
    {
        sfxSource.PlayOneShot(loseSound);
    }

    // Starts looping background music
    public void PlayBackgroundMusic()
    {
        // Prevent errors if music clip is missing
        if (backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;

        musicSource.loop = true;

        musicSource.Play();
    }

    // Stops background music
    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
}