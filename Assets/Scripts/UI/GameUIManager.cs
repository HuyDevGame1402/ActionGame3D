using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Slider healthSlider;
    private Health health;

    [SerializeField] private GameObject uiPause;
    [SerializeField] private GameObject uiGameOver;
    [SerializeField] private GameObject uiGameIsFinish;

    private enum GameUIState
    {
        GamePlay,
        GamePause,
        GameOver,
        GameIsFinish,
    }

    private GameUIState currentState;

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

    private void SwitchUIState(GameUIState state)
    {
        uiPause.SetActive(false);
        uiGameOver.SetActive(false);
        uiGameIsFinish.SetActive(false);

        Time.timeScale = 1f;

        switch (state)
        {
            case GameUIState.GamePlay:
                break;

            case GameUIState.GamePause:
                Time.timeScale = 0f;
                uiPause.SetActive(true);
                break;

            case GameUIState.GameOver:
                uiGameOver.SetActive(true);
                break;

            case GameUIState.GameIsFinish:
                uiGameIsFinish.SetActive(true);
                break;
        }
        currentState = state;
    }

    public void TogglePauseUI()
    {
        if(currentState == GameUIState.GamePlay)
        {
            SwitchUIState(GameUIState.GamePause);
        }
        else if(currentState == GameUIState.GamePause)
        {
            SwitchUIState(GameUIState.GamePlay);
        }
    }

}
