using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglyLinkedList<T> : IEnumerable<T>
{
    public readonly T Value;
    public readonly SinglyLinkedList<T> Previous;
    public readonly int Length;

    public SinglyLinkedList(T value, SinglyLinkedList<T> previous = null)
    {
        Value = value;
        Previous = previous;
        Length = previous?.Length + 1 ?? 1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        yield return Value;
        var pathItem = Previous;
        while (pathItem != null)
        {
            yield return pathItem.Value;
            pathItem = pathItem.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

/*
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
        if (level.busy) return;
        var (dx, dy) = (X - level.PlayerX, Y - level.PlayerY);
        if ((dx == 1 || dx == -1) && dy == 0 || dx == 0 && (dy == 1 || dy == -1))
        {
            if (Power > level.map[level.PlayerX, level.PlayerY].GetComponent<Card>().Power)
            {
                Destroy(level.map[level.PlayerX, level.PlayerY]);
                level.map[X, Y].GetComponent<Card>().Move(-dx, -dy);
                level.PlayerX = level.PlayerY = -1;
                level.Count++;
                Debug.Log("You lose!");
                return;
            }
            level.mobsCards.Remove(level.map[X, Y]);
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

    public IEnumerator PrepareToMobMove()
    {
        yield return new WaitWhile(() => level.busy);
        MobMove();
    } 
    
    void MobMove()
    {
        var pointToFistStep = FindPathToPlayer();
        if (pointToFistStep.Equals(new Point(-1, -1)))
            return;
        
        var (dx, dy) = (X - pointToFistStep.X, Y - pointToFistStep.Y);
        if (level.map[X - dx, Y - dy].TryGetComponent(out Player player1) &&
            player1.Power > Power)
            return;

        if (level.map[X - dx, Y - dy].TryGetComponent(out Player player))
        {
            level.PlayerX = level.PlayerY = -1;
            Debug.Log("You lose, mf!");
        }
        Destroy(level.map[X - dx, Y - dy]);
        level.map[X, Y].GetComponent<Card>().Move(-dx, -dy);
        level.Check();
    }

    public Point FindPathToPlayer()
    {
        SinglyLinkedList<Point> path = null;
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(new Point(X, Y)));
        var visits = new HashSet<Point>();
        while (queue.Count != 0)
        {
            var point = queue.Dequeue();
            if (!level.InBounds(point.Value) ||
                level.map[point.Value.X, point.Value.Y].TryGetComponent(out Location location) &&
                level.map[point.Value.X, point.Value.Y].GetComponent<Card>().Power > Power ||
                level.map[point.Value.X, point.Value.Y].TryGetComponent(out Mob mob) && !point.Value.Equals(new Point(X, Y)) ||
                visits.Contains(point.Value))
                continue;
            visits.Add(point.Value);
            if (level.PlayerX == point.Value.X && level.PlayerY == point.Value.Y)
            {
                path = point;
                break;
            }
            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if (dx == 0 || dy == 0)
                    queue.Enqueue(new SinglyLinkedList<Point>(new Point() {X = point.Value.X + dx, Y = point.Value.Y + dy}, point ));
        }
        
        if (path == null)
            return new Point(-1, -1);
        var pointToFistStep = path;
        while (pointToFistStep.Previous.Previous != null)
            pointToFistStep = pointToFistStep.Previous;
        return pointToFistStep.Value;
    }
}
*/
