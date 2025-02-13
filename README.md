#######################################################
# LISTA DE TAREAS :smile:
#######################################################

### Este proyecto esta creado tanto en Python como C# para practica de creacion de CRUD por medio de comandos y obtencion de datos a traves de Json, el Json interactua tanto para el ejecutable en Python como en C#

## Requerimientos
    - Tener Python 3 o superior
    - Tener dotnet

## Lista de comnados
    - add <descripción>
    - update <id> <desc>
    - delete <id>	
    - mark-in-progress <id>
    - mark-done <id>
    - list
    - list <estado>

## Instalación:

## 1. Clona el repositorio:

   - git clone https://github.com/kyoxd1/task-tracker-cli.git
   - cd task-tracker-cli

## 2. Se ejecuta el archivo en Python:

    - py task_cli.py

## para obtener los comandos

## y para añadir alguna tarea o otro comando sigue la regla
    - py task_cli.py [comando] ['argumentos']

### Ejemplo en python:
    - py task_cli.py add "prueba desde python"

## 3. Ejecutar el archivo en C#:
    - dotnet build taskcs
    - cd taskcs (necesario acceder a la carpeta para su ejecucion )
    - dotnet run -- [comando] ["argumentos"]

### Ejemplo en C# (asegurate de estar dentro del programa taskcs)
    - dotnet run -- add "prueba desde c#"


