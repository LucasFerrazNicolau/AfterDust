using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cursor;
    public string nextScene;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(StartNewGame);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cursor != null)
            cursor.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cursor != null)
            cursor.SetActive(false);
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}
