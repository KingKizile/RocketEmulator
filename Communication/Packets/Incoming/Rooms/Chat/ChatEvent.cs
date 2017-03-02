using System;

using Rocket.Core;
using Rocket.Communication.Packets.Incoming;
using Rocket.Utilities;
using Rocket.HabboHotel.Global;
using Rocket.HabboHotel.Quests;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Chat.Logs;
using Rocket.Communication.Packets.Outgoing.Messenger;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.Communication.Packets.Outgoing.Moderation;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.HabboHotel.Rooms.Chat.Styles;

namespace Rocket.Communication.Packets.Incoming.Rooms.Chat
{
    public class ChatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            string Message = StringCharFilter.Escape(Packet.PopString());
            if (Message.Length > 100)
                Message = Message.Substring(0, 100);

            int Colour = Packet.PopInt();

            ChatStyle Style = null;
            if (!RocketEmulador.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
                Colour = 0;

            User.UnIdle();

            if (RocketEmulador.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
                return;

            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendMessage(new MutedComposer(Session.GetHabbo().TimeMuted));
                return;
            }
            if (RocketEmulador.GetGame().GetChatManager().GetFilter().IsFiltered(Message))
            {

                RocketEmulador.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta de divulgação !",
                             "O Usuário: <b>" + Session.GetHabbo().Username + "<br>" +

                             "<br></b> Está divulgando uma palavra que foi bloqueada" + "<br>" +

                             "<br><b>A palavra usada foi:</b><br>" +
                               "<br>" + "<b>" + "<font color =\"#FF0000\">" + Message + "</font>" + "</b><br>" +
                             "<br>Para ir a la sala, clique em \"Ir a Sala \"</b>",
                             "filter", "Ir a Sala", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                Session.GetHabbo().GetClient().SendMessage(new WhisperComposer(User.VirtualId, "A seguinte palavra foi bloqueada em nosso hotel:" + " " + Message, 0, 34));
                Message = null;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
            {
                Session.SendWhisper("Opa, você atualmente está silenciado.", 34);
                return;
            }

            User.LastBubble = Session.GetHabbo().CustomBubbleId == 0 ? Colour : Session.GetHabbo().CustomBubbleId;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                int MuteTime;
                if (User.IncrementAndCheckFlood(out MuteTime))
                {
                    Session.SendMessage(new FloodControlComposer(MuteTime));
                    return;
                }
            }

            if (Message.StartsWith(":", StringComparison.CurrentCulture) && RocketEmulador.GetGame().GetChatManager().GetCommands().Parse(Session, Message))
                return;

            RocketEmulador.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (RocketEmulador.GetGame().GetChatManager().GetFilter().CheckBannedWords(Message))
            {
                Session.GetHabbo().BannedPhraseCount++;

                if (Session.GetHabbo().BannedPhraseCount >= RocketGame.BannedPhrasesAmount)
                {
                    RocketEmulador.GetGame().GetModerationManager().BanUser("RocketEmulador", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Spamming banned phrases (" + Message + ")", (RocketEmulador.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }

                Session.SendMessage(new ChatComposer(User.VirtualId, Message, 0, Colour));
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override"))
                Message = RocketEmulador.GetGame().GetChatManager().GetFilter().CheckMessage(Message);


            RocketEmulador.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

            User.OnChat(User.LastBubble, Message, false);
        }
    }
}