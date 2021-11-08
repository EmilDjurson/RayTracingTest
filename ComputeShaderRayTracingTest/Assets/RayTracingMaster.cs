using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RayTracingMaster : MonoBehaviour
{
    public Texture SkyboxTexture;
    private Camera _camera;

    private uint _currentSample = 0;
    private Material _addMaterial;

    public float rayTraces = 2;
    public Slider rayTraceSlider;
    public TextMeshProUGUI rayTracesText;

    public float SphereColummns = 0;
    public Slider SphereColummnsSlider;
    public TextMeshProUGUI SphereColummnsText;

    public float SphereRows = 0;
    public Slider SphereRowsSlider;
    public TextMeshProUGUI SphereRowsText;

    private void Update()
    {
        if (transform.hasChanged)
        {
            _currentSample = 0;
            transform.hasChanged = false;
        }
    }

    private void Start()
    {
        SphereColummns = SphereColummnsSlider.value;
        RayTracingShader.SetFloat("SphereColummns", SphereColummns);
        SphereColummnsText.text = SphereColummns.ToString();

        SphereRows = SphereRowsSlider.value;
        RayTracingShader.SetFloat("SphereRows", SphereRows);
        SphereRowsText.text = SphereRows.ToString();

        rayTraces = 2;
        RayTracingShader.SetFloat("ammountOfTraces", rayTraces);
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
    }

    public void changeAmmountOfRayTraces()
    {
        rayTraces = rayTraceSlider.value;
        RayTracingShader.SetFloat("ammountOfTraces", rayTraces);
        rayTracesText.text = (rayTraces - 1).ToString();
    }

    public void AddMoreSpheres()
    {
        SphereColummns = SphereColummnsSlider.value;
        RayTracingShader.SetFloat("SphereColummns", SphereColummns);
        SphereColummnsText.text = (SphereColummns + 1).ToString();

        SphereRows = SphereRowsSlider.value;
        RayTracingShader.SetFloat("SphereRows", SphereRows);
        SphereRowsText.text = SphereRows.ToString();
    }

    public ComputeShader RayTracingShader;

    private RenderTexture _target;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        // Blit the result texture to the screen
        if (_addMaterial == null)
            _addMaterial = new Material(Shader.Find("Hidden/AddShader"));
        _addMaterial.SetFloat("_Sample", _currentSample);
        Graphics.Blit(_target, destination, _addMaterial);
        _currentSample++;
    }

    private void InitRenderTexture()
    {
        _currentSample = 0;
        SetShaderParameters();
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();
            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
}