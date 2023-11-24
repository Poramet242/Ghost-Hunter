using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    [Header("Player Controller")]
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private Transform _contane;
    [Header("Data")]
    [SerializeField] public List<ItemDetail> m_AvailableItem = new List<ItemDetail>();
    [Header("Item Live")]
    [SerializeField] public GameObject GunObject;
    [SerializeField] public GameObject ArmerObject;
    [SerializeField] public List<GameObject> AliveItemList = new List<GameObject>();

    private void Start()
    {
        GunObject = ItemDataObject.instance._gunObject;
        ArmerObject = ItemDataObject.instance._armerObject;
        StartCoroutine(GenerateItem());
    }
    public void Initialize()
    {
        m_AvailableItem = new List<ItemDetail>();
        m_AvailableItem.Clear();
        for (int i = 0; i < ItemDataObject.instance.itemInLocation.Count; i++)
        {
            m_AvailableItem.Add(ItemDataObject.instance.itemInLocation[i]);
        }
    }
    IEnumerator GenerateItem()
    {
        while (true)
        {
            InstanceItem();
            yield return new WaitForSeconds(ItemDataObject.instance.waitTime);
        }
    }
    private void Update()
    {
        for (int i = 0; i < AliveItemList.Count; i++)
        {
            AliveItemList[i].GetComponent<ItemController>().timeToAlive -= Time.deltaTime;
            if (AliveItemList[i].GetComponent<ItemController>().timeToAlive < 0)
            {
                AliveItemList[i].GetComponent<ItemController>().timeToAlive = 0;
                Destroy(AliveItemList[i]);
                AliveItemList.Remove(AliveItemList[i]);
            }
        }
    }
    public void InstanceItem()
    {
        if (m_AvailableItem.Count == 0)
            return;
        int index = UnityEngine.Random.Range(0, m_AvailableItem.Count);
        float x = PlayerController.transform.position.x + GennerateRange();
        float z = PlayerController.transform.position.z + GennerateRange();
        float y = PlayerController.transform.position.y;
        GameObject ItemInstan;
        if (m_AvailableItem[index].isWeapon)
        {
            ItemInstan = Instantiate(GunObject, new Vector3(x, y + 4f, z), Quaternion.identity, _contane.transform);
            ItemInstan.name = "Weapon Object";
            ItemInstan.GetComponent<ItemController>().timeToAlive = UnityEngine.Random.Range(10, ItemInstan.GetComponent<ItemController>().maxTimeAlive);
            ItemInstan.GetComponent<ItemController>()._itemDetail = m_AvailableItem[index];
            AliveItemList.Add(ItemInstan);
        }
        else if (m_AvailableItem[index].isArmor)
        {
            ItemInstan = Instantiate(ArmerObject, new Vector3(x, y + 4f, z), Quaternion.identity, _contane.transform);
            ItemInstan.name = "Armor Object";
            ItemInstan.GetComponent<ItemController>().timeToAlive = UnityEngine.Random.Range(10, ItemInstan.GetComponent<ItemController>().maxTimeAlive);
            ItemInstan.GetComponent<ItemController>()._itemDetail = m_AvailableItem[index];
            AliveItemList.Add(ItemInstan);
        }
    }
    public float GennerateRange()
    {
        float range = UnityEngine.Random.Range(GhostDataObject.instance.minRange, GhostDataObject.instance.maxRange);
        bool isPositive = UnityEngine.Random.Range(0, 10) < 5;
        return range * (isPositive ? 1 : -1);
    }
}
