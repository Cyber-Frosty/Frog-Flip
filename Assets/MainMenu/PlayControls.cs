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
        var campaignCount = GameObject.Find("CampaignNumber").GetComponentInChildren<TMP_Text>().text;
        var campaignLeft = GameObject.Find("CampaignLeftButton");
        campaignLeft.GetComponent<Button>().interactable = !campaignCount.Equals("1");
        var campaignRight = GameObject.Find("CampaignRightButton");
        campaignRight.GetComponent<Button>().interactable =
            !campaignCount.Equals(PlayerPrefs.GetInt("currentCampaignLevel").ToString());
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

    public void CampaignLeftPressed()
    {
        var campaignCount = GameObject.Find("CampaignNumber");
        campaignCount.GetComponentInChildren<TMP_Text>().text =
            (int.Parse(campaignCount.GetComponentInChildren<TMP_Text>().text) - 1).ToString();
    }
    
    public void CampaignRightPressed()
    {
        var campaignCount = GameObject.Find("CampaignNumber");
        campaignCount.GetComponentInChildren<TMP_Text>().text =
            (int.Parse(campaignCount.GetComponentInChildren<TMP_Text>().text) + 1).ToString();
    }

    public void ExitPressed()
    {
        Destroy(gameObject);
    }
}
