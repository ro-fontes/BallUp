﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_DissolveOverTime : MonoBehaviour
{
    private MeshRenderer meshRenderer;


    public float speed = .5f;

    private void Start(){
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private float t = 1f;
    private void Update(){
        Material[] mats = meshRenderer.materials;

        mats[0].SetFloat("_Cutoff", t * speed);
        if(t >= 0)
        {
            t -= Time.deltaTime;
        }
        
        // Unity does not allow meshRenderer.materials[0]...
        meshRenderer.materials = mats;
    }
}
