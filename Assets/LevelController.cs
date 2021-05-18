using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject levelPrefab;
    
    int levelWidth;
    int levelHeight;

    int finishPower;

    Tuple<int, int> playerCoordinates;

    string mapCode;
    string generatorCode;

    void Level_1()
    {
        levelHeight = 2;
        levelWidth = 2;
        finishPower = 15;
        playerCoordinates = Tuple.Create(0, 0);
        mapCode = "p-1.f-1.f-1.f-2";
        generatorCode = "f-1.f-2.f-3";
        LevelInitialization();
    }

    public void RandomLevel()
    {
        var levelController = GameObject.Find("LevelController");
        DontDestroyOnLoad(levelController);
        SceneManager.LoadSceneAsync("Game");
        levelController.GetComponentInChildren<Level>().Activate();
        levelController.GetComponent<LevelController>().ToActiveScene();
    }

    public void ToActiveScene()
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
    }
    
    public void ButtonPressed()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    
    void LevelInitialization()
    {
        var game = Instantiate(levelPrefab, transform.position, Quaternion.identity);
        var level = game.GetComponent<Level>();
        level.Width = levelWidth;
        level.Height = levelHeight;
        (level.PlayerX, level.PlayerY) = playerCoordinates;
        level.Activate();
    }
}
