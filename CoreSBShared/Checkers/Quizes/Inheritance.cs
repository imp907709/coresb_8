using System;

namespace CoreSBShared.Universal.Checkers.Quizes
{
    public class Inheritance
    {
        class MainClass
        {
            static void Main(string[] args)
            {
                Senior senior = new Senior();
                Student student = senior;
                student.study();
            }
        }

        class Student
        {
            public Student()
            {
                Console.Write("1");
            }

            public void study()
            {
                Console.Write("2");
            }
        }

        class Senior : Student
        {
            public Senior()
            {
                Console.Write("3");
            }

            public void study()
            {
                Console.Write("4");
            }
        }
    }
}
