using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
namespace input_prog
{
    class Program
    {
        static void Main(string[] args)
        {
           
            List<char[]> massiveMessage = new List<char[]>();
            int size = 0 ;
            
            while (true)
            {
                Console.WriteLine("Введите сообщение");
               

                //Ввод слова для записи в общую память
                
                char[] message = Console.ReadLine().ToCharArray();
         
                massiveMessage.Add(message);

                //Размер введенного сообщения
                size += message.Length * 2 + 4;

                //Создание участка разделяемой памяти (Название участка, размер памяти)
                MemoryMappedFile sharedMemory = MemoryMappedFile.CreateOrOpen("MemoryFile", 4 + size);

                //запись в разделяемую память
                //Создаем объект для записи в разделяемый участок памяти
                using (MemoryMappedViewAccessor writer = sharedMemory.CreateViewAccessor(0, 4 + size))
                {
                    int count;
                    count = 0;

                    writer.Write(count, massiveMessage.Count());
                    count += 4;

                    for (int i = 0; i < massiveMessage.Count(); i++)
                    {
                        //запись размера с нулевого байта в разделяемой памяти
                        writer.Write(count, massiveMessage[i].Length);
                        count += 4;

                        //запись сообщения с четвертого байта в разделяемой памяти
                        writer.WriteArray<char>(count, massiveMessage[i], 0, massiveMessage[i].Length);
                        count += massiveMessage[i].Length * 2;
                    }
                }

                Console.WriteLine("Сообщение записано в разделяемую память"); 
            }

        }
    }
}
