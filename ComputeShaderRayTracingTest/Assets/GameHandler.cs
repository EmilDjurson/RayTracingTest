using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameHandler : MonoBehaviour
{
    [Header("RayTraces")]
    public float RayTraces;
    public TextMeshProUGUI RayTracingCount;

    [Header("Sphere Count")]
    public uint Spheres;
    public TextMeshProUGUI sphereCountText;

    [Header("Sphere Radius")]
    public float SpherePlacementRadius;
    public TextMeshProUGUI SphereRadiusText;

    [Header("Miscellaneous")]
    public GameObject SettingsGroup;
    public GameObject StopGroup;

    [Header("Scripts")]
    public RayTracingMaster rayTracing;
    public CameraMovement cameraMovement;

    private void Start()
    {
        SettingsGroup.SetActive(true);
        rayTracing.enabled = false;
        StopGroup.SetActive(false);
    }

    public void SetRayTraces(float rayTraces)
    {
        RayTraces = rayTraces;
        RayTracingCount.text = (RayTraces - 1).ToString();
    }

    public void setSphereCount(float sphereCount)
    {
        Spheres = (uint) sphereCount;
        sphereCountText.text = Spheres.ToString();
    }

    public void setSpheresPlacementRadius(float radius)
    {
        SpherePlacementRadius = radius;
        SphereRadiusText.text = SpherePlacementRadius.ToString();
    }

    public void _Start()
    {
        SettingsGroup.SetActive(false);
        rayTracing.enabled = true;
        StopGroup.SetActive(true);
        rayTracing._Start();
        cameraMovement.enabled = true;
    }

    public void _Stop()
    {
        SettingsGroup.SetActive(true);
        rayTracing.enabled = false;
        StopGroup.SetActive(false);
        cameraMovement.enabled = false;
    }
}
