using DG.Tweening;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // Child objects
    public GameObject gameOverText;
    public GameObject backToTitleButton;

    // Audio
    private AudioSource audioSource;
    public AudioClip gameOverTheme;

    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        audioSource = gameObject.AddComponent<AudioSource>();
        PlayGameOverSound();

        transform.DOMoveX(0, 0.5f).OnComplete(TerminateAnimation);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverTheme);
    }

    private void TerminateAnimation()
    {
        gameOverText.SetActive(true);
        backToTitleButton.gameObject.SetActive(true);

        SpriteRenderer srGameOver = gameOverText.GetComponent<SpriteRenderer>();
        srGameOver.DOFade(0.5f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
