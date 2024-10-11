using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    

    [SerializeField] PartsManager Head;
    [SerializeField] PartsManager Arm;
    [SerializeField] PartsManager Leg;

    [SerializeField] BodyPartsData[] sobjHeadDatas;
    [SerializeField] BodyPartsData[] sobjArmDatas;
    [SerializeField] BodyPartsData[] sobjLegDatas;
    // Start is called before the first frame update
    void Start()
    {
        Head.setParameter(sobjHeadDatas[0]);
        Arm.setParameter(sobjArmDatas[0]);
        Leg.setParameter(sobjLegDatas[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
