using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapData : MonoBehaviour
{
    private static MapData instance;

    public static MapData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MapData>();
            }

            return instance;
        }
    }

    public int row;
    public int column;

    public string[,] ReadMapData(string fileName)
    {
        TextAsset mapData = Resources.Load<TextAsset>(fileName);
        if (mapData != null)
        {
            string[] lines = mapData.text.Split('\n');
            row = lines.Length;

            string[] cols = lines[0].Split(' ');
            column = cols.Length;
            //Debug.Log("Rows = " + row + ". Column = " + column);

            string[,] mapType = new string[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    string[] columns = lines[i].Split(' ');
                    mapType[i, j] = columns[j].Replace("\r", string.Empty);
                }
            }
            return mapType;
        }
        else
        {
            Debug.LogError("File not found: " + fileName);
            return null;
        }
    }
}
