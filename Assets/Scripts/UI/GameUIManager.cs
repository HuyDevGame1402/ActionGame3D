using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Slider healthSlider;
    private Health health;

    private void Awake()
    {
        health = GameManager.Instance.GetPlayerCharacter().GetComponent<Health>();
        health.OnDamage += ChangeHealthUI;
        (GameManager.Instance.GetPlayerCharacter() as PlayerCharacter).AddCoinEventAction += PlayerCharacter_AddCoinEventAction;
    }

    private void PlayerCharacter_AddCoinEventAction(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void ChangeHealthUI(object sender, EventArgs e)
    {
        healthSlider.value = ((Health.OnDamageClass)e).healthPercentage;
    }
}
