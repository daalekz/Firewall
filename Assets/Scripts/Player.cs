using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public int Health { get; private set; }

	public Player (int _health)
	{
		Health = _health;
	}

    //reduces the player's health by a set inputted amount
	public void ApplyDamage (int damage)
	{
		Health -= damage;
	}
}
