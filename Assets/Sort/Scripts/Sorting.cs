using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{
    private enum SortingTypes { BubbleSort, ShakerSort, CombSort, InsertionSort }

    //TODO: Create dynamic count
    private const int COUNT = 21; // Count of elements for sorting

    private Color defaultColor = Color.black;
    private Color pointerColor = Color.white;

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
            block.GetComponent<MeshRenderer>().material.color = defaultColor;

            elements.Add(block);
        }
    }

    public void StarSorting()
    {
        sortingType = (SortingTypes)dropDown.value;

        switch (sortingType)
        {
            case SortingTypes.BubbleSort:
                BubbleSorting bubbleSorting = new BubbleSorting();
                IEnumerator bubble = bubbleSorting.BubbleSort(elements);
                StartCoroutine(bubble);
                break;

            case SortingTypes.ShakerSort:
                ShakerSorting shakerSorting = new ShakerSorting();
                IEnumerator shaker = shakerSorting.ShakerSort(elements);
                StartCoroutine(shaker);
                break;

            case SortingTypes.CombSort:
                CombSorting combSorting = new CombSorting();
                IEnumerator comb = combSorting.CombSort(elements);
                StartCoroutine(comb);
                break;

            case SortingTypes.InsertionSort:
                InsertionSorting insertionSorting = new InsertionSorting();
                IEnumerator insertion = insertionSorting.InsertionSort(elements);
                StartCoroutine(insertion);
                break;

            default:
                break;
        }
    }

    public void ShuffleElements()
    {
        int randomCount = elements.Count;

        while (randomCount > 1)
        {
            randomCount--;
            int randomIndex = Random.Range(0, elements.Count);

            GameObject temp = elements[randomIndex];
            elements[randomIndex] = elements[randomCount];
            elements[randomCount] = temp;

            /*elements[random1].transform.position = new Vector3(elements[random2].transform.position.x, elements[random1].transform.localScale.y / 2, 0f);
            elements[random2].transform.position = new Vector3(elements[random1].transform.position.x, elements[random2].transform.localScale.y / 2, 0f);*/
        }
    }
}