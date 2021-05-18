using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public GameObject PlayPanel;
    
    public void PlayPressed()
    {
        var playPanel = Instantiate(PlayPanel, transform.position, Quaternion.identity);
        playPanel.transform.SetParent(transform);
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit!");
    }
}