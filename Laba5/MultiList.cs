namespace Laba5;

public class MultiList
{
    private StudentHash _students = null!;
    private CourseHash _courses = null!;

    public void Init()
    {
        _students = new StudentHash();
        _courses = new CourseHash();

        _students.Insert(Common.MakeName("Ivanov"));
        _students.Insert(Common.MakeName("Petrov"));
        _students.Insert(Common.MakeName("Sidorov"));

        _courses.Insert(Common.MakeName("101"));
        _courses.Insert(Common.MakeName("102"));
        _courses.Insert(Common.MakeName("103"));
    }

    // 1. Добавить студента на курс
    public void AddStudentToCourse(char[] sName, char[] cName)
    {
        // Ищем студента и курс в хеш-таблицах.
        int studentIndex = _students.Member(sName);
        int courseIndex = _courses.Member(cName);

        // Если хотя бы одного элемента нет, связь создать нельзя.
        if (studentIndex == -1 || courseIndex == -1) return;

        StudentNode? student = _students.GetArray()[studentIndex];
        CourseNode? course = _courses.GetArray()[courseIndex];

        if (student == null || course == null) return;

        // Перед добавлением проверяем, что такой пары ещё нет.
        // Если предыдущий найден, значит регистрация уже существует.
        BaseNode? prevStudent = FindPrevInStudentRing(student, course);
        if (prevStudent != null) return;

        // Создаём новую регистрационную запись.
        RegistrationNode reg = new RegistrationNode();

        // Вставка в кольцо студента — в начало.
        if (student.Next == null)
        {
            // Если у студента ещё нет связей, регистрация замыкается на самого студента.
            student.Next = reg;
            reg.StudentLink = student;
        }
        else
        {
            // Если связи уже есть, новая регистрация становится первой.
            reg.StudentLink = student.Next;
            student.Next = reg;
        }

        // Вставка в кольцо курса — тоже в начало.
        if (course.Next == null)
        {
            // Если у курса ещё нет студентов, регистрация замыкается на сам курс.
            course.Next = reg;
            reg.CourseLink = course;
        }
        else
        {
            // Если связи уже есть, новая регистрация становится первой.
            reg.CourseLink = course.Next;
            course.Next = reg;
        }
    }

    // 2. Удалить студента с конкретного курса
    public void RemoveStudentFromCourse(char[] sName, char[] cName)
    {
        // Ищем студента и курс в хеш-таблицах.
        int studentIndex = _students.Member(sName);
        int courseIndex = _courses.Member(cName);

        // Если студента или курса нет, удалять нечего.
        if (studentIndex == -1 || courseIndex == -1) return;

        StudentNode? student = _students.GetArray()[studentIndex];
        CourseNode? course = _courses.GetArray()[courseIndex];

        // Сначала ищем предыдущий узел по кольцу студента.
        // Если не нашли, значит такой связи студент-курс нет.
        BaseNode? prevStudent = FindPrevInStudentRing(student, course);
        if (prevStudent == null) return;

        // Если связь нашлась по кольцу студента,
        // ищем предыдущий узел этой же связи в кольце курса.
        BaseNode? prevCourse = FindPrevInCourseRing(course, student);

        // Получаем саму удаляемую регистрационную запись.
        RegistrationNode reg;

        if (prevStudent.IsRegistration())
        {
            // Если предыдущий узел — регистрационная запись,
            // значит удаляемая запись стоит после неё по StudentLink.
            RegistrationNode prevReg = (RegistrationNode)prevStudent;
            reg = (RegistrationNode)prevReg.StudentLink;
        }
        else
        {
            // Иначе предыдущий узел — сам StudentNode,
            // значит удаляемая регистрация первая в кольце студента.
            reg = (RegistrationNode)student.Next!;
        }

        // Удаляем запись из кольца студента.
        if (prevStudent.IsRegistration())
        {
            // Если перед удаляемой записью стоит другая регистрация,
            // перепрыгиваем через удаляемую запись.
            RegistrationNode prevReg = (RegistrationNode)prevStudent;
            prevReg.StudentLink = reg.StudentLink;
        }
        else
        {
            // Если предыдущий узел — сам студент,
            // меняем вход в кольцо студента.
            student.Next = reg.StudentLink == student ? null : reg.StudentLink;
        }

        // Удаляем эту же запись из кольца курса.
        if (prevCourse.IsRegistration())
        {
            // Если перед удаляемой записью стоит другая регистрация,
            // перепрыгиваем через удаляемую запись по CourseLink.
            RegistrationNode prevReg = (RegistrationNode)prevCourse;
            prevReg.CourseLink = reg.CourseLink;
        }
        else
        {
            // Если предыдущий узел — сам курс,
            // меняем вход в кольцо курса.
            course.Next = reg.CourseLink == course ? null : reg.CourseLink;
        }
    }

    // 3. Удалить конкретного студента со всех курсов
    public void RemoveStudentFromAll(char[] sName)
    {
        // Ищем студента в хеш-таблице.
        int studentIndex = _students.Member(sName);

        // Если студента нет, удалять нечего.
        if (studentIndex == -1) return;

        StudentNode? student = _students.GetArray()[studentIndex];

        // Если у студента нет регистрационных записей, он никуда не записан.
        if (student?.Next == null) return;

        // Начинаем обход с первой регистрации студента.
        BaseNode? cur = student.Next;

        // Идём по кольцу студента, пока не вернёмся к самому StudentNode.
        while (cur != null && cur != student)
        {
            // Текущий узел — регистрационная запись.
            RegistrationNode reg = (RegistrationNode)cur;

            // Сразу запоминаем следующий узел по кольцу студента,
            // потому что текущую регистрацию будем отвязывать из другого кольца.
            BaseNode? next = reg.StudentLink;

            // По регистрационной записи определяем курс,
            // к которому относится эта связь.
            CourseNode? course = GetCourseFromRegistration(reg);

            if (course != null)
            {
                // В кольце курса эта регистрация может быть не первой,
                // поэтому ищем предыдущий узел в кольце курса.
                BaseNode? prevCourse = FindPrevInCourseRing(course, student);

                if (prevCourse.IsRegistration())
                {
                    // Если предыдущий узел — регистрация,
                    // перепрыгиваем через удаляемую запись по CourseLink.
                    ((RegistrationNode)prevCourse).CourseLink = reg.CourseLink;
                }
                else
                {
                    // Если предыдущий узел — сам курс,
                    // значит удаляемая регистрация первая в кольце курса.
                    course.Next = reg.CourseLink == course ? null : reg.CourseLink;
                }
            }

            // Переходим к следующей регистрации студента,
            // которую запомнили до удаления текущей.
            cur = next;
        }

        // После обхода все регистрации студента удалены,
        // поэтому вход в кольцо студента обнуляем.
        student.Next = null;
    }

    // 4. Удалить всех студентов с конкретного курса
    public void RemoveAllFromCourse(char[] cName)
    {
        // Ищем курс в хеш-таблице.
        int courseIndex = _courses.Member(cName);
        // Если курса нет, удалять нечего.
        if (courseIndex == -1) return;
        CourseNode? course = _courses.GetArray()[courseIndex];

        // Если у курса нет регистрационных записей, на него никто не записан.
        if (course?.Next == null) return;

        // Начинаем обход с первой регистрации курса.
        BaseNode? cur = course.Next;

        // Идём по кольцу курса, пока не вернёмся к самому CourseNode.
        while (cur != null && cur != course)
        {
            // Текущий узел — регистрационная запись.
            RegistrationNode reg = (RegistrationNode)cur;

            // Сразу запоминаем следующий узел по кольцу курса,
            // потому что текущую регистрацию будем отвязывать из кольца студента.
            BaseNode? next = reg.CourseLink;
            StudentNode? student = GetStudentFromRegistration(reg);

            if (student != null)
            {
                // В кольце студента эта регистрация может быть не первой,
                // поэтому ищем предыдущий узел по кольцу студента.
                BaseNode? prevStudent = FindPrevInStudentRing(student, course);


                if (prevStudent.IsRegistration())
                {
                    // Если предыдущий узел — регистрация,
                    // перепрыгиваем через удаляемую запись по StudentLink.
                    ((RegistrationNode)prevStudent).StudentLink = reg.StudentLink;
                }
                else
                {
                    // Если предыдущий узел — сам студент,
                    // значит удаляемая регистрация первая в кольце студента.
                    student.Next = reg.StudentLink == student ? null : reg.StudentLink;
                }
            }

            // Переходим к следующей регистрации курса,
            // которую запомнили до удаления текущей.
            cur = next;
        }

        // После обхода все регистрации курса удалены,
        // поэтому вход в кольцо курса обнуляем.
        course.Next = null;
    }

    // 5. Вывести список курсов, которые посещает конкретный студент
    public void PrintCoursesOfStudent(char[] sName)
    {
        int studentIndex = _students.Member(sName);

        if (studentIndex == -1)
        {
            Console.WriteLine("Студент не найден или не записан на курсы.");
            return;
        }

        StudentNode? student = _students.GetArray()[studentIndex];

        if (student?.Next == null)
        {
            Console.WriteLine("Студент не найден или не записан на курсы.");
            return;
        }

        Console.Write("Курсы студента ");
        PrintName(sName);
        Console.WriteLine(":");

        BaseNode? cur = student.Next;

        while (cur != null && cur != student)
        {
            RegistrationNode reg = (RegistrationNode)cur;

            CourseNode? course = GetCourseFromRegistration(reg);

            if (course != null)
            {
                PrintName(course.Name);
                Console.WriteLine();
            }

            cur = reg.StudentLink;
        }
    }

    // 6. Вывести список студентов, которые посещают конкретный курс
    public void PrintStudentsOfCourse(char[] cName)
    {
        int courseIndex = _courses.Member(cName);

        if (courseIndex == -1)
        {
            Console.WriteLine("Курс не найден или нет записанных студентов.");
            return;
        }

        CourseNode? course = _courses.GetArray()[courseIndex];

        if (course?.Next == null)
        {
            Console.WriteLine("Курс не найден или нет записанных студентов.");
            return;
        }

        Console.Write("Студенты курса ");
        PrintName(cName);
        Console.WriteLine(":");

        BaseNode? cur = course.Next;

        while (cur != null && cur != course)
        {
            RegistrationNode reg = (RegistrationNode)cur;

            StudentNode? student = GetStudentFromRegistration(reg);

            if (student != null)
            {
                PrintName(student.Name);
                Console.WriteLine();
            }

            cur = reg.CourseLink;
        }
    }

    // ==================== Вспомогательные методы ====================

    // Поиск предыдущего узла в кольце студента.
    // Ищем регистрацию, которая связывает данного студента с данным курсом.
    private BaseNode? FindPrevInStudentRing(StudentNode student, CourseNode course)
    {
        BaseNode? cur = student.Next;
        BaseNode prev = student;

        while (cur != null && cur != student)
        {
            RegistrationNode reg = (RegistrationNode)cur;

            CourseNode? currentCourse = GetCourseFromRegistration(reg);

            if (currentCourse == course)
                return prev;

            prev = reg;
            cur = reg.StudentLink;
        }

        return null;
    }

    // Поиск предыдущего узла в кольце курса.
    // Ищем регистрацию, которая связывает данный курс с данным студентом.
    private BaseNode? FindPrevInCourseRing(CourseNode course, StudentNode student)
    {
        BaseNode? cur = course.Next;
        BaseNode prev = course;

        while (cur != null && cur != course)
        {
            RegistrationNode reg = (RegistrationNode)cur;

            StudentNode? currentStudent = GetStudentFromRegistration(reg);

            if (currentStudent == student)
                return prev;

            prev = reg;
            cur = reg.CourseLink;
        }

        return null;
    }

    // Получить курс по регистрационной записи.
    private CourseNode? GetCourseFromRegistration(RegistrationNode reg)
    {
        BaseNode? cur = reg.CourseLink;

        while (cur != null && cur.IsRegistration())
            cur = ((RegistrationNode)cur).CourseLink;

        return (CourseNode?)cur;
    }

    // Получить студента по регистрационной записи.
    private StudentNode? GetStudentFromRegistration(RegistrationNode reg)
    {
        BaseNode? cur = reg.StudentLink;

        while (cur != null && cur.IsRegistration())
            cur = ((RegistrationNode)cur).StudentLink;

        return (StudentNode?)cur;
    }

    // Вывод имени, которое хранится как char[].
    private static void PrintName(char[] name)
    {
        int i = 0;

        while (i < name.Length && name[i] != '\0')
        {
            Console.Write(name[i]);
            i++;
        }
    }
}