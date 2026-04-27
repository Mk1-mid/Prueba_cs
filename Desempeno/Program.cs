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