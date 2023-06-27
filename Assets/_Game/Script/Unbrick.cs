using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unbrick : MonoBehaviour
{
    public bool active;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        active = true;
    }
}
