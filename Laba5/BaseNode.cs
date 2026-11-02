namespace Laba5;

// Базовый класс для всех узлов мультисписка.
// Не содержит данных, только виртуальный флаг.
public class BaseNode
{
    // Возвращает true, если узел является регистрационной записью.
    // Переопределяется в RegistrationNode.
    public virtual bool IsRegistration()
    {
        return false;
    }
}