using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPlayer(int code)
    {
        GameState.playerCode = code;
        SelectPlayer();
    }

    public void SelectPlayer()
    {
        if (GameState.HasMoreEnemies())
        {
            GameState.RemoveEnemy(GameState.playerCode);
            GameState.SelectNextEnemy();
            CharCode.charCode.playerCode = GameState.playerCode;
            CharCode.charCode.enemyCode = GameState.enemyCode;

            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            GameState.ResetGame();
            MainMenu();
        }

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameState.ResetGame();
        SceneManager.LoadScene("MainMenu");
    }

}
