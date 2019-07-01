using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SerializationApp
{
    class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                ListRandom listRandom = new ListRandom();
                listRandom.AddNode("Первый");
                listRandom.AddNode("Второй");
                listRandom.AddNode("Третий");
                listRandom.AddNode("Чётвёртый");
                listRandom.AddNode("Пятый");
                listRandom.AddNode("Шестой");
                listRandom.AddNode("Седьмой");
                listRandom.AddNode("Восьмой");
                listRandom.AddNodeFirst("Нулевой");
                listRandom.SetRandomListNode();

                foreach (ListNode item in listRandom)
                {
                    Console.WriteLine(item.Data + " " + item.Random.Data);
                }

                using (FileStream fileStream = new FileStream("listRandom.dat", FileMode.Create))
                {
                    listRandom.Serialize(fileStream);
                }

                listRandom.Clear();

                using (FileStream fileStream = new FileStream("listRandom.dat", FileMode.Open))
                {
                    listRandom.Deserialize(fileStream);
                }

                Console.WriteLine("----------------------------");

                foreach (ListNode item in listRandom)
                {
                    Console.WriteLine(item.Data + " " + item.Random.Data);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
                Console.WriteLine("Объект: " + e.Source);
                Console.WriteLine("Метод, вызвавший исключение: " + e.TargetSite);
                Console.WriteLine("Стэк: " + e.StackTrace);
                Console.WriteLine("====================================");
            }
            Console.ReadKey();
        }
    }

    #region Задание
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
    class ListRandom : IEnumerable<ListNode>
    {
        public ListNode Head { get; set; }
        public ListNode Tail { get; set; }
        public int Count;

        /// <summary>
        /// Сериализация 
        /// </summary>
        public void Serialize(Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                foreach(ListNode listNode in this)
                {
                    var Previous = this.ToList().IndexOf(listNode.Previous).ToString();
                    var Next = this.ToList().IndexOf(listNode.Next).ToString();
                    var Random = this.ToList().IndexOf(listNode.Random).ToString();
                    var Data = listNode.Data;
                    streamWriter.WriteLine(Random + "~" + Data);
                }
            }
        }

        /// <summary>
        /// Десериализация
        /// </summary>
        /// <param name="s"></param>
        public void Deserialize(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                ListRandom listRandom = new ListRandom();
                string readedLine;
                List<Tuple<int, ListNode>> randomIndexList = new List<Tuple<int, ListNode>>();

                while ((readedLine = streamReader.ReadLine()) != null)
                {
                    var a = readedLine.Split('~');
                    if (a.Length != 2)
                        throw new Exception("Файл повреждён");
                    randomIndexList.Add(new Tuple<int, ListNode>(Convert.ToInt32(a[0]), listRandom.AddNode(a[1])));
                }

                foreach(var item in randomIndexList)
                {
                    if(item.Item1 > -1)
                        item.Item2.Random = randomIndexList[item.Item1].Item2;
                }

                this.Head = listRandom.Head;
                this.Tail = listRandom.Tail;
                this.Count = listRandom.Count;
            }
        }

        /// <summary>
        /// Очистка всего связного списка
        /// </summary>
        public void Clear()
        {
            Count = 0;
            Head = null;
            Tail = null;
        }

        /// <summary>
        /// Добавление узла после текущего узла
        /// </summary>
        public ListNode AddNode(string Data)
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
            return node;
        }

        /// <summary>
        /// Добавление узла перед текущем узлом
        /// </summary>
        public ListNode AddNodeFirst(string Data)
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
                node.Next = temp;
            }
            Count++;
            return node;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<ListNode> IEnumerable<ListNode>.GetEnumerator()
        {
            ListNode current = Head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        /// <summary>
        /// Превращаем в лист
        /// </summary>
        public List<ListNode> ToList()
        {
            List<ListNode> listNodes = new List<ListNode>();
            foreach (ListNode item in this)
            {
                listNodes.Add(item);
            }
            return listNodes;
        }

        /// <summary>
        /// Устанавливаем случайную связь в всвязном списке
        /// </summary>
        public void SetRandomListNode()
        {
            Random rand = new Random();
            var listNodes = this.ToList();
            foreach (ListNode item in this)
            {
                item.Random = listNodes[rand.Next(0, listNodes.Count)];
            }
        }
    }
    #endregion
}

