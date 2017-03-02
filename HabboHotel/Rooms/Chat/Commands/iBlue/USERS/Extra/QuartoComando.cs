
using System.Text;
using Rocket.Communication.Packets.Outgoing.Rooms.Engine;

using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class QuartoComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "push/pull/enables/respect"; }
        }

        public string Description
        {
            get { return "Dá-lhe a capacidade de habilitar ou desabilitar comandos básicos do quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Opa, você deve escolher uma opção de sala para desativar.");
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Opa, apenas o proprietário do quarto ou Staff pode usar este comando.");
                return;
            }

            string Option = Params[1];
            switch (Option)
            {
                case "list":
                    {
                        StringBuilder List = new StringBuilder("");
                        List.AppendLine("Room Command List");
                        List.AppendLine("-------------------------");
                        List.AppendLine("Pet Morphs: " + (Room.PetMorphsAllowed == true ? "enabled" : "disabled"));
                        List.AppendLine("Pull: " + (Room.PullEnabled == true ? "enabled" : "disabled"));
                        List.AppendLine("Push: " + (Room.PushEnabled == true ? "enabled" : "disabled"));
                        List.AppendLine("Super Pull: " + (Room.SPullEnabled == true ? "enabled" : "disabled"));
                        List.AppendLine("Super Push: " + (Room.SPushEnabled == true ? "enabled" : "disabled"));
                        List.AppendLine("Respect: " + (Room.RespectNotificationsEnabled == true ? "enabled" : "disabled"));
                        List.AppendLine("Enables: " + (Room.EnablesEnabled == true ? "enabled" : "disabled"));
                        Session.SendNotification(List.ToString());
                        break;
                    }

                case "push":
                    {
                        Room.PushEnabled = !Room.PushEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `push_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", RocketEmulador.BoolToEnum(Room.PushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo de envio é agora " + (Room.PushEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "spush":
                    {
                        Room.SPushEnabled = !Room.SPushEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spush_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", RocketEmulador.BoolToEnum(Room.SPushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Super modo Push é agora" + (Room.SPushEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "spull":
                    {
                        Room.SPullEnabled = !Room.SPullEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", RocketEmulador.BoolToEnum(Room.SPullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Super modo de pull é agora" + (Room.SPullEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "pull":
                    {
                        Room.PullEnabled = !Room.PullEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", RocketEmulador.BoolToEnum(Room.PullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo de pull é agora " + (Room.PullEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "enable":
                case "enables":
                    {
                        Room.EnablesEnabled = !Room.EnablesEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `enables_enabled` = @EnablesEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("EnablesEnabled", RocketEmulador.BoolToEnum(Room.EnablesEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Enables agora é " + (Room.EnablesEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "respect":
                    {
                        Room.RespectNotificationsEnabled = !Room.RespectNotificationsEnabled;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `respect_notifications_enabled` = @RespectNotificationsEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("RespectNotificationsEnabled", RocketEmulador.BoolToEnum(Room.RespectNotificationsEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo de notificações de respeito definido como " + (Room.RespectNotificationsEnabled == true ? "enabled!" : "disabled!"));
                        break;
                    }

                case "pets":
                case "morphs":
                    {
                        Room.PetMorphsAllowed = !Room.PetMorphsAllowed;
                        using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pet_morphs_allowed` = @PetMorphsAllowed WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PetMorphsAllowed", RocketEmulador.BoolToEnum(Room.PetMorphsAllowed));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Humanos de estimação morfos notificações modo definido como " + (Room.PetMorphsAllowed == true ? "enabled!" : "disabled!"));

                        if (!Room.PetMorphsAllowed)
                        {
                            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
                            {
                                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                                    continue;

                                User.GetClient().SendWhisper("O dono do quarto desativou a capacidade de usar um animal de estimação nesta sala.");
                                if (User.GetClient().GetHabbo().PetId > 0)
                                {
                                    //Tell the user what is going on.
                                    User.GetClient().SendWhisper("Opa, o proprietário do quarto só tiver desabilitado do animal de estimação-morfos, un-morphing você.");

                                    //Change the users Pet Id.
                                    User.GetClient().GetHabbo().PetId = 0;

                                    //Quickly remove the old user instance.
                                    Room.SendMessage(new UserRemoveComposer(User.VirtualId));

                                    //Add the new one, they won't even notice a thing!!11 8-)
                                    Room.SendMessage(new UsersComposer(User));
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}
