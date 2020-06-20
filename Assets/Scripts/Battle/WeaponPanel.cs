using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    private Image img;
    private Text textChildComponent;
    private Image imageChildComponent;
    private PlayerController playerController;
    private WeaponBase weapon;

    public int weaponIndex;
    public GameObject player;
    public Sprite defaultPanel;
    public Sprite selectedPanel;

    private void Start()
    {
        img = GetComponent<Image>();
        textChildComponent = transform.Find("Bullets Text").gameObject.GetComponent<Text>();
        imageChildComponent = transform.Find("Image").gameObject.GetComponent<Image>();
        playerController = player.GetComponent<PlayerController>();
        weapon = playerController.weapons[weaponIndex];

        if (weapon.magazineSize != 0)
            textChildComponent.text = "x" + weapon.magazineSize;

        imageChildComponent.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        imageChildComponent.enabled = true;
    }

    private void Update()
    {
        if (playerController.equippedWeaponIndex == weaponIndex)
            img.sprite = selectedPanel;
        else
            img.sprite = defaultPanel;

        if (weapon.magazineSize != 0)
            textChildComponent.text = "x" + weapon.currentMagazine;
    }
}
