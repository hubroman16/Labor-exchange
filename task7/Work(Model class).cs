using System;
// Элемент Модель в системе MVC. Содержит все основные классы для работы приложения, вместе с их методами.
// Так как это модель то здесь не содержится логика взаимодействия с пользователем и не определяется механизм обработки запроса.
// Кроме того, модель не содержит логику отображения данных в представлении.

namespace task7
{
    //интерфейс по заработку
    public interface IEarning
    {
        int Earn(int exp);
    }

    //класс "Работа" родитель для класса "Работа в айти", к нему подвязан интерфейс по заработку и реализован метод
    public class Work :IEarning
    {
        //поля
        public string Name { get; set; }
        public string FieldOfActivity { get; set; }
        public int Experience { get; set; }
        public string Education { get; set; }
        //конструктор класса
        public Work(string name,string field,int exp,string education)
        {
            Name = name;
            FieldOfActivity = field;
            Experience = exp;
            Education = education;
        }
        //метод проверки, который будет переопределён в дочернем классе
        public virtual bool checker(int exp,string edu)
        {
            return true;
        }
        //реализация метода интерфейса, определяет размер зп
        public int Earn(int exp)
        {
            if (exp >= 2)
            {
                return exp * 25000;
            }
            else
            {
                return 0;
            }
        }
    }

    //дочерний класс, наследуется от класса "Работа"
    public class WorkIT : Work
    {
        //конструктор класса наследника, наследуется от родительского
        public WorkIT(string name, string field, int exp, string education) : base(name, field, exp, education)
        {
        }
        //переопределение метода проверки на условия работы
        public override bool checker(int exp, string edu)
        {
            if (exp >= 2 & edu == "Высшее")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //класс вакансии
    public class Vacancy
    {
        //поля
        public string Name { get; set; }
        public int ID { get; set; }
        public int Experience { get; set; }
        public string Education { get; set; }
        //конструктор
        public Vacancy(string name, int id, int exp, string education)
        {
            Name = name;
            ID = id;
            Experience = exp;
            Education = education;
        }
    }
}
