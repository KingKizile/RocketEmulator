using System;
using System.Data;

namespace Rocket.HabboHotel.Users.Authenticator
{
    public static class HabboFactory
    {
        public static Habbo GenerateHabbo(DataRow Row, DataRow UserInfo)
        {
            return new Habbo(Convert.ToInt32(Row["id"]), Convert.ToString(Row["username"]), Convert.ToInt32(Row["rank"]), Convert.ToString(Row["motto"]), Convert.ToString(Row["look"]),
                Convert.ToString(Row["gender"]), Convert.ToInt32(Row["credits"]), Convert.ToInt32(Row["activity_points"]), 
                Convert.ToInt32(Row["home_room"]), RocketEmulador.EnumToBool(Row["block_newfriends"].ToString()), Convert.ToInt32(Row["last_online"]),
                RocketEmulador.EnumToBool(Row["hide_online"].ToString()), RocketEmulador.EnumToBool(Row["hide_inroom"].ToString()),
                Convert.ToDouble(Row["account_created"]), Convert.ToInt32(Row["vip_points"]),Convert.ToString(Row["machine_id"]), (Row["nux_user"].ToString() == "true"), RocketEmulador.EnumToBool(Row["is_nuevo"].ToString()), Convert.ToString(Row["volume"]),
                RocketEmulador.EnumToBool(Row["chat_preference"].ToString()), RocketEmulador.EnumToBool(Row["focus_preference"].ToString()), RocketEmulador.EnumToBool(Row["pets_muted"].ToString()), RocketEmulador.EnumToBool(Row["bots_muted"].ToString()),
                RocketEmulador.EnumToBool(Row["advertising_report_blocked"].ToString()), Convert.ToDouble(Row["last_change"].ToString()), Convert.ToInt32(Row["gotw_points"]),
                RocketEmulador.EnumToBool(Convert.ToString(Row["ignore_invites"])), Convert.ToDouble(Row["time_muted"]), Convert.ToDouble(UserInfo["trading_locked"]),
                RocketEmulador.EnumToBool(Row["allow_gifts"].ToString()), Convert.ToInt32(Row["friend_bar_state"]),  RocketEmulador.EnumToBool(Row["disable_forced_effects"].ToString()),
                RocketEmulador.EnumToBool(Row["allow_mimic"].ToString()), RocketEmulador.EnumToBool(Row["allow_events"].ToString()), RocketEmulador.EnumToBool(Row["allow_commands"].ToString()),
 Convert.ToInt32(Row["rank_vip"]));
        }
    }
}