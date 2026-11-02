namespace Laba5;

// Вспомогательный класс для работы с символьными массивами.
public static class Common
{
    // Вспомогательный метод: создаёт char[10] из строки.
    public static char[] MakeName(string s)
    {
        char[] name = new char[10];
        for (int i = 0; i < s.Length && i < 10; i++)
            name[i] = s[i];
        // Остальные символы уже '\0' по умолчанию
        return name;
    }
    
    // Создаёт копию имени в виде массива char[10] с терминатором '\0'.
    // Копируются только символы до '\0' или до исчерпания 10 символов.

    public static char[] CopyName(char[] target, char[] source)
    {
        //char[] copy = new char[10];
        int i = 0;
        while (i < source.Length && i < 10 && source[i] != '\0')
        {
            target[i] = source[i];
            i++;
        }

        // Остальные элементы автоматически '\0'
        return target;
    }
    
    // Хэш-функция: сумма кодов символов.
    public static int SumHash(char[] name)
    {
        int sum = 0;
        int i = 0;
        while (i < name.Length && name[i] != '\0')
        {
            sum += name[i];
            i++;
        }
        return sum;
    }
    
    // Посимвольное сравнение двух имён.
    public static bool ArraysEqual(char[]? a, char[]? b)
    {
        if (a == null || b == null) return false;
        int i = 0;
        while (i < a.Length && i < b.Length)
        {
            if (a[i] != b[i]) return false;
            if (a[i] == '\0') return true; // дошли до конца строки одновременно
            i++;
        }
        // Если вышли за пределы, проверяем, что оба закончились
        if (i < a.Length && a[i] != '\0') return false;
        if (i < b.Length && b[i] != '\0') return false;
        return true;
    }
}