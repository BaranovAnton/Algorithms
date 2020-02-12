using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour
{
    #region Constants
    const int SPEED = 50; // Speed changing elements while sorting
    const float DELAY = 0.05f; // Delay between frames chaning elements position
    #endregion

    private enum SortingTypes { BubbleSort, ShakerSort, CombSort, InsertionSort, GnomeSort, TreeSort, QuickSort, SelectionSort, HeapSort, Counting }

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
            float height = Random.Range(1, 10);
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
            case SortingTypes.GnomeSort:
                StartCoroutine(GnomeSorting());
                break;
            case SortingTypes.TreeSort:
                StartCoroutine(TreeSorting());
                break;
            case SortingTypes.QuickSort:
                StartCoroutine(QuickSorting());
                break;
            case SortingTypes.SelectionSort:
                StartCoroutine(SelectionSorting());
                break;
            case SortingTypes.HeapSort:
                StartCoroutine(HeapSorting());
                break;
            case SortingTypes.Counting:
                StartCoroutine(CountingSorting());
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

    // https://en.wikipedia.org/wiki/Gnome_sort
    IEnumerator GnomeSorting()
    {
        int i = 1, j = 2;

        while (i < elements.Count)
        {
            if (elements[i - 1].transform.localScale.y < elements[i].transform.localScale.y)
            {
                i = j;
                j++;
            }
            else
            {
                Swap(elements, i - 1, i);
                #region Visualization
                Vector3 posOne, newPosOne, posTwo, newPosTwo;
                CalcNewPositions(elements, i - 1, i, out posOne, out newPosOne, out posTwo, out newPosTwo);

                while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                {
                    ChangePositions(elements, i - 1, i, ref posOne, newPosOne, ref posTwo, newPosTwo);
                    yield return new WaitForSeconds(DELAY);
                }
                #endregion

                i--;
                if (i == 0)
                {
                    i = j;
                    j++;
                }
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

        // Retrieve elements in sorting order
        elements = treeNode.TreeToList();

        // Change position (there is no visual delay)
        for (int i = 0, pos = -COUNT / 2; i < COUNT; i++, pos++)
        {
            float height = elements[i].transform.localScale.y;
            elements[i].transform.position = new Vector3(pos, height / 2, 0f);
        }

        yield return null;
    }

    // https://en.wikipedia.org/wiki/Quicksort
    IEnumerator QuickSorting()
    {
        GameObject[] elementsArray = elements.ToArray();

        QuickSort(elementsArray, 0, elementsArray.Length - 1);

        elements = new List<GameObject>(elementsArray);

        // Change position (there is no visual delay)
        for (int i = 0, pos = -COUNT / 2; i < COUNT; i++, pos++)
        {
            float height = elements[i].transform.localScale.y;
            elements[i].transform.position = new Vector3(pos, height / 2, 0f);
        }

        yield return null;
    }

    // https://en.wikipedia.org/wiki/Selection_sort
    IEnumerator SelectionSorting()
    {
        for (int i = 0; i < elements.Count - 1; i++)
        {
            int jMin = i;

            for (int j=i+1; j < elements.Count; j++)
            {
                if (elements[j].transform.localScale.y < elements[jMin].transform.localScale.y)
                {
                    jMin = j;
                }
            }

            if (jMin != i)
            {
                Swap(elements, i, jMin);

                #region Visualization
                Vector3 posOne, newPosOne, posTwo, newPosTwo;
                CalcNewPositions(elements, i, jMin, out posOne, out newPosOne, out posTwo, out newPosTwo);

                while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                {
                    ChangePositions(elements, i, jMin, ref posOne, newPosOne, ref posTwo, newPosTwo);
                    yield return new WaitForSeconds(DELAY);
                }
                #endregion
            }
        }

        yield return null;
    }

    // https://en.wikipedia.org/wiki/Heapsort
    IEnumerator HeapSorting()
    {
        Heap heap = new Heap();
        heap.HeapSort(elements);

        // Change position (there is no visual delay)
        for (int i = 0, pos = -COUNT / 2; i < COUNT; i++, pos++)
        {
            float height = elements[i].transform.localScale.y;
            elements[i].transform.position = new Vector3(pos, height / 2, 0f);
        }

        yield return null;
    }

    // https://en.wikipedia.org/wiki/Counting_sort
    // Element value has to be less than count of elements
    // Elements have to be integer
    IEnumerator CountingSorting()
    {
        int[] tempArray = new int[elements.Count];
        for (int i=0; i< tempArray.Length; i++)
            tempArray[(int)elements[i].transform.localScale.y] = tempArray[(int)elements[i].transform.localScale.y] + 1;

        int b = 0;
        for (int i=0; i< tempArray.Length; i++)
        {
            for (int j=0; j<tempArray[i]; j++)
            {
                elements[b].transform.localScale = new Vector3(elements[b].transform.localScale.x, i, elements[b].transform.localScale.z);

                #region Visualization
                /*Vector3 posOne, newPosOne, posTwo, newPosTwo;
                CalcNewPositions(elements, b, b+1, out posOne, out newPosOne, out posTwo, out newPosTwo);

                while (Vector3.Distance(posOne, newPosOne) > 0.01f)
                {
                    ChangePositions(elements, b, b+1, ref posOne, newPosOne, ref posTwo, newPosTwo);
                    yield return new WaitForSeconds(DELAY);
                }*/
                #endregion

                b++;
            }
        }

        // Change position (there is no visual delay)
        for (int i = 0, pos = -COUNT / 2; i < COUNT; i++, pos++)
        {
            float height = elements[i].transform.localScale.y;
            elements[i].transform.position = new Vector3(pos, height / 2, 0f);
        }

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

    private int Partition(GameObject[] nums, int low, int high)
    {
        // Choose pivot element
        int pivot = low;
        for (int i = low; i <= high; i++)
        {
            if (nums[i].transform.localScale.y <= nums[high].transform.localScale.y)
            {
                GameObject temp = nums[pivot];
                nums[pivot] = nums[i];
                nums[i] = temp;
                pivot += 1;
            }
        }
        return pivot - 1;
    }

    private void QuickSort(GameObject[] nums, int low, int high)
    {
        if (low < high)
        {
            int p = Partition(nums, low, high);
            QuickSort(nums, low, p - 1);
            QuickSort(nums, p + 1, high);
        }
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

    // Specific heap structure
    public class Heap
    {
        private List<GameObject> list;

        public int HeapSize {
            get { return list.Count; }
        }

        public void Heapify(int i)
        {
            int leftChild, rightChild, largestChild;

            for (; ; )
            {
                leftChild = 2 * i + 1;
                rightChild = 2 * i + 2;
                largestChild = i;

                if (leftChild < HeapSize && list[leftChild].transform.localScale.y > list[largestChild].transform.localScale.y)
                    largestChild = leftChild;

                if (rightChild < HeapSize && list[rightChild].transform.localScale.y > list[largestChild].transform.localScale.y)
                    largestChild = rightChild;

                if (largestChild == i)
                    break;

                GameObject temp = list[i];
                list[i] = list[largestChild];
                list[largestChild] = temp;
                i = largestChild;
            }
        }

        public void BuildHeap(GameObject[] sourceArray)
        {
            list = sourceArray.ToList();

            for (int i = HeapSize / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        public GameObject GetMax()
        {
            GameObject result = list[0];
            list[0] = list[HeapSize - 1];
            list.RemoveAt(HeapSize - 1);

            return result;
        }

        public void HeapSort(List<GameObject> sourceList)
        {
            BuildHeap(sourceList.ToArray());

            for (int i = sourceList.Count - 1; i >= 0; i--)
            {
                sourceList[i] = GetMax();
                Heapify(0);
            }
        }
    }
    #endregion
}