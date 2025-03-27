using UnityEngine;

public class ChangeCustomer : MonoBehaviour
{
    public List<GameObject> list_Customers;

    public GameObject gameObject_activeCustomer;
    public GameObject gameObject_nextCustomer;

    public GameObject gameObject_BoxText;
    public GameObject gameObject_BoxName;
    public GameObject gameObject_ObjectToBuy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject_activeCustomer = list_Customers[random];
    }

    public void Change()
    {

    }
}
