using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZoneController : MonoBehaviour
{
    [Header("Win Screen")]
    public GameObject winPanel;               

    [Header("Referencias")]
    public GunScript gunScript;               
    public string playerTag = "Player";       
    public string mainMenuSceneName = "MainMenu";

    private bool hasWon = false;

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        if (other.CompareTag(playerTag))
        {
            hasWon = true;

            Time.timeScale = 0f;

            if (gunScript != null)
                gunScript.enabled = false;

            if (winPanel != null)
                winPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("[WinZone] Juego detenido y Win Screen mostrada.");
        }
    }

    public void RestartLevel()
    {

        if (gunScript != null)
            gunScript.enabled = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (gunScript != null)
            gunScript.enabled = true;

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
