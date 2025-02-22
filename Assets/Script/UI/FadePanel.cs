using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
public class FadePanel : MonoBehaviour
{
    VisualElement bg;
    
    private void Awake() {
        bg = GetComponent<UIDocument>().rootVisualElement;
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0f,1f,duration,value => {
            bg.style.opacity = value;
        } ).SetEase(Ease.InQuad);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1f,0f,duration,value => {
            bg.style.opacity = value;
        } ).SetEase(Ease.OutQuad);
    }
}
