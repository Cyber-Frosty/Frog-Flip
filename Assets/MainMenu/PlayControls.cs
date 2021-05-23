using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayControls : MonoBehaviour
{
    void Update()
    {
        var tutorialCount = GameObject.Find("TutorialNumber").GetComponentInChildren<TMP_Text>().text;
        var tutorialLeft = GameObject.Find("TutorialLeftButton");
        tutorialLeft.GetComponent<Button>().interactable = !tutorialCount.Equals("1");
        var tutorialRight = GameObject.Find("TutorialRightButton");
        tutorialRight.GetComponent<Button>().interactable =
            !tutorialCount.Equals(PlayerPrefs.GetInt("currentTutorialLevel").ToString());
    }

    public void TutorialLeftPressed()
    {
        var tutorialCount = GameObject.Find("TutorialNumber");
        tutorialCount.GetComponentInChildren<TMP_Text>().text =
            (int.Parse(tutorialCount.GetComponentInChildren<TMP_Text>().text) - 1).ToString();
    }
    
    public void TutorialRightPressed()
    {
        var tutorialCount = GameObject.Find("TutorialNumber");
        tutorialCount.GetComponentInChildren<TMP_Text>().text =
            (int.Parse(tutorialCount.GetComponentInChildren<TMP_Text>().text) + 1).ToString();
    }
    
    public void ExitPressed()
    {
        Destroy(gameObject);
    }
}
