using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
	public int Health { get; private set; }
	public float Speed { get; set; }
	public int Damage { get; private set; }
	public int Value { get; private set; }
	public readonly EnemyType Type;
    private int _path_num;

	public Enemy (int _health, float _speed, int _damage, int _value, EnemyType _type)
	{
		Health = _health;
		Speed = _speed;
		Damage = _damage;
		Value = _value;
		Type = _type;
        _path_num = 0;
	}

	public void ApplyDamage (int damage)
	{
		Health -= damage;
	}

    public int PathNum
    {
        get
        {
            return _path_num;
        }

        set
        {
            _path_num = value;
        }
    }

   
}
