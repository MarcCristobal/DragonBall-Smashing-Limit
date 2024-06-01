using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public Controls controls;

    public float defaultTime;
    private float currentTime;
    public TMP_Text timerText;

    public int playerCode;
    public int enemyCode;

    public bool isGameFinished;
    public bool isEnemyDead;
    public bool isPlayerDead;
    public bool isEnemyWin;
    public bool isPlayerWin;
    public bool isTie;
    private bool isPaused;

    public float playerHealth;
    public float enemyHealth;

    public List<GameObject> playerCharList;
    public List<GameObject> enemyCharList;

    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject pauseMenu;


    void Awake()
    {
        controls = new();
        gameController = this;
        playerCode = CharCode.charCode.playerCode;
        enemyCode = CharCode.charCode.enemyCode;
        SpawnChar(playerCode, enemyCode);
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameFinished = false;
        currentTime = defaultTime;
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGameFinished();
        UpdateUI();

        controls.Movement.Pause.started += _ => Pause();
    }

    void CheckIsGameFinished()
    {
        if (isGameFinished)
        {
            if (isPlayerWin)
            {
                winMenu.SetActive(true);
            }
            if (isEnemyWin)
            {
                loseMenu.SetActive(true);
            }
            return;
        }

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            isGameFinished = true;
            if (playerHealth > enemyHealth)
            {
                isPlayerWin = true;
            }
            else if (playerHealth < enemyHealth)
            {
                isEnemyWin = true;
            }
            else if (playerHealth == enemyHealth)
            {
                isTie = true;
            }
        }
        else if (isEnemyDead)
        {
            isGameFinished = true;
            isPlayerWin = true;
        }
        else if (isPlayerDead)
        {
            isGameFinished = true;
            isEnemyWin = true;
        }
    }

    void UpdateUI()
    {
        timerText.text = currentTime.ToString("0");
    }

    void SpawnChar(int player, int enemy)
    {
        Instantiate(playerCharList[player]);
        Instantiate(enemyCharList[enemy]);
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);
        }
    }
}
