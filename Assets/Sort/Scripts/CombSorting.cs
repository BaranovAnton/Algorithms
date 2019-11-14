using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://en.wikipedia.org/wiki/Comb_sort
/// </summary>
public class CombSorting : MonoBehaviour
{
    #region Constants
    const int SPEED = 50; // Speed changing elements while sorting
    const float DELAY = 0.05f; // Delay between frames chaning elements position
    #endregion

    public IEnumerator CombSort(List<GameObject> elements)
    {
        float shrink = 1.24733f;
        int step = elements.Count - 1;

        while (step > 1)
        {
            for (int i=0; i + step < elements.Count; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + step].transform.localScale.y)
                {
                    Swap(elements, i, i+step);

                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i+step, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i + step, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion
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
                    Swap(elements, i, i + 1);

                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i+1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i+1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion

                    sorted = true;
                }
            }
        }
    }

    #region Additional private methods
    private void Swap(List<GameObject> _elements, int _index1, int _index2)
    {
        GameObject temp = _elements[_index1];
        _elements[_index1] = _elements[_index2];
        _elements[_index2] = temp;
    }

    private void CalcNewPositions(List<GameObject> elements, int index1, int index2, out Vector3 posOne, out Vector3 newPosOne, out Vector3 posTwo, out Vector3 newPosTwo)
    {
        posOne = elements[index1].transform.position;
        newPosOne = new Vector3(elements[index2].transform.position.x, elements[index1].transform.localScale.y / 2, 0f);
        posTwo = elements[index2].transform.position;
        newPosTwo = new Vector3(elements[index1].transform.position.x, elements[index2].transform.localScale.y / 2, 0f);
    }

    private void ChangePositions(List<GameObject> elements, int index1, int index2, ref Vector3 posOne, Vector3 newPosOne, ref Vector3 posTwo, Vector3 newPosTwo)
    {
        posOne = Vector3.Lerp(posOne, newPosOne, SPEED * Time.deltaTime);
        elements[index1].transform.position = posOne;
        posTwo = Vector3.Lerp(posTwo, newPosTwo, SPEED * Time.deltaTime);
        elements[index2].transform.position = posTwo;
    }
    #endregion
}