using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UniversalRendererData _renderData;

    public void PixelationSetting()
    {
        if(_renderData.rendererFeatures[2].isActive)
        {
            _renderData.rendererFeatures[2].SetActive(false);
        }
        else if (!_renderData.rendererFeatures[2].isActive)
        {
            _renderData.rendererFeatures[2].SetActive(true);
        }
    }
}
