using UnityEngine;
using TMPro;    

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float startTime = 60f;   

    private float currentTime;
    private bool isRunning = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
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
}
