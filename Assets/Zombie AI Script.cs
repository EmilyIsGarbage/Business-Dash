
using System.Collections;
using UnityEngine;

public enum SpawnState { SPAWNING, WAITING, COUNTING };



public class ZombieAIScript : MonoBehaviour
{
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;

    }


    //[System.Serializable]

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                //begin new round

                WaveCompleted();

                /*   
                   Debug.log("Wave Completed") ;
                   return;                                     
               */
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                //Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));

            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;

        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE! Looping..");
        }

        nextWave++;
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= timeBetweenWaves * Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy") == null)
            {
                return false;
            }

        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        //Spawn

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);

        }

        state = SpawnState.WAITING;


        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn enemy
        Debug.Log("Spawning Enemy: " + _enemy.name);

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }


        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, transform.position, transform.rotation);


    }



}

