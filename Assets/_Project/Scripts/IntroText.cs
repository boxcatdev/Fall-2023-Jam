using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class IntroText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textBox;
    [SerializeField] float _fadeTime;
    [SerializeField] Image _background;

    private void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(1f, _textBox));
    }
    void Update()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            StartCoroutine(FadeTextToZeroAlpha(_fadeTime, _textBox));
            StartCoroutine(FadeTextToZeroAlpha(_fadeTime, _background));
        }
    }
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}