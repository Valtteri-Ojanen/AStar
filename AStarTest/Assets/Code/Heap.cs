using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

    T[] items;
    int currentItemCount;

    public Heap(int heapSize)
    {
        items = new T[heapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        ++currentItemCount;
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int temp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = temp;
    }

    public T RemoveFirst()
    {
        T first = items[0];
        --currentItemCount;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return first;
    }

    void SortDown (T item)
    {
        while(true)
        {
            int left = (item.HeapIndex * 2) + 1;
            int right = (item.HeapIndex * 2) + 2;

            int swapIndex = 0;

            if(left < currentItemCount)
            {
                swapIndex = left;
                if(right < currentItemCount)
                {
                    if(items[left].CompareTo(items[right]) < 0)
                    {
                        swapIndex = right;
                    } 
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
            {
                return;
            }
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
	
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
