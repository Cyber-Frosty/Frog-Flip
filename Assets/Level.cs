using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public GameObject Player;
    public GameObject Location;
    public GameObject Food;

    float startPosX = -5f;
    float startPosY = 5f;
    float outX = 2f;
    float outY = 2f;

    public int Width = 4;
    public int Height = 4;
    
    public GameObject[,] grid;
    public Vector3[,] position;
    
    public int PlayerX = 1;
    public int PlayerY = 1;

    // Start is called before the first frame update
    void Start()
    {
        var posXreset = startPosX;
        position = new Vector3[Width, Height];
        for (var y = 0; y < Height; y++)
        {
            startPosY -= outY;
            for (var x = 0; x < Width; x++)
            {
                startPosX += outX;
                position[x, y] = new Vector3(startPosX, startPosY, 0);
            }
            startPosX = posXreset;
        }
        grid = new GameObject[Width, Height];
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            if (x == PlayerX && y == PlayerY)
                Generate(x, y, Player, 1);
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
        grid[x, y] = Instantiate(card, position[x, y], Quaternion.identity);
        grid[x, y].transform.SetParent(transform);
        grid[x, y].GetComponent<Card>().X = x;
        grid[x, y].GetComponent<Card>().Y = y;
        grid[x, y].GetComponent<Card>().level = this;
        grid[x, y].GetComponent<Card>().Power = power;
        grid[x, y].GetComponent<BoxCollider2D>().enabled = true;
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
        else
        {
            power = Random.Range(1, 8);
            Generate(x, y, Location, power);
        }
    }
}
