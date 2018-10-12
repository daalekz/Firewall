using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
	public int Health { get; private set; }
	public float Speed { get; private set; }
	public int Damage { get; private set; }
	public int Value { get; private set; }
	public readonly EnemyType Type;

	public Enemy (int _health, float _speed, int _damage, int _value, EnemyType _type)
	{
		Health = _health;
		Speed = _speed;
		Damage = _damage;
		Value = _value;
		Type = _type;
	}

	public void ApplyDamage (int damage)
	{
		Health -= damage;
	}
}
