using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenButtonController : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject barrier;
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;
    [SerializeField] int numCorrect = 0;

    private bool hasSpawned = false;

    public void AddCorrect()
    {
        numCorrect++;
    }

    private void Update()
    {
        if (numCorrect == 2)
        {
            StartCoroutine(BarrierDown());
            SpawnReward();
        }
    }

    IEnumerator BarrierDown()
    {
        barrier.GetComponent<Animator>().Play("barrier_down");
        yield return new WaitForSeconds(1f);
        barrier.SetActive(false);
    }

    public void SpawnReward()
    {
        if (hasSpawned) return;

        if (chestPrefab != null)
        {
            Vector3 spawnPos = new Vector3(spawnX, spawnY, -0.01f);
            Instantiate(chestPrefab, spawnPos, Quaternion.identity);
            
            hasSpawned = true;
        }
    }
}