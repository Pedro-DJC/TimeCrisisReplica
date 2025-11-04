using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostprocessManager : MonoBehaviour
{
    public Volume volume;
    public Vignette vigentte;
    private Tween vignetteTween;
    public ColorAdjustments colorAdjustments;
    private Tween adjustmentTween;

    void Start()
    {
        volume.profile.TryGet(out vigentte);
        volume.profile.TryGet(out colorAdjustments);
        vigentte.intensity.value = 0.3f;
        vigentte.color.value = Color.black;
        colorAdjustments.colorFilter.value = Color.white;
    }

    public void CrouchVignette(Color vignetteColor, float intensity, float tweenTime, int loops)
    {
        vigentte.color.value = vignetteColor;
        vignetteTween?.Kill();
        vignetteTween = DOTween.To(() => vigentte.intensity.value, x => vigentte.intensity.value = x, intensity, tweenTime).SetLoops(loops, LoopType.Yoyo).OnComplete(() => vigentte.intensity.value = 0);
    }
    public void DamageColorFilter(Color filterColor, Color endColor, float tweenTime)
    {
        colorAdjustments.colorFilter.value = filterColor;
        vignetteTween?.Kill();
        DOTween.To(() => colorAdjustments.colorFilter.value, x => colorAdjustments.colorFilter.value = x, endColor, tweenTime).SetLoops(2, LoopType.Yoyo);
    }
}
