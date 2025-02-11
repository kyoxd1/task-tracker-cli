
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Task
{
    public  int id {get; set;}
    public  required string description {get; set;} 
    public  string status {get; set;} = "todo";
    public  string createdAt {get; set;} = DateTime.Now.ToString("o");
    public  string uptadedAt {get; set;} = DateTime.Now.ToString("o");
}

public class Comands
{
    private const string TASK_FILE = "../tasks.json";

    private List<Task> _originalTasks = new List<Task>();
    public List<Task> load_tasks()
    {
        if (File.Exists(TASK_FILE))
        {
            var json = File.ReadAllText(TASK_FILE);
            _originalTasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
            return JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
        }
        return new List<Task>();
    }

    public void add_task(List<Task> tasks, string description)
    {
        var task = new Task
        {
            id = tasks.Count +1,
            description = description,
            status = "todo",
            createdAt = DateTime.Now.ToString("o"),
            uptadedAt = DateTime.Now.ToString("o")
        };

        tasks.Add(task);
        Console.WriteLine($"Tarea añadida con éxito, (ID: {task.id})");
    }

    public void list_tasks(List<Task> tasks)
    {
        foreach (var task in tasks)
        {
            Console.WriteLine($"{task.id} : {task.description}  [{task.status}]");
        }
    }

    public void update_task(List<Task> tasks, int task_id, string description)
    {
        var task = tasks.FirstOrDefault(t => t.id == task_id);

        if (task != null)
        {
            task.description = description;
            task.uptadedAt = DateTime.Now.ToString("o");
            Console.WriteLine($"Tarea {task_id} modificada exitosamente");
        }
        else
        {
            Console.WriteLine($"Tarea {task_id} no encontrada");
        }
    }

    public void delete_task(List<Task> tasks, int task_id)
    {
        tasks.RemoveAll(t => t.id == task_id);
        Console.WriteLine($"Tarea {task_id} eliminada.");
    }

    public void changeStatus(List<Task> tasks, int task_id, string status)
    {
        var task = tasks.FirstOrDefault(t => t.id == task_id);

        if (task != null)
        {
            task.status = status;
            task.uptadedAt = DateTime.Now.ToString("o");
            Console.WriteLine($"Tarea {task_id} marcado como {status}");
        }
        else
        {
            Console.WriteLine($"Tarea {task_id} no encontrada");
        }
    }

    public void list_taks_by_status(List<Task> tasks, string status)
    {
        var filltered_tasks = tasks.Where(t => t.status == status).ToList();

        if (!filltered_tasks.Any())
        {
            Console.WriteLine($"No se encontraron tareas con el estado '{status}'.");
            return;
        }

        Console.WriteLine($"Tareas con el estado '{status}':");
        list_tasks(filltered_tasks);
    }

    public void save_tasks(List<Task> tasks)
    {
        try
        {
            if (!HaveTasksChanged(tasks))
            {
                return;
            }

            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(TASK_FILE, json);
            Console.WriteLine("Tareas guardadas exitosamente.");
        }
        catch (System.Exception e)
        {
            Console.WriteLine($"Error al guardar tareas: {e.Message}");
        }
    }

    private bool HaveTasksChanged(List<Task> tasks)
    {
        var currentJson = JsonSerializer.Serialize(tasks);
        var originalJson = JsonSerializer.Serialize(_originalTasks);
        return currentJson != originalJson;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: task_cs [comando] ['argumentos']");
            Console.WriteLine("Commands:");
            Console.WriteLine("  add <description>          - Añade una tarea");
            Console.WriteLine("  update <id> <description>  - Actualiza una tarea");
            Console.WriteLine("  delete <id>               - Elimina una tarea");
            Console.WriteLine("  mark-in-progress <id>      - Marca una tarea en progreso");
            Console.WriteLine("  mark-done <id>            - Marca una tarea como finalizada");
            Console.WriteLine("  list                      - Lista de las tareas");
            Console.WriteLine("  list todo                 - Lista todas las tareas en todo");
            Console.WriteLine("  list in-progress          - Lista todas las tareas 'en progreso'");
            Console.WriteLine("  list done                 - Lista todas las tareas 'finalizadas'");
            return;
        }

        var comands = new Comands();
        var tasks = comands.load_tasks();

        var command = args[0];

        switch (command)
        {
            case "add":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: La tarea debe tener una descripcion.");
                    return;
                }
                var descripcion = args[1];
                comands.add_task(tasks, descripcion);
                break;

            case "update":
                if (args.Length < 3)
                {
                    Console.WriteLine("Error la tarea debe tener id y descripcion");
                    return;
                }

                if (int.TryParse(args[1], out var updateTaskId))
                {
                    var updateTaksDescription = args[2];
                    comands.update_task(tasks, updateTaskId, updateTaksDescription);                    
                }
                else
                {
                    Console.WriteLine("Error: el ID debe ser un numero");
                }
                break;

            case "delete":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: se debe poner el ID de la tarea.");
                    return;
                }
                if (int.TryParse(args[1], out var deleteTaskId))
                {
                    comands.delete_task(tasks, deleteTaskId);
                }
                else
                {
                    Console.WriteLine("Error: Task ID debe ser un numero.");
                }
                break;

            case "mark-in-progress":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: No se encontro la tarea con ese ID.");
                    return;
                }

                if (int.TryParse(args[1], out var taskId))
                {
                    comands.changeStatus(tasks, taskId, "in-progress");
                }
                else
                {
                    Console.WriteLine("Error: Task ID debe ser un numero.");
                }
                break;

            case "mark-done":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: No se encontro la tarea con ese ID.");
                    return;
                }

                if (int.TryParse(args[1], out var task_id))
                {
                    comands.changeStatus(tasks, task_id, "done");
                }
                else
                {
                    Console.WriteLine("Error: Task ID debe ser un numero.");
                }
                break;

            case "list":
                if (args.Length == 1)
                {
                    comands.list_tasks(tasks);
                }
                else if (args.Length == 2)
                {
                    var status = args[1];
                    if (new[] { "todo", "in-progress", "done" }.Contains(status))
                    {
                        comands.list_taks_by_status(tasks, status);
                    }
                    else
                    {
                        Console.WriteLine("Error: No existe ese estado. Use 'todo', 'in-progress', o 'done'.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Comando de lista invalido.");
                }
                break;
                

            default:
                Console.WriteLine($"Error: comando desconocido '{command}'.");
                break;
        }

        comands.save_tasks(tasks);
    }
}
    