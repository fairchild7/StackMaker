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

    [SerializeField] Wall wallPrefab;
    [SerializeField] Brick brickPrefab;
    [SerializeField] Unbrick unbrickPrefab;
    [SerializeField] Unbrick unbrickCrossPrefab;
    [SerializeField] Unbrick unbrickTurnPrefab;
    [SerializeField] GameObject startPosPrefab;
    [SerializeField] GameObject endPosPrefab;

    private GameObject startPos;
    private GameObject endPos;

    private string[,] mapType;
    private int currentLevel;
    private List<Brick> listBrick;
    private List<Wall> listWall;
    private List<Unbrick> listUnbrick; 

    public bool isCompleted = false;
    public bool isGameOver = false;

    public GameObject player;
    public GameObject map;

    private void Start()
    {
        listWall = new List<Wall>();
        listBrick = new List<Brick>();
        listUnbrick = new List<Unbrick>();
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
        if (isGameOver)
        {
            UIManager.Instance.SetRetryPanel(true);
        }
    }

    private void OnInit(int mapNumber)
    {
        isCompleted = false;
        isGameOver = false;
        UIManager.Instance.SetPoint(0);
        UIManager.Instance.SetCompletePanel(false);
        UIManager.Instance.SetRetryPanel(false);

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
                Wall wall = Instantiate(wallPrefab, pos, Quaternion.identity, map.transform);
                wall.position = pos;
                listWall.Add(wall);
                return;
            case "1":
                Brick brick = Instantiate(brickPrefab, pos, Quaternion.identity, map.transform);
                brick.position = pos;
                listBrick.Add(brick);
                return;
            case "2":
                Unbrick unbrick = Instantiate(unbrickPrefab, pos, Quaternion.identity, map.transform);
                unbrick.transform.Rotate(Vector3.up, 90f);
                unbrick.rotateAngle = -1f;
                listUnbrick.Add(unbrick);
                return;
            case "2.0":
                Unbrick unbrick1 = Instantiate(unbrickPrefab, pos, Quaternion.identity, map.transform);
                unbrick1.rotateAngle = -1f;
                listUnbrick.Add(unbrick1);
                return;
            case "2.1":
                Unbrick unbrick2 = Instantiate(unbrickCrossPrefab, pos, Quaternion.identity, map.transform);
                unbrick2.rotateAngle = -1f;
                listUnbrick.Add(unbrick2);
                return;
            case "2.2":
                //Turn left and move horizontal
                Unbrick unbrick3 = Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform);
                unbrick3.transform.Rotate(Vector3.up, 180f);
                unbrick3.rotateAngle = 180f;
                listUnbrick.Add(unbrick3);
                return;
            case "2.3":
                //Turn right and move vertical
                Unbrick unbrick4 = Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform);
                unbrick4.rotateAngle = 0f;
                listUnbrick.Add(unbrick4);
                return;
            case "2.4":
                //Turn left and move vertical
                Unbrick unbrick5 = Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform);
                unbrick5.transform.Rotate(Vector3.up, 90f);
                unbrick5.rotateAngle = 90f;
                listUnbrick.Add(unbrick5);
                return;
            case "2.5":
                //Turn right and move horizontal
                Unbrick unbrick6 = Instantiate(unbrickTurnPrefab, pos, Quaternion.identity, map.transform);
                unbrick6.transform.Rotate(Vector3.up, 270f);
                unbrick6.rotateAngle = 270f;
                listUnbrick.Add(unbrick6);
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

    public void ResetLevel()
    {
        ClearMap();
        OnInit(currentLevel);
    } 

    private void ClearMap()
    {
        Destroy(map);
    }
}
