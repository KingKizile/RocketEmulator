using System;
using System.Collections.Generic;

using Rocket.HabboHotel.Users;

namespace Rocket.Communication.Packets.Outgoing.Handshake
{
    public class UserPerksComposer : ServerPacket
    {
        public UserPerksComposer(Habbo Habbo)
            : base(ServerPacketHeader.UserPerksMessageComposer)
        {
            base.WriteInteger(17); // Count
            base.WriteString("USE_GUIDE_TOOL");
            base.WriteString("requirement.unfulfilled.helper_level_1");
            base.WriteBoolean(true);
            base.WriteString("GIVE_GUIDE_TOURS");
            base.WriteString("requirement.unfulfilled.helper_level_1");
            base.WriteBoolean(true);
            base.WriteString("JUDGE_CHAT_REVIEWS");
            base.WriteString("requirement.unfulfilled.helper_level_1"); // ??
            base.WriteBoolean(true);
            base.WriteString("VOTE_IN_COMPETITIONS");
            base.WriteString("requirement.unfulfilled.helper_level_1"); // ??
            base.WriteBoolean(true);
            base.WriteString("CALL_ON_HELPERS");
            base.WriteString("requirement.unfulfilled.helper_level_1"); // ??
            base.WriteBoolean(true);
            base.WriteString("CITIZEN");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("TRADE");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("HEIGHTMAP_EDITOR_BETA");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("EXPERIMENTAL_CHAT_BETA");
            base.WriteString("");
            base.WriteBoolean(true);
            base.WriteString("EXPERIMENTAL_TOOLBAR");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("BUILDER_AT_WORK");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("NAVIGATOR_PHASE_ONE_2014");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("CAMERA");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("NAVIGATOR_PHASE_TWO_2014");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("MOUSE_ZOOM");
            base.WriteString(""); // ??
            base.WriteBoolean(true);
            base.WriteString("NAVIGATOR_ROOM_THUMBNAIL_CAMERA");
            base.WriteString(""); // ??
            base.WriteBoolean(false);
            base.WriteString("HABBO_CLUB_OFFER_BETA");
            base.WriteString("");
            base.WriteBoolean(true);
        }
    }
}