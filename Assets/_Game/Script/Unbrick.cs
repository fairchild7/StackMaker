using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unbrick : MonoBehaviour
{
    public bool active;
    public Vector3 position;
    public float rotateAngle;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        active = true;
    }
}
