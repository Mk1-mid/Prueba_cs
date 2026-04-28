using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Desempeno.Data;
using Desempeno.services;
using Desempeno.models;
using Desempeno.repositories;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("No se encontró ConnectionStrings:DefaultConnection en appsettings.json.");

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ISportSpaceRepository, SportSpaceRepository>();
        serviceCollection.AddScoped<IReservationRepository, ReservationRepository>();
        serviceCollection.AddScoped<UserServices>();
        serviceCollection.AddScoped<SportSpaceServices>();
        serviceCollection.AddScoped<ReservationServices>();

        using var serviceProvider = serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var userServ = scope.ServiceProvider.GetRequiredService<UserServices>();
        var spaceServ = scope.ServiceProvider.GetRequiredService<SportSpaceServices>();
        var reserveServ = scope.ServiceProvider.GetRequiredService<ReservationServices>();

        string? opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("SISTEMA DE RESERVAS DEPORTIVAS");
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
            Console.WriteLine("GESTIÓN DE USUARIOS");
            Console.WriteLine("1. Registrar nuevo usuario");
            Console.WriteLine("2. Editar usuario");
            Console.WriteLine("3. Listar todos los usuarios");
            Console.WriteLine("4. Eliminar usuario");
            Console.WriteLine("5. Volver al menú principal");
            Console.Write("Seleccione una opción: ");
            subOpcion = Console.ReadLine();

            switch (subOpcion)
            {
                case "1":
                    RegistrarUsuario(userServ);
                    break;
                case "2":
                    EditarUsuario(userServ);
                    break;
                case "3":
                    ListarUsuarios(userServ);
                    break;
                case "4":
                    EliminarUsuario(userServ);
                    break;
                case "5":
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Pausa();
                    break;
            }
        } while (subOpcion != "5");
    }

    public static void MenuReservas(ReservationServices reserveServ)
    {
        string? subOpcion;
        do
        {
            Console.Clear();
            Console.WriteLine("GESTIÓN DE RESERVAS");
            Console.WriteLine("1. crear Reserva");
            Console.WriteLine("2. Listar reservas");
            Console.WriteLine("3. Editar reserva");
            Console.WriteLine("4. cancelar reserva");
            Console.WriteLine("5. volver");
            Console.Write("Seleccione una opción: ");
            subOpcion = Console.ReadLine();
            switch (subOpcion)
            {
                case "1":
                    RegistrarReserva(reserveServ);
                    break;
                case "2":
                    ListarReservas(reserveServ);
                    break;
                case "3":
                    EditarReserva(reserveServ);
                    break;
                case "4":
                    CancelarReserva(reserveServ);
                    break;
                case "5":
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Pausa();
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
            Console.WriteLine("GESTIÓN DE ESPACIOS");
            Console.WriteLine("1. crear Espacio");
            Console.WriteLine("2. editar Espacio");
            Console.WriteLine("3. Listar espacios");
            Console.WriteLine("4. Eliminar espacio");
            Console.WriteLine("5. volver");
            Console.Write("Seleccione una opción: ");
            subOpcion = Console.ReadLine();
            switch (subOpcion)
            {
                case "1":
                    RegistrarEspacio(spaceServ);
                    break;
                case "2":
                    EditarEspacio(spaceServ);
                    break;
                case "3":
                    ListarEspacios(spaceServ);
                    break;
                case "4":
                    EliminarEspacio(spaceServ);
                    break;
                case "5":
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Pausa();
                    break;
            }
        } while (subOpcion != "5");
    }

    static void RegistrarUsuario(UserServices userServ)
    {
        var user = new Users
        {
            Id = LeerEntero("Id: "),
            Name = LeerTexto("Nombre: "),
            Email = LeerTexto("Email: "),
            Phone = LeerEntero("Teléfono: ")
        };

        var response = userServ.RegistrarUsuario(user);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void EditarUsuario(UserServices userServ)
    {
        int id = LeerEntero("Id del usuario a editar: ");
        var user = new Users
        {
            Name = LeerTexto("Nuevo nombre: "),
            Email = LeerTexto("Nuevo email: "),
            Phone = LeerEntero("Nuevo teléfono: ")
        };

        var response = userServ.EditarUsuario(id, user);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void ListarUsuarios(UserServices userServ)
    {
        var response = userServ.ListarUsuarios();
        if (!response.Success || response.Data == null || response.Data.Count == 0)
        {
            Console.WriteLine(response.Message ?? "No hay usuarios registrados.");
            Pausa();
            return;
        }

        foreach (var u in response.Data)
        {
            Console.WriteLine($"Id: {u.Id} | Nombre: {u.Name} | Email: {u.Email} | Tel: {u.Phone}");
        }
        Pausa();
    }

    static void EliminarUsuario(UserServices userServ)
    {
        int id = LeerEntero("Id del usuario a eliminar: ");
        var response = userServ.EliminarUsuario(id);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void RegistrarEspacio(SportSpaceServices spaceServ)
    {
        var space = new SportSpace
        {
            Id = LeerEntero("Id: "),
            Name = LeerTexto("Nombre: "),
            Tipe = LeerTexto("Tipo de espacio: "),
            capacity = LeerEntero("Capacidad: ")
        };

        var response = spaceServ.RegistrarEspacio(space);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void EditarEspacio(SportSpaceServices spaceServ)
    {
        int id = LeerEntero("Id del espacio a editar: ");
        var space = new SportSpace
        {
            Name = LeerTexto("Nuevo nombre: "),
            Tipe = LeerTexto("Nuevo tipo: "),
            capacity = LeerEntero("Nueva capacidad: ")
        };

        var response = spaceServ.EditarEspacio(id, space);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void ListarEspacios(SportSpaceServices spaceServ)
    {
        var response = spaceServ.ListarEspacios();
        if (!response.Success || response.Data == null || response.Data.Count == 0)
        {
            Console.WriteLine(response.Message ?? "No hay espacios registrados.");
            Pausa();
            return;
        }

        foreach (var s in response.Data)
        {
            Console.WriteLine($"Id: {s.Id} | Nombre: {s.Name} | Tipo: {s.Tipe} | Capacidad: {s.capacity}");
        }
        Pausa();
    }

    static void EliminarEspacio(SportSpaceServices spaceServ)
    {
        int id = LeerEntero("Id del espacio a eliminar: ");
        var response = spaceServ.EliminarEspacio(id);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void RegistrarReserva(ReservationServices reserveServ)
    {
        var reserv = new Reserv
        {
            IdUser = LeerEntero("Id usuario: "),
            IdSpace = LeerEntero("Id espacio: "),
            Date = LeerFecha("Fecha (yyyy-MM-dd): "),
            strat = LeerHora("Hora inicio (HH:mm): "),
            end = LeerHora("Hora fin (HH:mm): ")
        };

        var response = reserveServ.RegistrarReserva(reserv);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void ListarReservas(ReservationServices reserveServ)
    {
        var response = reserveServ.ListarReservas();
        if (!response.Success || response.Data == null || response.Data.Count == 0)
        {
            Console.WriteLine(response.Message ?? "No hay reservas registradas.");
            Pausa();
            return;
        }

        foreach (var r in response.Data)
        {
            Console.WriteLine(
                $"Id: {r.Id} | Usuario: {r.IdUser} | Espacio: {r.IdSpace} | Fecha: {r.Date:yyyy-MM-dd} | {r.strat:hh\\:mm}-{r.end:hh\\:mm} | Estado: {r.status}");
        }
        Pausa();
    }

    static void EditarReserva(ReservationServices reserveServ)
    {
        int id = LeerEntero("Id de la reserva a editar: ");
        var reserv = new Reserv
        {
            IdUser = LeerEntero("Nuevo Id usuario: "),
            IdSpace = LeerEntero("Nuevo Id espacio: "),
            status = LeerTexto("Nuevo estado: "),
            Date = LeerFecha("Nueva fecha (yyyy-MM-dd): "),
            strat = LeerHora("Nueva hora inicio (HH:mm): "),
            end = LeerHora("Nueva hora fin (HH:mm): ")
        };

        var response = reserveServ.EditarReserva(id, reserv);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static void CancelarReserva(ReservationServices reserveServ)
    {
        int id = LeerEntero("Id de la reserva a cancelar: ");
        var response = reserveServ.CancelarReserva(id);
        Console.WriteLine(response.Message);
        Pausa();
    }

    static int LeerEntero(string mensaje)
    {
        int value;
        Console.Write(mensaje);
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Valor inválido. Intente de nuevo: ");
        }

        return value;
    }

    static string LeerTexto(string mensaje)
    {
        Console.Write(mensaje);
        string? value = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(value))
        {
            Console.Write("Valor requerido. Intente de nuevo: ");
            value = Console.ReadLine();
        }

        return value.Trim();
    }

    static DateTime LeerFecha(string mensaje)
    {
        DateTime value;
        Console.Write(mensaje);
        while (!DateTime.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Fecha inválida. Intente de nuevo: ");
        }

        return value.Date;
    }

    static TimeSpan LeerHora(string mensaje)
    {
        TimeSpan value;
        Console.Write(mensaje);
        while (!TimeSpan.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Hora inválida. Intente de nuevo: ");
        }

        return value;
    }

    static void Pausa()
    {
        Console.WriteLine();
        Console.Write("Presione Enter para continuar...");
        Console.ReadLine();
    }
}
