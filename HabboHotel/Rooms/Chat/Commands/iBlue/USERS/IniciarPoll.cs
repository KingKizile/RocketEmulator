using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Polls.Quick;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    internal class IniciarPoll : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "comando_vip";
            }
        }

        public string Parameters
        {
            get
            {
                return "[questao]";
            }
        }

        public string Description
        {
            get
            {
                return "Inicie um poll!";
            }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            if (Room.CheckRights(Session, false, true))
            {
                if (Params.Length < 2)
                {
                    Session.SendWhisper("Use corretamente o comando.", 0);
                }
                else
                {
                    string questao = CommandManager.MergeParams(Params, 1);
                    int duracao = 60;



                    QuickPoll quickPoll = Room.GenerateQPoll(duracao, questao);
                    Room.QuickPoll.Start();
                }
            }
        }
    }
}