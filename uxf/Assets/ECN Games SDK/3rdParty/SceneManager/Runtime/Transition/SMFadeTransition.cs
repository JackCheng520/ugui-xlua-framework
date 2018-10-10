//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;

/// <summary>
/// A transition that simply fades out the old level and fades in the new one.
/// </summary>
[AddComponentMenu("Scripts/SceneManager/Fade Transition")]
public class SMFadeTransition : SMTransition
{

    /// <summary>
    ///  The fade time.
    /// </summary>
    public float duration = 1f;

    /// <summary>
    /// The overlay texture.
    /// </summary>
    public Texture[] overlayTexture;

    /// <summary>
    /// Loading Icon.
    /// </summary>
    public Texture loadingIcon;

    /// <summary>
    /// Loading Bg.
    /// </summary>
    public Texture loadingBg;

    /// <summary>
    /// Loading Front.
    /// </summary>
    public Texture loadingFront;

    /// <summary>
    /// Float Point. 
    /// </summary>
    public Texture loadingFloatPoint;

    /// <summary>
    /// 加载进度
    /// </summary>
    private float progress;

    //private float randPro = 0.5f;
    private float randPro = 0.0f;

    //随机的loading背景.
    private int randomOverlayTexture = 0;

    void Awake()
    {
        if (overlayTexture == null)
        {
            Debug.LogError("Overlay texture is missing");
        }

        //randPro = UnityEngine.Random.Range(0.25f, 0.75f);
        randomOverlayTexture = Random.Range(0, overlayTexture.Length);
    }

    protected override bool Process(float elapsedTime)
    {
        float effectTime = elapsedTime;

        if (state == SMTransitionState.Out)
        {
            progress = SMTransitionUtils.SmoothProgress(0, duration, effectTime) * randPro;
        }
        else if (state == SMTransitionState.In)
        {
            progress = randPro + SMTransitionUtils.SmoothProgress(0, duration, effectTime) * (1.0f - randPro);
        }

        if (state == SMTransitionState.In)
        {
            progress = randPro + SMTransitionUtils.SmoothProgress(0, duration, effectTime) * (1.0f - randPro);
        }

        return elapsedTime < duration;
    }

    public void OnGUI()
    {
        if (progress >= 1) return;

        GUI.depth = 0;
        Color c = GUI.color;
        //GUI.color = new Color(1, 1, 1, progress);
        GUI.color = c;

        //bg
        if (overlayTexture[randomOverlayTexture])
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlayTexture[randomOverlayTexture]);
        }

        //loading icon
        if (loadingIcon)
        {
            //GUI.DrawTexture(new Rect((Screen.width - loadingIcon.width) * 0.5f, Screen.height * 0.725f, loadingIcon.width, loadingIcon.height), loadingIcon);
        }

        //loading bg
        if (loadingBg)
        {
            GUI.DrawTexture(new Rect((Screen.width - loadingBg.width) * 0.5f, Screen.height * 0.85f, loadingBg.width, loadingBg.height), loadingBg);

            //loading front
            if (loadingFront)
            {
                //指定要显示在的屏幕区域
                Rect destRect = new Rect((Screen.width - loadingFront.width) * 0.5f,
                    Screen.height * 0.85f + (loadingBg.height - loadingFront.height) * 0.5f,
                    loadingFront.width * progress, loadingFront.height);

                //指定要显示的图片区域
                Rect sourceRect = new Rect(0, 0, progress, 1);

                GUI.DrawTextureWithTexCoords(destRect, loadingFront, sourceRect, true);
            }
        }

        //loading float point
        if (loadingFloatPoint)
        {
            GUI.DrawTexture(new Rect(((Screen.width - loadingFront.width - loadingFloatPoint.width) * 0.5f + loadingFront.width * progress), Screen.height * 0.85f + (loadingBg.height - loadingFloatPoint.height) * 0.5f, loadingFloatPoint.width, loadingFloatPoint.height), loadingFloatPoint);
        }
    }
}