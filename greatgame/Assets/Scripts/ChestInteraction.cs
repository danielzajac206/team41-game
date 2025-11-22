using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChestInteraction : MonoBehaviour
{
    public GameObject rewardPrefab;

    private void Awake()
    {
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = false;
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.activeInHierarchy)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        
        if (rewardPrefab != null)
        {
            Instantiate(rewardPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}