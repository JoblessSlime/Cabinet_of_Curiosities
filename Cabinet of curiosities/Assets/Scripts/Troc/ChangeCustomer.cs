using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ChangeCustomer : MonoBehaviour
{
    public List<GameObject> list_Customers;
    private List<GameObject> list_CustomersInGame = new List<GameObject>();

    public GameObject gameObject_activeCustomer;
    public int maxCustomers;
    private int numberOfCustomers;

    public GameObject gameObject_ObjectToBuyParent;
    public GameObject gameObject_ObjectToBuy;
    public GameObject gameObject_CustomerObject;

    public GameObject openCollection;
    public GameObject UI_Troc;

    public GameObject shop;
    public GameObject ui_shop;

    public TextMeshProUGUI TMP_BoxText;
    public TextMeshProUGUI TMP_BoxName;
    public TextMeshProUGUI TMP_Paying;

    public LayerMask common;
    public LayerMask uncommon;
    public LayerMask rare;
    public LayerMask epic;

    public AudioSource SFX;

    private int price;
    private int paying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0;  i < list_Customers.Count; i++)
        {
            list_CustomersInGame.Add(list_Customers[i]);
        }

        gameObject_activeCustomer = list_Customers[Random.Range(0, list_Customers.Count)];
        gameObject_activeCustomer.SetActive(true);

        ChangeInterface(gameObject_activeCustomer);
    }

    public void ChangeInterface(GameObject new_Customer)
    {
        new_Customer.GetComponent<Animator>().SetBool("Bool_CustomerLeaving", false);

        TMP_BoxText.text = new_Customer.GetComponent<Customer>().text;
        TMP_BoxName.text = new_Customer.GetComponent<Customer>().name;

        gameObject_CustomerObject = new_Customer.GetComponent<Customer>().list_objectsToBuy[Random.Range(0, new_Customer.GetComponent<Customer>().list_objectsToBuy.Count)];
        if(gameObject_ObjectToBuyParent.transform.childCount > 0)
        {
            Destroy(gameObject_ObjectToBuyParent.transform.GetChild(0).gameObject);
        }
        gameObject_ObjectToBuy = Instantiate(gameObject_CustomerObject, gameObject_ObjectToBuyParent.transform);

        // sprite_ObjectToBuy = new_Customer.GetComponent<Customer>().objectToBuy.GetComponent<Sprite>();
    }

    public void Change()
    {
        gameObject_activeCustomer.GetComponent<Animator>().SetBool("Bool_CustomerLeaving", true);
        list_CustomersInGame.Remove(gameObject_activeCustomer);
        numberOfCustomers += 1;

        if(list_CustomersInGame.Count <= 0)
        {
            Debug.Log("count < 0");
            for (int i = 0; i < list_Customers.Count; i++)
            {
                list_CustomersInGame.Add(list_Customers[i]);
            }
            Debug.Log(list_CustomersInGame.Count);
        }


        gameObject_activeCustomer = list_CustomersInGame[Random.Range(0, list_CustomersInGame.Count)];
        gameObject_activeCustomer.SetActive(true);

        ChangeInterface(gameObject_activeCustomer);
    }

    public void Buy()
    {
        SFX.Play();

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

            UI_Troc.SetActive(false);
            openCollection.SetActive(false);

            Change();
        }

    }

    public void OpenBuyingPanel()
    {
        SFX.Play();

        openCollection.SetActive(true);
        UI_Troc.SetActive(true);

        UI_Troc.transform.GetChild(0).gameObject.SetActive(false);
        UI_Troc.transform.GetChild(1).gameObject.SetActive(false);
        UI_Troc.transform.GetChild(2).gameObject.SetActive(false);

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
        SFX.Play();

        CollectionManager manager = FindObjectOfType<CollectionManager>();
        if (manager.inventory.ContainsKey(itemName))
        {
            if (manager.inventory[itemName] >= 1)
            {
                manager.inventory[itemName] -= 1;
                manager.UpdateItemCountText(itemName);
                paying += 1;
            }
        }

        TMP_Paying.text = paying.ToString() + " / " + price.ToString();
    }

    public void ClikedUnCommon(string itemName)
    {
        SFX.Play();

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
        SFX.Play();

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
