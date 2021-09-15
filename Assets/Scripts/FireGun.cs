using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    public ParticleSystem sparkles;
    private bool _shoot;
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) { _shoot = true; }
        else { _shoot = false; }

        if (_shoot) { sparkles.Play(); }
        else { sparkles.Stop(); }
    }
}
