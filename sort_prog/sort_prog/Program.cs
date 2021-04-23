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

            for (int i = 1; i < massiveMessage.Count(); i++)
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
                List<string> massiveForNewMessage = new List<string>();
          
                //Массив для сообщения из общей памяти
                char[] message;
                //Размер введенного сообщения
                int size, size2, count;

                //Получение существующего участка разделяемой памяти (название участка)
                MemoryMappedFile sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");

                //считываем размер сообщения
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
                {
                    size = reader.ReadInt32(0);
                }
                count = 4;
                while (size != 0)
                {
                    using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(count, 4, MemoryMappedFileAccess.Read))
                    {
                        size = reader.ReadInt32(0);
                    }

                    count += 4;
                    //Считываем сообщение, используя полученный выше размер
                    using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(count, size * 2, MemoryMappedFileAccess.Read))
                    {
                        //Массив символов сообщения
                        message = new char[size];
                        reader.ReadArray<char>(0, message, 0, size);
                    }

                    count += size * 2;
                    //Переводим из char[] в string
                    string stringMessage = new string(message);

                    massiveForNewMessage.Add(stringMessage);
                }

                if (massiveForNewMessage.Count() > massiveMessage.Count())
                {
                    massiveMessage.Clear();
                    massiveMessage = massiveForNewMessage;

                    outList(massiveMessage, "Получено новое сообщение :");
                    //Сортировка
                    massiveMessage.Sort();

                    outList(massiveMessage, "Сортируем :");
                    Console.WriteLine(" ");
                }
              
                Thread.Sleep(200);
            }
        }
    }
}
