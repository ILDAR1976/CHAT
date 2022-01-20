using System;
using System.Collections.Generic;

namespace Chat
{
    class Graph
    {
        private Stack<Vertex> theStack;
        public string Route;

        public Graph() 
        {
            theStack = new Stack<Vertex>();
        }

        public List<Vertex> Vertexes = new List<Vertex>();
        List<Edge> Edges = new List<Edge>();

        public int VertexCount => Vertexes.Count;
        public int EdgeCount => Edges.Count;

        public void AddVertex(Vertex vertex)
        {
            Vertexes.Add(vertex);
        }

        public void AddEdge(Vertex from, Vertex to, int weight = 1)
        {
            var edge = new Edge(from, to, weight);
            Edges.Add(edge);
        }

        public int[,] GetMatrix()
        {
            var matrix = new int[Vertexes.Count, Vertexes.Count];

            foreach(var edge in Edges)
            {
                var row = edge.From.Number - 1;
                var column = edge.To.Number - 1;

                matrix[row, column] = edge.Weight;
            }

            return matrix;
        }

        public List<Vertex> GetAdjVetexLists(Vertex vertex)
        {
            var result = new List<Vertex>();

            foreach(var edge in Edges)
            {
                if(edge.From == vertex)
                {
                    result.Add(edge.To);
                }
            }

            return result;
        }

        public bool Wave(Vertex start, Vertex finish)
        {
            var list = new List<Vertex>
            {
                start
            };

            for (int i = 0; i < list.Count; i++)
            {
                var vertex = list[i];
                foreach (var v in GetAdjVetexLists(vertex))
                {
                    if (!list.Contains(v))
                    {
                        list.Add(v);
                    }
                }
            }

            return list.Contains(finish);
        }

        public virtual List<Vertex> DFS()
        {
            var visitedVertexes = new List<Vertex>();
            visitedVertexes.Add(Vertexes[0]);
            theStack.Push(Vertexes[0]); 
            while (theStack.Count != 0) 
            {
                
                Vertex v = getAdjUnvisitedVertex(theStack.Peek(),visitedVertexes);
                theStack.Peek().SubWeight(1);

                string str = "" + theStack.Peek();
               
                if (v == null) 
                {
                    theStack.Pop();
                }
                else
                {
                    visitedVertexes.Add(v);
                    theStack.Push(v);
                }

                if (v != null) Route +=  str + " " + v + "\n";
            } 
            return visitedVertexes;
        }

        public virtual Vertex getAdjUnvisitedVertex(Vertex vertex, List<Vertex> adj)
        {
   
            foreach (var edge in Edges)
            {
                if (edge.From == vertex && !adj.Contains(edge.To) && vertex.Weight != 0 )
                {
                    return edge.To;
                }
            }

            return null;

        } // end getAdjUnvisitedVert()
    
        public virtual void Initializing(int[] input)
        {
            int index = 1;

            foreach (int i in input)
            {
                var v = new Vertex(index, i);
                AddVertex(v);
                index++;
            }

            foreach (Vertex vi in Vertexes)
            {
                foreach (Vertex vj in Vertexes)
                {
                    if (vi.Number != vj.Number && vi.Weight != 0)
                    {
                        AddEdge(vi, vj, 1);
                    }
                }
            }


        }
    }

    
}
