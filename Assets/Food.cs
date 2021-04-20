public class Food : Card
{
    private void OnMouseDown()
    {
        var (dx, dy) = (X - level.PlayerX, Y - level.PlayerY);
        if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
        {
            level.grid[level.PlayerX, level.PlayerY].GetComponent<Card>().Power += Power;
            Destroy(level.grid[X, Y]);
            //level.grid[level.PlayerX, level.PlayerY].GetComponent<Card>().Move(dx, dy);
            Move(level.PlayerX, level.PlayerY, dx, dy);
            (level.PlayerX, level.PlayerY) = (level.PlayerX + dx, level.PlayerY + dy);
        }
    }
}
