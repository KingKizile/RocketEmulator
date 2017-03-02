
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    using Rocket;
    using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
    using Database.Interfaces;
    using Rocket.HabboHotel.GameClients;
    using System;
    using System.Diagnostics;
    using System.Data;
    internal class InfoComando : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            PerformanceCounter ramCounter;
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            DataRow UserData = null;
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `userpeak` FROM `server_status` LIMIT 1");
                UserData = dbClient.getRow();
            }
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            string cpu = cpuCounter.NextValue() + "%";
            string ram = ramCounter.NextValue() + "MB";
            TimeSpan span = (TimeSpan)(DateTime.Now - RocketEmulador.ServerStarted);
            int count = RocketEmulador.GetGame().GetClientManager().Count;
            int num2 = RocketEmulador.GetGame().GetRoomManager().Count;
            string hotelName = RocketEmulador.RocketData().data["hotelname"];
            string hotelURL = RocketEmulador.RocketData().data["hotellink"];
            string Descri = RocketEmulador.RocketData().data["descrição.emulador"];
            string Ultima = RocketEmulador.RocketData().data["última.atualização"];
            object[] objArray1 = new object[] { "<font size='20' color='#0073ad'><b>Rocket Emulador</b></font>\n", "<font size='11' color='#00ad79'><b>", Descri, " </b></font>\n", "\n <font size='13' color='#ad0025'><b>Créditos:</b></font>" + "<font size='11' color='#646161'><b>" + "\nVitorNobre\n100Bug\nAlb1no\nRocketDev\n", hotelName, " - Por estar usando o Rocket\n\n" + " </b></font>\n"+ "<font size='13' color='#5600ad'><b>Informações</b></font>:" +  "<font size='11' color='#646161'><b>" + "\nQuartos Carregados: ", num2, "\nUsuários Onlines: ", count, "\nMemória usada: " + ram + "\nCPU usada: " + cpu + "\nLigado \x00e0: ", span.Days, " dia(s) ", span.Hours, " Horas  ", span.Minutes, " minutos.\n\n " + "</b></font>" + "<font size='10' color='#22a3ad'><b> Recorde de usuários: " + Convert.ToString(UserData["userpeak"]) + "</b></font>\n" + "<font size='10' color='#FFA500'><b> Última atualização: ", Ultima, "</b></font>\n" };
            Session.SendMessage(new RoomNotificationComposer("Rocket Emulador - Versão 2.0", string.Concat(objArray1), "Rocket", hotelName, hotelURL));
            cpuCounter.Dispose();
            ramCounter.Dispose();
        }


        public string Description =>
            "Mostra as informações do servidor";

        public string Parameters =>
            "";

        public string PermissionRequired =>
            "comando_users";
    }
}

