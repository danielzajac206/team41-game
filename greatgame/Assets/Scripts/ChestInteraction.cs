using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChestInteraction : MonoBehaviour
{
    public GameObject rewardPrefab;
    [SerializeField] PlayerController player;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = false;
        }
    }

    //private void OnMouseDown()
    //{
    //    if (gameObject.activeInHierarchy)
    //    {
    //        OpenChest();
    //    }
    //}

    public void OpenChest()
    {
        
        if (rewardPrefab != null)
        {
            //Instantiate(rewardPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            //player.SetBowPrefab(rewardPrefab);
        }

        Destroy(gameObject);
    }
}