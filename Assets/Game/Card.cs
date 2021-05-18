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
        //level.map[X, Y].transform.position = level.position[X + dx, Y + dy];
        level.map[X + dx, Y + dy] = level.map[X, Y];
        level.map[X, Y] = null;
        GetComponentInChildren<TMP_Text>().text = $"{Power}";
        //GetComponentInChildren<TMP_Text>().rectTransform.position = level.position[X + dx, Y + dy];
        if (this.TryGetComponent(out Mob mob))
            mob.didMobGoInLastStep = true;
        if (X == 0 && dx == 1 || X == level.Width - 1 && dx == -1
                              || Y == 0 && dy == 1 || Y == level.Height - 1 && dy == -1)
            level.GenerateRandom(X, Y);
        else
            level.map[X - dx, Y - dy].GetComponent<Card>().Move(dx, dy);
        X += dx;
        Y += dy;
    }

    IEnumerator Replace(int x, int y, int dx, int dy)
    {
        level.busyCount++;
        var totalMovementTime = 1f;
        var currentMovementTime = 0f;
        while (Vector3.Distance(transform.position, level.position[x + dx, y + dy]) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.position, level.position[x + dx, y + dy], currentMovementTime / totalMovementTime);
            yield return null;
        }
        level.busyCount--;
    }
}