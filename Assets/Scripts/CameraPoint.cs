using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    private Transform _player;
    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        transform.position = _player.position;
    }
}
