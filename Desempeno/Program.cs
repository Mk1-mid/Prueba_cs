using Microsoft.EntityFrameworkCore;
using Desempeno.Data;
using Desempeno.services;
public class Program
{
    public static void Main(string[] args)
    {
        const string connectionString = "server=localhost;database=desempeno;user=root;password=;";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

        using var context = new AppDbContext(options);
        var userServ = new UserServices(context);
        var spaceServ = new SportSpaceServices(context);
        var reserveServ = new ReservationServices(context);

        string? opcion;
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
                case "1": MenuUsuarios(userServ); break;
                case "2": MenuEspacios(spaceServ); break;
                case "3": MenuReservas(reserveServ); break;
                case "4": Console.WriteLine("Saliendo..."); break;
                default: Console.WriteLine("Opción inválida."); break;
            }
        } while (opcion != "4");
    }

    static void MenuUsuarios(UserServices userServ)
    {
        string? subOpcion;
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
                    break;
                case "2":
                    break;
                case "3":
                    break;
            }
        } while (subOpcion != "4");
    }

    public static void MenuReservas(ReservationServices reserveServ)
    {
        string? subOpcion;
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

    public static void MenuEspacios(SportSpaceServices spaceServ)
    {
        string? subOpcion;
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
}
