using System;
using UnityEngine;
using PrimeTween;
using TMPro;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public Vector3 originalPos;
    public Quaternion originalRot;
    public Slider timerSlider;
    public TextMeshProUGUI descriptionText;
    private float velocity;
    private Tween slideTween;
    
    private void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }
    
    private void PlayUseAnimation()
    {
        Tween.LocalPosition(this.transform, endValue: transform.localPosition + new Vector3(0, 0.5f, 0), duration: 0.25f,
            ease: Ease.InOutElastic, cycles: 2, CycleMode.Rewind);
    }
    
    
    public void StopAnimation()
    {
        slideTween.Complete();
        timerSlider.value = 0;
    }
    
    
    public void AnimateSlider(int cooldown)
    {
        timerSlider.value = 0;
        timerSlider.maxValue = cooldown * 20;
        slideTween = Tween.Custom(0,timerSlider.maxValue, duration: cooldown, ease: Ease.Linear, onValueChange:
            newVal =>
            {
                if (Mathf.Abs(newVal - timerSlider.maxValue) < 0.01f)
                {
                    timerSlider.value = timerSlider.maxValue;
                    slideTween.Complete();
                }
                else
                {
                    timerSlider.value = newVal;
                }
            });
    }
}
