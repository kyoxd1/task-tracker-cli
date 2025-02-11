import sys
import json
from datetime import datetime

# leemos las tareas ya creadas
TASKS_FILE = 'tasks.json'

def load_tasks():
    try:
        with open(TASKS_FILE, 'r') as file:
            return json.load(file)

    except FileNotFoundError:
        return []

def add_task(tasks, description):
    task = {
        'id': len(tasks) + 1,
        'description': description,
        'status': 'todo',
        'createdAt': datetime.now().isoformat(),
        'updatedAt': datetime.now().isoformat()
    }
    tasks.append(task)
    print(f"Tarea añadida con exito, (ID: {task['id']})")

def list_tasks(tasks):
    for task in tasks:
        print(f"{task['id']}: {task['description']} [{task['status']}]")

def update_task(tasks, task_id, description):
    for task in tasks:
        if task['id'] == task_id:
            task['description'] = description
            task['updatedAt'] = datetime.now().isoformat()
            print(f"Task {task_id} updated successfully.")
            return
    print(f"Tarea {task_id} no encontrado.")

def delete_task(tasks, task_id):
    tasks[:] = [task for task in tasks if task['id'] != task_id]
    print(f"Tarea {task_id} eliminado.")

def mark_in_progress(tasks, task_id):
    for task in tasks:
        if task['id'] == task_id:
            task['status'] = 'in-progress'
            task['updatedAt'] = datetime.now().isoformat()
            print(f"Tarea {task_id} marcado como in progress.")
            return
    print(f"Task {task_id} no encontrada.")

def mark_done(tasks, task_id):
    for task in tasks:
        if task['id'] == task_id:
            task['status'] = 'done'
            task['updatedAt'] = datetime.now().isoformat()
            print(f"Task {task_id} marked as done.")
            return
    print(f"Tarea {task_id} no encontrado.")

def list_tasks_by_status(tasks, status):
    filtered_tasks = [task for task in tasks if task['status'] == status]
    
    if not filtered_tasks:
        print(f"No se encontro tareas con el estado '{status}'.")
        return
    
    print(f"Tareas con el estado '{status}':")
    for task in filtered_tasks:
        print(f"  {task['id']}: {task['description']} (Creado: {task['createdAt']}, Actualizado: {task['updatedAt']})")

def save_tasks(tasks, existing_tasks):
    try:
        # Cargar las tareas actuales del archivo JSON
        """try:
            with open(TASKS_FILE, 'r') as file:
                existing_tasks = json.load(file)
        except FileNotFoundError:
            existing_tasks = []"""

        # Comparar las tareas actuales con las nuevas
        if existing_tasks != tasks:
            with open(TASKS_FILE, 'w') as file:
                json.dump(tasks, file, indent=4)
            print("Tareas guardadas exitosamente.")

    except Exception as e:
        print(f"Error al guardar tareas: {e}")

def main():
    if len(sys.argv) < 2:
        print("Usage: task_cli.py [comando] ['argumentos']")
        print("Commands:")
        print("  add <description>          - Añade una tarea")
        print("  update <id> <description>  - Actualiza una tarea")
        print("  delete <id>               - Elimina una tarea")
        print("  mark-in-progress <id>      - Marca una tarea en progreso")
        print("  mark-done <id>            - Marca una tarea como finalizada")
        print("  list                      - Lista de las tareas")
        print("  list todo                 - Lista todas las tareas en todo")
        print("  list in-progress          - Lista todas las tareas 'en progreso'")
        print("  list done                 - Lista todas las tareas 'finalizadas'")
        return

    command = sys.argv[1]
    tasks, existing_tasks = load_tasks()

    if command == 'add':
        if len(sys.argv) < 3:
            print("Error: La tarea debe tener una descripcion.")
            return
        description = sys.argv[2]
        add_task(tasks, description)

    elif command == 'update':
        if len(sys.argv) < 4:
            print("Error: la tarea no tiene ID o descripcion.")
            return
        try:
            task_id = int(sys.argv[2])
            description = sys.argv[3]
            update_task(tasks, task_id, description)
        except ValueError:
            print("Error: el ID debe ser un numero.")

    elif command == 'delete':
        if len(sys.argv) < 3:
            print("Error: se debe poner el ID de la tarea.")
            return
        try:
            task_id = int(sys.argv[2])
            delete_task(tasks, task_id)
        except ValueError:
            print("Error: Task ID debe ser un numero.")

    elif command == 'mark-in-progress':
        if len(sys.argv) < 3:
            print("Error: No se encontro la tarea con ese ID.")
            return
        try:
            task_id = int(sys.argv[2])
            mark_in_progress(tasks, task_id)
        except ValueError:
            print("Error: Task ID debe ser un numero.")

    elif command == 'mark-done':
        if len(sys.argv) < 3:
            print("Error: Missing task ID.")
            return
        try:
            task_id = int(sys.argv[2])
            mark_done(tasks, task_id)
        except ValueError:
            print("Error: Task ID debe ser un numero.")

    elif command == 'list':
        if len(sys.argv) == 2:
            list_tasks(tasks)
        elif len(sys.argv) == 3:
            status = sys.argv[2]
            if status in ['todo', 'in-progress', 'done']:
                list_tasks_by_status(tasks, status)
            else:
                print("Error: No existe ese estado. Use 'todo', 'in-progress', o 'done'.")
        else:
            print("Error: Comando de lista invalido.")

    else:
        print(f"Error: comando desconocido '{command}'.")

    save_tasks(tasks, existing_tasks)

if __name__ == "__main__":
    main()
