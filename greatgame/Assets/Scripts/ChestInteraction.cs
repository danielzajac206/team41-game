using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChestInteraction : MonoBehaviour
{
    public GameObject rewardPrefab; // e.g., a special item or particle effect

    // Ensure there's a collider so OnMouseDown will be called when clicking the chest.
    private void Awake()
    {
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = false;
            Debug.Log("ChestInteraction: added BoxCollider automatically to allow clicks.");
        }
    }

    // This is a simple way to detect a click on the object in 3D space
    private void OnMouseDown()
    {
        // Ensure the chest is not clicked twice
        if (gameObject.activeInHierarchy)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        Debug.Log("Chest clicked! Spawning reward.");
        
        // Spawn the reward at the chest's location (or slightly above it)
        if (rewardPrefab != null)
        {
            Instantiate(rewardPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        // Add any animation or particle effects for opening here

        // Destroy the chest object after opening
        Destroy(gameObject);
    }
}