namespace Laba5;

// Узел студента: хранит имя и ссылку на начало кольца регистраций.
public class StudentNode : BaseNode
{
    public char[] Name;        // имя студента
    public BaseNode? Next;      // ссылка на первый узел-регистрацию или null

    public StudentNode(char[] name)
    {
        Name = name;
        Next = null;
    }
}