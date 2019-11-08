using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSorting : MonoBehaviour
{
    public IEnumerator BubbleSort(List<GameObject> elements)
    {
        const int SPEED = 50; // Speed changing elements while sorting
        const float DELAY = 0.05f; // Delay between frames chaning elements position

        Color defaultColor = Color.black;
        Color pointerColor = Color.white;

    bool sorted = true;
        int max = elements.Count;

        while (sorted)
        {
            sorted = false;
            for (int i = 0; i < max - 1; i++)
            {
                // Change color pointed element
                elements[i].gameObject.GetComponent<MeshRenderer>().material.color = pointerColor;

                if (elements[i].transform.localScale.y > elements[i + 1].transform.localScale.y)
                {
                    // Sorting
                    GameObject temp = elements[i];
                    elements[i] = elements[i + 1];
                    elements[i + 1] = temp;

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
                else
                {

                    elements[i].gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;
                }
            }
            max--;

            // Change color sorted elements
            if (sorted)
                elements[max].gameObject.GetComponent<MeshRenderer>().material.color = pointerColor;
            else
                for (int i = 0; i <= max; i++)
                    elements[i].gameObject.GetComponent<MeshRenderer>().material.color = pointerColor;
        }
    }
}