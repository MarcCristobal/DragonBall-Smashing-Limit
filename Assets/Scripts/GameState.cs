using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static List<int> availableEnemies = new List<int> { 0, 1, 2, 3 };
    public static int playerCode;
    public static int enemyCode;
    public static int gameNumber = 0;

    public static void ResetGame()
    {
        availableEnemies = new List<int> { 0, 1, 2, 3 };
        gameNumber = 0;
    }

    public static void RemoveEnemy(int code)
    {
        availableEnemies.Remove(code);
    }

    public static bool HasMoreEnemies()
    {
        return availableEnemies.Count > 0;
    }

    public static void SelectNextEnemy()
    {
        if (HasMoreEnemies())
        {
            ++gameNumber;
            enemyCode = availableEnemies[Random.Range(0, availableEnemies.Count)];
            RemoveEnemy(enemyCode);
        }
    }

    public static void SetPlayerCode(int code)
    {
        playerCode = code;
    }
}
