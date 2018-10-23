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
    /*
    Description: 
    Tests the the ApplyDamage method within the enemy class works
    */
    [Test]
    public void TestDamage()
    {
        Enemy test_enemy = new DDoS();

        int Current_Health = test_enemy.Health;

        //reduces the enemy health by 5
        test_enemy.ApplyDamage(5);

        //checks that it actually has been reduced by 5
        Assert.That(test_enemy.Health == Current_Health-5);
    }

    /*
    Description: 
    Tests ReduceSpeed method within the enemy class
    */
    [Test]
    public void TestReduceSpeed()
    {
        Enemy test_enemy = new DDoS();

        float Current_Speed = test_enemy.Speed;
        //reduces the current enemy speed by a factor of 2
        test_enemy.ReduceSpeed(2);
        //checks that the enemy speed has actually been reduced by a factor of 2
        Assert.That(test_enemy.Speed == (Current_Speed / 2));
    }
}

