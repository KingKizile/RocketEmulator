using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Rooms.Games;
using Rocket.HabboHotel.Rooms.Games.Teams;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.Blue.USERS.Extra
{
    class EfeitoJogador : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%EffectId%"; }
        }

        public string Description
        {
            get { return "Gives you the ability to set an effect on your user!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendNotification("You must enter an effect ID!");
                return;
            }

            if (!Room.EnablesEnabled && !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendNotification("Oops, it appears that the room owner has disabled the ability to use the enable command in here.");
                return;
            }

            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (ThisUser == null)
                return;

            if (ThisUser.RidingHorse)
            {
                Session.SendNotification("You cannot enable effects whilst riding a horse!");
                return;
            }
            else if (ThisUser.Team != TEAM.NONE)
                return;
            else if (ThisUser.isLying)
                return;

            int EffectId = 0;
            if (!int.TryParse(Params[1], out EffectId))
                return;

            if (EffectId > int.MaxValue || EffectId < int.MinValue)
                return;

            if ((EffectId == 102 || EffectId == 187) && !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendNotification("Sorry, only staff members can use this effects.");
                return;
            }
            if ((EffectId == 178) && !Session.GetHabbo().GetPermissions().HasRight("rocket_amb"))
            {
                Session.SendNotification("Sorry, only staff members can use this effects.");
                return;
            }

            Session.GetHabbo().Effects().ApplyEffect(EffectId);

            if (EffectId == 178 && (!Session.GetHabbo().GetPermissions().HasRight("Rocket_amb") && !Session.GetHabbo().GetPermissions().HasRight("events_staff")))
            {
                Session.SendNotification("Sorry, only Gold VIP and Events Staff members can use this effect.");
                return;
            }

            Session.GetHabbo().Effects().ApplyEffect(EffectId);
        }
    }
}
