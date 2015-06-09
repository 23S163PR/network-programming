using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEvenNumbersArray
{
    class Program
    {
        public static int[] GetEvenNumbersArray(int[] source)
        {
           
            return source.Where(x => x % 2 == 0)
                         .ToArray();
        }

        static void Main(string[] args)
        {
            const int size = 10;
            int[] arr = new int[size];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i + 1;
                Console.Write("{0, 2} ", arr[i]);
            }
            Console.WriteLine();
            
            var res = GetEvenNumbersArray(arr);
            foreach (var el in res)
            {
                Console.Write("{0, 2} ", el);
            }
            Console.WriteLine();
        }
    }
}
