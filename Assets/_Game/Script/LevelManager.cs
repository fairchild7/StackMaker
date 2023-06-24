using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] TextAsset mapData;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject unbrickPrefab;
    [SerializeField] GameObject startPosPrefab;
    [SerializeField] GameObject endPosPrefab;

    private GameObject startPos;
    public Transform endPos;

    public GameObject player;

    private string[,] mapType;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        UIManager.Instance.SetPoint(0);
        mapType = MapData.Instance.ReadMapData(mapData.name);
        //Debug.Log("i: " + mapType.GetLength(0) + ". j: " + mapType.GetLength(1));
        for (int i = mapType.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < mapType.GetLength(1); j++)
            {
                //Debug.Log("[" + i + "," + " " + j + "]");
                SetType(mapType[i, j], new Vector3(i, 0f, j));
            }
        }
        
        player.transform.position = startPos.transform.position;
    }

    private void SetType(string type, Vector3 pos)
    {
        switch (type)
        {
            case "-1":
                return;
            case "0":
                Instantiate(wallPrefab, pos, Quaternion.identity);
                return;
            case "1":
                Instantiate(brickPrefab, pos, Quaternion.identity);
                return;
            case "2":
                Instantiate(unbrickPrefab, pos, Quaternion.identity).transform.Rotate(Vector3.up, 90f);
                return;
            case "3":
                startPos = Instantiate(startPosPrefab, pos, Quaternion.identity);
                return;
            case "4":
                //end
                return;
        }
    }
}
