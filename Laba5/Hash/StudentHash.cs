namespace Laba5;

public class StudentHash
{
    private const int Size = 10;
    private readonly StudentNode?[] _array = new StudentNode?[Size];

    // Проверка, является ли ячейка удалённой (первый символ '\0')
    private bool IsDeleted(int index) => _array[index] != null && _array[index]!.Name[0] == '\0';

    // Приватный поиск индекса по имени
    private int Find(char[] name)
    {
        int h = Hash(name);
        int cur = h;
        int i = 1;

        do
        {
            if (_array[cur] == null) return -1;
            if (!IsDeleted(cur) && Common.ArraysEqual(_array[cur]!.Name, name))
                return cur;
            cur = NextHash(h, i);
            i++;
        } while (cur != h);
        return -1;
    }

    // Публичный Member – возвращает индекс или -1
    public int Member(char[] name) => Find(name);

    // Доступ к внутреннему массиву
    public StudentNode?[] GetArray() => _array;

    // Хеш-функция
    private static int Hash(char[] name) => Common.SumHash(name) % Size;

    // Линейное пробирование
    private static int NextHash(int hash, int i) => (hash + i) % Size;

    // Вставка нового студента (обновлённая логика)
    public void Insert(char[] name)
    {
        int hash = Hash(name);
        int cur = hash;
        int i = 0;
        int firstDeleted = -1;

        do
        {
            if (_array[cur] == null)
            {
                if (firstDeleted == -1)
                {
                    // Нет удалённых – вставляем в пустую ячейку
                    char[] copy = new char[10];
                    Common.CopyName(copy, name);
                    _array[cur] = new StudentNode(copy);
                    return;
                }
                // Есть удалённая – выходим из цикла, вставим в неё после цикла
                break;
            }

            // Если ячейка живая и имя совпадает – ничего не делаем
            if (!IsDeleted(cur) && Common.ArraysEqual(_array[cur]!.Name, name))
                return;

            // Запоминаем первую удалённую ячейку
            if (IsDeleted(cur) && firstDeleted == -1)
                firstDeleted = cur;

            i++;
            cur = NextHash(hash, i);
        } while (cur != hash);

        // Если есть удалённая ячейка – вставляем в неё
        if (firstDeleted != -1)
        {
            char[] copy = new char[10];
            Common.CopyName(copy, name);
            _array[firstDeleted] = new StudentNode(copy);
        }
    }

    // Удаление (помечает ячейку как удалённую)
    public void Delete(char[] name)
    {
        int index = Member(name);
        if (index == -1) return;
        _array[index]!.Name[0] = '\0';
    }

    // Полная очистка таблицы
    public void MakeNull()
    {
        for (int i = 0; i < Size; i++)
            _array[i] = null;
    }

    // Печать живых элементов (для отладки)
    public void Print()
    {
        for (int i = 0; i < Size; i++)
        {
            if (_array[i] != null && _array[i]!.Name[0] != '\0')
            {
                int j = 0;
                while (j < _array[i]!.Name.Length && _array[i]!.Name[j] != '\0')
                {
                    Console.Write(_array[i]!.Name[j]);
                    j++;
                }
                Console.WriteLine();
            }
        }
    }
}