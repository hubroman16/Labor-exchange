using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Элемент модели View. Отвечают за визуальную часть и пользовательский интерфейс, через который пользователь взаимодействует с приложением.
// Содержит логику, связанную с отображением данных. 
// В то же время представление не содержит логику обработки запроса пользователя или управления данными.

namespace task7
{
    //оповещатель вакансий/наблюдателей
    public class VacancyNotifier : INotifier { public void Notify() { MessageBox.Show("Новая вакансия/соискатель!"); } }
    public partial class Form1 : Form
    {
        //создание экземпляра класса менеджера
        Manager mg = new Manager();

        //создание новых форм
        Form3 newForm = new Form3();
        //конструктор формы
        public Form1()
        {
            InitializeComponent();
        }
        //обновление таблиц dgw и расширение колонок
        void updateData(IEnumerable<WorkIT> works)
        {
            dataGridView1.DataSource = works;
            dataGridView1.Columns[0].Width = 220;
            dataGridView1.Columns[1].Width = 140;
        }
        void updateDataV(IEnumerable<Vacancy> vacancies)
        {
            dataGridView2.DataSource = vacancies;
            dataGridView2.Columns[0].Width = 220;
            dataGridView2.Columns[1].Width = 140;
        }
        // функция импорта из файла
        public void inputtext()
        {
            string namefile = "worklist.txt";
            StreamReader file;
            try
            {
                file = new StreamReader(namefile);
                int n = Convert.ToInt32(file.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    var name = file.ReadLine();
                    var fieldOfActivity = file.ReadLine();
                    var expereince = file.ReadLine();
                    var education = file.ReadLine();
                    var work = new WorkIT(name, fieldOfActivity, int.Parse(expereince), education);
                    mg.Add(work);
                }
                updateData(mg.upd());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //импорт для второй таблицы
        public void inputtextV()
        {
            string namefile = "vacansylist.txt";
            StreamReader file;
            try
            {
                file = new StreamReader(namefile);
                int n = Convert.ToInt32(file.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    var name = file.ReadLine();
                    var id = file.ReadLine();
                    var expereince = file.ReadLine();
                    var education = file.ReadLine();
                    var vac = new Vacancy(name, int.Parse(id), int.Parse(expereince), education);
                    mg.AddV(vac);
                }
                updateDataV(mg.updV());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //обработка события загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            inputtext();
            inputtextV();
            loadform();
            //создание оповещателя
            VacancyNotifier vacancyNotifier = new VacancyNotifier();
            mg.AddObserver(vacancyNotifier);
        }
        //кнопка добавления вакансии
        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string fieldOfActivity = textBox2.Text;
            int expereince = int.Parse(textBox3.Text);
            string education = comboBox2.SelectedItem.ToString();
            if (name == "" || fieldOfActivity == "" || Convert.ToString(expereince) == "" || education == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                var work = new WorkIT(name, fieldOfActivity, Convert.ToInt32(expereince), education);
                mg.Add(work);
            }
            updateData(mg.upd());
            changer("На рынке новая вакансия!", 2);
        }
        //кнопка удаления вакансии
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                WorkIT work = mg.upd()[index];
                mg.Remove(work.Name);
                updateData(mg.upd());
            }
        }
        //кнопка изменения вакансии
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                WorkIT oldWork = mg.upd()[index];
                string name = textBox1.Text;
                string fieldOfActivity = textBox2.Text;
                int expereince = int.Parse(textBox3.Text);
                string education = comboBox2.SelectedItem.ToString();
                var newWork = new WorkIT(name, fieldOfActivity, Convert.ToInt32(expereince), education);
                mg.Edit(oldWork.Name, newWork);
                updateData(mg.upd());
            }
        }
        //кнопка вывода информации о вакансии (новая форма)
        private void button1_Click(object sender, EventArgs e)
        {
            string name = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
            string fieldOfActivity = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
            int expereince = Convert.ToInt32(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
            string education = Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
            int salary = mg.earner(mg.Find(name),expereince);
            Form2 newForm = new Form2(name, fieldOfActivity, Convert.ToInt32(expereince), education,salary);
            changer("Направили информацию \nна почту!", 4);
            newForm.ShowDialog();
        }
        //кнопка добавления соискателя
        private void button7_Click(object sender, EventArgs e)
        {
            string name = textBox7.Text;
            int id = Convert.ToInt32(textBox8.Text);
            int expereince = int.Parse(textBox9.Text);
            string education = comboBox1.SelectedItem.ToString();
            if (name == "" || Convert.ToString(id) == "" || Convert.ToString(expereince) == "" || education == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                var vac = new Vacancy(name, Convert.ToInt32(id), Convert.ToInt32(expereince), education);
                mg.AddV(vac);
            }
            updateDataV(mg.updV());
            changer("Новое резюме было создано!", 3);
        }
        //кнопка изменения соискателя
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int index = dataGridView2.SelectedRows[0].Index;
                Vacancy oldVac = mg.updV()[index];
                string name = textBox7.Text;
                int id = Convert.ToInt32(textBox8.Text);
                int expereince = int.Parse(textBox9.Text);
                string education = comboBox1.SelectedItem.ToString();
                var newVac = new Vacancy(name, Convert.ToInt32(id), Convert.ToInt32(expereince), education);
                mg.EditV(oldVac.ID, newVac);
                updateDataV(mg.updV());
            }
        }
        //кнопка удаления соискателя
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int index = dataGridView2.SelectedRows[0].Index;
                Vacancy vac = mg.updV()[index];
                mg.RemoveV(vac.ID);
                updateDataV(mg.updV());
            }
        }
        //кнопка подачи вакансии
        private void button3_Click(object sender, EventArgs e)
        {
            Form4 visual = new Form4();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                visual.Show();
                newForm.Hide();
                timer1.Enabled = true;
            }
        }
        
        public void loadform()
        {
            newForm.Show();
            changer("Человек в поисках работы", 1);
        }

        public void changer(string text,int num)
        {
            newForm.label2.Text = text;
            if (num == 1)
            {
                newForm.pictureBox2.Visible = true;
                newForm.pictureBox1.Visible = false;
                newForm.pictureBox3.Visible = false;
                newForm.pictureBox5.Visible = false;
                newForm.pictureBox4.Visible = false;
                newForm.pictureBox6.Visible = false;
            }
            if (num == 2)
            {
                newForm.pictureBox2.Visible = false;
                newForm.pictureBox1.Visible = false;
                newForm.pictureBox3.Visible = false;
                newForm.pictureBox5.Visible = false;
                newForm.pictureBox4.Visible = true;
                newForm.pictureBox6.Visible = false;
            }
            if (num == 3)
            {
                newForm.pictureBox2.Visible = false;
                newForm.pictureBox1.Visible = true;
                newForm.pictureBox3.Visible = false;
                newForm.pictureBox5.Visible = false;
                newForm.pictureBox4.Visible = false;
                newForm.pictureBox6.Visible = false;
            }
            if (num == 4)
            {
                newForm.pictureBox2.Visible = false;
                newForm.pictureBox1.Visible = false;
                newForm.pictureBox3.Visible = true;
                newForm.pictureBox5.Visible = false;
                newForm.pictureBox4.Visible = false;
                newForm.pictureBox6.Visible = false;
            }
            if (num == 5)
            {
                newForm.pictureBox2.Visible = false;
                newForm.pictureBox1.Visible = false;
                newForm.pictureBox3.Visible = false;
                newForm.pictureBox5.Visible = true;
                newForm.pictureBox4.Visible = false;
                newForm.pictureBox6.Visible = false;
            }
            if (num == 6)
            {
                newForm.pictureBox2.Visible = false;
                newForm.pictureBox1.Visible = false;
                newForm.pictureBox3.Visible = false;
                newForm.pictureBox5.Visible = false;
                newForm.pictureBox4.Visible = false;
                newForm.pictureBox6.Visible = true;
            }
        }

        //* Потоки
        //В данном коде создание потоков осуществляется при помощи метода Task.Run, который запускает задачу в отдельном потоке из пула потоков.
        //Этот метод возвращает объект типа Task, который можно сохранить в списке для дальнейшей синхронизации.
        //Для того чтобы дождаться завершения всех задач, использован метод Task.WhenAll, который ждет окончания всех переданных ему задач и возвращает задачу,
        //которая завершится после завершения всех задач из списка.В данном случае, когда все потоки закончат свою работу, будет выполнен код после метода Task.WhenAll.
        //Для синхронизации доступа к результатам проверки используется словарь idResults, который является объектом, доступным для чтения и записи из разных потоков.
        //В каждой задаче результаты проверки для соответствующего айди записываются в словарь через индексатор (idResults[id] = results;),
        //тем самым синхронизируя доступ к этим данным между потоками.
        //Затем, после завершения всех задач, результаты проверки объединяются и используются для вывода информации пользователю.
        //При выводе результатов на экран задействован главный поток (UI Thread), который в данном случае использует методы Show и MessageBox.Show.

        //Создадим метод, который будет запускать каждую задачу в отдельном потоке:
        private async Task CheckVacancyAsync(int id, DataGridViewSelectedRowCollection selectedRows, Dictionary<int, bool[]> idResults)
        {
            bool[] results = new bool[selectedRows.Count];
            for (int i = 0; i < selectedRows.Count; i++) 
            { 
                int index = selectedRows[i].Index;
                WorkIT work = mg.upd()[index];
                Vacancy search = mg.FindV(id);
                bool match = work.checker(search.Experience, search.Education);
                results[i] = match; 
            }
            idResults[id] = results; }

        //Затем вызываем этот метод для каждой вакансии в отдельном потоке:
        private async void btnCheckVacancy_Click()
        { // разбиваем айди по запятым и преобразуем в массив чисел 
            string[] idStrings = textBox6.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] ids = Array.ConvertAll(idStrings, x => int.Parse(x.Trim()));

            // переменная для хранения всех сообщений
            string message = "";

            // сохраняем результаты проверки каждой вакансии для каждого айди в словарь
            Dictionary<int, bool[]> idResults = new Dictionary<int, bool[]>();

            List<Task> tasks = new List<Task>();
            foreach (int id in ids)
            {
                tasks.Add(Task.Run(() => CheckVacancyAsync(id, dataGridView1.SelectedRows, idResults)));

                // добавляем задержку, чтобы избежать проблем с сетью
                await Task.Delay(500);
            }

            await Task.WhenAll(tasks);

            // выводим результаты проверки для всех вакансий
            int approvedCount = 0;
            int rejectedCount = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                int id = ids[i];
                bool[] results = idResults[id];

                bool allMatch = results.All(x => x);
                if (allMatch)
                {
                    approvedCount++;
                    message += "Ваше резюме одобрено для вакансии #" + id.ToString() + "!\n";
                }
                else
                {
                    rejectedCount++;
                    message += "Ваше резюме отклонено для вакансии #" + id.ToString() + "!\n";
                }
            }

            // выводим общий результат проверки для всех вакансий
            if (approvedCount == ids.Length)
            {
                textBox5.Text = "В обработке";
                textBox4.Text = "...";
                MessageBox.Show(message, "Результаты проверки", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Enabled = false;
            }
            else if (rejectedCount == ids.Length)
            {
                textBox5.Text = "В обработке";
                textBox4.Text = "...";
                MessageBox.Show(message, "Результаты проверки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
            }
            else
            {
                textBox5.Text = "В обработке";
                textBox4.Text = "...";
                MessageBox.Show(message, "Результаты проверки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timer1.Enabled = false;
            }

            newForm.Show();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnCheckVacancy_Click();
            timer1.Enabled = false;
        }

    }
}
