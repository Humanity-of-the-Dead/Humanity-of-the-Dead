using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testUpdate : MonoBehaviour
{
    [SerializeField] textdisplay textdisplay;

    private void OnEnable()
    {
        textdisplay.UpdateText();
    }
}
