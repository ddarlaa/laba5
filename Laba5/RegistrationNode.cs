namespace Laba5;

public class RegistrationNode : BaseNode
{
    public BaseNode StudentLink;
    public BaseNode CourseLink;
    
    public override bool IsRegistration()
    {
        return true;
    }
    
}