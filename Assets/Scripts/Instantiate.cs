using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    public bool isCoroutineReady = false;
    public float howMuchToWait = 0;

    public void CoroutineStarter()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            while (!isCoroutineReady)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f + howMuchToWait/5);
            SpawnObstacle();
            isCoroutineReady = false;
        }
    }

    void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(prefabs[Random.Range(0,prefabs.Length)], transform.position, transform.rotation);
        obstacle.GetComponent<ObstacleMover>().speed = FindObjectOfType<BackgroundMover>().moveSpeed;
        Debug.Log("I'm trying to instantiate");
    }
}
