using System;
using TMPro;
using UnityEngine;

public class Finish : Card
{
    public GameObject Panel;
    
    void OnMouseDown()
    {
        if (level.Busy) return;
        if (level.PlayerX == level.width - 1 && level.PlayerY == level.height - 1)
        {
            if (level.Map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power >= Power)
            {
                var current = PlayerPrefs.GetInt("currentTutorialLevel") + 1;
                PlayerPrefs.SetInt("currentTutorialLevel", Math.Min(current, 3));
                var victoryPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
                victoryPanel.GetComponentInChildren<TMP_Text>().text = "Победа!";
                Debug.Log("Victory!");
            }
            else 
                Debug.Log("Play!");
        }
    }
}
