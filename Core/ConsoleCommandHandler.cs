using System;
using log4net;
using Rocket.HabboHotel;

using Rocket.Communication.Packets.Outgoing.Moderation;

namespace Rocket.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Rocket.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region stop
                    case "stop":
                    case "shutdown":
                    case "desligar":
                        {
                            Logging.DisablePrimaryWriting(true);
                            Logging.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!", ConsoleColor.Yellow);
                            RocketEmulador.PerformShutDown();
                            break;
                        }
                    #endregion

                    #region alert
                    case "alertar":
                        {
                            string Notice = inputData.Substring(6);

                            RocketEmulador.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(RocketEmulador.GetGame().GetLanguageLocale().TryGetValue("console.noticefromadmin") + "\n\n" + Notice));

                            log.Info("Alertar eviado com sucesso");
                            break;
                        }
                    #endregion

                    default:
                        {
                            log.Error(parameters[0].ToLower() + " is an unknown or unsupported command. Type help for more information");
                            break;
                        }
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error in command [" + inputData + "]: " + e);
            }
        }
    }
}