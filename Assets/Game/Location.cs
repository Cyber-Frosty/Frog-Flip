public class Location : Card
{
    void OnMouseDown()
    {
        if (level.busy) return;
        var (dx, dy) = (X - level.PlayerX, Y - level.PlayerY);
        if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
        {
            if (Power > level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power)
                return;
            Destroy(level.map[X, Y]);
            level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Move(dx, dy);
            (level.PlayerX, level.PlayerY) = (level.PlayerX + dx, level.PlayerY + dy);
            level.Count++;
            level.MobsMove();
            level.mobsCards.AddRange(level.newMobsCards);
            level.newMobsCards.Clear();
            level.Check();
        }
    }
}