using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    public GameObject player;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        player.transform.position = startPos.position;
    }
}
