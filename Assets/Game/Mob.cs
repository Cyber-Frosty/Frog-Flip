using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Mob : Card
{
    public bool didMobGoInLastStep = true;

    private void OnMouseDown()
    {
        if (level.IsRendered) return;
        var (dx, dy) = (X - level.PlayerX, Y - level.PlayerY);
        if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
        {
            if (Power > level.Map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power)
            {
                Destroy(level.Map[level.PlayerX, level.PlayerY]);
                level.Map[X, Y].GetComponent<Card>().Move(-dx, -dy);
                level.PlayerX = level.PlayerY = -1;
                level.movesCount++;
                Debug.Log("You lose!");
                level.Check();
                return;
            }
            level.mobsCards.Remove(level.Map[X, Y]);
            Destroy(level.Map[X, Y]);
            level.Map[level.PlayerX, level.PlayerY].GetComponent<Card>().Move(dx, dy);
            (level.PlayerX, level.PlayerY) = (level.PlayerX + dx, level.PlayerY + dy);
            level.movesCount++;
            level.MobsMove();
            level.mobsCards.AddRange(level.newMobsCards);
            level.newMobsCards.Clear();
            level.Check();
        }
    }

    public IEnumerator PrepareToMobMove()
    {
        yield return new WaitWhile(() => level.IsRendered);
        MobMove();
    } 
    
    void MobMove()
    {
        var (pointToFistStepX, pointToFistStepY)  = FindPointToFistStep(
            CardClass.GetMapCardsClassFromMapGameObjects(level.Map),
            level.PlayerX, level.PlayerY, X, Y);
        if (pointToFistStepX == -1 || pointToFistStepY == -1)
            return;
        
        var (dx, dy) = (X - pointToFistStepX, Y - pointToFistStepY);
        if (level.Map[X - dx, Y - dy].TryGetComponent(out Player player1) &&
            player1.Power > Power)
            return;
            
        if (level.Map[X - dx, Y - dy].TryGetComponent(out Player player))
        {
            level.PlayerX = level.PlayerY = -1;
            Debug.Log("You lose, mf!");
        }
        Destroy(level.Map[X - dx, Y - dy]);
        level.Map[X, Y].GetComponent<Card>().Move(-dx, -dy);
        level.Check();
    }

    public static (int, int) FindPointToFistStep(CardClass[,] map, int playerX, int playerY, int mobX, int mobY)
    {
        var path = FindPathToPlayer(map, new Point(playerX, playerY), new Point(mobX, mobY));
        var pointToFistStep = path;
        if (path == null)
            return (-1, -1);
        while (pointToFistStep.Previous.Previous != null)
            pointToFistStep = pointToFistStep.Previous;

        return (pointToFistStep.Value.X, pointToFistStep.Value.Y);
    }

    static SinglyLinkedList<Point> FindPathToPlayer(CardClass[,] map, Point playerPoint, Point mobPoint)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(mobPoint));
        var visits = new HashSet<Point>();
        while (queue.Count != 0)
        {
            var point = queue.Dequeue();
            if (!new Rectangle(0, 0, map.GetLength(0), map.GetLength(1)).Contains(point.Value) ||
                map[point.Value.X, point.Value.Y].IsCardType('L') &&
                map[point.Value.X, point.Value.Y].Power > map[mobPoint.X, mobPoint.Y].Power ||
                map[point.Value.X, point.Value.Y].IsCardType('M') && !point.Value.Equals(mobPoint) ||
                visits.Contains(point.Value))
                continue;
            if (point.Value.Equals(playerPoint))
                return point;
            visits.Add(point.Value);
            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if (dx == 0 || dy == 0)
                    queue.Enqueue(new SinglyLinkedList<Point>(new Point() {X = point.Value.X + dx, Y = point.Value.Y + dy}, point ));
        }
        return null;
    }
}
