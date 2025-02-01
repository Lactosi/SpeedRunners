using System;
public class Program
{
    public static void Main()
    {

        while (true)
        {
           string audioFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio");
        string[] audioFilePaths = {
            Path.Combine(audioFolderPath, "pixel-dreams.mp3")
        };
        BackgroundMusic backgroundMusic = new BackgroundMusic(audioFilePaths);

        backgroundMusic.Play(0);

            Console.WriteLine("Bienvenido a SpeedRunners");
            Console.WriteLine("Elija la opcion que desee:");
            Console.WriteLine("(1) Un Jugador");
            Console.WriteLine("(2) Multijugador");
            Console.WriteLine("(3) Salir");

            string? opcion = Console.ReadLine();
            Console.Clear();

            if (opcion == "1")
            {
                int player;
                string sprite = "";

                //Elegir Personaje
                while (true)
                {
                    Console.WriteLine("Elija su personaje:");
                    PrintCharacters();
                    Console.WriteLine("Introduzca H para ver las habilidades de cada personaje");
                    Console.WriteLine();

                    string? op = Console.ReadLine();
                    bool is_int = int.TryParse(op, out player);

                    if (op == "H" || op == "h")
                    {
                        while (true)
                        {
                            Console.Clear();
                            PrintSkills();
                            Console.WriteLine("Presione X para salir");
                            var a = Console.ReadKey();
                            if (a.Key == ConsoleKey.X)
                            {
                                Console.Clear();
                                break;
                            }

                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Opcion no valida.");
                                Console.ReadKey();
                            }
                        }
                    }

                    else if (!is_int || String.IsNullOrEmpty(op) || player < 1 || player > 5)
                    {
                        Console.WriteLine("Opcion no valida.");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else
                    {
                        player = int.Parse(op);
                        Console.Clear();
                        break;
                    }
                }

                if (player == 1) sprite = "🧌";
                if (player == 2) sprite = "🧟";
                if (player == 3) sprite = "🧛";
                if (player == 4) sprite = "🧝";
                if (player == 5) sprite = "🧙";


                //Ingresar dimensiones del laberinto
                int n = 0, m = 0, t = 0;

                while (true)
                {
                    Console.WriteLine("Elija el tamaño del laberinto:");
                    Console.WriteLine("(1) Pequeño");
                    Console.WriteLine("(2) Mediano");
                    Console.WriteLine("(3) Grande");
                    string? a = Console.ReadLine();

                    bool is_int = int.TryParse(a, out t);
                    if (!is_int || String.IsNullOrEmpty(a) || t <= 0 || t > 3)
                    {
                        Console.WriteLine("Tamaño no valido. Ingrese el tamaño del laberinto nuevamente.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        break;
                    }
                }

                Console.Clear();
                if (t == 1) { n = 10; m = 10; }
                if (t == 2) { n = 20; m = 20; }
                if (t == 3) { n = 30; m = 30; }

                backgroundMusic.Stop();

                 string audioFolderPath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio");
        string[] audioFilePaths2 = {
            Path.Combine(audioFolderPath2, "gaming-music-8-bit-console.mp3")
        };
        BackgroundMusic backgroundMusic2 = new BackgroundMusic(audioFilePaths2);

        backgroundMusic2.Play(0);

                
                Grid b = new Grid(n, m, sprite, "⬛", false, t);
                break;
            }

            //Multiplayer

            else if (opcion == "2")
            {

                int[] player = new int[3];
                string[] sprite = new string[3];

                for (int i = 1; i <= 2; i++)
                {
                    //Elegir Personaje
                    while (true)
                    {
                        Console.WriteLine($"Elija el personaje del jugador {i}:");
                        PrintCharacters();

                        Console.WriteLine("Introduzca H para ver las habilidades de cada personaje");
                        Console.WriteLine();

                        string? op = Console.ReadLine();
                        bool is_int = int.TryParse(op, out player[i]);

                        if (op == "H" || op == "h")
                        {
                            while (true)
                            {
                                Console.Clear();
                                PrintSkills();
                                Console.WriteLine("Presione X para salir");
                                var a = Console.ReadKey();
                                if (a.Key == ConsoleKey.X)
                                {
                                    Console.Clear();
                                    break;
                                }

                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Opcion no valida.");
                                    Console.ReadKey();
                                }
                            }
                        }

                        else if (!is_int || String.IsNullOrEmpty(op) || player[i] < 1 || player[i] > 5)
                        {
                            Console.WriteLine("Opcion no valida.");
                            Console.ReadKey();
                            Console.Clear();
                        }

                        else
                        {
                            player[i] = int.Parse(op);
                            Console.Clear();
                            break;
                        }
                    }


                    if (player[i] == 1) sprite[i] = "🧌";
                    if (player[i] == 2) sprite[i] = "🧟";
                    if (player[i] == 3) sprite[i] = "🧛";
                    if (player[i] == 4) sprite[i] = "🧝";
                    if (player[i] == 5) sprite[i] = "🧙";
                }


                //Ingresar dimensiones del laberinto
                int n = 0, m = 0, t = 0;

                while (true)
                {
                    Console.WriteLine("Elija el tamaño del laberinto:");
                    Console.WriteLine("(1) Pequeño");
                    Console.WriteLine("(2) Mediano");
                    Console.WriteLine("(3) Grande");
                    string? a = Console.ReadLine();

                    bool is_int = int.TryParse(a, out t);
                    if (!is_int || String.IsNullOrEmpty(a) || t <= 0 || t > 3)
                    {
                        Console.WriteLine("Tamaño no valido. Ingrese el tamaño del laberinto nuevamente.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        break;
                    }
                }

                Console.Clear();
                if (t == 1) { n = 10; m = 10; }
                if (t == 2) { n = 20; m = 20; }
                if (t == 3) { n = 35; m = 35; }

                backgroundMusic.Stop();

                 string audioFolderPath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio");
        string[] audioFilePaths2 = {
            Path.Combine(audioFolderPath2, "gaming-music-8-bit-console.mp3")
        };
        BackgroundMusic backgroundMusic2 = new BackgroundMusic(audioFilePaths2);

        backgroundMusic2.Play(0);
                Grid b = new Grid(n, m, sprite[1], sprite[2], true, t);
                break;
            }

            //Salir
            else if (opcion == "3")
            {
                Console.Clear();
                Environment.Exit(0);
                break;
            }

            //Entrada no valida
            else
            {
                Console.WriteLine("Opcion no valida.");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    public static void PrintCharacters()
    {
        Console.WriteLine("(1) 🧌  Ogro:");
        Console.WriteLine("❤️ ​ Vida - 100​");
        Console.WriteLine("⚡ ​Energia - 80");
        Console.WriteLine("🦶 Pasos por turno: 1");
        Console.WriteLine();

        Console.WriteLine("(2) 🧟 Zombie:");
        Console.WriteLine("❤️ ​ Vida - 80​");
        Console.WriteLine("⚡ ​Energia - 50");
        Console.WriteLine("🦶 Pasos por turno: 2");
        Console.WriteLine();

        Console.WriteLine("(3) 🧛 Vampiro:");
        Console.WriteLine("❤️ ​ Vida - 70​");
        Console.WriteLine("⚡​ Energia - 70");
        Console.WriteLine("🦶 Pasos por turno: 2");
        Console.WriteLine();

        Console.WriteLine("(4) 🧝​ Elfo:");
        Console.WriteLine("❤️​  Vida - 70​");
        Console.WriteLine("⚡​ Energia - 60");
        Console.WriteLine("🦶 Pasos por turno: 3");
        Console.WriteLine();

        Console.WriteLine("(5) 🧙 Hechicero:");
        Console.WriteLine("❤️​  Vida - 50​");
        Console.WriteLine("⚡​ Energia - 90");
        Console.WriteLine("🦶 Pasos por turno: 3");
        Console.WriteLine();
    }

    public static void PrintSkills()
    {
        Console.WriteLine("Ogro 🧌  - Habilidad: Golpe Demoledor");
        Console.WriteLine("El jugador destruye el objeto que haya en la proxima casilla donde se mueva, aunque sea una pared.");
        Console.WriteLine("Tiempo de enfriamiento: 2 turnos");
        Console.WriteLine();

        Console.WriteLine("Zombie 🧟 - Habilidad: Mordida de Zombie");
        Console.WriteLine("Si el jugador se mueve en el proximo turno a una casilla donde este otro jugador, este es mordido y asesinado, haciendo que regrese a su casilla inicial");
        Console.WriteLine("Tiempo de enfriamiento: 8 turnos");
        Console.WriteLine();

        Console.WriteLine("Vampiro 🧛 - Habilidad: Absorcion de Sangre");
        Console.WriteLine("El jugador chupa la sangre del rival, quitandole 10 de vida y curandose 10");
        Console.WriteLine("Tiempo de enfriamiento: 3 turnos");
        Console.WriteLine();

        Console.WriteLine("Elfo 🧝 - Habilidad: Ocarina del tiempo");
        Console.WriteLine("El jugador se teletransporta a un arbol aleatorio, sin recibir daño.");
        Console.WriteLine("Tiempo de enfriamiento: 2 turnos");
        Console.WriteLine();

        Console.WriteLine("Hechicero 🧙 - Habilidad: Rayo paralizador");
        Console.WriteLine("Se salta el turno del proximo jugador.");
        Console.WriteLine("Tiempo de enfriamiento: 4 turnos");
        Console.WriteLine();

    }
}