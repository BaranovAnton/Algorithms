using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{
    private enum SortingTypes { BubbleSort, ShakerSort, CombSort }

    private const int COUNT = 21; // Count of elements for sorting

    private SortingTypes sortingType;
    private List<GameObject> elements = new List<GameObject>();

    public Dropdown dropDown;

    void Start()
    {
        // Add options (sorting types) to DropDown
        var types = new List<string>();
        string[] typeNames = System.Enum.GetNames(typeof(SortingTypes));
        for (int i = 0; i < typeNames.Length; i++)
            types.Add(typeNames[i]);
        dropDown.AddOptions(types);

        // Instantiate elements on the scene
        CreateElements();
    }

    private void CreateElements()
    {
        // Delete all previous elements if there are
        for (int i=0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        // Creat block with random height
        for (int i=0, pos = -COUNT/2; i < COUNT; i++, pos++)
        {          
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = "Element_" + i;
            float height = Random.Range(1f, 10f);
            block.transform.localScale = new Vector3(0.9f, height, 0.9f);
            block.transform.position = new Vector3(pos, height/2, 0f);
            block.transform.SetParent(this.transform);

            elements.Add(block);
        }
    }

    public void StarSorting()
    {

    }

    public void MixElements()
    {

    }

    #region SORTING TYPES
    IEnumerable BubbleSort()
    {
        yield return null;
    }

    IEnumerable ShakerSort()
    {
        yield return null;
    }

    IEnumerable CombSort()
    {
        yield return null;
    }
    #endregion
}
