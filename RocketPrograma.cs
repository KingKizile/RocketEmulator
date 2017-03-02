using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Rocket.Core;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;

namespace Rocket
{
    public class Programa
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        private static readonly ILog log = LogManager.GetLogger("Programa");

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

      

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]


        public static void Main(string[] Args)

        {
            /*// #############################
            // ##  Licença Rocket System  ##
            // #############################
            
            Console.ForegroundColor = ConsoleColor.White;
            bool podeIniciar = false;
            string webData = "";
            WebClient wc = new WebClient();
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                webData = wc.DownloadString("http://www.RocketEmulador.ml/licençaRocket.txt");
            }
            catch (ArgumentNullException e)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Erro #1: " + e.Message);
                Console.ReadKey();
                Environment.Exit(1);


            }
            catch (WebException e)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Erro #2: " + e.Message);
                Console.ReadKey();
                Environment.Exit(1);

            }
            catch (NotSupportedException e)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Erro #3: " + e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }

            if (
                   File.ReadAllText(Environment.SystemDirectory + "\\drivers\\etc\\hosts")
                       .ToLower()
                       .Contains("RocketEmulador.ml"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("       [CARREGANDO] => [Rocket Licença] =>  O seu arquivo 'host' já tem 'RocketEmulador.ml' remova-o.");
                Console.ReadKey();
                Environment.Exit(1);
            }
            else
            {

                var macs = webData.Split(';');
                var macAdrress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                                  where nic.OperationalStatus == OperationalStatus.Up
                                  select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("       [CARREGANDO] => [Rocket Licença] =>  Sua licença foi aceita, aproveite o emulador.");
                Console.WriteLine("       [CARREGANDO] => [Rocket Licença] =>  Lembre-se de pagar sua licença em dia, para evitar futuros problemas.");
                foreach (var macAtual in macs)

                {
                    if (macAtual == macAdrress.ToString())
                        podeIniciar = true;

                }

                if (podeIniciar == false)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("       [CARREGANDO] => [Rocket Licença] =>  Não conseguimos encontrar sua licença, entre em contato por favor.");
                    Console.WriteLine("       [CARREGANDO] => [Rocket Licença] =>  Possivelmente sua licença está atrasada, por favor pague sua licença.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            } 
            
        // #############################
        // ##  Licença Rocket System  ##
        // #############################*/


        Console.ForegroundColor = ConsoleColor.White;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            XmlConfigurator.Configure();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.CursorVisible = false;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            Console.ForegroundColor = ConsoleColor.White;
            RocketEmulador.Initialize();

            while (true)
            {
                Console.CursorVisible = true;
                if (Logging.DisabledState)
                    Console.Write("Rocket> ");

                ConsoleCommandHandler.InvokeCommand(Console.ReadLine());
                continue;
            }
        }



        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            var e = (Exception)args.ExceptionObject;
            Logging.LogCriticalException("ERRO NO SISTEMA: " + e);
            RocketEmulador.PerformShutDown();
        }

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);

    }
}