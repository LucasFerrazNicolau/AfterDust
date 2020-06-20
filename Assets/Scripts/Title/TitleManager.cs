using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject title;
    public GameObject newGameText;

    public GameObject zombie;
    public GameObject bat;
    private int zombieDirection;
    private int batDirection;

    private void Start()
    {
        zombieDirection = 1;
        batDirection = 1;

        SpriteRenderer srTitle = title.GetComponent<SpriteRenderer>();

        srTitle.color = new Color(srTitle.color.r, srTitle.color.g, srTitle.color.b, 0);
        srTitle.enabled = true;

        srTitle.DOFade(1, 2.9f).OnComplete(TerminateAnimation);
    }

    private void Update()
    {
        Vector3 zombieNewPosition = zombie.transform.position;
        zombieNewPosition.x += (0.2f * zombieDirection);
        zombie.transform.position = zombieNewPosition;
        if (Mathf.Abs(zombieNewPosition.x) > 200)
        {
            zombieDirection *= -1;
            zombie.GetComponent<SpriteRenderer>().flipX ^= true;
        }

        Vector3 batNewPosition = bat.transform.position;
        batNewPosition.x -= (1.4f * batDirection);
        bat.transform.position = batNewPosition;
        if (Mathf.Abs(batNewPosition.x) > 200)
        {
            batDirection *= -1;
            bat.GetComponent<SpriteRenderer>().flipX ^= true;
        }
    }

    private void TerminateAnimation()
    {
        newGameText.SetActive(true);
        Image imgNewGame = newGameText.GetComponent<Image>();
        imgNewGame.DOFade(0.5f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
