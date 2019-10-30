using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{
    private enum SortingTypes { BubbleSort, ShakerSort, CombSort }

    private SortingTypes sortingType;
    private int elementsCount;

    public Dropdown dropDown;

    void Start()
    {
        // Add options (sorting types) to DropDown
        var types = new List<string>();
        string[] typeNames = System.Enum.GetNames(typeof(SortingTypes));
        for (int i = 0; i < typeNames.Length; i++)
            types.Add(typeNames[i]);
        dropDown.AddOptions(types);
    }

    private void SetSortingTypes()
    {

    }

    public void StarSorting()
    {

    }

    public void MixElements()
    {

    }

    /*IEnumerable BubbleSort()
    {

    }*/
}
