using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

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
        public TextMeshProUGUI itemCountText;  // Text component to display the item count
    }
    
    public List<CuriositySlot> curiositySlots = new List<CuriositySlot>();
    private Dictionary<string, CuriositySlot> curiositySlotMap = new Dictionary<string, CuriositySlot>();
    
    private int totalCollectibles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        totalCollectibles = FindObjectsOfType<CollectibleItem>().Length;
        openBookUI.SetActive(false);
        closedBookUI.SetActive(true);
        
        // Convert list to dictionary for faster access
        foreach (var slot in curiositySlots)
        {
            curiositySlotMap[slot.curiosityName] = slot;
            slot.slotUI.SetActive(false); // Hide all slots initially
            if (slot.itemCountText != null)
            {
                slot.itemCountText.text = "0";  // Initialize item count text to "0"
            }
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
                curiositySlotMap[itemName].slotUI.SetActive(true);  // Activate the UI slot for this item
                UpdateItemCountText(itemName);  // Update the item count
                Debug.Log($"Activated UI slot for {itemName}.");
            }
            else
            {
                Debug.LogWarning($"No UI slot found for {itemName}.");
            }
        }
        else
        {
            UpdateItemCountText(itemName);  // Update the item count if it's already in the book
        }

        if (collectionBook.Count >= totalCollectibles)
        {
            completionMessage.gameObject.SetActive(true);
            completionMessage.text = "Collection Book Completed!";
        }
    }

    // Update the item count text when an item is collected
    private void UpdateItemCountText(string itemName)
    {
        if (curiositySlotMap.ContainsKey(itemName))
        {
            CuriositySlot slot = curiositySlotMap[itemName];
            if (slot.itemCountText != null)
            {
                slot.itemCountText.text = inventory[itemName].ToString();  // Update the text with the current count
            }
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
