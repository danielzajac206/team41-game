using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BowDropScript : MonoBehaviour
{
    [SerializeField] GameObject bowPrefab;
    [SerializeField] TextMeshProUGUI bowText;
    // Start is called before the first frame update
    void Start()
    {
        bowText = GameObject.Find("WorldSpaceCanvas").transform.Find("bow text").GetComponent<TextMeshProUGUI>();
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.PlayerBowPickup();
            player.SetBowPrefab(bowPrefab);
            bowText.gameObject.SetActive(true);
            bowText.gameObject.GetComponent<Animator>().Play("bowtext_enable");
            Destroy(gameObject);
        }
    }
}
