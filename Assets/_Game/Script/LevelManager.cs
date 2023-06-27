using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            return instance;
        }
    }

    [SerializeField] TextAsset mapData;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject unbrickPrefab;
    [SerializeField] GameObject unbrickCrossPrefab;
    [SerializeField] GameObject unbrickTurnPrefab;
    [SerializeField] GameObject startPosPrefab;
    [SerializeField] GameObject endPosPrefab;

    private GameObject startPos;
    private GameObject endPos;

    private string[,] mapType;
    private int currentLevel;

    public bool isCompleted = false;

    public GameObject player;
    public GameObject map;

    private void Start()
    {
        currentLevel = 1;
        OnInit(currentLevel);
    }

    private void Update()
    {
        if (isCompleted)
        {
            UIManager.Instance.SetCompletePanel(true);
            UIManager.Instance.SetCompleteText(currentLevel);
        }   
    }

    private void OnInit(int mapNumber)
    {
        isCompleted = false;
        UIManager.Instance.SetPoint(0);
        UIManager.Instance.SetCompletePanel(false);

        string mapName = "Map_" + mapNumber.ToString();
        mapType = MapData.Instance.ReadMapData(mapName);
        map = new GameObject("Map");
        //Debug.Log("i: " + mapType.GetLength(0) + ". j: " + mapType.GetLength(1));
        for (int i = mapType.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < mapType.GetLength(1); j++)
            {
                //Debug.Log("[" + i + "," + " " + j + "]");
                SetType(mapType[i, j], map, new Vector3(i, 0f, j));
            }
        }

        player.GetComponent<Player>().OnInit();
        player.transform.position = startPos.transform.position;
    }

    private void SetType(string type, GameObject map, Vector3 pos)
    {
        switch (type)
        {
            case "-1":
                return;
            case "0":
                Instantiate(wallPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "1":
                Instantiate(brickPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "2":
                Instantiate(unbrickPrefab, pos, Quaternion.identity, map.transform).transform.Rotate(Vector3.up, 90f);
                return;
            case "2.0":
                Instantiate(unbrickPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "2.1":
                Instantiate(unbrickCrossPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "2.2":
                //Turn left and move horizontal
                Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform).transform.Rotate(Vector3.up, 180f);
                return;
            case "2.3":
                //Turn right and move vertical
                Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "2.4":
                //Turn left and move vertical
                Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform).transform.Rotate(Vector3.up, 90f);
                return;
            case "2.5":
                //Turn right and move horizontal
                Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform).transform.Rotate(Vector3.up, 270f);
                return;
            case "3":
                startPos = Instantiate(startPosPrefab, pos, Quaternion.identity, map.transform);
                return;
            case "4":
                endPos = Instantiate(endPosPrefab, pos, Quaternion.identity, map.transform);
                return;
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        ClearMap();
        OnInit(currentLevel);
    }

    private void ClearMap()
    {
        Destroy(map);
    }
}
