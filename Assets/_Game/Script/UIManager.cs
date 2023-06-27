using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField] Text pointText;
    [SerializeField] Text completeText;
    [SerializeField] GameObject completePanel;

    public void SetPoint(int point)
    {
        pointText.text = point.ToString();
    }

    public void SetCompleteText(int level)
    {
        completeText.text = "LEVEL " + level.ToString() + "\n COMPLETED";
    }

    public void SetCompletePanel(bool status)
    {
        completePanel.SetActive(status);
    }
}
