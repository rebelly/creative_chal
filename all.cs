using System;

class Program
{

    static int fnd_fr(long bas)
    {
        if (bas == 0) return 0; // в самом начале сдвиг должен быть на 0
        int o = 0;
        int k = 0;
        long mask = 0x800000000000;
        while ((bas & mask) == 0 && mask!= 0)
        {
            k++; // счетчик каждого бита
            if (k % 4 == 0)
            {
                k = 1;
                o++; // счетчик четверок битов
            }
            mask >>= 1;
        }
        return (int)Math.Pow(2, 16 - o); // 16 - o : это первая свободная четверка, нам нужно физически подвинуть код числа (пусть будет 15) чтобы он стал не 1111, а 11110000, если первая свободна я четверка - 2
    }
    static void read(long bas, int pos)
    {
        if (pos == 1) bas >>= 0;
        else bas >>= (int)Math.Pow(2, pos); // делаем нужный нам разряд ведущим

        int res = 0; 
            bas &= 1111; // оставляем только его
            beauty_print(bas);
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
        to_add <<= fnd_fr(bas); // делаем то что задумывали в 22ой строчке
        bas += to_add; //все коды предшествующих четверок не затрагиваются, потому что если к 0000_0101 прибавить 1111_0000, станет 1111_0101, и никак иначе
        beauty_print(bas);

    }
    static void beauty_print(long bas) // это функция для красивого вывода
    {
        long mask = 0x800000000000;
        while (mask > 0)
        {
            if ((bas & mask) != 0) Console.Write(1);
            else Console.Write(0);
            mask >>= 1;
        }
        Console.WriteLine();
    }
    public static void Main()
    {
        long bas = 0x0000000000000000;
       add_n(ref bas, 4);
       add_n(ref bas, 5);
       add_n(ref bas, 3);
       read(bas, 3);
    }
}
