using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossGateScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] SpriteRenderer lock1;
    [SerializeField] SpriteRenderer lock2;
    [SerializeField] TextMeshProUGUI bossText;
    [SerializeField] TextMeshProUGUI unlockText;

    private bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        lock1.enabled = true;
        lock2.enabled = true;
        bossText.enabled = true;
        unlockText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(unlocked) return;

        if(player.GetComponent<PlayerController>().GetKeys() >= 1)
        {
            lock1.enabled = false;
        }
        if (player.GetComponent<PlayerController>().GetKeys() >= 2)
        {
            lock2.enabled = false;
        }
        if (player.GetComponent<PlayerController>().GetKeys() >= 3)
        {
            StartCoroutine(UnlockBoss());
            bossText.enabled = false;
            unlocked = true;
        }
    }

    IEnumerator UnlockBoss()
    {
        unlockText.enabled = true;
        unlockText.gameObject.GetComponent<Animator>().Play("unlocktext_enable");

        yield return new WaitForSeconds(3f);
        unlockText.gameObject.GetComponent<Animator>().SetTrigger("Disable");
        yield return new WaitForSeconds(2f);
        unlockText.enabled = false;
        gameObject.SetActive(false);
    }
}
