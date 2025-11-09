using UnityEngine;
using UnityEngine.Playables;

public class ReturnToMainCamera : MonoBehaviour
{
    public PlayableDirector director;
    public Camera mainCamera;
    public Camera[] timelineCameras;

    void Start()
    {
        director.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector d)
    {
        // Desactiva todas las cámaras de la cinemática
        foreach (var cam in timelineCameras)
            cam.enabled = false;

        // Activa la cámara principal
        mainCamera.enabled = true;
    }

    void OnDestroy()
    {
        director.stopped -= OnTimelineStopped;
    }
}
