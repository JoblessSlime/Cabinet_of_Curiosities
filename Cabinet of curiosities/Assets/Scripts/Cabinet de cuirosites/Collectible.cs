using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemTag;

    private void OnMouseDown()
    {
        CollectionManager manager = FindObjectOfType<CollectionManager>();
        if (manager != null)
        {
            Debug.Log($"Collecting {gameObject.tag}");
            manager.CollectItem(gameObject.tag);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("CollectionManager not found in the scene!");
        }

    }
    
}
