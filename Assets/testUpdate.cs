using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testUpdate : MonoBehaviour
{
    [SerializeField] TextDisplay textDisplay;

    private void OnEnable()
    {
        textDisplay.UpdateText();
    }
}
