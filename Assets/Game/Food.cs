using UnityEditor;
using UnityEngine;

public class Food : Card
{
    private void OnMouseDown()
    {
        if (level.busy) return;
        var (dx, dy) = (X - level.PlayerX, Y - level.PlayerY);
        if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
        {
            level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power += Power;
            level.PlayerPower += Power;
            Destroy(level.map[X, Y]);
            level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Move(dx, dy);
            (level.PlayerX, level.PlayerY) = (level.PlayerX + dx, level.PlayerY + dy);
            level.Count++;
            level.MobsMove();
            level.mobsCards.AddRange(level.newMobsCards);
            level.newMobsCards.Clear();
            level.Check();
        }
        
        

        /*if (level.PlayerPower > 2)
        {
            var a = level.map[level.PlayerX, level.PlayerY].GetComponent<Player>().FrogSprite;
            level.map[level.PlayerX, level.PlayerY].GetComponent<SpriteRenderer>().sprite = a;
        }*/
    }
}
