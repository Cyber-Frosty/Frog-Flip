using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public class Level : MonoBehaviour
{
    public GameObject Player;
    public GameObject Location;
    public GameObject Food;
    public GameObject Text;
    public GameObject Mob;
    public GameObject Finish;
    public GameObject Panel;

    float startPosX = -5f;
    float startPosY = -5f;
    float outX = 2f;
    float outY = 2f;
    private Vector3 finish;

    public int Width = 4;
    public int Height = 4;
    
    public GameObject[,] map;
    public Vector3[,] position;
    public int Count;
    public List<GameObject> mobsCards;
    public List<GameObject> newMobsCards;
    
    public int PlayerX = 1;
    public int PlayerY = 1;
    public int PlayerPower = 1;

    public int busyCount;
    public bool busy => busyCount > 0;
    
    public bool InBounds(Point point)
    {
        var bounds = new Rectangle(0, 0, Width, Height);
        return bounds.Contains(point);
    }
    
    public void Activate()
    {
        var posXreset = startPosX;
        position = new Vector3[Width, Height];
        for (var y = 0; y < Height; y++)
        {
            startPosY += outY;
            for (var x = 0; x < Width; x++)
            {
                startPosX += outX;
                position[x, y] = new Vector3(startPosX, startPosY, 0);
            }

            if (y == Height - 1)
                finish = new Vector3(startPosX + outX, startPosY, 0);
            startPosX = posXreset;
        }
        map = new GameObject[Width, Height];
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
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

        var f = Instantiate(Finish, finish, Quaternion.identity);
        f.transform.SetParent(transform);
        f.GetComponent<Card>().Power = 15;
        f.GetComponent<Card>().level = this;
        var text = Instantiate(Text, finish, Quaternion.identity);
        text.transform.SetParent(f.transform);
        text.GetComponentInChildren<TMP_Text>().text = "15";
        text.GetComponentInChildren<TMP_Text>().rectTransform.position = finish;
        f.GetComponent<BoxCollider2D>().enabled = true;

    }

    private void Generate(int x, int y, GameObject card, int power)
    {
        map[x, y] = Instantiate(card, position[x, y], Quaternion.identity);
        map[x, y].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        StartCoroutine(Grow(x, y, power));
        map[x, y].transform.SetParent(transform);
        map[x, y].GetComponent<Card>().X = x;
        map[x, y].GetComponent<Card>().Y = y;
        map[x, y].GetComponent<Card>().level = this;
        map[x, y].GetComponent<Card>().Power = power;
        map[x, y].GetComponent<BoxCollider2D>().enabled = true;
        //map[x, y].GetComponent<SpriteRenderer>().sprite = Sprite.Create();
        //SpriteRenderer m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (map[x, y].TryGetComponent(out Mob mob))
            newMobsCards.Add(map[x, y]);
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
            power = Random.Range(1 + 2 * Count / 3, 8 + 2 * Count / 3);
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
            map[x, y].transform.localScale = new Vector3(currentMovementTime / totalMovementTime, currentMovementTime / totalMovementTime, currentMovementTime / totalMovementTime);
            yield return null;
        }
        //map[x, y].transform.localScale = new Vector3(1f, 1f, 1f);
        var text = Instantiate(Text, position[x, y], Quaternion.identity);
        text.transform.SetParent(map[x, y].transform);
        text.GetComponent<TMP_Text>().text = $"{power}";
        text.GetComponent<TMP_Text>().rectTransform.position = position[x, y];
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
        if (PlayerX == 0 || !map[PlayerX - 1, PlayerY].TryGetComponent(out Food f1)
            && map[PlayerX - 1, PlayerY].GetComponent<Card>().Power
            > map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerX == Width - 1 || !map[PlayerX + 1, PlayerY].TryGetComponent(out Food f2)
            && map[PlayerX + 1, PlayerY].GetComponent<Card>().Power
            > map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerY == 0 || !map[PlayerX, PlayerY - 1].TryGetComponent(out Food f3)
            && map[PlayerX, PlayerY - 1].GetComponent<Card>().Power
            > map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (PlayerY == Height - 1 || !map[PlayerX, PlayerY + 1].TryGetComponent(out Food f4)
            && map[PlayerX, PlayerY + 1].GetComponent<Card>().Power
            > map[PlayerX, PlayerY].GetComponent<Card>().Power)
            checkCount--;
        if (checkCount == 0)
            StartCoroutine(Defeat());
    }

    public IEnumerator Defeat()
    {
        yield return new WaitWhile(() => busy);
        busyCount++;
        var defeatPanel = Instantiate(Panel, new Vector3(862, 401, -1), Quaternion.identity);
        defeatPanel.GetComponentInChildren<TMP_Text>().text = "Поражение!";
    }
}
