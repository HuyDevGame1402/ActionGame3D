using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterBase playerCharacter;
    private bool gameIsOver;

    private void Awake()
    {
        Instance = this;
    }

    private void GameOver()
    {

    }

    public void GameIsFinished()
    {

    }

    private void Update()
    {
        if (gameIsOver) return;

        if(playerCharacter.currentState == CharacterBase.CharacterState.Dead)
        {
            gameIsOver = true;
            GameOver();
        }
    }

    public CharacterBase GetPlayerCharacter()
    {
        return playerCharacter; 
    }
}
