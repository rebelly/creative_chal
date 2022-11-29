using System;

class Program
{
    static int fnd_free(long bas)
    {
        
        int o = 0;
        int k = 0;
        for (int i = 0; i < 16; i++ )
        {
            k = 0;
            for (int j = 0; j < 4; j++)
            {
                if ((bas & 1) == 0) { k++; }
                bas >>= 1;
            }

            if (k == 4)
            {
                break;
            }
            else {
                o++;
            }
        
    }
        if (o == 16)
        {
            Console.WriteLine("К сожалению все ячейки диапазона заняты");
            return -1;
        }
        return o;
        
    }

    static void read(long bas, int pos)
    {
        pos++;
        beauty_print(bas, pos);
        if (pos != 1) bas >>= 4 * pos;// делаем нужный нам разряд ведущим
        
        int res = 0;
        bas &= 0xF;// оставляем только его 
        for (int i = 0; i < 4; i++)
            {
            
                if ((bas & 1) != 0) res += (int)Math.Pow(2, i); //собираем из двоичного в десятичное
                bas >>= 1;
            }
        Console.WriteLine(res);
        // выводим 
    } 
    static void add_n(ref long bas, int to_add)
    {
        if (fnd_free(bas) != -1)
        {
            Console.WriteLine(fnd_free(bas));
            if (fnd_free(bas) != 0) to_add <<= 4 * fnd_free(bas); // делаем то что задумывали в 22ой строчке
            bas += to_add;
            //все коды предшествующих четверок не затрагиваются, потому что если к 0000_0101 прибавить 1111_0000, станет 1111_0101, и никак иначе
            Console.WriteLine();
            beauty_print(bas, fnd_free(bas));
        }

    }
    static void del(ref long bas, int pos )
    {
        long mask = 0xF; //делаем маску вида 000000...1111
        mask <<= (pos *4); // сдвигаем ее чтобы единички оказались над нужной нам четверкой
        mask = ~mask; // делаем инверсию, теперь всё в маске единички кроме тех которые над нужной четверкой
        bas &= mask;// сбрасываем нашу четверку
        beauty_print(bas, pos);
    }
    static void beauty_print(long bas, int pos) // это функция для красивого вывода
    {
        long mask = 0x800000000000;
        byte o = 0;
        Console.WriteLine(pos);
        int k = 0;
        while (mask > 0)
        {
            if (k == 12-pos)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            if ((bas & mask) != 0) Console.Write(1);
            else Console.Write(0);
            o++;
            if (o == 4)
            {
                k++;
                o = 0;
                Console.Write(" ");
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
            Console.WriteLine("Введите одну из команд, далее , на следующей строке - требуемое число:");
            Console.WriteLine("Ввести <число от 0 до 15> - добавляет число на первое свободное место");
            Console.WriteLine("Удалить <число от 0 до 16> - стирает число из заданной позиции: ");
            Console.WriteLine("Прочитать <число от 0 до 16> - вывести число с заданной позиции: ");
            req = Console.ReadLine();
            switch (req) {
                case "Ввести":
                    Console.Write("Введите число, которое нужно добавить: ");
                    int addit = int.Parse(Console.ReadLine());
                    if (addit >= 0 && addit <= 15) add_n(ref bas, addit);
                    else Console.WriteLine("Извините, данная операция невозможна, нужно ввести число от 0 включительно до 15 включительно");
                    break;

                case "Удалить":
                    Console.Write("Введите индекс числа, которое нужно удалить: ");
                    int delet = int.Parse(Console.ReadLine());
                    if (delet >= 0 && delet <= 15) del(ref bas, delet);
                    else Console.WriteLine("Извините, данная операция невозможна, индекс находится в диапазоне от 0 включительно до 15 включительно");
                    break;
            
                case "Прочитать":
                    Console.Write("Введите индекс числа, которое нужно прочитать: ");
                    int read_n = int.Parse(Console.ReadLine());
                    if (read_n >= 0 && read_n <= 15) read(bas, read_n);
                    else Console.WriteLine("Извините, данная операция невозможна, индекс находится в диапазоне от 0 включительно до 15 включительно");
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            
            }
        }


    }
}
