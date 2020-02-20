using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structures : MonoBehaviour
{
    // Stack
    public class Stack
    {
        List<int> stack = new List<int>();
        int head = -1;

        public void Push(int _element)
        {
            head++;
            stack.Add(_element);
        }

        public int? Pop()
        {
            if (head != -1)
            {
                head--;
                return stack[head + 1];
            } else
            {
                return null; // Try to return element from empty Stack
            }
        } 

        public int? Peek()
        {
            if (head != -1)
            {
                return stack[head + 1];
            }
            else
            {
                return null; // Try to return element from empty Stack
            }
        }

        public bool IsEmpty()
        {
            return head == -1;
        }
    }

    // Queue
    public class Queue
    {
        int[] queue;
        int head = 0;
        int tail = 0;

        public Queue(int _count)
        {
            queue = new int[_count];
        }

        public void Push(int _element)
        {
            head++;
            queue[head] = _element;
        }

        public int? Pop()
        {
            if (head != -1)
            {
                head--;
                return queue[head + 1];
            }
            else
            {
                return null; // Try to return element from empty Stack
            }
        }

        public bool IsEmpty()
        {
            return head == tail;
        }
    }

    // Dequeue
    public class Dequeue
    {

    }

    // Binary tree
    public class TreeNode
    {
        public TreeNode(int data)
        {
            this.data = data;
        }

        public int data { get; set; }
        public TreeNode leftSubTree { get; set; }
        public TreeNode rightSubTree { get; set; }

        // Recursive adding node to tree
        public void Insert(TreeNode node)
        {
            if (node.data < data)
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
        /*public List<GameObject> TreeToList(List<GameObject> tempElements = null)
        {
            if (tempElements == null)
                tempElements = new List<GameObject>();

            if (leftSubTree != null)
                leftSubTree.TreeToList(tempElements);

            tempElements.Add(data);

            if (rightSubTree != null)
                rightSubTree.TreeToList(tempElements);

            return tempElements;
        }*/
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

    // Hash table
    public class HashTable
    {

    }

    // Linked List
    public class LinkedList
    {

    }

    // Graph
    public class Graph
    {

    }
}
