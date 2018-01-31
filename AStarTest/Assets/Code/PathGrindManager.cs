using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrindManager : MonoBehaviour {

    public LayerMask obstacleMask;
    public Vector2 gridSize;
    public float halfNodeWidth;
    public int gridSizeX, gridSizeZ;
    public List<Node> path;

    Node[,] grid;

    public void Start()
    {
        gridSizeX = Mathf.RoundToInt(gridSize.x / (halfNodeWidth * 2));
        gridSizeZ = Mathf.RoundToInt(gridSize.y / (halfNodeWidth * 2));
        CreateGrid();
    }
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = new Vector3(transform.position.x - gridSize.x / 2, 0, transform.position.z - gridSize.y / 2);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = new Vector3(worldBottomLeft.x + x * halfNodeWidth * 2 +halfNodeWidth, 0 , worldBottomLeft.z + z * halfNodeWidth * 2 + halfNodeWidth);
                bool walkAble = Physics.CheckSphere(worldPoint, halfNodeWidth - 0.05f, obstacleMask);
                grid[x, z] = new Node(walkAble, worldPoint,x,z);
            }
        }
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                grid[x, z].neighbours = GetNeighbours(grid[x, z]);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if(grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.m_bIsBlocked) ? Color.black : Color.white;
                if(path != null)
                        if(path.Contains(n))
                             Gizmos.color = Color.red;
                Gizmos.DrawWireCube(n.m_vPosition, new Vector3(2 * halfNodeWidth, 1, 2 *halfNodeWidth));
            }
            Gizmos.color = Color.blue;

        }

    }

    public Node NodeFromWorldPosition (Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPosition.z - transform.position.z + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        
        int x = (int)Mathf.Clamp((gridSizeX) * percentX, 0,gridSizeX - 1);
        int z = (int)Mathf.Clamp((gridSizeZ) * percentY, 0, gridSizeZ - 1);
        return grid[x,z];
    }

    public List<Node> GetNeighbours (Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
                if(z == 0 && x == 0)
                    continue;
                int checkX = node.m_iGridX + x;
                int checkZ = node.m_iGridY + z;
                if(checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbours.Add(grid[checkX, checkZ]);
                } 
            }
        }

        return neighbours;
    }

    public void ResetNodes()
    {
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                grid[x, z].m_bProcessed = false;
            }
        }
    }
}
