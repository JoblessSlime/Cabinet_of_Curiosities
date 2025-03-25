using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public HashSet<string> collectionBook = new HashSet<string>();
    
    public GameObject closedBookUI;
    public GameObject openBookUI;
    public TextMeshProUGUI completionMessage;
    public Button closeButton;
    
    [System.Serializable]
    public class CuriositySlot
    {
        public string curiosityName;
        public GameObject slotUI;
    }
    
    public List<CuriositySlot> curiositySlots = new List<CuriositySlot>();
    private Dictionary<string, GameObject> curiositySlotMap = new Dictionary<string, GameObject>();
    
    private int totalCollectibles;

    void Start()
    {
        totalCollectibles = FindObjectsOfType<CollectibleItem>().Length;
        completionMessage.gameObject.SetActive(false);
        openBookUI.SetActive(false);
        closedBookUI.SetActive(true);
        
        // Convert list to dictionary for faster access
        foreach (var slot in curiositySlots)
        {
            curiositySlotMap[slot.curiosityName] = slot.slotUI;
            slot.slotUI.SetActive(false); // Hide all slots initially
        }
        
        // Assign button function
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCollectionBook);
        }
    }

    public void CollectItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory[itemName] = 1;
        }

        // Check if the item is already in the collection book
        if (!collectionBook.Contains(itemName))
        {
            collectionBook.Add(itemName);
            Debug.Log($"Added {itemName} to collection book.");
            
            if (curiositySlotMap.ContainsKey(itemName))
            {
                curiositySlotMap[itemName].SetActive(true);
                Debug.Log($"Activated UI slot for {itemName}.");
            }
            else
            {
                Debug.LogWarning($"No UI slot found for {itemName}.");
            }
        }
        else
        {
            Debug.Log($"{itemName} was already in the collection book.");
        }

        if (collectionBook.Count >= totalCollectibles)
        {
            completionMessage.gameObject.SetActive(true);
            completionMessage.text = "Collection Book Completed!";
        }
    }

    public void ToggleCollectionBook()
    {
        bool isOpen = openBookUI.activeSelf;
        openBookUI.SetActive(!isOpen);
        closedBookUI.SetActive(isOpen);
    }

    public void CloseCollectionBook()
    {
        openBookUI.SetActive(false);
        closedBookUI.SetActive(true);
    }
}