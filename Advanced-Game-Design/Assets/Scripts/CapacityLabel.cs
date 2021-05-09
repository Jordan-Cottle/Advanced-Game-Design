using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacityLabel : MonoBehaviour
{
    public Slider CapacityDisplay;
    public CapacityContainer Container;

    void Start()
    {
        CapacityDisplay.minValue = 0;
        CapacityDisplay.maxValue = Container.MaxCapacity;
        CapacityDisplay.value = Container.CurrentCapacity;
    }

    void OnGUI()
    {
        CapacityDisplay.value = Container.CurrentCapacity;
    }
}
