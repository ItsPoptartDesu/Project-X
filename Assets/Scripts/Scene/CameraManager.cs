using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private Camera PlayerCamera;

    [SerializeField]
    private Camera UICamera;

    public void Init()
    {
        UICamera.gameObject.SetActive(true);
        PlayerCamera.transform.SetParent(null);
        PlayerCamera.gameObject.SetActive(false);
    }

    public void ToGame()
    {
        PlayerCamera.gameObject.SetActive(true);
        UICamera.gameObject.SetActive(false);
    }
    public void GameToMainMenu()
    {
        PlayerCamera.gameObject.SetActive(false);
        UICamera.gameObject.SetActive(true);
    }

    public void AttachPlayerCamera(GameObject _player)
    {
        PlayerCamera.transform.LookAt(_player.transform);
        PlayerCamera.transform.SetParent(_player.transform);
    }
}
