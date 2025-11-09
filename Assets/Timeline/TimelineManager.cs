using UnityEngine;
using UnityEngine.Playables;//Importar PlayableDirector
using DG.Tweening;
using UnityEngine.UI; //Importar UI


public class TimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    //public Image timelineFader;
    public Transform playerMesh;
    public KeyCode skipKey;
    private float fadeDuration = 1;
    private float timelineDuration;


    void Awake()
    {
        timelineDuration = (float)playableDirector.playableAsset.duration;//obtener duración de la timeline
    }


    void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            SkipTimeline();
        }
    }

    public void SkipTimeline()
    {
        //setar que la duración de la timeline sea el último segundo de esta
        playableDirector.time = timelineDuration;
    }

    public void TimelineEnd()
    {
        playerMesh.localPosition = Vector3.zero;
        //timelineFader.DOFade(0, fadeDuration);
    }
}