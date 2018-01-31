﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : IHeapItem<Node>
{

    public bool m_bIsBlocked;
    public Vector3 m_vPosition;

    public int m_iGridX;
    public int m_iGridY;
    public List<Node> neighbours;

    public int m_iGCost;
    public int m_iHCost;

    public Node m_Parent;

    public bool m_bProcessed = false;

    int m_iHeapIndex;

    public int m_ifCost
    {
        get
        {
            return m_iGCost + m_iHCost;
        }
    }


    public int HeapIndex
    {
        get
        {
            return m_iHeapIndex;
        }

        set
        {
            m_iHeapIndex = value;
        }
    }

    public int CompareTo( Node node )
    {
        int iComp = m_ifCost.CompareTo(node.m_ifCost);
        if(iComp == 0)
        {
            iComp = m_iHCost.CompareTo(node.m_iHCost);
        }
        return ~iComp;
    }

    public Node( bool bIsBlocked, Vector3 vPos, int x, int y )
    {
        m_bIsBlocked = bIsBlocked;
        m_vPosition = vPos;
        m_iGridX = x;
        m_iGridY = y;
    }




}
