using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public int[,] FindPath(int[,] grid, Vector2Int start, Vector2Int goal)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[,] gridNav = new int[rows, cols];

        // 오픈 리스트와 클로즈 리스트 생성
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        // 시작 노드 추가
        Node startNode = new Node(start, null, 0, GetHeuristic(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // 오픈 리스트에서 F 값이 가장 작은 노드를 선택
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

            // 목표 지점에 도달하면 경로를 생성
            if (currentNode.Position == goal)
            {
                Node temp = currentNode;
                while (temp != null)
                {
                    gridNav[temp.Position.x, temp.Position.y] = 1; // 경로를 1로 표시
                    temp = temp.Parent;
                }
                break;
            }

            // 인접한 노드들을 확인
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentNode.Position + direction;

                // 그리드 범위 내인지 확인
                if (neighborPos.x >= 0 && neighborPos.x < rows && neighborPos.y >= 0 && neighborPos.y < cols)
                {
                    // 이동할 수 있는지 확인 (0은 이동 가능, 1은 이동 불가능)
                    if (grid[neighborPos.x, neighborPos.y] == 1 || closedList.Contains(new Node(neighborPos)))
                    {
                        continue;
                    }

                    int gCost = currentNode.G + 1; // G 값 증가
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

    // 휴리스틱 계산 (맨해튼 거리)
    private int GetHeuristic(Vector2Int pos, Vector2Int goal)
    {
        return Mathf.Abs(pos.x - goal.x) + Mathf.Abs(pos.y - goal.y);
    }

    // 노드 클래스
    private class Node
    {
        public Vector2Int Position { get; }
        public Node Parent { get; }
        public int G { get; } // 시작점으로부터의 비용
        public int H { get; } // 목표점까지의 휴리스틱
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
