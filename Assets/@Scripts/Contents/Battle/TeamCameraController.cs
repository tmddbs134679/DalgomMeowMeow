using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _vCamFollowMain;
    [SerializeField] private CinemachineVirtualCamera _vCamVictory;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.orthographic = true;
    }
    private void Start()
    {
        _vCamFollowMain.Priority = 10;
        _vCamVictory.Priority = 0;
    }

    public void Victory()
    {
        _mainCamera.orthographic = false;
        _vCamVictory.Priority = 20;
    }


}
