using System;

class Program
{
    static int fnd_free(long bas)
    {
        
        int quad = 0;
        int counter = 0;
        long mask = 0x1;
        for (int i = 0; i < 16; i++ )
        {
            counter = 0;
            for (int j = 0; j < 4; j++)
            {
                if ((bas & mask) == 0) counter++;  // для каждой четверки подсчитываем количество ноликов, 
                mask <<= 1; 
            }

            if (counter == 4) break; // если оно равно четырем => ячейка свободна, значит она нам нужна
            else quad++; // а если нет то мы считаем четверку занятой
        
    }
        if (quad == 15)
        {
            Console.WriteLine();
            Console.WriteLine("К сожалению, все ячейки диапазона заняты");
            Console.WriteLine(); // если самая последняя четверка и та заполнена, то у нас переполнение
            return -1;
        }
        return quad;
        
    }

    static void read(long bas, int pos)
    {
        beauty_print(bas, pos + 1);
        long to_read = (bas >> (4 * pos)) & 0xF; ; // делаем нужную нам четверку ведущей и оставляем только ее
        int rate = 1;
        int res = 0;
        for (int i = 0; i < 4; i++)
            {
            if ((to_read & 1) != 0) res += rate << i; //собираем из двоичного в десятичное
            to_read >>= 1;
            }
        Console.WriteLine(res);   // выводим  

    } 
    static void add_n(ref long bas, long to_add)
    {
        if (fnd_free(bas) != -1)
        {
            int prev_pos = fnd_free(bas)+1; // запоминаем ту позицию с которой работаем, чтобы красиво вывести
            to_add <<= 4 * fnd_free(bas); // сдвигаем число которое нужно сделать чтобы оно наложилось поверх всех до этого (число 1100 становится 1100_0000 при свободной второй ячейцки)
            bas += to_add; //все коды предшествующих четверок не затрагиваются, потому что если к 0000_0101 прибавить 1111_0000, станет 1111_0101, и никак иначе
            beauty_print(bas, prev_pos);
        }

    }
    static void del(ref long bas, int pos )
    {
        long mask = 0xF; //делаем маску вида 000000...1111
        mask <<= (pos *4); // сдвигаем ее чтобы единички оказались над нужной нам четверкой
        mask = ~mask; // делаем инверсию, теперь всё в маске единички кроме тех которые над нужной четверкой
        bas &= mask;// сбрасываем нашу четверку
        beauty_print(bas, pos+1); // красивый вывод
    }
    static void beauty_print(long bas, int pos) // это функция для красивого вывода
    {
        long mask = 0x0800000000000000;
        byte counter = 0; 
        int quad = 0;
        while (mask > 0)
        {
            if (quad == 15-pos) Console.ForegroundColor = ConsoleColor.Red; // это если четверка равна той которую нужно выделить
            else Console.ForegroundColor = ConsoleColor.White;
            if ((bas & mask) != 0) Console.Write(1); 
            else Console.Write(0);
            counter++;
            if (counter == 4)
            {
                quad++;
                counter = 0;
                Console.Write(" "); // это мы накручиываем четверки во первых для действий в 81 строке, во вторых для того чтобы выводить все четверки через пробелы 
            }
            
            mask >>= 1;
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }
    public static void Main()
    {

        long bas = 0x00000000000000000000000000000000;
        string req = " ";
        while (req != "")
        {
            Console.WriteLine("0. Введите одну из команд, далее , на следующей строке - требуемое число:");
            Console.WriteLine("1. Ввести <число от 0 до 15> - добавляет число на первое свободное место");
            Console.WriteLine("2. Удалить <число от 0 до 16> - стирает число из заданной позиции: ");
            Console.WriteLine("3. Прочитать <число от 0 до 16> - вывести число с заданной позиции: ");
            Console.WriteLine();
            Console.WriteLine("Чтобы завершить работу с программой введите пустой запрос.");
            req = Console.ReadLine();
            switch (req) {
                case "Ввести":
                    Console.Write("Введите число, которое нужно добавить: ");
                    long addit = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    if (addit >= 0 && addit <= 15) add_n(ref bas, addit);
                    else Console.WriteLine("Извините, данная операция невозможна, нужно ввести число от 0 включительно до 15 включительно");
                    Console.WriteLine();
                    break;

                case "Удалить":
                    Console.Write("Введите индекс числа, которое нужно удалить: ");
                    int deletit = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    if (deletit >= 0 && deletit <= 15) del(ref bas, deletit);
                    else Console.WriteLine("Извините, данная операция невозможна, индекс находится в диапазоне от 0 включительно до 15 включительно");
                    Console.WriteLine();
                    break;
            
                case "Прочитать":
                    Console.Write("Введите индекс числа, которое нужно прочитать: ");
                    int read_n = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    if (read_n >= 0 && read_n <= 15) read(bas, read_n);
                    
                    else Console.WriteLine("Извините, данная операция невозможна, индекс находится в диапазоне от 0 включительно до 15 включительно");
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Неизвестная команда");
                    Console.WriteLine();
                    break;
            }
        }
    }
}
