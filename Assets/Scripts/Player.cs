using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int Health { get; private set; }

	public Player (int _health)
	{
		Health = _health;
	}

	public void ApplyDamage (int damage)
	{
		Health -= damage;
	}
}
