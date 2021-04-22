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
            while (true)
            {
                Console.WriteLine("Введите сообщение");

                //Ввод слова для записи в общую память
                char[] message = Console.ReadLine().ToCharArray();
                //Размер введенного сообщения
                int size = message.Length; 

                //Создание участка разделяемой памяти (Название участка, размер памяти)
                MemoryMappedFile sharedMemory = MemoryMappedFile.CreateOrOpen("MemoryFile", size * 2 + 4);

                //запись в разделяемую память
                //Создаем объект для записи в разделяемый участок памяти
                using (MemoryMappedViewAccessor writer = sharedMemory.CreateViewAccessor(0, size * 2 + 4))
                {
                    //запись размера с нулевого байта в разделяемой памяти
                    writer.Write(0, size);
                    //запись сообщения с четвертого байта в разделяемой памяти
                    writer.WriteArray<char>(4, message, 0, message.Length);
                }

                Console.WriteLine("Сообщение записано в разделяемую память"); 
            }

        }
    }
}
