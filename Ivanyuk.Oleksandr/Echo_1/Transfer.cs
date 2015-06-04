using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo_1
{
    class Transfer // класс для перевода byte[] в string i наоборот
    {
        public static string BytesToString(byte[] buff, int size)
        {
            Array.Resize(ref buff, size - 1); //обрізаємо буфер по розміру меседжа
            string message = System.Text.Encoding.UTF8.GetString(buff);
            return message;
        }

        public static byte[] StringToBytes(string message)
        {
            byte[] buff;
            buff = System.Text.Encoding.ASCII.GetBytes(message);
            return buff;
        }
    }
}
