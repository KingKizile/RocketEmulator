using System.Linq;
using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.Core;
using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class AtualizarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_adm"; }
        }

        public string Parameters
        {
            get { return "%variable%"; }
        }

        public string Description
        {
            get { return "Reloga uma parte do hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor digite algo.. :atualizar navegador :atualizar catalago",34);
                return;
            }

            string UpdateVariable = Params[1];
            switch (UpdateVariable.ToLower())
            {
                case "catalago":
                case "catalog":
                case "catalogue":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem permissão para atualizar isso.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetCatalog().Init(RocketEmulador.GetGame().GetItemManager());
                        RocketEmulador.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                        Session.SendWhisper("Catalago atualizado com sucesso",34);
                        break;
                    }

                case "items":
                case "itens":
                case "furni":
                case "furniture":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_furni' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetItemManager().Init();
                        Session.SendWhisper("Items atualizado com sucesso.",34);
                        break;
                    }

                case "models":
                case "modelos":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_models' permission.");
                            break;
                        }

                        RocketEmulador.GetGame().GetRoomManager().LoadModels();
                        Session.SendWhisper("Modelos de quartos atualizado com sucesso .",34);
                        break;
                    }

                case "promotions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'comando_adm' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetLandingManager().LoadPromotions();
                        Session.SendWhisper("Landing view promotions atualizado com sucesso.",34);
                        break;
                    }

                case "youtube":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_youtube' permissão.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetTelevisionManager().Init();
                        Session.SendWhisper("Youtube televisions playlist atualizado com sucesso.",34);
                        break;
                    }

                case "filter":
                case "filtro":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_filter' permissão.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetChatManager().GetFilter().Init();
                        Session.SendWhisper("Filter definitions atualizado com sucesso.",34);
                        break;
                    }

                case "navigator":
                case "navegador":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'comando_adm' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetNavigator().Init();
                        Session.SendWhisper("Navigator items atualizado com sucesso.",34);
                        break;
                    }

                case "ranks":
                case "rights":
                case "permissions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_rights' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetPermissionManager().Init();

                        foreach (GameClient Client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                        {
                            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                                continue;

                            Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                        }

                        Session.SendWhisper("Rank definitions atualizado com sucesso.",34);
                        break;
                    }

                case "config":
                case "settings":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_configuration' permission.",34);
                            break;
                        }

                        RocketEmulador.ConfigData = new ConfigData();
                        Session.SendWhisper("Server configuration atualizado com sucesso.",34);
                        break;
                    }

                case "bans":
                case "banidos":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_bans' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetModerationManager().ReCacheBans();
                        Session.SendWhisper("Ban cache re-loaded.",34);
                        break;
                    }

                case "quests":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_quests' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetQuestManager().Init();
                        Session.SendWhisper("Quest definitions atualizado com sucesso.",34);
                        break;
                    }

                case "achievements":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_achievements' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetAchievementManager().LoadAchievements();
                        Session.SendWhisper("Achievement definitions bans atualizado com sucesso.",34);
                        break;
                    }

                case "moderation":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_moderation' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetModerationManager().Init();
                        RocketEmulador.GetGame().GetClientManager().ModAlert("Moderation presets have been updated. Please reload the client to view the new presets.");

                        Session.SendWhisper("Moderation configuration atualizado com sucesso.",34);
                        break;
                    }

                case "tickets":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_tickets' permission.",34);
                            break;
                        }

                        if (RocketEmulador.GetGame().GetModerationTool().Tickets.Count > 0)
                            RocketEmulador.GetGame().GetModerationTool().Tickets.Clear();

                        RocketEmulador.GetGame().GetClientManager().ModAlert("Tickets have been purged. Please reload the client.");
                        Session.SendWhisper("Tickets successfully purged.",34);
                        break;
                    }

                case "vouchers":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_vouchers' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetCatalog().GetVoucherManager().Init();
                        Session.SendWhisper("Catalogue vouche cache atualizado com sucesso.",34);
                        break;
                    }

                case "gc":
                case "games":
                case "gamecenter":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_game_center' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetGameDataManager().Init();
                        Session.SendWhisper("Game Center cache atualizado com sucesso.",34);
                        break;
                    }

                case "pet_locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_pet_locale' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetChatManager().GetPetLocale().Init();
                        Session.SendWhisper("Pet locale cache atualizado com sucesso.",34);
                        break;
                    }

                case "locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_locale' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetLanguageLocale().Init();
                        Session.SendWhisper("Locale cache atualizado com sucesso.",34);
                        break;
                    }

                case "mutant":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_anti_mutant' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetAntiMutant().Init();
                        Session.SendWhisper("Anti mutant successfully reloaded.",34);
                        break;
                    }

                case "bots":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_bots' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetBotManager().Init();
                        Session.SendWhisper("Bot managaer successfully reloaded.",34);
                        break;
                    }

                case "rewards":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_rewards' permission.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetRewardManager().Reload();
                        Session.SendWhisper("Rewards managaer successfully reloaded.",34);
                        break;
                    }

                case "chat_styles":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'command_update_chat_styles' permissão.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetChatManager().GetChatStyles().Init();
                        Session.SendWhisper("Chat Styles successfully reloaded.",34);
                        break;
                    }

                case "badge_definitions":
                case "emblemas":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("comando_adm"))
                        {
                            Session.SendWhisper("Opa, você não tem acesso ao 'comando_adm' permissão.",34);
                            break;
                        }

                        RocketEmulador.GetGame().GetBadgeManager().Init();
                        Session.SendWhisper("Badge definitions successfully reloaded.",34);
                        break;
                    }

                default:
                    Session.SendWhisper("'" + UpdateVariable + "' is not a valid thing to reload.",34);
                    break;
            }
        }
    }
}
