using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public Texture2D mouseCursor;

    private AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip tradeSound;

    // Stats
    public int damage;
    public int magazineSize;
    public int currentMagazine;
    public int cost;

    // Abstract methods
    public abstract void InitializeStats();
    public abstract List<Tile> GetWeaponRange(int x, int y);

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        InitializeStats();
        currentMagazine = magazineSize;
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound, 0.5f);
    }

    public void PlayTradeSound()
    {
        audioSource.PlayOneShot(tradeSound, 0.5f);
    }
}
