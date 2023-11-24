using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GhostFactory : MonoBehaviour
{
    public static GhostFactory instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Player Controller")]
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private Transform _contane;
    [SerializeField] private bool isStartGenerate = false;
    [Header("Data")]
    [SerializeField] private List<GhostUnitData> m_AvailableGhostUnits = new List<GhostUnitData>();
    //[SerializeField] private GhostController[] availableGhost;
    [Header("Ghots Live")]
    [SerializeField] public List<GhostController> AliveGhotsList = new List<GhostController>();

    private void Start()
    {
        StartCoroutine(GenerateGhost());
    }
    private void Update()
    {
        for (int i = 0; i < AliveGhotsList.Count; i++)
        {
            AliveGhotsList[i].timeToAlive -= Time.deltaTime;
            if (AliveGhotsList[i].timeToAlive < 0)
            {
                AliveGhotsList[i].timeToAlive = 0;
                Destroy(AliveGhotsList[i].gameObject);
                AliveGhotsList.Remove(AliveGhotsList[i]);
            }
        }
    }
    public void Initialize()
    {
        m_AvailableGhostUnits = new List<GhostUnitData>();
        m_AvailableGhostUnits.Clear();
        for (int i = 0; i < GhostDataObject.instance.GhostUnitData_listZone.Count; i++)
        {
            GhostUnitData ghostUnit = new GhostUnitData();
            ghostUnit.unitData = GhostDataObject.instance.GhostUnitData_listZone[i].unitData;
            ghostUnit.detail = GhostDataObject.instance.GhostUnitData_listZone[i].detail;
            m_AvailableGhostUnits.Add(ghostUnit);
        }
    }
    public void GenerateStart()
    {
        if (!isStartGenerate)
        {
            for (int i = 0; i < GhostDataObject.instance.GhostUnitData_listZone.Count; i++)
            {
                GhostUnitData ghostUnit = new GhostUnitData();
                ghostUnit.unitData = GhostDataObject.instance.GhostUnitData_listZone[i].unitData;
                ghostUnit.detail = GhostDataObject.instance.GhostUnitData_listZone[i].detail;
                m_AvailableGhostUnits.Add(ghostUnit);
            }
            InstanceGhost();
            isStartGenerate = true;
        }
    }
    public IEnumerator GenerateGhost()
    {
        while (true)
        {
            InstanceGhost();
            yield return new WaitForSeconds(GhostDataObject.instance.waitTime);
        }
    }
    public void InstanceGhost()
    {
        if (m_AvailableGhostUnits.Count == 0) 
        return;
        for (int i = 0; i < UnityEngine.Random.Range(2, 3); i++)
        {
            int index = UnityEngine.Random.Range(0, m_AvailableGhostUnits.Count);
            float x = PlayerController.transform.position.x + GennerateRange();
            float z = PlayerController.transform.position.z + GennerateRange();
            float y = PlayerController.transform.position.y;
            GhostController GhostInstan = Instantiate(m_AvailableGhostUnits[index].detail._ghost_ctr, new Vector3(x, y + (0.8f), z), Quaternion.identity, _contane.transform);
            GhostInstan.name = (index + 1).ToString();
            GhostInstan.ghostData = m_AvailableGhostUnits[index].unitData;
            GhostInstan.ghostDetail = m_AvailableGhostUnits[index].detail;
            GhostInstan.timeToAlive = UnityEngine.Random.Range(5f, GhostInstan.maxTimeAlive);
            GhostInstan.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 180), 0f);
            AliveGhotsList.Add(GhostInstan);
            m_AvailableGhostUnits.RemoveAt(index);
        }
       
    }
    public float GennerateRange()
    {
        float range = UnityEngine.Random.Range(GhostDataObject.instance.minRange, GhostDataObject.instance.maxRange);
        bool isPositive = UnityEngine.Random.Range(0, 10) < 5;
        return range * (isPositive ? 1 : -1);
    }
}
