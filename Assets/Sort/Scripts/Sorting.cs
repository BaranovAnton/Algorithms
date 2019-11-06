﻿using System.Collections;
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
        sortingType = (SortingTypes)dropDown.value;

        switch (sortingType)
        {
            case SortingTypes.BubbleSort:
                //BubbleSort();

                IEnumerator cor = ElementTranslate(elements[0], elements[1]);
                StartCoroutine(cor);

                break;
            case SortingTypes.ShakerSort:
                StartCoroutine("ShakerSort");
                break;
            case SortingTypes.CombSort:
                StartCoroutine("CombSort");
                break;
            default:
                break;
        }
    }

    public void MixElements()
    {

    }

    #region SORTING TYPES
    private void BubbleSort()
    {
        bool sorted = true;
        int max = COUNT;

        while (sorted)
        {
            sorted = false;
            for (int i=0; i<max-1; i++)
            {
                if (elements[i].transform.localScale.y > elements[i+1].transform.localScale.y)
                {
                    /*Vector3 posOne = elements[i].transform.position;
                    Vector3 posTwo = elements[i + 1].transform.position;*/

                    GameObject temp = elements[i];
                    elements[i] = elements[i + 1];
                    elements[i + 1] = temp;

                    IEnumerator cor = ElementTranslate(elements[i], elements[i+1]);
                    StartCoroutine(cor);

                    //elements[i].transform.position = new Vector3(posOne.x, elements[i].transform.localScale.y / 2, 0f);
                    /*IEnumerator cor = ElementTranslate(elements[i].transform.position, new Vector3(posOne.x, elements[i].transform.localScale.y / 2, 0f));
                    StartCoroutine(cor);*/

                    //elements[i + 1].transform.position = new Vector3(posTwo.x, elements[i+1].transform.localScale.y / 2, 0f);

                    sorted = true;
                }
            }
            max--;
        }
    }

    IEnumerator ShakerSort()
    {
        yield return null;
    }

    IEnumerator CombSort()
    {
        yield return null;
    }
    #endregion

    IEnumerator ElementTranslate(GameObject _goPrev, GameObject _goNext)
    {
        Vector3 posOne = _goNext.transform.position;
        Vector3 posTwo = _goPrev.transform.position;

        Vector3 _posPrev = _goPrev.transform.position;
        Vector3 _newPosPrev = new Vector3(posOne.x, _goPrev.transform.localScale.y / 2, 0f);

        Vector3 _posNext = _goNext.transform.position;
        Vector3 _newPosNext = new Vector3(posTwo.x, _goNext.transform.localScale.y / 2, 0f);

        while (Vector3.Distance(_posPrev, _newPosPrev) > 0.01f)
        {
            _posPrev = Vector3.Lerp(_posPrev, _newPosPrev, 5 * Time.deltaTime);
            _goPrev.transform.position = _posPrev;

            _posNext = Vector3.Lerp(_posNext, _newPosNext, 10 * Time.deltaTime);
            _goNext.transform.position = _posNext;

            yield return new WaitForSeconds(0.1f);
        }
    }
}