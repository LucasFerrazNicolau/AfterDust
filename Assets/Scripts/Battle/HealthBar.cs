using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Text textChildComponent;
    private RectTransform redBar;
    private PlayerController player;

    private readonly float initialSize = 140;
    private float currentSize = 140;

    public GameObject playerObject;

    private void Start()
    {
        textChildComponent = transform.Find("Health Text").gameObject.GetComponent<Text>();
        redBar = transform.Find("Red Bar").gameObject.GetComponent<RectTransform>();
        player = playerObject.GetComponent<PlayerController>();

        textChildComponent.text = player.maxHealth + "/" + player.maxHealth;
    }

    private void Update()
    {
        textChildComponent.text = player.currentHealth + "/" + player.maxHealth;

        if ((currentSize * 1.0f / initialSize) > (player.currentHealth * 1.0f / player.maxHealth))
        {
            currentSize--;
            redBar.sizeDelta = new Vector2(redBar.sizeDelta.x - 1, redBar.sizeDelta.y);
            redBar.anchoredPosition = new Vector2(redBar.anchoredPosition.x - 0.5f, redBar.anchoredPosition.y);
        }
    }
}
