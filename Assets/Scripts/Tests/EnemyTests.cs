using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

public class TestEnemyDataManipulation
{
    [Test]
    public void TestDamage()
    {
        Enemy test_enemy = new DDoS();

        int Current_Health = test_enemy.Health;

        test_enemy.ApplyDamage(5);

        Assert.That(test_enemy.Health == Current_Health-5);
    }

    [Test]
    public void TestReduceSpeed()
    {
        Enemy test_enemy = new DDoS();

        float Current_Speed = test_enemy.Speed;

        test_enemy.ReduceSpeed(2);

        Assert.That(test_enemy.Speed == (Current_Speed / 2));
    }
}

