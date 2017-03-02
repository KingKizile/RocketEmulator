
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class LimparInventario : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%sim%"; }
        }

        public string Description
        {
            get { return "Seu inventário está cheio? Limpe com este comando.."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendNotification("Tem certeza de que deseja limpar seu inventário? Você vai perder toda a mobília! \n " +
"Para confirmar, digite \":limpar sim\". \n\nOnce você fazer isso, não há volta! \n (se você não quiser esvaziá-la, apenas ignore esta mensagem!) \n\n" +
"POR FAVOR, NOTE! Se você tem mais de 3.000 itens, os itens ocultos também serão excluídos.");
                return;
            }
            else
            {
                if (Params.Length == 2 && Params[1].ToString() == "sim")
                {
                    Session.GetHabbo().GetInventoryComponent().ClearItems();
                    Session.SendNotification("Seu inventário foi limpado!");
                    return;
                }
                else if (Params.Length == 2 && Params[1].ToString() != "sim")
                {
                    Session.SendNotification("Para confirmar, você deve digitar: limpar sim");
                    return;
                }
            }
        }
    }
}
