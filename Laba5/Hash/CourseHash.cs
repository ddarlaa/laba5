namespace Laba5;

public class CourseHash
{
    private const int Size = 10;
    private readonly CourseNode?[] _array = new CourseNode?[Size];

    // Проверка, является ли ячейка удалённой (первый символ '\0')
    private bool IsDeleted(int index) => _array[index] != null && _array[index]!.Name[0] == '\0';

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

    public int Member(char[] name) => Find(name);

    public CourseNode?[] GetArray() => _array;

    private static int Hash(char[] name) => Common.SumHash(name) % Size;
    private static int NextHash(int hash, int i) => (hash + i) % Size;

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
                    // Создаём копию имени (защита от внешних изменений)
                    char[] copy = new char[10];          // размер фиксированный, как в Common
                    Common.CopyName(copy, name);
                    _array[cur] = new CourseNode(copy);
                    return;
                }
                break; // есть удалённая ячейка, выходим из цикла
            }

            if (!IsDeleted(cur) && Common.ArraysEqual(_array[cur]!.Name, name))
                return; // уже существует

            if (IsDeleted(cur) && firstDeleted == -1)
                firstDeleted = cur;

            i++;
            cur = NextHash(hash, i);
        } while (cur != hash);

        // Если нашли удалённую ячейку – вставляем туда
        if (firstDeleted != -1)
        {
            char[] copy = new char[10];
            Common.CopyName(copy, name);
            _array[firstDeleted] = new CourseNode(copy);
        }
    }

    public void Delete(char[] name)
    {
        int index = Member(name);
        if (index == -1) return;
        _array[index]!.Name[0] = '\0';
    }

    public void MakeNull()
    {
        for (int i = 0; i < Size; i++)
            _array[i] = null;
    }
}