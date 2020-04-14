using System;
using System.Text.RegularExpressions;

namespace DelegatesEventsLambdaExpressions//Вариант 14
{//№9 Делегаты, события и лямбда выражения
    class Program
    {/*Задание
1. Используя делегаты (множественные) и события промоделируйте
ситуации приведенные в таблице ниже. Можете добавить новые типы (классы),
если существующих недостаточно. При реализации методов везде где возможно
использовать лямбда-выражения.
2. Создайте пять методов пользовательской обработки строки (например
удаление знаков препинания, добавление символов, замена на заглавные,
удаление лишних пробелов и т.п.). Используя стандартные типы делегатов
(Action, Func) организуйте алгоритм последовательной обработки строки
написанными методами.*/

        /*Создать класс Программист с событиями Переименовать и
        Новое свойство. В main создать некоторое количество объектов
        (языков программирования). Подпишите объекты на события
        произвольным образом. Реакция на события может быть
        следующая: изменение имени/версии, добавление новых операций,
        технологий или понятий. Проверить результаты изменения
        состояния объектов после наступления событий, возможно не
        однократном*/
        static void Main(string[] args)
        {           
            Programmer<string>[] programmers = new Programmer<string>[5];           
            programmers[0] = new Programmer<string>("C#", "Egor");
            programmers[1] = new Programmer<string>("Swift", "Roman");
            programmers[2] = new Programmer<string>("C++", "Dima");
            programmers[3] = new Programmer<string>("     Java_", "Michael_   ");
            programmers[4] = new Programmer<string>("Python", "Slava");

            for (int i = 0; i < programmers.Length; i++)
            {
                programmers[i].Notify += DisplayMessage;
            }
            //Вывод класса Программист
            Console.WriteLine("//Вывод класса Программист// \n");
            programmers[4].ViewEverythingFromAnyNumber(programmers);

            //Добавление символов
            programmers[4].Rename(programmers, "Dima", "DIMA");
            Console.WriteLine("/Добавление символов(переименование Dima в DIMA)/ \n");
            programmers[4].ViewEverythingFromAnyNumber(programmers);

            CustomStringHandling<Programmer<string>> customStringHandling = new CustomStringHandling<Programmer<string>>();
            Action action = () =>
            {               
                programmers[3] = customStringHandling.Capitalization(programmers[3]);
                Console.WriteLine("/Смена регистра(вместо 'Michael_' теперь 'MICHAEL_')/ \n");
                programmers[4].ViewEverythingFromAnyNumber(programmers);
            };
            action();

            //Замена на заглавные
            for (int i = 0; i < programmers.Length; i++)
            {
                Func<Programmer<string>, Programmer<string>> selector = str => customStringHandling.Capitalization(programmers[i]);
                selector(programmers[i]);
            }
           Console.WriteLine("/Замена на заглавные/ \n");
            programmers[4].ViewEverythingFromAnyNumber(programmers);

            //Смена регистра
            Func<string, string> convert = s => s.ToLower();
            for (int i = 0; i < programmers.Length; i++)
            {
                programmers[i].Name = convert(programmers[i].Name);
            }
            Console.WriteLine("/Смена регистра(только имена)/ \n");
            programmers[4].ViewEverythingFromAnyNumber(programmers);

            //Удаление лишних пробелов
            CustomStringHandling<Programmer<string>> RemoveSpacesHandling= new CustomStringHandling<Programmer<string>>();
            Action action2 = () =>
            {
                programmers[3] = RemoveSpacesHandling.RemoveExtraSpaces(programmers[3]);
                Console.WriteLine("/Удаление лишних пробелов/ \n");
                programmers[3].ViewEverythingFromAnyNumber(programmers);
            };
            action2();

            //Удаление знаков препинания
            CustomStringHandling<Programmer<string>> DeletingPunctuationMarks = new CustomStringHandling<Programmer<string>>();
            Action action3 = () =>
            {
                programmers[3] = DeletingPunctuationMarks.RemovingPunctuationMarks(programmers[3]);
                Console.WriteLine("/Удаление знаков препинания(вместо '_michael_' теперь 'michael')/ \n");
                programmers[3].ViewEverythingFromAnyNumber(programmers);
            };
            action3();

            //Вставка одной строки в другую
            CustomStringHandling<Programmer<string>> InsertOneLineIntoAnother = new CustomStringHandling<Programmer<string>>();
            Action action4 = () =>
            {
                programmers[3] = InsertOneLineIntoAnother.AddingCharacters(programmers[3], programmers[4], 2);
                Console.WriteLine("/Вставка одной строки в другую/ \n");
                programmers[3].ViewEverythingFromAnyNumber(programmers);
            };
            action4();         

            Console.ReadKey();
        }
        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }      
    }
    public class CustomStringHandling<T>
    {
        delegate void MessageHandler(string message);
        public T Line { get; set; }
        public CustomStringHandling()
        {

        }
        public CustomStringHandling(T Line)
        {
            this.Line = Line;
        }
        public Programmer<string> RemovingPunctuationMarks(Programmer<string> p)//Удаление знаков препинания, используя регулярные выражения
        {
            p.ProgrammingLanguage = Regex.Replace(p.ProgrammingLanguage, "[-.?!)(,:_]", "");
            p.Name = Regex.Replace(p.Name, "[-.?!)(,:_]", "");
            return p;
        }
        public Programmer<string> AddingCharacters(Programmer<string> p1, Programmer<string> p2, int space)//Вставка одной строки в другую
        {                
            p1.Name = p2.Name.Insert(space, p2.Name);
            p1.ProgrammingLanguage = p2.ProgrammingLanguage.Insert(space, p2.Name);
            return p2;
        }
        public Programmer<string> Capitalization(Programmer<string> p)//Смена регистра
        {                    
            p.ProgrammingLanguage = p.ProgrammingLanguage.ToUpper();
            p.Name = p.Name.ToUpper();         
            return p;
        }
        public Programmer<string> RemoveExtraSpaces(Programmer<string> p)//удаление лишних пробелов
        {                   
            p.ProgrammingLanguage = p.ProgrammingLanguage.Trim();
            p.Name = p.Name.Trim();
            return p;
        }
        public void LineSplitting(string text)//Разделение строк
        {          
            string[] words = text.Split(new char[] { ' ' });

            foreach (string s in words)
            {
                MessageHandler handler = delegate (string mes)
                {
                    Console.WriteLine(mes);
                };
                handler(s);               
            }
        }
    }
    public class Programmer<T>
    {
        public delegate void ProgrammerHandler(string message);
        public event ProgrammerHandler Notify;              //1. Определение события
        public T ProgrammingLanguage { get; set; }
        public T Name { get; set; }
       
        public void Rename(Programmer<string>[] programmers, string Name, string NewName)
        {
            for (int i = 0; i < programmers.Length; i++)
            {
                if (programmers[i].Name == Name)
                {
                    Action action = () =>
                    {
                        programmers[i].Name = NewName;
                    };
                    action();
                }
            }
        }
        public Programmer<string> RenameOwner(Programmer<string> programmers, string NewName)
        {
            programmers.Name = NewName;
            return programmers;
        }
        public void ViewEverythingFromAnyNumber(Programmer<string>[] programmers)
        {
            for (int i = 0; i < programmers.Length; i++)
            {
                Console.WriteLine($"{programmers[i].Name}: {programmers[i].ProgrammingLanguage}\n");
            }
            Notify?.Invoke(new string('-', 20));   // 2. Вызов события 
            //Console.WriteLine(new string('-', 20));
        }
        public Programmer()
        {
            //ProgrammingLanguage = "ProgrammingLanguage";
            //Name = "Name";
        }       
        public Programmer(T ProgrammingLanguage, T Name)
        {
            this.ProgrammingLanguage = ProgrammingLanguage;
            this.Name = Name;
        }
    }
}
