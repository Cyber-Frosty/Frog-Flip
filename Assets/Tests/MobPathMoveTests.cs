using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Drawing;
using System.Linq;

public class MobPathMoveTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ReturnFistMove_WhenPathToPlayerExist()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 F1 M10 F2",
            "F1 P1 F1 L6",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 2, 2);
        Assert.That(pointToMoveX == 2 && pointToMoveY == 1 || pointToMoveX == 1 && pointToMoveY == 2);
    }
    
    [Test]
    public void ReturnFistMove_WhenPathToPlayerNotStraight()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 L12 M10 F2",
            "F1 P1 L34 L15",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 2, 2);
        Assert.That(pointToMoveX == 2 && pointToMoveY == 3);
    }
    
    [Test]
    public void ReturnFistMove_WhenPlayerNearMob()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 L1 L2 F2",
            "F1 P1 M5 L5",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 2, 1);
        Assert.That(pointToMoveX == 1 && pointToMoveY == 1);
    }
    
    [Test]
    public void ReturnFistMove_WhenThereIsAnotherMobBetweenPlayerAndMob()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 L1 L9 F2",
            "F1 P1 M5 M8",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 3, 1);
        Assert.That(pointToMoveX == 3 && pointToMoveY == 0);
    }
    
    [Test]
    public void ReturnFistMove_WhenPathToPlayerNotExist1_BecauseOfMobs()
    {
        var textMap = new[]
        {
            "P1 M10 F4 F1",
            "L5 M5 F4 F2",
            "L9 L1 M2 M8",
            "L1 M1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 0, 3, 3, 1);
        Assert.That(pointToMoveX == -1 && pointToMoveY == -1);
    }
    
    [Test]
    public void ReturnFistMove_WhenPathToPlayerNotExist2_BecauseLocation()
    {
        var textMap = new[]
        {
            "L2 M8 L9 F1",
            "L5 L16 F4 F2",
            "L9 P1 L1 L1",
            "L1 F1 F2 L20"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 1, 3);
        Assert.That(pointToMoveX == -1 && pointToMoveY == -1);
    }
    
    [Test]
    public void ReturnFistMove_WhenPathToPlayerNotExist3()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 M3 L4 F2",
            "L9 P1 L10 M8",
            "L1 F1 F2 M2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 1, 1, 3, 1);
        Assert.That(pointToMoveX == -1 && pointToMoveY == -1);
    }
    
    [Test]
    public void ReturnFistMove_ShortestPath1()
    {
        var textMap = new[]
        {
            "L2 L3 F4 M8",
            "L5 L1 L9 F2",
            "F1 F3 P1 M8",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 2, 1, 3, 3);
        Assert.That(pointToMoveX == 2 && pointToMoveY == 3);
    }
    
    [Test]
    public void ReturnFistMove_ShortestPath2()
    {
        var textMap = new[]
        {
            "P1 L9 F4 M8",
            "L5 L1 L9 F2",
            "F1 L19 L7 F18",
            "L1 F1 F2 M2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 0, 3, 3, 3);
        Assert.That(pointToMoveX == 3 && pointToMoveY == 2);
    }
    
    [Test]
    public void ReturnFistMove_ShortestPath3()
    {
        var textMap = new[]
        {
            "L2 L3 F4 F1",
            "L5 L1 L9 F2",
            "F1 L9 P2 M8",
            "L1 F1 F2 L2"
        };
        var map = CardClass.GetMapCardsClassFromLines(textMap);
        var (pointToMoveX, pointToMoveY) = Mob.FindPointToFistStep(map, 2, 1, 3, 3);
        Assert.That(pointToMoveX == 2 && pointToMoveY == 3);
    }
}
