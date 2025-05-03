using UnityEngine;
using System.Collections.Generic;

public class RopeMinigame : MonoBehaviour
{
    public Rope[] ropes;
    public Knot[] knots;
    private List<int> knotsList = new List<int>();

    private void Awake()
    {
        knots = FindObjectsByType<Knot>(FindObjectsSortMode.None);
        for (int i = 0; i < knots.Length; i++)
        {
            knotsList.Add(i);
        }
        AssignKnotTypes();
    }

    void AssignKnotTypes()
    {
        ShuffleKnotsList();
        for (int i = 0; i < 5; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.Knot;
        }
        for (int i = 5; i < 9; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.KnotFrayed;
        }
        for (int i = 9; i < 12; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.KnotTight;
        }

        foreach (Knot knot in knots)
        {
            knot.AssignSprite();
        }

        if (!ValidateKnotLayout())
        {
            Debug.LogWarning("Invalid layout generated, reshuffling");
            AssignKnotTypes();
        }
        else { Debug.Log("Valid layout generated"); }
    }

    void ShuffleKnotsList()
    {
        for (int i = 0; i < knotsList.Count; i++)
        {
            int randomIndex = Random.Range(i, knotsList.Count);
            int temp = knotsList[i];
            knotsList[i] = knotsList[randomIndex];
            knotsList[randomIndex] = temp;
        }
    }

    bool ValidateKnotLayout()
    {
        List<int> frayedKnots = new List<int>();
        HashSet<int> tightKnots = new HashSet<int>();

        for (int i = 0; i < knots.Length; i++)
        {
            if (knots[i].myKnotType == Knot.KnotType.KnotFrayed) { frayedKnots.Add(i); }
            else if (knots[i].myKnotType == Knot.KnotType.KnotTight) { tightKnots.Add(i); }
        }

        Dictionary<int, List<int>> fullGraph = BuildKnotGraph();
        foreach (int frayedIndex in frayedKnots)
        {
            if (!fullGraph.ContainsKey(frayedIndex)) { return false; }

            int validNeighborCount = 0;
            foreach (int neighbor in fullGraph[frayedIndex])
            {
                if (!tightKnots.Contains(neighbor)) { validNeighborCount++; }
            }

            if (validNeighborCount < 2) { return false; }
        }

        Dictionary<int, List<int>> filteredGraph = BuildKnotGraph(tightKnots);
        HashSet<int> visited = new HashSet<int>();
        DepthFirstSearch(frayedKnots[0], filteredGraph, visited);

        foreach (int frayedIndex in frayedKnots)
        {
            if (!visited.Contains(frayedIndex)) { return false; }
        }
        
        return true;
    }

    Dictionary<int, List<int>> BuildKnotGraph()
    {
        return BuildKnotGraph(new HashSet<int>());
    }

    Dictionary<int, List<int>> BuildKnotGraph(HashSet<int> blockedKnots)
    {
        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
        
        foreach (Rope rope in ropes)
        {
            //int a = rope.knotA;
            //int b = rope.knotB;
            int a = System.Array.IndexOf(knots, rope.knotA);
            int b = System.Array.IndexOf(knots, rope.knotB);

            if (blockedKnots.Contains(a) || blockedKnots.Contains(b)) { continue; }

            if (!graph.ContainsKey(a)) graph[a] = new List<int>();
            if (!graph.ContainsKey(b)) graph[b] = new List<int>();

            graph[a].Add(b);
            graph[b].Add(a);
        }

        return graph;
    }

    void DepthFirstSearch(int current, Dictionary<int, List<int>> graph, HashSet<int> visited)
    {
        if (visited.Contains(current)) return;
        visited.Add(current);

        if (!graph.ContainsKey(current)) return;

        foreach (int neighbor in graph[current]) { DepthFirstSearch(neighbor, graph, visited); }
    }
}
