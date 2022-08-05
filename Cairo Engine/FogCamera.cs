using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FogCamera : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    public void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        RenderSettings.fog = true;
    }

    public void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        RenderSettings.fog = true;
    }

    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
}
