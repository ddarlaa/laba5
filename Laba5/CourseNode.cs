namespace Laba5;

public class CourseNode : BaseNode
{
    public char[] Name;
    public BaseNode? Next;

    public CourseNode(char[] name)
    {
        Name = name;
        Next = null;
    }
}