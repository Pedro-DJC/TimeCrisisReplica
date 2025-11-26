using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class ReturnToMainCamera : MonoBehaviour
{
    [Header("Asignar desde el inspector")]
    public PlayableDirector director;                  // Tu Timeline
    public CinemachineCamera mainVCam;          // La cámara principal (por ejemplo, la del riel)
    public CinemachineCamera[] timelineVCams;   // Cámaras de la Timeline

    void Start()
    {
        director.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector d)
    {
        // Bajar prioridad de las cámaras usadas en la Timeline
        foreach (var vcam in timelineVCams)
        {
            if (vcam != null)
                vcam.Priority = 0;
        }

        // Subir prioridad de la cámara principal (la del riel)
        if (mainVCam != null)
            mainVCam.Priority = 20;

        Debug.Log("Timeline terminada: regresando a la cámara principal (en riel).");
    }

    void OnDestroy()
    {
        director.stopped -= OnTimelineStopped;
    }
}
