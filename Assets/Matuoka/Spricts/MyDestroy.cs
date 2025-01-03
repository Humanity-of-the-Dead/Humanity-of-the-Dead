using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDestroy : MonoBehaviour
{
    [SerializeField] private float time = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
