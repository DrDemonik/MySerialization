using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SerializationApp
{
    class Program
    {

        private static void Main(string[] args)
        {
            ListRandom listRandom = new ListRandom();
            listRandom.AddNode("Первый");
            listRandom.AddNode("Второй");
            listRandom.AddNode("Третий");
            foreach(string item in listRandom)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Узел связного списка
    /// </summary>
    class ListNode 
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string Data;
    }

    /// <summary>
    /// Связной список
    /// </summary>
    class ListRandom : IEnumerable<string>
    {
        public ListNode Head { get; set; }
        public ListNode Tail { get; set; }
        public int Count;

        /// <summary>
        /// Сериализация 
        /// </summary>
        public void Serialize(Stream s)
        {

        }

        /// <summary>
        /// Десериализация
        /// </summary>
        /// <param name="s"></param>
        public void Deserialize(Stream s)
        {

        }

        /// <summary>
        /// Добавление узла после текущего узла
        /// </summary>
        public void AddNode(string Data)
        {
            ListNode node = new ListNode{Data = Data};
            if (Head == null)
            {
                Head = node;
            }
            else
            {
                Tail.Next = node;
                node.Previous = Tail;
            }
            Tail = node;
            Count++;
        }

        /// <summary>
        /// Добавление узла перед текущем узлом
        /// </summary>
        public void AddNodeFirst(string Data)
        {
            ListNode node = new ListNode { Data = Data };
            ListNode temp = Head;
            Head = node;
            if (Count == 0)
            {
                Tail = Head;
            }
            else
            {
                temp.Previous = node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            ListNode current = Head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}

