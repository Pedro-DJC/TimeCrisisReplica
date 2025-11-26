using UnityEngine;
using TMPro;    

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject restartPanel; 

    public float startTime = 60f;   

    private float currentTime;
    private bool isRunning = true;

    public float timePerEnemyKill = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();

        if (restartPanel != null)
            restartPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            UpdateTimerText();
            Debug.Log("Time´s over");
            StopGame();
            return;
        }

        UpdateTimerText();
    }
    void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        int centiseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}",seconds, centiseconds);
    }
    void StopGame()
    {
        Time.timeScale = 0f;

        if (restartPanel != null)
            restartPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void AddTime(float seconds)
    {
        if (!isRunning) return;

        currentTime += seconds;
        UpdateTimerText();
    }

    public void OnEnemyKilled()
    {
        AddTime(timePerEnemyKill);
    }
}
