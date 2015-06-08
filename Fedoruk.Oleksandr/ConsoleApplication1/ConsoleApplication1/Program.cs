using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class ArrayExtensions
    {
        public static int[] Shuffle(int[] source)
        {
            Random rnd = new Random();
            int[] arr = new int[source.Length];
            source.CopyTo(arr, 0);

            int prevIdx = 0;
            int idx = rnd.Next(1, arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                if (prevIdx != idx)
                {
                    var tmp = arr[idx];
                    arr[idx] = arr[prevIdx];
                    arr[prevIdx] = tmp;
                    prevIdx = idx;
                }
                idx = rnd.Next(0, arr.Length);

            }
            return arr;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            const int size = 3;
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = i + 1;
            }

            foreach (var el in arr)
            {
                Console.Write("{0, 2} ", el);
            }
            Console.WriteLine();

            //var res = ArrayExtensions.Shuffle(arr);
            //foreach (var el in res)
            //{
            //    Console.Write("{0, 2} ", el);
            //}

            List<int[]> numsTemplate = new List<int[]>{
                             new int[]{1, 2, 3}
                            ,new int[]{1, 3, 2}
                            ,new int[]{2, 1, 3}
                            ,new int[]{2, 3, 1}
                            ,new int[]{3, 1, 2}
                            ,new int[]{3, 2, 1}
            };

            int[] numsMatches = new int[6] { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 60000; i++)
            {
                var resArr = ArrayExtensions.Shuffle(arr);
                int idx = numsTemplate.IndexOf(resArr);
                var tmp = numsTemplate.First(x => x.Equals(resArr));
                numsMatches[idx] += 1;
            }

            for (int i = 0; i < numsMatches.Length; i++)
            {
                foreach (var el in numsTemplate[i])
                {
                    Console.Write("{0, 2} ", el);
                }
                Console.WriteLine(" -  {0, 5}", numsMatches[i]);
            }

        }
    }
}
