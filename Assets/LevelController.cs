using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public GameObject levelPrefab;

    LevelType type;
    int number;
    
    int width;
    int height;

    int finishPower;

    Tuple<int, int> playerCoordinates;

    string mapCode;
    string generatorCode;

    void Start()
    {
        if (PlayerPrefs.HasKey("currentTutorialLevel")) return;
        PlayerPrefs.SetInt("currentTutorialLevel", 1);
        PlayerPrefs.SetInt("currentCampaignLevel", 1);
    }

    public void TutorialPressed()
    {
        var tutorialMethod = GetType().GetMethod($@"LevelTutorial{GameObject
            .Find("TutorialNumber").GetComponentInChildren<TMP_Text>().text}");
        tutorialMethod?.Invoke(this, null);
    }

    public void LevelTutorial1()
    {
        type = LevelType.Tutorial;
        number = 1;
        height = 2;
        width = 2;
        finishPower = 10;
        playerCoordinates = Tuple.Create(0, 0);
        mapCode = "Player-1.Food-1/Food-1.Food-2";
        generatorCode = "Food-2:10.Food-1:20";
        LevelInitialization();
    }

    public void LevelTutorial2()
    {
        type = LevelType.Tutorial;
        number = 2;
        height = 3;
        width = 3;
        finishPower = 10;
        playerCoordinates = Tuple.Create(1, 1);
        mapCode = "Food-2.Location-3.Food-1/Location-2.Player-1.Food-1/Location-1.Location-3.Food-1";
        generatorCode = "Food-2:5.Food-1:8.Location-1:14.Location-2:18.Location-3:20";
        LevelInitialization();
    }

    public void LevelTutorial3()
    {
        type = LevelType.Tutorial;
        number = 3;
        height = 5;
        width = 5;
        finishPower = 10;
        playerCoordinates = Tuple.Create(3, 2);
        mapCode = "Location-2.Location-4.Food-1.Location-1.Food-2/Food-2.Location-6.Food-1.Location-2.Location-1/Food-1.Location-7.Location-3.Player-1.Location-2/Location-4.Location-6.Location-3.Food-2.Location-4/Mob-5.Location-7.Food-2.Location-3.Food-1";
        generatorCode = "Food-3:2.Food-2:4.Food-1:8.Location-1:12.Location-2:15.Location-3:17.Location-4:18.Location-5:19.Location-6:20";
        LevelInitialization();
    }

    public void CampaignPressed()
    {
        var campaignMethod = GetType().GetMethod($@"LevelCampaign{GameObject
            .Find("CampaignNumber").GetComponentInChildren<TMP_Text>().text}");
        campaignMethod?.Invoke(this, null);
    }

    public void LevelCampaign1()
    {
        type = LevelType.Campaign;
        number = 1;
        height = 2;
        width = 3;
        finishPower = 15;
        playerCoordinates = Tuple.Create(1, 0);
        mapCode = "Food-1.Player-1.Location-2/Location-1.Food-2.Food-1";
        LevelInitialization();
    }

    public void LevelCampaign2()
    {
        type = LevelType.Campaign;
        number = 2;
        height = 3;
        width = 2;
        finishPower = 15;
        playerCoordinates = Tuple.Create(1, 0);
        mapCode = "Food-2.Player-1/Location-2.Food-1/Food-2.Location-3";
        LevelInitialization();
    }
    
    public void RandomLevel()
    {
        type = LevelType.Random;
        width = Random.Range(2, 8);
        height = Random.Range(2, 6);
        finishPower = 30;
        playerCoordinates = Tuple.Create(Random.Range(0, width), Random.Range(0, height));
        mapCode = "random";
        generatorCode = "random";
        playerCoordinates = Tuple.Create(1, 1);
        LevelInitialization();
    }

    void ToActiveScene()
    {
        StartCoroutine(MoveMoveMove());
    }

    IEnumerator MoveMoveMove()
    {
        var currentMovementTime = 0f;
        while (currentMovementTime < 0.3f)
        {
            currentMovementTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        while (currentMovementTime < 0.6f)
        {
            currentMovementTime += Time.deltaTime;
            yield return null;
        }
    }
    
    public void FinishButtonPressed()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    
    void LevelInitialization()
    {
        var levelController = GameObject.Find("LevelController");
        var game = Instantiate(levelPrefab, levelController.transform.position, Quaternion.identity);
        game.transform.SetParent(levelController.transform);
        var level = game.GetComponent<Level>();
        level.type = type;
        level.number = number;
        level.width = width;
        level.height = height;
        level.finishPower = finishPower;
        (level.PlayerX, level.PlayerY) = playerCoordinates;
        level.mapCode = mapCode;
        level.generatorCode = generatorCode;
        DontDestroyOnLoad(levelController);
        SceneManager.LoadSceneAsync("Game");
        game.GetComponentInChildren<Level>().Activate();
        levelController.GetComponent<LevelController>().ToActiveScene();
    }
}