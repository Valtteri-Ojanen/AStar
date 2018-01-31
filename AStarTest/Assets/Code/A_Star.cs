using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour {

    public Transform target;
    public Transform seeker;
    PathGrindManager grid;
    
	void Awake () {
        grid = GetComponent<PathGrindManager>();
	}

    private void Update()
    {

        FindPath(seeker.position, target.position);

    }

    public List<Node> FindPath (Vector3 start, Vector3 end)
    {
        Node startNode = grid.NodeFromWorldPosition(start);
        Node endNode = grid.NodeFromWorldPosition(end);
        Heap<Node> openSet = new Heap<Node>(grid.gridSizeX * grid.gridSizeZ);
        //HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            currentNode.m_bProcessed = true;
            if(currentNode == endNode)
            {
                grid.ResetNodes();
                return RetracePath(startNode, endNode);
            }
            foreach(Node neighbour in currentNode.neighbours)
            {
                if(neighbour.m_bProcessed || neighbour.m_bIsBlocked)
                    continue;
                int newMovementCost = currentNode.m_iGCost + GetDistance(currentNode, neighbour);
                if(newMovementCost < neighbour.m_iGCost || !openSet.Contains(neighbour))
                {
                    neighbour.m_iGCost = newMovementCost;
                    neighbour.m_iHCost = GetDistance(neighbour, endNode);
                    neighbour.m_Parent = currentNode;
                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        grid.path = null;
        grid.ResetNodes();
        return null;
    }

    private List<Node> RetracePath (Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (startNode != currentNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.m_Parent;
        }
        path.Reverse();
        grid.path = path;
        return path;
    }

    private int GetDistance (Node a, Node b) 
    {
        int distX = Mathf.Abs(a.m_iGridX - b.m_iGridX);
        int distZ = Mathf.Abs(a.m_iGridY - b.m_iGridY);

        if(distX > distZ)
            return 14 * distZ + 10 * (distX - distZ);
        else
            return 14 * distX + 10 * (distZ - distX);
    }
}
