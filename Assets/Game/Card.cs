using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public int X;
    public int Y;
    public int Power;
    public Level level;

    public void Move(int dx, int dy)
    {
        StartCoroutine(Replace(X, Y, dx, dy));
        level.Map[X + dx, Y + dy] = level.Map[X, Y];
        level.Map[X, Y] = null;
        GetComponentInChildren<TMP_Text>().text = $"{Power}";
        if (TryGetComponent(out Mob mob))
            mob.didMobGoInLastStep = true;
        if (X == 0 && dx == 1 || X == level.width - 1 && dx == -1
                              || Y == 0 && dy == 1 || Y == level.height - 1 && dy == -1)
            level.Map[X, Y] = level.GenerateRandomCard(X, Y, false);
        else
            level.Map[X - dx, Y - dy].GetComponent<Card>().Move(dx, dy);
        X += dx;
        Y += dy;
    }

    IEnumerator Replace(int x, int y, int dx, int dy)
    {
        level.renderCount++;
        var total = 1f;
        var current = 0f;
        while (Vector3.Distance(transform.position, level.Positions[x + dx, y + dy]) > 0)
        {
            current += Time.deltaTime;
            transform.localPosition = 
                Vector3.Lerp(transform.position, level.Positions[x + dx, y + dy], current / total);
            yield return null;
        }
        level.renderCount--;
    }
}