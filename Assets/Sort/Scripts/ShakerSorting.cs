using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerSorting : MonoBehaviour
{
    public IEnumerator ShakerSort(List<GameObject> elements)
    {
        const int SPEED = 50; // Speed changing elements while sorting
        const float DELAY = 0.05f; // Delay between frames chaning elements position

        bool sorted = true;

        int begin = 0;
        int end = elements.Count - 1;

        while (sorted)
        {
            sorted = false;

            for (int i = begin; i < end; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + 1].transform.localScale.y)
                {
                    // Sorting
                    GameObject temp = elements[i];
                    elements[i] = elements[i + 1];
                    elements[i + 1] = temp;
                    sorted = true;

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
                }
            }

            if (sorted) end--;

            for (int i = end; i > begin; i--)
            {
                if (elements[i].transform.localScale.y < elements[i - 1].transform.localScale.y)
                {
                    // Sorting
                    GameObject temp = elements[i];
                    elements[i] = elements[i - 1];
                    elements[i - 1] = temp;
                    sorted = true;

                    // Visualization
                    Vector3 posOne = elements[i].transform.position;
                    Vector3 newPosOne = new Vector3(elements[i - 1].transform.position.x, elements[i].transform.localScale.y / 2, 0f);

                    Vector3 posTwo = elements[i - 1].transform.position;
                    Vector3 newPosTwo = new Vector3(elements[i].transform.position.x, elements[i - 1].transform.localScale.y / 2, 0f);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        posOne = Vector3.Lerp(posOne, newPosOne, SPEED * Time.deltaTime);
                        elements[i].transform.position = posOne;

                        posTwo = Vector3.Lerp(posTwo, newPosTwo, SPEED * Time.deltaTime);
                        elements[i - 1].transform.position = posTwo;

                        yield return new WaitForSeconds(DELAY);
                    }
                }
            }

            if (sorted) begin++;
        }
    }
}