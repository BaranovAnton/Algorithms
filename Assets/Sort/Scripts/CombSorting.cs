using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://en.wikipedia.org/wiki/Comb_sort
/// </summary>
public class CombSorting : MonoBehaviour
{
    public IEnumerator CombSort(List<GameObject> elements)
    {
        const int SPEED = 50; // Speed changing elements while sorting
        const float DELAY = 0.05f; // Delay between frames chaning elements position

        float shrink = 1.24733f;
        int step = elements.Count - 1;
        
        while (step > 1)
        {
            for (int i=0; i + step < elements.Count; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + step].transform.localScale.y)
                {
                    Swap(elements, i, i+step);

                    // Visualization
                    Vector3 posOne = elements[i].transform.position;
                    Vector3 newPosOne = new Vector3(elements[i + step].transform.position.x, elements[i].transform.localScale.y / 2, 0f);

                    Vector3 posTwo = elements[i + step].transform.position;
                    Vector3 newPosTwo = new Vector3(elements[i].transform.position.x, elements[i + step].transform.localScale.y / 2, 0f);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        posOne = Vector3.Lerp(posOne, newPosOne, SPEED * Time.deltaTime);
                        elements[i].transform.position = posOne;

                        posTwo = Vector3.Lerp(posTwo, newPosTwo, SPEED * Time.deltaTime);
                        elements[i + step].transform.position = posTwo;

                        yield return new WaitForSeconds(DELAY);
                    }
                }
            }

            step = Mathf.FloorToInt(step / shrink);
        }

        // Last iteration when step = 1
        bool sorted = true;
        while (sorted)
        {
            sorted = false;
            for (int i=0; i + 1 < elements.Count; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + 1].transform.localScale.y)
                {
                    Swap(elements, i, i+1);

                    // Visualization
                    Vector3 posOne = elements[i].transform.position;
                    Vector3 newPosOne = new Vector3(elements[i + 1].transform.position.x, elements[i].transform.localScale.y / 2, 0f);

                    Vector3 posTwo = elements[i + 1].transform.position;
                    Vector3 newPosTwo = new Vector3(elements[i].transform.position.x, elements[i + 1].transform.localScale.y / 2, 0f);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        posOne = Vector3.Lerp(posOne, newPosOne, SPEED * Time.deltaTime);
                        elements[i].transform.position = posOne;

                        posTwo = Vector3.Lerp(posTwo, newPosTwo, SPEED * Time.deltaTime);
                        elements[i + 1].transform.position = posTwo;

                        yield return new WaitForSeconds(DELAY);
                    }

                    sorted = true;
                }
            }
        }
    }

    private void Swap(List<GameObject> _elements, int _index1, int _index2)
    {
        GameObject temp = _elements[_index1];
        _elements[_index1] = _elements[_index2];
        _elements[_index2] = temp;
    }

    private void Visualization()
    {

    }
}