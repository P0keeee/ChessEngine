using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ChessEngine
{
    class Tree<T>
    {
        public List<Tree<T>> Children = new List<Tree<T>>();

        public T Value { get; set; }
        public string Edge { get; set; }   

        public Tree(T nodeValue, string edgeValue)
        {
            Value = nodeValue;
            Edge = edgeValue;
        }

        public Tree<T> AddChild(T nodeValue, string edgeValue)
        {
            Tree<T> nodeItem = new Tree<T>(nodeValue, edgeValue);
            Children.Add(nodeItem);
            return nodeItem;
        }
        public void DeleteSubtree()
        {
            foreach (var child in Children)
            {
                child.DeleteSubtree();
            }
            Children.Clear();
        }
    }
}   
