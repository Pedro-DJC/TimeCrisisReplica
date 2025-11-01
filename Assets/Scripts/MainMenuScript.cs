using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;  

public class MainMenuScript : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        fadePanel.color = new Color(0, 0, 0, 1);
        fadePanel.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            fadePanel.raycastTarget = false;
        });
    }

    public void PlayGame()
    {
        fadePanel.raycastTarget = true;

        fadePanel.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit(); 
    }
}
