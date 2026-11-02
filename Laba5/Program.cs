namespace Laba5
{
    public class Program
    {
        public static void Main()
        {
            // Создаём мультисписок
            MultiList ml = new MultiList();

            ml.Init();
            
            // === 3. ДОБАВЛЯЕМ СВЯЗИ (addStudentToCourse) ===
            ml.AddStudentToCourse(Common.MakeName("Ivanov"), Common.MakeName("101"));
            ml.AddStudentToCourse(Common.MakeName("Ivanov"), Common.MakeName("102"));
            ml.AddStudentToCourse(Common.MakeName("Petrov"), Common.MakeName("102"));
            ml.AddStudentToCourse(Common.MakeName("Petrov"), Common.MakeName("103"));
            ml.AddStudentToCourse(Common.MakeName("Sidorov"), Common.MakeName("101"));
            ml.AddStudentToCourse(Common.MakeName("Sidorov"), Common.MakeName("103"));

            // === 4. ПРОВЕРКА ВЫВОДА (printStudentsOfCourse, printCoursesOfStudent) ===
            ml.PrintStudentsOfCourse(Common.MakeName("101"));
            ml.PrintStudentsOfCourse(Common.MakeName("102"));
            ml.PrintStudentsOfCourse(Common.MakeName("103"));

            // === 5. УДАЛЕНИЕ СТУДЕНТА С КОНКРЕТНОГО КУРСА (removeStudentFromCourse) ===
            Console.WriteLine("\n=== Удаляем Ivanov с курса 102 ===");
            
            ml.RemoveStudentFromCourse(Common.MakeName("Ivanov"), Common.MakeName("102"));
            ml.PrintCoursesOfStudent(Common.MakeName("Ivanov"));

            // === 6. УДАЛЕНИЕ СТУДЕНТА СО ВСЕХ КУРСОВ (removeStudent) ===
            Console.WriteLine("\n=== Удаляем Sidorov со всех курсов ===");
            ml.RemoveStudentFromAll(Common.MakeName("Sidorov"));
            ml.PrintStudentsOfCourse(Common.MakeName("101"));
            Console.WriteLine("\n=== Вывод курсов, которые посещает Sidorov  ===");
            ml.PrintCoursesOfStudent(Common.MakeName("Sidorov"));

            // === 7. УДАЛЕНИЕ ВСЕХ СТУДЕНТОВ С КУРСА (removeCourse) ===
            Console.WriteLine("\n=== Удаляем всех студентов с курса 103 ===");
            ml.RemoveAllFromCourse(Common.MakeName("103"));
            ml.PrintStudentsOfCourse(Common.MakeName("103"));
            ml.PrintCoursesOfStudent(Common.MakeName("Petrov"));

            Console.WriteLine("\nТестирование всех операций завершено.");
        }
    }
}