using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{

    public Skybox box;
    public float speed = 5f;

    void LateUpdate()
    {
        box.material.SetFloat("_Rotation", Time.time * speed);
    }
}
