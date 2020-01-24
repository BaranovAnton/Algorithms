using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structures : MonoBehaviour
{
    // Binary tree
    // TODO: Create this solution classical
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

    // Binary heap
    public class Heap
    {
        private List<int> list;

        public int HeapSize {
            get { return list.Count; }
        }

        public void Add(int value)
        {
            list.Add(value);
            int i = HeapSize - 1;
            int parent = (i - 1) / 2;

            while (i > 0 && list[parent] < list[i])
            {
                int temp = list[i];
                list[i] = list[parent];
                list[parent] = temp;

                i = parent;
                parent = (i - 1) / 2;
            }
        }

        public void Heapify(int i)
        {
            int leftChild, rightChild, largestChild;

            for (; ; )
            {
                leftChild = 2 * i + 1;
                rightChild = 2 * i + 2;
                largestChild = i;

                if (leftChild < HeapSize && list[leftChild] > list[largestChild])
                    largestChild = leftChild;

                if (rightChild < HeapSize && list[rightChild] > list[largestChild])
                    largestChild = rightChild;

                if (largestChild == i)
                    break;

                int temp = list[i];
                list[i] = list[largestChild];
                list[largestChild] = temp;
                i = largestChild;
            }
        }

        public void BuildHeap(int[] sourceArray)
        {
            list = sourceArray.ToList();

            for (int i = HeapSize / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        public int GetMax()
        {
            int result = list[0];
            list[0] = list[HeapSize - 1];
            list.RemoveAt(HeapSize - 1);

            return result;
        }

        public void HeapSort(int[] array)
        {
            BuildHeap(array);
            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i] = GetMax();
                Heapify(0);
            }
        }
    }
}
