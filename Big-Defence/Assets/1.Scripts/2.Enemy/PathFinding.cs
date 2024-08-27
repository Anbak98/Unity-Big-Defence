using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public int[,] FindPath(int[,] grid, Vector2Int start, Vector2Int goal)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[,] gridNav = new int[rows, cols];

        // ���� ����Ʈ�� Ŭ���� ����Ʈ ����
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        // ���� ��� �߰�
        Node startNode = new Node(start, null, 0, GetHeuristic(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // ���� ����Ʈ���� F ���� ���� ���� ��带 ����
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // ��ǥ ������ �����ϸ� ��θ� ����
            if (currentNode.Position == goal)
            {
                Node temp = currentNode;
                while (temp != null)
                {
                    gridNav[temp.Position.x, temp.Position.y] = 1; // ��θ� 1�� ǥ��
                    temp = temp.Parent;
                }
                break;
            }

            // ������ ������ Ȯ��
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentNode.Position + direction;

                // �׸��� ���� ������ Ȯ��
                if (neighborPos.x >= 0 && neighborPos.x < rows && neighborPos.y >= 0 && neighborPos.y < cols)
                {
                    // �̵��� �� �ִ��� Ȯ�� (0�� �̵� ����, 1�� �̵� �Ұ���)
                    if (grid[neighborPos.x, neighborPos.y] == 1 || closedList.Contains(new Node(neighborPos)))
                    {
                        continue;
                    }

                    int gCost = currentNode.G + 1; // G �� ����
                    Node neighborNode = new Node(neighborPos, currentNode, gCost, GetHeuristic(neighborPos, goal));

                    if (!openList.Exists(node => node.Position == neighborPos && node.G <= gCost))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return gridNav;
    }

    // �޸���ƽ ��� (����ư �Ÿ�)
    private int GetHeuristic(Vector2Int pos, Vector2Int goal)
    {
        return Mathf.Abs(pos.x - goal.x) + Mathf.Abs(pos.y - goal.y);
    }

    // ��� Ŭ����
    private class Node
    {
        public Vector2Int Position { get; }
        public Node Parent { get; }
        public int G { get; } // ���������κ����� ���
        public int H { get; } // ��ǥ�������� �޸���ƽ
        public int F { get { return G + H; } } // G + H

        public Node(Vector2Int position, Node parent = null, int g = 0, int h = 0)
        {
            Position = position;
            Parent = parent;
            G = g;
            H = h;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                Node other = (Node)obj;
                return Position.Equals(other.Position);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
