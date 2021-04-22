using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading;

namespace sort_prog
{

    class Program
    {
        //Вывод массива в консоль
        static void outList(List<string> massiveMessage, string line)
        {
            Console.WriteLine(line);

            for (int i = 0; i < massiveMessage.Count(); i++)
            {
                Console.WriteLine(massiveMessage[i]);
            }
        }

        static void Main(string[] args)
        {

            //Массив для сообщений из общей памяти
            List<string> massiveMessage = new List<string>();
            massiveMessage.Clear();

            while (true)
            {
                //Массив для сообщения из общей памяти
                char[] message;
                //Размер введенного сообщения
                int size;

                //Получение существующего участка разделяемой памяти (название участка)
                MemoryMappedFile sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");

                //считываем размер сообщения
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
                {
                    size = reader.ReadInt32(0);
                }

                //Считываем сообщение, используя полученный выше размер
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
                {
                    //Массив символов сообщения
                    message = new char[size];
                    reader.ReadArray<char>(0, message, 0, size);
                }
                //Переводим из char[] в string
                string stringMessage = new string(message);

                //Провека присутсвия строки в массиве
                if (massiveMessage.IndexOf(stringMessage) == -1)
                {
                    //Добавление строки в массив
                    massiveMessage.Add(stringMessage);
                    
                    outList(massiveMessage, "Получено сообщение :");
                    //Сортировка
                    massiveMessage.Sort();
                    
                    outList(massiveMessage, "Сортируем :");

                }

                Thread.Sleep(200);
            }
        }
    }
}
