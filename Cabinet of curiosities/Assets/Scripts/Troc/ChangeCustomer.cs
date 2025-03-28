using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ChangeCustomer : MonoBehaviour
{
    public List<GameObject> list_Customers;
    private List<GameObject> list_CustomersInGame;

    public GameObject gameObject_activeCustomer;

    public GameObject gameObject_ObjectToBuyParent;
    public GameObject gameObject_ObjectToBuy;
    public GameObject gameObject_CustomerObject;

    public GameObject openCollection;
    public GameObject UI_Troc;

    public TextMeshProUGUI TMP_BoxText;
    public TextMeshProUGUI TMP_BoxName;
    public TextMeshProUGUI TMP_Paying;

    public LayerMask common;
    public LayerMask uncommon;
    public LayerMask rare;
    public LayerMask epic;

    private int price;
    private int paying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        list_CustomersInGame = list_Customers;

        for (int i = 0; i < list_Customers.Count; i++)
        {
            list_Customers[i].SetActive(false);
        }

        gameObject_activeCustomer = list_Customers[Random.Range(0, list_Customers.Count)];
        gameObject_activeCustomer.SetActive(true);

        ChangeInterface(gameObject_activeCustomer);
    }

    public void ChangeInterface(GameObject new_Customer)
    {
        new_Customer.GetComponent<Animator>().Play("Anim_CustomerEnter");

        TMP_BoxText.text = new_Customer.GetComponent<Customer>().text;
        TMP_BoxName.text = new_Customer.GetComponent<Customer>().name;

        gameObject_CustomerObject = new_Customer.GetComponent<Customer>().list_objectsToBuy[Random.Range(0, new_Customer.GetComponent<Customer>().list_objectsToBuy.Count)];
        gameObject_ObjectToBuy = Instantiate(gameObject_CustomerObject, gameObject_ObjectToBuyParent.transform);

        // sprite_ObjectToBuy = new_Customer.GetComponent<Customer>().objectToBuy.GetComponent<Sprite>();
    }

    public void Change()
    {
        gameObject_activeCustomer.GetComponent<Animator>().SetBool("Bool_CustomerLeaving", true);

        list_CustomersInGame.Remove(gameObject_activeCustomer);
        gameObject_activeCustomer = list_CustomersInGame[Random.Range(0, list_CustomersInGame.Count)];
        gameObject_activeCustomer.SetActive(true);

        ChangeInterface(gameObject_activeCustomer);
    }

    public void Buy()
    {
        if(paying >= price)
        {
            CollectionManager manager = FindObjectOfType<CollectionManager>();
            if (manager != null)
            {
                // payer
                paying = 0;

                Debug.Log($"Collecting {gameObject_ObjectToBuy.tag}");
                manager.CollectItem(gameObject_ObjectToBuy.tag);
            }
            else
            {
                Debug.LogError("CollectionManager not found in the scene!");
            }

            Change();
        }

    }

    public void OpenBuyingPanel()
    {
        openCollection.SetActive(true);
        UI_Troc.SetActive(true);

        // gameObject_CustomerObject.GetComponent<LayerMask>() == uncommon

        if ((uncommon & (1 << gameObject_CustomerObject.layer)) != 0)
        {
            UI_Troc.transform.GetChild(0).gameObject.SetActive(true);
            price = 5;
        }
        else if ((rare & (1 << gameObject_CustomerObject.layer)) != 0)
        {
            UI_Troc.transform.GetChild(1).gameObject.SetActive(true);
            price = 10;
        }
        else if ((epic & (1 << gameObject_CustomerObject.layer)) != 0)
        {
            UI_Troc.transform.GetChild(2).gameObject.SetActive(true);
            price = 15;
        }

        TMP_Paying.text = paying.ToString() + " / " + price.ToString();
    }

    public void ClikedCommon(string itemName)
    {
        CollectionManager manager = FindObjectOfType<CollectionManager>();
        if (manager.inventory.ContainsKey(itemName))
        {
            if (manager.inventory[itemName] >= 1)
            {
                manager.inventory[itemName] -= 1;
                paying += 1;
            }
        }

        TMP_Paying.text = paying.ToString() + " / " + price.ToString();
    }

    public void ClikedUnCommon(string itemName)
    {
        CollectionManager manager = FindObjectOfType<CollectionManager>();
        if (manager.inventory.ContainsKey(itemName))
        {
            if (manager.inventory[itemName] >= 1)
            {
                manager.inventory[itemName] -= 1;
                paying += 5;
            }
        }
        TMP_Paying.text = paying.ToString() + " / " + price.ToString();
    }

    public void ClickedRare(string itemName)
    {
        CollectionManager manager = FindObjectOfType<CollectionManager>();
        if (manager.inventory.ContainsKey(itemName))
        {
            if (manager.inventory[itemName] >= 1)
            {
                manager.inventory[itemName] -= 1;
                paying += 10;
            }
        }
        TMP_Paying.text = paying.ToString() + " / " + price.ToString();
    }


}
