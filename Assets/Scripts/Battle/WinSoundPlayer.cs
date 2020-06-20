using UnityEngine;

public class WinSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private bool soundPlayed;

    public AudioClip winSound;
    public bool levelEnded;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = winSound;

        soundPlayed = false;
        levelEnded = false;
    }

    private void Update()
    {
        if (audioSource.isPlaying)
            soundPlayed = true;
        else
        {
            if (soundPlayed && !levelEnded)
            {
                levelEnded = true;
                BattleManager.Instance.NewLevel();
            }
        }
    }

    public void PlayWinSound()
    {
        audioSource.Play();
    }
}
