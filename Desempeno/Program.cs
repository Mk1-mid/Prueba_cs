static void Main(string[] args)
{
    string opcion;
    do
    {
        Console.Clear();
        Console.WriteLine("--- SISTEMA DE RESERVAS DEPORTIVAS ---");
        Console.WriteLine("1. Gestión de Usuarios");
        Console.WriteLine("2. Gestión de Espacios Deportivos");
        Console.WriteLine("3. Gestión de Reservas");
        Console.WriteLine("4. Salir");
        Console.Write("Seleccione una opción: ");
        opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1": MenuUsuarios(); break;
            case "2": MenuEspacios(); break;
            case "3": MenuReservas(); break;
            case "4": Console.WriteLine("Saliendo..."); break;
            default: Console.WriteLine("Opción inválida."); break;
        }
    } while (opcion != "4");
}

static void MenuUsuarios()
{
    string subOpcion;
    do
    {
        Console.Clear();
        Console.WriteLine("--- GESTIÓN DE USUARIOS ---");
        Console.WriteLine("1. Registrar nuevo usuario");
        Console.WriteLine("2. Editar usuario");
        Console.WriteLine("3. Listar todos los usuarios");
        Console.WriteLine("4. Volver al menú principal");
        subOpcion = Console.ReadLine();

        switch (subOpcion)
        {
            case "1":
                // Aquí llamarías a tu lógica de registrar (con try-catch)
                break;
            case "2":
                // Aquí llamarías a tu lógica de editar
                break;
            case "3":
                // Aquí usarías LINQ para listar
                break;
        }
    } while (subOpcion != "4");
}

public static void MenuReservas()
{
    string subOpcion;
    do
    {
        Console.Clear();
        Console.WriteLine("--- GESTIÓN DE Reservas ---");
        Console.WriteLine("1. crear Reserva");
        Console.WriteLine("2. Listar reservas por usuario");
        Console.WriteLine("3. Listar reservas por espacios");
        Console.WriteLine("4. cancelar reserva");
        Console.WriteLine("5. volver");
        subOpcion = Console.ReadLine();
        switch (subOpcion)
        {
            case "1":
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
        }
    } while (subOpcion != "5");
}

public static void Menuespacione()
    {
        string subOpcion;
        do
        {
            Console.Clear();
            Console.WriteLine("--- GESTIÓN DE Espacio ---");
            Console.WriteLine("1. crear Espacio");
            Console.WriteLine("2. editar Espacio");
            Console.WriteLine("3. Listar reservas por espacios");
            Console.WriteLine("4. cancelar reserva");
            Console.WriteLine("5. volver");
            subOpcion = Console.ReadLine();
            switch (subOpcion)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;

            }
        } while (subOpcion != "5");
    }
