using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayExtension
{
    class Program
    {
        static void Main(string[] args)
        {
            const int countOfShuffle = 600000000;
            const int arrayLength = 3;
            const int countOfArrayCombination = 6;
            int[] array = new int[arrayLength];
            int[] arrayCombination = new int[countOfArrayCombination];
            array[0] = 1;
            array[1] = 2;
            array[2] = 3;
            // initialize
            for(int i = 0; i < arrayCombination.Length; i++)
            {
                arrayCombination[i] = 0;
            }

            for (int i = 0; i < countOfShuffle; i++)
            {
                ArrayEtension.Shuffle(array);
                arrayCombination[ArrayEtension.ArrayCombination(array)] ++;
            }




            Console.WriteLine("Variant: 1 2 3 randomized: {0} in percent: {1}", arrayCombination[0], ((float)arrayCombination[0] / countOfShuffle) * 100);
            Console.WriteLine("Variant: 1 3 2 randomized: {0} in percent: {1}", arrayCombination[1], ((float)arrayCombination[1] / countOfShuffle) * 100);
            Console.WriteLine("Variant: 2 1 3 randomized: {0} in percent: {1}", arrayCombination[2], ((float)arrayCombination[2] / countOfShuffle) * 100);
            Console.WriteLine("Variant: 2 3 1 randomized: {0} in percent: {1}", arrayCombination[3], ((float)arrayCombination[3] / countOfShuffle) * 100);
            Console.WriteLine("Variant: 3 1 2 randomized: {0} in percent: {1}", arrayCombination[4], ((float)arrayCombination[4] / countOfShuffle) * 100);
            Console.WriteLine("Variant: 3 2 1 randomized: {0} in percent: {1}", arrayCombination[5], ((float)arrayCombination[5] / countOfShuffle) * 100);


        }
    }
    static class ArrayEtension
    {
        private static Random random = new Random();
        public static int ArrayCombination(int[] array)
        {
            if (array[0] == 1 && array[1] == 2 && array[2] == 3)
            {                                           
                return 0;                               
            }                                           
            if (array[0] == 1 && array[1] == 3 && array[2] == 2)
            {                                           
                return 1;                               
            }                                           
            if (array[0] == 2 && array[1] == 1 && array[2] == 3)
            {                                           
                return 2;                               
            }                                           
            if (array[0] == 2 && array[1] == 3 && array[2] == 1)
            {                                           
                return 3;                               
            }                                           
            if (array[0] == 3 && array[1] == 1 && array[2] == 2)
            {                                           
                return 4;                               
            }                                           
            if (array[0] == 3 && array[1] == 2 && array[2] == 1)
            {
                return 5;
            }
            return 10;
        }
        private static void Swap(ref int a, ref int b)
        {
            int c = a;
            a = b;
            b = c;
        }
        public static int[] Shuffle( this int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Swap(ref array[random.Next(0, array.Length)], ref array[random.Next(0, array.Length)]);
            }
            return array;
        }
        public static void RandomizeArray( ref int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, 100);
            }
        }
        public static void PrintArray(int[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                Console.Write("{0} ", array[i]);
            }
            Console.WriteLine();
        }
    }
}
