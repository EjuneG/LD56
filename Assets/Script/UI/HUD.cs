using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]Image HPBar;
    [SerializeField]Image XPBar;
    Slime player;

    void Start(){
        player = GameManager.Instance.Player.GetComponent<Slime>();
    }
    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = player.CurrentHP / player.MaxHP;
        XPBar.fillAmount = player.Energy / player.EnergyForNextLevel;
    }
}
