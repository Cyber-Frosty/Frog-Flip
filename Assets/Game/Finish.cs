using System;
using TMPro;
using UnityEngine;

public class Finish : Card
{
    public GameObject Panel;
    
    void OnMouseDown()
    {
        if (level.IsRendered) return;
        if (level.PlayerX == level.width - 1 && level.PlayerY == level.height - 1)
        {
            if (level.Map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power >= Power)
            {
                if (level.type == LevelType.Tutorial)
                {
                    var current = PlayerPrefs.GetInt("currentTutorialLevel") + 1;
                    PlayerPrefs.SetInt("currentTutorialLevel", Math.Min(current, 3));
                }

                if (level.type == LevelType.Campaign)
                {
                    var current = PlayerPrefs.GetInt("currentCampaignLevel") + 1;
                    PlayerPrefs.SetInt("currentCampaignLevel", Math.Min(current, 2));
                }
                var victoryPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
                victoryPanel.GetComponentInChildren<TMP_Text>().text = "Победа!";
                Debug.Log("Victory!");
            }
            else 
                Debug.Log("Play!");
        }
    }
}
