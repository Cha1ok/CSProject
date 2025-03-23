using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{

    class Task
    {
        public string Tasks { get; set; }
        public int Iscomplete { get; set; }
    }
    internal class Program
    {
        static List<Task> tasks = new List<Task>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- To-Do List ---");
                Console.WriteLine("1. Показать задачи");
                Console.WriteLine("2. Добавить задачу");
                Console.WriteLine("3. Отметить задачу как выполненную");
                Console.WriteLine("4. Удалить задачу");
                Console.WriteLine("5. Выйти");
                Console.Write("Выберите действие: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        showTasks();
                        break;
                    case 2:
                        addTask();
                        break;
                    case 3:
                        completeTask();
                        break;
                    case 4:
                        deleteTask();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("неверный выбор, попробуйте ещё раз");
                        break;
                }
                void showTasks()
                {
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Список пуст");
                        return;
                    }
                    Console.WriteLine("\n--- To-Do List ---");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"Состояние {tasks[i].Iscomplete} задния {tasks[i].Tasks}");
                    }

                }
                void addTask()
                {
                    Console.WriteLine("Добавьте новую здачу");
                    Task task = new Task();
                    string descreption = Console.ReadLine();
                    task.Tasks = descreption;
                    task.Iscomplete = 0;
                    tasks.Add(task);
                }
                void completeTask()
                {
                    Console.WriteLine("Выберите задачу которую нужно звершить");
                    string task = Console.ReadLine();
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].Iscomplete == 0)
                        {
                            tasks[i].Iscomplete = 1;

                        }
                        else
                        {
                            Console.WriteLine("Задание уже завершено");
                        }
                    }
                }
                void deleteTask()
                {
                    Console.WriteLine("Выберите задание которое нужно завершить");
                    showTasks();
                    string result = Console.ReadLine();
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].Tasks.Equals(result))
                        {
                            tasks.RemoveAt(i);
                        }
                    }
                    {

                    }
                }
            }
        }
    }
}