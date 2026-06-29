using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameUIManager.Instance.TogglePauseUI();
        }

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
    public void ReturnToTheMainMenu()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
