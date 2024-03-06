using AGENDA;
const string FICHERO_DATOS = "datos_agenda.txt";
List<string[]> agenda = new();
int opcion;

if (!File.Exists(FICHERO_DATOS)) File.Create(FICHERO_DATOS).Close();

do
{
    Console.Clear();//Limpiar la pantalla cada vez que se muestre el menú
    MostrarMenu();
    Console.WriteLine();
    Console.Write("Dime la opción elegida: ");
    opcion = int.Parse(Console.ReadLine());//Convierte a entero la opción elegida y entra en el switch

    Console.Clear();
    switch (opcion)
    {
        case 0:
            Contacto.Despedida();
            break;
        case 1:
            Contacto.Lista(FICHERO_DATOS);
            break;
        case 2:
            Contacto.Crear(FICHERO_DATOS);
            break;
        case 3:
            Contacto.Borrar(FICHERO_DATOS);
            break;
        case 4:
            Contacto.Buscar(FICHERO_DATOS);
            break;
        case 5:
            Contacto.Modificar(FICHERO_DATOS);
            break;
        case 6:
            Contacto.BorrarTodos(FICHERO_DATOS);
            break;
        default:
            Console.WriteLine("Opción no disponible"); //Si el usuario elige una opción que no está en el menú, se muestra este mensaje
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Pulse cualquier tecla para seguir"); //Hace una pausa para que el usuario pueda leer la información
    Console.ReadKey(); // al pulsar cualquier tecla se vuelve al inicio del bucle o sale si se seleccionó 0

} while (opcion != 0);


void MostrarMenu()
{
    Console.WriteLine("   ------------- AGENDA -------------");
    Console.WriteLine();
    Console.WriteLine("\t1. Mostrar contactos");
    Console.WriteLine();
    Console.WriteLine("\t2. Crear un contacto");
    Console.WriteLine();
    Console.WriteLine("\t3. Borrar un contacto");
    Console.WriteLine();
    Console.WriteLine("\t4. Buscar contactos");
    Console.WriteLine();
    Console.WriteLine("\t5. Modificar un contacto");
    Console.WriteLine();
    Console.WriteLine("\t6. Borrar todos los contactos");
    Console.WriteLine();

    Console.WriteLine("\t0. Salir");
    Console.WriteLine();
}
