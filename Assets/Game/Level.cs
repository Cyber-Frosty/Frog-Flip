using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
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

    public int width;
    public int height;
    public int finishPower;
    public string mapCode;
    public string generatorCode;
    
    public GameObject[,] Map;
    public Vector3[,] Positions;
    Vector3 finishPosition;
    public int movesCount;
    List<Tuple<GameObject, int>> generator;
    public List<GameObject> mobsCards;
    public List<GameObject> newMobsCards;
    public int PlayerX;
    public int PlayerY;
    public int PlayerPower = 1;

    public int renderCount;
    public bool IsRendered => renderCount > 0;
    
    public bool InBounds(Point point)
    {
        var bounds = new Rectangle(0, 0, width, height);
        return bounds.Contains(point);
    }

    public void Activate()
    {
        SetPositions();
        Map = new GameObject[width, height];
        if (!generatorCode.Equals("random"))
            SetGenerator();
        else
            SetStandardGenerator();
        if (!mapCode.Equals("random"))
            GenerateMap();
        else
            GenerateRandomMap();
        var finishCard = GenerateCard(width, height - 1, finishPosition, Finish, finishPower);
        Check();
    }
    
    void SetPositions()
    {
        var xPosition = 1 - width;
        var yPosition = 1 - height;
        Positions = new Vector3[width, height];
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            Positions[x, y] = new Vector3(xPosition + 2 * x, yPosition + 2 * y, 0);
        finishPosition = new Vector3(xPosition + 2 * width, yPosition + 2 * (height - 1), 0);
    }
    
    void SetGenerator()
    {
        generator = new List<Tuple<GameObject, int>>();
        var items = generatorCode.Split('.');
        var previousMaxRoll = 0;
        foreach (var item in items)
        {
            var s = item.Split(':');
            var (card, maxRoll) = (s[0], int.Parse(s[1]));
            var t = card.Split('-');
            var (cardType, cardPower) = 
                (GetType().GetField(t[0]).GetValue(this) as GameObject, int.Parse(t[1]));

            for (var i = previousMaxRoll; i < maxRoll; i++)
                generator.Add(Tuple.Create(cardType, cardPower));

            previousMaxRoll = maxRoll;
        }
    }

    void SetStandardGenerator()
    {
        generator = new List<Tuple<GameObject, int>>();
        for (var i = 0; i < 6; i++)
            generator.Add(Tuple.Create(Food, Math.Max((6 - i) / 2, 1)));
        for (var i = 6; i < 18; i++)
            generator.Add(Tuple.Create(Location, i / 4));
        for (var i = 18; i < 20; i++)
            generator.Add(Tuple.Create(Mob, 6 / 2 * (i - 17)));
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
                Map[x, y] = GenerateCard(x, y, Positions[x, y], cardPrefab, cardPower);
            }
        }
    }

    void GenerateRandomMap()
    {
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            if (x == PlayerX && y == PlayerY)
                Map[x, y] = GenerateCard(x, y, Positions[x, y], Player, 1);
            else 
                Map[x, y] = GenerateRandomCard(x, y, true);
        }
    }

    GameObject GenerateCard(int x, int y, Vector3 position, GameObject cardPrefab, int power)
    {
        var card = Instantiate(cardPrefab, position, Quaternion.identity);
        card.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(CardGrow(card, position, power));
        card.transform.SetParent(transform);
        card.GetComponent<Card>().X = x;
        card.GetComponent<Card>().Y = y;
        card.GetComponent<Card>().level = this;
        card.GetComponent<Card>().Power = power;
        card.GetComponent<BoxCollider2D>().enabled = true;
        if (card.TryGetComponent(out Mob mob))
            newMobsCards.Add(card);
        return card;
    }

    public GameObject GenerateRandomCard(int x, int y, bool isStart)
    {
        var roll = Random.Range(0, 20);
        roll = isStart 
            ? Math.Max(0, roll - (width + height) / 2
                          + Math.Abs(x - PlayerX) + Math.Abs(y - PlayerY) + (x + y) / 2) % 20
            : Math.Max(0, roll - (Math.Abs(x - PlayerX) + Math.Abs(y - PlayerY)) * 4 + (x + y) / 2) % 20;
        Debug.Log(roll);
        var (cardType, cardPower) = generator[roll];
        if (isStart || cardType.TryGetComponent(out Food f))
        {
            return GenerateCard(x, y, Positions[x, y], cardType, cardPower);
        }
        return GenerateCard(x, y, Positions[x, y], cardType,
            cardPower + Random.Range(1 + movesCount / 4, 4 + movesCount / 4));
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

    IEnumerator CardGrow(GameObject card, Vector3 position, int power)
    {
        renderCount++;
        var total = 0.3f;
        var current = 0f;
        while (current < total)
        {
            current += Time.deltaTime;
            if (current > total)
                current = total;
            card.transform.localScale = new Vector3(current / total, current / total, current / total);
            yield return null;
        }
        var text = Instantiate(Text, position, Quaternion.identity);
        text.transform.SetParent(card.transform);
        text.GetComponent<TMP_Text>().text = power.ToString();
        text.GetComponent<TMP_Text>().rectTransform.position = position + new Vector3(0.5f, -0.5f);
        text.GetComponent<TMP_Text>().font = cardFont;
        text.GetComponent<TMP_Text>().color = Color.white;
        text.GetComponent<TMP_Text>().fontSize = 0.6f;
        renderCount--;
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
        if (PlayerPower >= finishPower)
            Map[PlayerX, PlayerY].GetComponent<SpriteRenderer>().sprite =
                Map[PlayerX, PlayerY].GetComponent<Player>().FrogSprite;
    }

    IEnumerator Defeat()
    {
        yield return new WaitWhile(() => IsRendered);
        renderCount++;
        var defeatPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
        defeatPanel.transform.SetParent(transform);
        defeatPanel.GetComponentInChildren<TMP_Text>().text = "Поражение!";
    }
}
