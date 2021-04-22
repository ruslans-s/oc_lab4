using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace sort_prog
{
   
    class Program
    {
        
        static void Main(string[] args)
        {
            

            List<string> massiveMessage = new List<string>();
            //Получение существующего участка разделяемой памяти
            //Параметр - название участка
            massiveMessage.Clear();
            bool enterMessage;
           
            while (true)
            {
                enterMessage = false;
                //Массив для сообщения из общей памяти
                char[] message;
                //Размер введенного сообщения
                int size;

                MemoryMappedFile sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");
                //Сначала считываем размер сообщения, чтобы создать массив данного размера
                //Integer занимает 4 байта, начинается с первого байта, поэтому передаем цифры 0 и 4
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
                {
                    size = reader.ReadInt32(0);
                }

                //Считываем сообщение, используя полученный выше размер
                //Сообщение - это строка или массив объектов char, ка ждый из которых занимает два байта
                //Поэтому вторым параметром передаем число символов умножив на из размер в байтах плюс
                //А первый параметр - смещение - 4 байта, которое занимает размер сообщения
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
                {
                    //Массив символов сообщения
                    message = new char[size];
                    reader.ReadArray<char>(0, message, 0, size);
                }
                     string stringMessage = new string(message);
                    if (massiveMessage.IndexOf(stringMessage) == -1)
                    {
                        //Переводим массив char в string
                       
                        massiveMessage.Add(stringMessage);

                        Console.WriteLine("Получено сообщение :");
                        for(int i = 0; i < massiveMessage.Count(); i++)
                    {
                         Console.WriteLine(massiveMessage[i]);
                    }
                    Console.WriteLine("Сортируем :");

                    massiveMessage.Sort();

                    for (int i = 0; i < massiveMessage.Count(); i++)
                    {
                        Console.WriteLine(massiveMessage[i]);
                    }
                }
                    
                Thread.Sleep(200);
            }
        }
    }
}
