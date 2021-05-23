using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public enum LevelType
{
    Tutorial,
    Campaign,
    Random
};

public class Level : MonoBehaviour
{
    public LevelType type;
    public int number;
    
    public GameObject Player;
    public GameObject Location;
    public GameObject Food;
    public GameObject Text;
    public GameObject Mob;
    public GameObject Finish;
    public GameObject Panel;
    public TMP_FontAsset cardFont;
    
    Vector3 finish;

    public int width = 4;
    public int height = 4;

    public int finishPower;

    public string mapCode;
    public string generatorCode;
    
    public GameObject[,] Map;
    public Vector3[,] Position;
    public int count;
    //List<Tuple<GameObject, int>> generator;
    
    public List<GameObject> mobsCards;
    public List<GameObject> newMobsCards;
    
    public int PlayerX = 1;
    public int PlayerY = 1;
    public int PlayerPower = 1;

    public int busyCount;
    public bool Busy => busyCount > 0;
    
    public bool InBounds(Point point)
    {
        var bounds = new Rectangle(0, 0, width, height);
        return bounds.Contains(point);
    }

    public void Activate()
    {
        SetPositions();
        Map = new GameObject[width, height];
        if (!mapCode.Equals("random"))
            GenerateMap();
        else
            GenerateRandomMap();
        /*if (!generatorCode.Equals("random"))
            SetGenerator();
        else
            SetStandartGenerator();*/
        var f = Instantiate(Finish, finish, Quaternion.identity);
        f.transform.SetParent(transform);
        f.GetComponent<Card>().Power = finishPower;
        f.GetComponent<Card>().level = this;
        var text = Instantiate(Text, finish, Quaternion.identity);
        text.transform.SetParent(f.transform);
        text.GetComponentInChildren<TMP_Text>().text = finishPower.ToString();
        text.GetComponentInChildren<TMP_Text>().rectTransform.position = finish + new Vector3(0.5f, -0.5f);
        text.GetComponent<TMP_Text>().font = cardFont;
        text.GetComponent<TMP_Text>().color = Color.white;
        text.GetComponent<TMP_Text>().fontSize = 0.6f;
        f.GetComponent<BoxCollider2D>().enabled = true;
    }
    
    /*private void SetGenerator()
    {
        generator = new List<Tuple<GameObject, int>>(20);
        var items = generatorCode.Split('.');
        foreach (var item in items)
        {
            var s = item.Split(':');
            var (card, maxRoll) = (s[0], s[1]);
            var t = card.Split('-');
            var (cardType, cardPower) = (t[0], t[1]);
        }
    }

    private void SetStandartGenerator()
    {
        //throw new NotImplementedException();
    }*/

    void SetPositions()
    {
        var xPosition = 1 - width;
        var yPosition = 1 - height;
        Position = new Vector3[width, height];
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            Position[x, y] = new Vector3(xPosition + 2 * x, yPosition + 2 * y, 0);
        finish = new Vector3(xPosition + 2 * width, yPosition + 2 * (height - 1), 0);
    }
    
    void GenerateMap()
    {
        var rows = mapCode.Split('/');
        for (var y = 0; y < height; y++)
        {
            var cards = rows[y].Split('.');
            for (var x = 0; x < width; x++)
            {
                var card = cards[x].Split('-');
                var (cardPrefab, cardPower) = (GetType()
                    .GetField(card[0])
                    .GetValue(this) as GameObject, int.Parse(card[1]));
                Generate(x, y, cardPrefab, cardPower);
            }
        }
    }

    void GenerateRandomMap()
    {
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            if (x == PlayerX && y == PlayerY)
                Generate(x, y, Player, 1);
            else if (x == 2 && y == 2)
                Generate(x, y, Mob, 10);
            else
            {
                var (dx, dy) = (x - PlayerX, y - PlayerY);
                if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
                    Generate(x, y, Food, 1);
                else
                    GenerateRandom(x, y);
            }
        }
    }

    private void Generate(int x, int y, GameObject card, int power)
    {
        Map[x, y] = Instantiate(card, Position[x, y], Quaternion.identity);
        Map[x, y].transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(Grow(x, y, power));
        Map[x, y].transform.SetParent(transform);
        Map[x, y].GetComponent<Card>().X = x;
        Map[x, y].GetComponent<Card>().Y = y;
        Map[x, y].GetComponent<Card>().level = this;
        Map[x, y].GetComponent<Card>().Power = power;
        Map[x, y].GetComponent<BoxCollider2D>().enabled = true;
        if (Map[x, y].TryGetComponent(out Mob mob))
            newMobsCards.Add(Map[x, y]);
    }

    public void GenerateRandom(int x, int y)
    {
        int power;
        var cardTypeNumber = Random.Range(0, 10);
        if (cardTypeNumber > 5)
        {
            power = Random.Range(1, 4);
            Generate(x, y, Food, power);
        }
        else if (cardTypeNumber == 5)
        {
            power = PlayerPower + 2;
            Generate(x, y, Mob, power);
        }
        else
        {
            power = Random.Range(1 + 2 * count / 3, 8 + 2 * count / 3);
            Generate(x, y, Location, power);
        }
    }

    public void MobsMove()
    {
        foreach (var mobCard in mobsCards)
            if (mobCard.TryGetComponent(out Mob mob))
                if (!mob.didMobGoInLastStep)
                {
                    StartCoroutine(mob.PrepareToMobMove());
                    mob.didMobGoInLastStep = true;
                }
                else
                    mob.didMobGoInLastStep = false;
    }

    IEnumerator Grow(int x, int y, int power)
    {
        var totalMovementTime = 0.3f;
        var currentMovementTime = 0f;
        while (currentMovementTime < totalMovementTime)
        {
            currentMovementTime += Time.deltaTime;
            if (currentMovementTime > totalMovementTime)
                currentMovementTime = totalMovementTime;
            Map[x, y].transform.localScale = new Vector3(currentMovementTime / totalMovementTime, currentMovementTime / totalMovementTime, currentMovementTime / totalMovementTime);
            yield return null;
        }
        var text = Instantiate(Text, Position[x, y], Quaternion.identity);
        text.transform.SetParent(Map[x, y].transform);
        text.GetComponent<TMP_Text>().text = power.ToString();
        text.GetComponent<TMP_Text>().rectTransform.position = Position[x, y] + new Vector3(0.5f, -0.5f);
        text.GetComponent<TMP_Text>().font = cardFont;
        text.GetComponent<TMP_Text>().color = Color.white;
        text.GetComponent<TMP_Text>().fontSize = 0.6f;
    }
    
    public void Check()
    {
        if (PlayerX == -1)
        {
            StartCoroutine(Defeat());
            return;
        }
        var checkCount = 4;
        if (PlayerX == 0 || !Map[PlayerX - 1, PlayerY].TryGetComponent(out Food f1)
            && Map[PlayerX - 1, PlayerY].GetComponent<Card>().Power
            > Map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerX == width - 1 || !Map[PlayerX + 1, PlayerY].TryGetComponent(out Food f2)
            && Map[PlayerX + 1, PlayerY].GetComponent<Card>().Power
            > Map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerY == 0 || !Map[PlayerX, PlayerY - 1].TryGetComponent(out Food f3)
            && Map[PlayerX, PlayerY - 1].GetComponent<Card>().Power
            > Map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerY == height - 1 || !Map[PlayerX, PlayerY + 1].TryGetComponent(out Food f4)
            && Map[PlayerX, PlayerY + 1].GetComponent<Card>().Power
            > Map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (checkCount == 0)
            StartCoroutine(Defeat());
        if (PlayerPower >= 10)
            Map[PlayerX, PlayerY].GetComponent<SpriteRenderer>().sprite =
                Map[PlayerX, PlayerY].GetComponent<Player>().FrogSprite;
    }

    public IEnumerator Defeat()
    {
        yield return new WaitWhile(() => Busy);
        busyCount++;
        var defeatPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
        defeatPanel.GetComponentInChildren<TMP_Text>().text = "Поражение!";
    }
}
