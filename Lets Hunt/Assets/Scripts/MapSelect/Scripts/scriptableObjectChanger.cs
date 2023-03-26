using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptableObjectChanger : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjects;
    [SerializeField] private MapDisplay mapDisplay;
    private int currentIndex;

    private void Awake()
    {
       ChangeMap(0);
    }

    public void ChangeMap(int index)
    {
        currentIndex += index;
        if(currentIndex < 0)
        {
            currentIndex = scriptableObjects.Length - 1;
        }
        else if(currentIndex > scriptableObjects.Length - 1)
        {
            currentIndex = 0;
        }
        if(mapDisplay != null)
        {
            mapDisplay.DisplayMap((Map)scriptableObjects[currentIndex]);
        }
    }
}
