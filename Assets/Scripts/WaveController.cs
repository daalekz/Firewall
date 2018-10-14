using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaveController : MonoBehaviour
{
	public static WaveController instance { get; private set; }
	private Spawner spawner;

	public GameObject[] Units;
	private Queue<GameObject> wave;
	[HideInInspector]
	public int WaveCount = 1;
	private bool WaveStarted = false;
	private float timer  = 0.0f;
	[SerializeField]
	private float SpawnDelay = 1.0f;
    private List<GameObject> spawned_objects;

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

	void Start ()
	{
		spawner = Spawner.instance;
		wave = new Queue<GameObject>();
        //keeps track of active game objects
        spawned_objects = new List<GameObject>();
	}

	void Update ()
	{
		if (!WaveStarted) return;

		timer += Time.deltaTime;
		if (timer < SpawnDelay) return; // Check if we're ready to spawn the next unit

        // We're clear to spawn the next unit
		timer = 0.0f;
        
        //active game objects are added to list (so that tower can target them)
        spawned_objects.Add(spawner.Spawn(wave.Dequeue()));

		// Finally, we need to check that we have any units left to spawn, if not then stop the wave
		if (wave.Count == 0)
		{
			WaveStarted = false;
			WaveCount++;
		}
	}

    public float Delay
    {
        get
        {
            return SpawnDelay;
        }

        set
        {
            SpawnDelay = value;
        }
    }

	private IEnumerator GenerateWave (int limit)
	{
		// Enemy points are the allowance for enemy units each wave. Each enemy unit
		// will have a cost which consumes these enemy points. Enemy points will be
		// calculated based on the wave number.
		int EnemyPoints = 10 + ((WaveCount * 5) - 5);

		// Here we will be selecting the units for the wave
        while (EnemyPoints != 0)
		{
			GameObject unit = Units[Random.Range(0, limit)]; // limit is used to make sure that only units that are allowed in that wave are spawned
			if (EnemyPoints - (int)unit.GetComponent<AIController>().Type >= 0) // Check if we have enough points to add this unit
			{
				wave.Enqueue(unit);
				EnemyPoints -= (int)unit.GetComponent<AIController>().Type;
				yield return new WaitForSeconds(0.001f); // Just to make the RNG work a bit better
			}
		}

		WaveStarted = true;
	}

	public void StartWave ()
	{
		if (WaveStarted) return; // Just in case the wave has already started, though this shouldn't happen normally

		// Reset timer
		timer = 0.0f;

		// Decide on the units to limit
		int limit;
		switch (WaveCount)
		{
			case 1:
			case 2:
			    limit = 1;
			    break;

			case 3:
			case 4:
			    limit = 2;
				break;

			default:
			    limit = 3;
				break;
		}

        // Start generating the new wave
		StartCoroutine(GenerateWave(limit));
	}

    public List<GameObject> SpawnedObjects
    {
        get
        {
            return spawned_objects;
        }

        set
        {
            spawned_objects = value;
        }
    }
}
