using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public int X;
    public int Y;
    public int Power;
    public Level level;

    public void Move(int dx, int dy)
    {
        level.grid[X, Y].transform.position = level.position[X + dx, Y + dy];
        //Destroy(level.grid[X + dx, Y + dy]);
        level.grid[X + dx, Y + dy] = level.grid[X, Y];
        level.grid[X + dx, Y + dy].GetComponent<Card>().X += dx;
        level.grid[X + dx, Y + dy].GetComponent<Card>().Y += dy;
        level.grid[X, Y] = null;
        if (X == 0 && dx == 1 || X == level.Width - 1 && dx == -1
                              || Y == 0 && dy == 1 || Y == level.Height - 1 && dy == -1)
            //level.grid[X - dx, Y - dy].GetComponent<Card>().Move(dx, dy);
            Move(X - dx, Y - dy, dx, dy);
        else
            level.GenerateRandom(X, Y);
    }

    public void Move(int x, int y, int dx, int dy)
    {
        level.grid[x, y].transform.position = level.position[x + dx, y + dy];
        //Destroy(level.grid[X + dx, Y + dy]);
        level.grid[x + dx, y + dy] = level.grid[x, y];
        level.grid[x + dx, y + dy].GetComponent<Card>().X += dx;
        level.grid[x + dx, y + dy].GetComponent<Card>().Y += dy;
        level.grid[x, y] = null;
        if (x == 0 && dx == 1 || x == level.Width - 1 && dx == -1 
                              || y == 0 && dy == 1 || y == level.Height - 1 && dy == -1)
            //level.grid[x - dx, y - dy].GetComponent<Card>().Move(dx, dy);
            level.GenerateRandom(x, y);
        else
            Move(x - dx,  y - dy, dx, dy);
    }
}
