using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{
    #region Constants
    const int SPEED = 50; // Speed changing elements while sorting
    const float DELAY = 0.05f; // Delay between frames chaning elements position
    #endregion

    private enum SortingTypes { BubbleSort, ShakerSort, CombSort, InsertionSort, TreeSort }

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
                StartCoroutine(BubbleSorting());
                break;
            case SortingTypes.ShakerSort:
                StartCoroutine(ShakerSorting());
                break;
            case SortingTypes.CombSort:
                StartCoroutine(CombSorting());
                break;
            case SortingTypes.InsertionSort:
                StartCoroutine(InsertionSorting());
                break;
            case SortingTypes.TreeSort:
                StartCoroutine(TreeSorting());
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

    #region Sorting implementation
    // https://en.wikipedia.org/wiki/Bubble_sort
    IEnumerator BubbleSorting()
    {
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
                    Swap(elements, i, i + 1);
                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i + 1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i + 1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion
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

    // https://en.wikipedia.org/wiki/Cocktail_shaker_sort
    IEnumerator ShakerSorting()
    {
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
                    Swap(elements, i, i + 1);
                    sorted = true;
                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i + 1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i + 1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion
                }
            }

            if (sorted) end--;

            for (int i = end; i > begin; i--)
            {
                if (elements[i].transform.localScale.y < elements[i - 1].transform.localScale.y)
                {
                    Swap(elements, i, i - 1);
                    sorted = true;
                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i - 1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i - 1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion
                }
            }

            if (sorted) begin++;
        }
    }       

    // https://en.wikipedia.org/wiki/Comb_sort
    IEnumerator CombSorting()
    {
        float shrink = 1.24733f;
        int step = elements.Count - 1;

        while (step > 1)
        {
            for (int i = 0; i + step < elements.Count; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + step].transform.localScale.y)
                {
                    Swap(elements, i, i + step);
                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i + step, out posOne, out newPosOne, out posTwo, out newPosTwo);

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
            for (int i = 0; i + 1 < elements.Count; i++)
            {
                if (elements[i].transform.localScale.y > elements[i + 1].transform.localScale.y)
                {
                    Swap(elements, i, i + 1);
                    #region Visualization
                    Vector3 posOne, newPosOne, posTwo, newPosTwo;
                    CalcNewPositions(elements, i, i + 1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                    while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                    {
                        ChangePositions(elements, i, i + 1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                        yield return new WaitForSeconds(DELAY);
                    }
                    #endregion
                    sorted = true;
                }
            }
        }
    }

    // https://en.wikipedia.org/wiki/Insertion_sort
    IEnumerator InsertionSorting()
    {
        for (int i = 1; i < elements.Count; i++)
        {
            int j = i;
            while (j > 0 && elements[j - 1].transform.localScale.y > elements[j].transform.localScale.y)
            {
                Swap(elements, j - 1, j);
                #region Visualization
                Vector3 posOne, newPosOne, posTwo, newPosTwo;
                CalcNewPositions(elements, j - 1, j, out posOne, out newPosOne, out posTwo, out newPosTwo);

                while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                {
                    ChangePositions(elements, j - 1, j, ref posOne, newPosOne, ref posTwo, newPosTwo);
                    yield return new WaitForSeconds(DELAY);
                }
                #endregion
                j--;
            }
        }
    }

    // https://en.wikipedia.org/wiki/Tree_sort
    IEnumerator TreeSorting()
    {
        // Create binary tree
        TreeNode treeNode = new TreeNode(elements[0]);
        for (int i=1; i<elements.Count; i++)
            treeNode.Insert(new TreeNode(elements[i]));

        elements = treeNode.TreeToList();

        yield return null;
    }
    #endregion

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

    #region Additional classes
    public class TreeNode
    {
        public TreeNode(GameObject data)
        {
            this.data = data;
        }

        public GameObject data { get; set; }
        public TreeNode leftSubTree { get; set; }
        public TreeNode rightSubTree { get; set; }

        // Recursive adding node to tree
        public void Insert(TreeNode node)
        {
            // Specific solution
            if (node.data.transform.localScale.y < data.transform.localScale.y)
            {
                if (leftSubTree == null)
                    leftSubTree = node;
                else
                    leftSubTree.Insert(node);
            }
            else
            {
                if (rightSubTree == null)
                    rightSubTree = node;
                else
                    rightSubTree.Insert(node);
            }
        }

        // Tree to sorting list
        public List<GameObject> TreeToList(List<GameObject> tempElements = null)
        {
            if (tempElements == null)
                tempElements = new List<GameObject>();

            if (leftSubTree != null)
                leftSubTree.TreeToList(tempElements);

            tempElements.Add(data);

            if (rightSubTree != null)
                rightSubTree.TreeToList(tempElements);

            return tempElements;
        }
    }

    Tree one = new Tree();
    #endregion
}