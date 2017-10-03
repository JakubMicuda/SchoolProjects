using UnityEngine;
using System.Collections;

public static class LeanTweenExtension
{

    public static void AnimateAlpha(this tk2dSprite sprite, float from, float to, float time, bool destroy = false)
    {
        LeanTween.value(sprite.gameObject, sprite.SetAlpha, from, to, time)
            .setEase(LeanTweenType.easeInOutQuad)
            .destroyOnComplete = destroy;
    }

    public static void AnimateAlpha(this tk2dTextMesh txt, float from, float to, float time, bool destroy = false)
    {
        LeanTween.value(txt.gameObject, txt.SetAlpha, from, to, time)
            .setEase(LeanTweenType.easeInOutQuad)
            .destroyOnComplete = destroy;
    }

    public static void SetAlpha(this tk2dBaseSprite sprite, float alpha)
    {
        Color c = sprite.color;
        c.a = Mathf.Clamp01(alpha);

        sprite.color = c;
    }

    public static void SetAlpha(this tk2dTextMesh textMesh, float alpha)
    {
        Color c = textMesh.color;
        c.a = Mathf.Clamp01(alpha);

        textMesh.color = c;
        textMesh.ForceBuild();
    }

}
