using TMPro;
using UnityEngine;

public class Finish : Card
{
    public GameObject Panel;
    
    void OnMouseDown()
    {
        if (level.busy) return;
        if (level.PlayerX == level.Width - 1 && level.PlayerY == level.Height - 1)
        {
            if (level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power >= Power)
            {
                var victoryPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
                victoryPanel.GetComponentInChildren<TMP_Text>().text = "Победа!";
                Debug.Log("Victory!");
            }
            else 
                Debug.Log("Play!");
        }
    }
}
