using System.Collections.Generic;
using System.Threading.Tasks;

// Элемент Контроллер.Представляет центральный компонент MVC, который обеспечивает связь между пользователем и приложением, представлением и хранилищем данных.
// Он содержит логику обработки запроса пользователя. Контроллер получает вводимые пользователем данные и обрабатывает их.
// И в зависимости от результатов обработки отправляет пользователю определенный вывод.

namespace task7
{ //интерфейс наблюдателя
    public interface INotifier { void Notify(); } 
    //класс менеджера 
    public class Manager { 
        //лист наблюдателей 
        private List<INotifier> observers; 
        //создаем листы вакансии и соискателей 
        List<WorkIT> works; 
        List<Vacancy> vacancies; 
        //объект для синхронизации 
        private object locker = new object(); 
        //конструктор 
        public Manager() 
        { 
            observers = new List<INotifier>(); 
            works = new List<WorkIT>(); 
            vacancies = new List<Vacancy>(); } 
        //добавление обсервера 
        public void AddObserver(INotifier observer) 
        { 
            observers.Add(observer); 
        } 
        //метод удаления наблюдателя 
        public void RemoveObserver(INotifier observer) 
        { 
            observers.Remove(observer); 
        } 
        //оповещение наблюдателя 
        private void NotifyObservers() 
        { 
            foreach (INotifier observer in observers) 
            { 
                observer.Notify(); 
            } 
        } 
        //методы добавления для двух листов 
        public void Add(WorkIT work) 
        {
            //lock (locker) создает блокировку и определяет объект, который будет использоваться в качестве монитора для синхронизации доступа к какому-либо ресурсу.
            //Только один поток может иметь доступ к коду внутри блока lock в одно время. Используя "lock", мы гарантируем,
            //что только один поток может получить доступ к блоку кода за раз, чтобы избежать потенциальных конфликтов .
            lock (locker) 
            { works.Add(work);
                NotifyObservers(); 
            } 
        } 
        public void AddV(Vacancy vac) 
        { 
            lock (locker) 
            { 
                vacancies.Add(vac); 
                NotifyObservers(); 
            } 
        }

    //методы обновления двух листов
    public List<WorkIT> upd()
    {
        lock (locker)
        {
            List<WorkIT> results = new List<WorkIT>();
            foreach (var item in works)
            {
                WorkIT work = (WorkIT)item;
                results.Add(work);
            }
            return results;
        }
    }

    public List<Vacancy> updV()
    {
        lock (locker)
        {
            List<Vacancy> results = new List<Vacancy>();
            foreach (var item in vacancies)
            {
                Vacancy vac = (Vacancy)item;
                results.Add(vac);
            }
            return results;
        }
    }
    //методы поиска по двум листам
    public WorkIT Find(string name)
    {
        lock (locker)
        {
            for (int i = 0; i < works.Count; i++)
            {
                var work = works[i];
                if (work.Name == name)
                {
                    return work;
                }
            }
            return null;
        }
    }
    public Vacancy FindV(int id)
    {
        lock (locker)
        {
            for (int i = 0; i < vacancies.Count; i++)
            {
                var vac = vacancies[i];
                if (vac.ID == id)
                {
                    return vac;
                }
            }
            return null;
        }
    }
    //методы удаления из двух листов
    public bool Remove(string name)
    {
        lock (locker)
        {
            var work = Find(name);
            if (work != null)
            {
                works.Remove(work);
                return true;
            }
            return false;
        }
    }

    public bool RemoveV(int id)
    {
        lock (locker)
        {
            var vac = FindV(id);
            if (vac != null)
            {
                vacancies.Remove(vac);
                return true;
            }
            return false;
        }
    }

    // метод редактирования 
    public void Edit(string name, WorkIT d)
    {
        lock (locker)
        {
            var work = Find(name);
            if (work != null)
            {
                work.Name = d.Name;
                work.FieldOfActivity = d.FieldOfActivity;
                work.Experience = d.Experience;
                work.Education = d.Education;
            }
        }
    }
    // метод редактирования второго листа
    public void EditV(int id, Vacancy d)
    {
        lock (locker)
        {
            var vac = FindV(id);
            if (vac != null)
            {
                vac.Name = d.Name;
                vac.ID = d.ID;
                vac.Experience = d.Experience;
                vac.Education = d.Education;
            }
        }
    }
    //метод подсчёта зп
    public int earner(WorkIT work, int exp)
    {
        return work.Earn(exp);
    }
    //метод оценки вакансии
    public bool check(WorkIT work, int id)
    {
        Vacancy search = FindV(id);
        return work.checker(search.Experience, search.Education);
    }
}
}