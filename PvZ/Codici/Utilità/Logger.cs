using System;

namespace Plants_Vs_Zombies
{
    static class Logger
    {
	    //Livelli classici del logger
        public enum Grades
        {
            Off,
            Fatal,
            Error,
            Warn,
            Info,
            Debug,
            Trace
        }
	
	    //Normalmente settato ad OFF, se lo si vuole usare va settato esternamente ad un livello diverso compresp tra 0 e 600
        private static Grades grade = Grades.Off;

        public static Grades Grade
        {
            get
            {
                return grade;
            }
            set
            {
                if (value < 0)
                    grade = (Grades)0;
                else if ((int)value > 600)
                    grade = (Grades)600;
                else
                    grade = value;
            }
        }

	    //Se il logger non è settato ad 0 e il messaggio ha una priorità minore o uguale al grado allora procede con la stampa su linea senza a capo
        public static void Write(string Text, int grade)
        {
            if (grade < 1)
                grade = 100;
            else if (grade > 600)
                grade = 600;

            if ((int)Grade >= grade)
                Console.Write(Text);
        }

        //Se il logger non è settato ad 0 e il messaggio ha una priorità minore o uguale al grado allora procede con la stampa su linea con a capo
        public static void WriteLine(string Text, int grade)
        {
            if (grade < 1)
                grade = 100;
            else if (grade > 600)
                grade = 600;

            if ((int)Grade >= grade)
                Console.WriteLine(Text);
        }
    }
}