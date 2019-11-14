using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://en.wikipedia.org/wiki/Insertion_sort
/// </summary>
public class InsertionSorting : MonoBehaviour
{
    public IEnumerator InsertionSort(List<GameObject> elements)
    {
        const int SPEED = 50; // Speed changing elements while sorting
        const float DELAY = 0.05f; // Delay between frames chaning elements position

        for (int i=1; i<elements.Count; i++)
        {
            int j = i;
            while (j > 0 && elements[j-1].transform.localScale.y > elements[j].transform.localScale.y)
            {
                Swap(elements, j - 1, j);

                // Visualization
                Vector3 posOne = elements[j - 1].transform.position;
                Vector3 newPosOne = new Vector3(elements[j].transform.position.x, elements[j - 1].transform.localScale.y / 2, 0f);

                Vector3 posTwo = elements[j].transform.position;
                Vector3 newPosTwo = new Vector3(elements[j - 1].transform.position.x, elements[j].transform.localScale.y / 2, 0f);

                while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                {
                    posOne = Vector3.Lerp(posOne, newPosOne, SPEED * Time.deltaTime);
                    elements[j - 1].transform.position = posOne;

                    posTwo = Vector3.Lerp(posTwo, newPosTwo, SPEED * Time.deltaTime);
                    elements[j].transform.position = posTwo;

                    yield return new WaitForSeconds(DELAY);
                }
                
                j--;
            }
        }
    }

    private void Swap(List<GameObject> _elements, int _index1, int _index2)
    {
        GameObject temp = _elements[_index1];
        _elements[_index1] = _elements[_index2];
        _elements[_index2] = temp;
    }
}