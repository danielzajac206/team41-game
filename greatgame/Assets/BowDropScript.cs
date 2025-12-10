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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetBowPrefab(bowPrefab);
            Destroy(gameObject);
        }
    }
}
