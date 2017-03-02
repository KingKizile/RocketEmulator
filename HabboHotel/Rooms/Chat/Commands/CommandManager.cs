using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD.Extra;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra;
using Rocket.Communication.Packets.Outgoing.Notifications;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Items.Wired;
using Rocket.HabboHotel.Rooms.Chat.Commands.Blue.USERS.Extra;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.LOC;

namespace Rocket.HabboHotel.Rooms.Chat.Commands
{
    public class CommandManager
    {
        private string _prefix = ":";
        private readonly Dictionary<string, IChatCommand> _commands;
        public CommandManager(string Prefix)
        {
            this._prefix = Prefix;
            this._commands = new Dictionary<string, IChatCommand>();

            this.RegisterVIP();
            this.RegisterUser();
            this.RegisterEvents();
            this.RegisterModerator();
            this.RegisterAdministrator();
        }
        public bool Parse(GameClient Session, string Message)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return false;

            if (!Message.StartsWith(_prefix))
                return false;

            if (Message == _prefix + "comandos")
            {
                StringBuilder List = new StringBuilder();
                List.Append("                                   | LISTA DE COMANDOS | \n-----------------------------------------------------------------------------\n\n");

                foreach (var CmdList in _commands.ToList())
                {
                    if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand(CmdList.Value.PermissionRequired))
                            continue;
                    }

                    List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " - " + CmdList.Value.Description + "\n");
                    List.Append("                                               \n-----------------------------------------------------------------------------\n\n");

                }
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return true;
            }

            Message = Message.Substring(1);
            string[] Split = Message.Split(' ');

            if (Split.Length == 0)
                return false;

            IChatCommand Cmd = null;
            if (_commands.TryGetValue(Split[0].ToLower(), out Cmd))
            {
                if (Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                    this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId);

                if (!string.IsNullOrEmpty(Cmd.PermissionRequired))
                {
                    if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
                        return false;
                }


                Session.GetHabbo().IChatCommand = Cmd;
                Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, Session.GetHabbo(), this);

                Cmd.Execute(Session, Session.GetHabbo().CurrentRoom, Split);
                return true;
            }
            return false;
        }
        private void RegisterVIP()
        {
            this.Register("commands", new DesativarComando());
            this.Register("chutar", new ChutarComando());
            this.Register("addquarto", new AdicionarQuarto());
            this.Register("poll", new IniciarPoll());
            this.Register("quarto", new QuartoComando());
            this.Register("casar", new CasarComando());
            this.Register("premiar", new EventoWins());
            this.Register("darrank", new AdicionarRank());
            this.Register("addfiltro", new AdicionarFiltro());
            this.Register("summonall", new SummonAll());
            this.Register("penis", new PenisComando());
            this.Register("picachu", new PicachuComando());
            this.Register("divulgar", new ComandoDivulgar());
            this.Register("mudarcor", new MudarCor());
            this.Register("locutar", new LocutorComando());
            this.Register("alertas", new DesativarAlertas());
            this.Register("online", new OnlineComando());
            this.Register("punheta", new PunhetaComando());
            this.Register("socar", new SocarComando());
            this.Register("rasteira", new RasteiraComando());
            this.Register("sexo", new SexoComando());
            this.Register("voar", new VoarComando());
            this.Register("atirar", new AtirarComando());
            this.Register("mp5", new MP5Comando());
            this.Register("beijar", new BeijarComando());
            this.Register("abracar", new AbracarComando());
            this.Register("ausente", new AusenteComando());

        }
        private void RegisterEvents()
        {

            this.Register("eha", new EventoAlerta());
            this.Register("alertarevento", new EventoAlerta());
        }

   
        private void RegisterUser()
        {
           
            this.Register("info", new InfoComando());
            this.Register("about", new InfoComando());
            this.Register("pickall", new PickallComando());
            this.Register("ejectall", new EjectallComando());
            this.Register("deitar", new DeitarJogador());
            this.Register("sentar", new SentarComando());
            this.Register("mutarpets", new MutarPets());
            this.Register("mutarbots", new MutarBots());
            this.Register("comprarquarto", new ComprarQuarto());
            this.Register("venderquarto", new VenderQuarto());
            this.Register("recusaroferta", new RecusarOferta());
            this.Register("aceitaroferta", new AceitarOferta());
            this.Register("spuxar", new SuperPuxar());
            this.Register("limpar", new LimparInventario());
            this.Register("copiar", new CopiarComando());
            this.Register("empurrar", new EmpurrarComando());

            this.Register("puxar", new PuxarComando());
            this.Register("efeito", new EfeitoJogador());
            this.Register("seguir", new SeguirJogador());
            this.Register("semcara", new SemCaraComando());

            this.Register("moonwalk", new MoonwalkComando());
            this.Register("desativarpresentes", new DesativarPresentes());
            this.Register("setmax", new SetMaxComando());
            this.Register("setspeed", new SetSpeedComando());

            this.Register("desativardiagonal", new DesativarDiagonal());
            this.Register("mudarnick", new MudarNome());
            this.Register("convertermoedas", new ConverterMoedas());
            this.Register("desativarsussurro", new DesativarSussurro());
            this.Register("desativarcopiar", new DesativarCopiar());
            this.Register("status", new StatusComando());
            this.Register("kikarpets", new KikarPets());
            this.Register("kikarbots", new KikarBots());
            this.Register("unload", new UnloadComando());
            this.Register("pet", new PetComando());

        }

        private void RegisterModerator()
        {


            this.Register("banir", new BanirComando());
            this.Register("mip", new BanirMIP());
            this.Register("ipban", new BanirIP());

            this.Register("ui", new VerJogador());
            this.Register("userinfo", new VerJogador());
            this.Register("sa", new AlertarEquipe());
            this.Register("desmutarquarto", new DesmutarQuarto());
            this.Register("mutarquarto", new MutarQuarto());
            this.Register("emblemaquarto", new QuartoEmblema());
            this.Register("alertarquarto", new AlertarQuarto());
            this.Register("kikarquarto", new KikarQuarto());
            this.Register("mutar", new MutarComando());
            this.Register("desmutarcomando", new DesmutarComando());
            this.Register("massemblema", new DarEmblemaATodos());
            this.Register("kikar", new KikarComando());
            this.Register("ha", new AlertarHotel());
            this.Register("alertarhotel", new AlertarHotel());
            this.Register("hal", new AlertaComLink());
            this.Register("dar", new DarComando());
            this.Register("daremblema", new DarEmblema());
            this.Register("dc", new DesconectarComando());
            this.Register("desconectar", new DesconectarComando());
            this.Register("alertar", new AlertarComando());
            this.Register("banirtrocas", new BanirTrocas());

            this.Register("dartodos", new DarParaTodos());
            this.Register("massdar", new DarAoHotel());
            this.Register("globaldar", new DarATodos());
            this.Register("teleportar", new TeleportarComando());
            this.Register("summon", new ConvocarComando());
            this.Register("congelar", new CongelarComando());
            this.Register("descongelar", new DescongelarComando());
            this.Register("forcarsentar", new ForcarSentar());
            this.Register("ignorarsussurro", new IgnorarSussurro());
            this.Register("forced_effects", new ForcarDesativarEfeito());
            this.Register("flaguser", new MudarNickJogador());
            this.Register("verfakes", new VerFakes());


        }

        private void RegisterAdministrator()
        {
            this.Register("bolha", new BolhaComando());
            this.Register("limparjogador", new LimparJogador());
            this.Register("atualizar", new AtualizarComando());
            this.Register("deletargrupo", new DeletarGrupo());
            this.Register("item", new ItemJogador());
        }
        public void Register(string CommandText, IChatCommand Command)
        {
            this._commands.Add(CommandText, Command);
        }

        public static string MergeParams(string[] Params, int Start)
        {
            var Merged = new StringBuilder();
            for (int i = Start; i < Params.Length; i++)
            {
                if (i > Start)
                    Merged.Append(" ");
                Merged.Append(Params[i]);
            }

            return Merged.ToString();
        }

        public void LogCommand(int UserId, string Data, string MachineId)
        {
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `logs_client_staff` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                dbClient.AddParameter("UserId", UserId);
                dbClient.AddParameter("Data", Data);
                dbClient.AddParameter("MachineId", MachineId);
                dbClient.AddParameter("Timestamp", RocketEmulador.GetUnixTimestamp());
                dbClient.RunQuery();
            }
        }

        public bool TryGetCommand(string Command, out IChatCommand IChatCommand)
        {
            return this._commands.TryGetValue(Command, out IChatCommand);
        }
    }
}
