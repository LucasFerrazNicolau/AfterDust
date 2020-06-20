using DG.Tweening;
using UnityEngine;

public class YouWinManager : MonoBehaviour
{
    // Child objects
    public GameObject youWinText;
    public GameObject thanksText;

    // Audio
    private AudioSource audioSource;
    public AudioClip winTheme;

    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        audioSource = gameObject.AddComponent<AudioSource>();
        PlayGameOverSound();

        transform.DOMoveX(0, 0.5f).OnComplete(TerminateAnimation);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(winTheme);
    }

    private void TerminateAnimation()
    {
        youWinText.SetActive(true);
        thanksText.gameObject.SetActive(true);

        SpriteRenderer srGameOver = youWinText.GetComponent<SpriteRenderer>();
        srGameOver.DOFade(0.5f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
