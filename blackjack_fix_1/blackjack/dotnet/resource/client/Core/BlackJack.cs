using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTANetworkAPI;
using NeptuneEvo;
using NeptuneEvo.Core;
using Newtonsoft.Json;
using Redage;
using Redage.SDK;
using Trigger = NeptuneEvo.Trigger;

namespace client.Core
{
    class BlackJack : Script
    {
        public static List<Thread> BlackJackThreads = new List<Thread>();

        private static nLog Log = new nLog("Blackjack");

        public static Dictionary<string, int> Cards = new Dictionary<string, int>()
        {
            { "vw_prop_cas_card_club_ace", 11 },
            {"vw_prop_cas_card_club_02", 2 },
            {"vw_prop_cas_card_club_03", 3},
            {"vw_prop_cas_card_club_04", 4},
            {"vw_prop_cas_card_club_05", 5},
            {"vw_prop_cas_card_club_06", 6},
            {"vw_prop_cas_card_club_07", 7},
            {"vw_prop_cas_card_club_08", 8},
            {"vw_prop_cas_card_club_09", 9},
            {"vw_prop_cas_card_club_10", 10},
            {"vw_prop_cas_card_club_jack", 10},
            {"vw_prop_cas_card_club_queen", 10},
            {"vw_prop_cas_card_club_king", 10},
            {"vw_prop_cas_card_dia_ace", 11},
            {"vw_prop_cas_card_dia_02", 2},
            {"vw_prop_cas_card_dia_03", 3},
            {"vw_prop_cas_card_dia_04", 4},
            {"vw_prop_cas_card_dia_05", 5},
            {"vw_prop_cas_card_dia_06", 6},
            {"vw_prop_cas_card_dia_07", 7},
            {"vw_prop_cas_card_dia_08", 8},
            {"vw_prop_cas_card_dia_09", 9},
            {"vw_prop_cas_card_dia_10", 10},
            {"vw_prop_cas_card_dia_jack", 10},
            {"vw_prop_cas_card_dia_queen", 10},
            {"vw_prop_cas_card_dia_king", 10},
            {"vw_prop_cas_card_hrt_ace", 11},
            {"vw_prop_cas_card_hrt_02", 2},
            {"vw_prop_cas_card_hrt_03", 3},
            {"vw_prop_cas_card_hrt_04", 4},
            {"vw_prop_cas_card_hrt_05", 5},
            {"vw_prop_cas_card_hrt_06", 6},
            {"vw_prop_cas_card_hrt_07", 7},
            {"vw_prop_cas_card_hrt_08", 8},
            {"vw_prop_cas_card_hrt_09", 9},
            {"vw_prop_cas_card_hrt_10", 10},
            {"vw_prop_cas_card_hrt_jack", 10},
            {"vw_prop_cas_card_hrt_queen", 10},
            {"vw_prop_cas_card_hrt_king", 10},
            {"vw_prop_cas_card_spd_ace", 11},
            {"vw_prop_cas_card_spd_02", 2},
            {"vw_prop_cas_card_spd_03", 3},
            {"vw_prop_cas_card_spd_04", 4},
            {"vw_prop_cas_card_spd_05", 5},
            {"vw_prop_cas_card_spd_06", 6},
            {"vw_prop_cas_card_spd_07", 7},
            {"vw_prop_cas_card_spd_08", 8},
            {"vw_prop_cas_card_spd_09", 9},
            {"vw_prop_cas_card_spd_10", 10},
            {"vw_prop_cas_card_spd_jack", 10},
            {"vw_prop_cas_card_spd_queen", 10},
            {"vw_prop_cas_card_spd_king", 10},
        };

        public static List<string> CardsKeys = new List<string>()
        {
             "vw_prop_cas_card_club_ace",
            "vw_prop_cas_card_club_02",
            "vw_prop_cas_card_club_03",
            "vw_prop_cas_card_club_04",
            "vw_prop_cas_card_club_05",
            "vw_prop_cas_card_club_06",
            "vw_prop_cas_card_club_07",
            "vw_prop_cas_card_club_08",
            "vw_prop_cas_card_club_09",
            "vw_prop_cas_card_club_10",
            "vw_prop_cas_card_club_jack",
           "vw_prop_cas_card_club_queen",
            "vw_prop_cas_card_club_king",
            "vw_prop_cas_card_dia_ace",
            "vw_prop_cas_card_dia_02",
            "vw_prop_cas_card_dia_03",
            "vw_prop_cas_card_dia_04",
            "vw_prop_cas_card_dia_05",
            "vw_prop_cas_card_dia_06",
            "vw_prop_cas_card_dia_07",
            "vw_prop_cas_card_dia_08",
            "vw_prop_cas_card_dia_09",
            "vw_prop_cas_card_dia_10",
            "vw_prop_cas_card_dia_jack",
            "vw_prop_cas_card_dia_queen",
            "vw_prop_cas_card_dia_king",
            "vw_prop_cas_card_hrt_ace",
            "vw_prop_cas_card_hrt_02",
            "vw_prop_cas_card_hrt_03",
            "vw_prop_cas_card_hrt_04",
            "vw_prop_cas_card_hrt_05",
            "vw_prop_cas_card_hrt_06",
            "vw_prop_cas_card_hrt_07",
            "vw_prop_cas_card_hrt_08",
            "vw_prop_cas_card_hrt_09",
            "vw_prop_cas_card_hrt_10",
            "vw_prop_cas_card_hrt_jack",
            "vw_prop_cas_card_hrt_queen",
            "vw_prop_cas_card_hrt_king",
            "vw_prop_cas_card_spd_ace",
            "vw_prop_cas_card_spd_02",
            "vw_prop_cas_card_spd_03",
            "vw_prop_cas_card_spd_04",
            "vw_prop_cas_card_spd_05",
            "vw_prop_cas_card_spd_06",
            "vw_prop_cas_card_spd_07",
            "vw_prop_cas_card_spd_08",
            "vw_prop_cas_card_spd_09",
            "vw_prop_cas_card_spd_10",
            "vw_prop_cas_card_spd_jack",
            "vw_prop_cas_card_spd_queen",
            "vw_prop_cas_card_spd_king",
        };

        public static List<int> BlackJackBetTime = new List<int>()
        {
            0, 0, 0, 0
        };

        public static List<List<Player>> BlackJackPlayers = new List<List<Player>>() {
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
        };

        public static List<List<Player>> BlackJackSeatsPlayers = new List<List<Player>>() {
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
            new List<Player>(){ },
        };

        public static List<int> BlackJackDealSum = new List<int>()
        {
            0,0,0,0
        };

        public static List<int> BlackJackTableSeatsNow = new List<int>()
        {
            0,0,0,0
        };

        public static List<int> BlackJackTableMaxBet = new List<int>()
        {
            100,100,100,100
        };

        public static List<int> BlackJackTableMinBet = new List<int>()
        {
            10,10,10,10
        };

        public static List<Player> BlackJackTablePlayerNow = new List<Player>()
        {
            null,null,null,null
        };

        public static List<int> BlackJackDealCardsNum = new List<int>()
        {
            0,0,0,0
        };

        public static List<List<Player>> BlackJackTablePlayers = new List<List<Player>>()
        {
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
            new List<Player>(){null, null, null, null},
        };




        public static List<List<List<int>>> BlackJackTables = new List<List<List<int>>>()
        {
            new List<List<int>>() {
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
            },
            new List<List<int>>() {
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
            },
            new List<List<int>>() {
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
            },
            new List<List<int>>() {
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
                new List<int>{ 0, 0, 0, 0, 0, 0, 0, },
            },
        };

        public static List<List<Vector3>> BlackJackTablesSeatsPos = new List<List<Vector3>>()
        {
            new List<Vector3>() {
                new Vector3(1148.367, 269.0835, -51.7879),
                new Vector3(1148.345, 269.7643, -51.7876),
                new Vector3(1148.821, 270.2321, -51.7708),
                new Vector3(1149.495, 270.2401, -51.7632),
            },
            new List<Vector3>() {
                new Vector3(1152.317, 267.4195, -51.8003),
                new Vector3(1152.337, 266.7202, -51.7913),
                new Vector3(1151.849, 266.2183, -51.7916),
                new Vector3(1151.182, 266.2501, -51.7864),
            },
            new List<Vector3>() {
                new Vector3(1128.713, 262.8658, -51.0035),
                new Vector3(1129.446, 262.8649, -51.0121),
                new Vector3(1129.932, 262.3822, -51.0027),
                new Vector3(1129.899, 261.6921, -51.0422),
            },
            new List<Vector3>() {
                new Vector3(1143.738, 247.8562, -51.034),
                new Vector3(1144.459, 247.8673, -51.0229),
                new Vector3(1144.951, 247.3612, -51.015),
                new Vector3(1144.913, 246.663, -51.0236),
            },
        };

        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {
            NAPI.Task.Run(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    Thread thd = new Thread(new ParameterizedThreadStart(StartDealing));
                    thd.Start(i);
                    BlackJackThreads.Add(thd);
                }
            }, 5000);

            for (int i = 0; i < BlackJackTablesSeatsPos.Count; i++)
            {
                for (int j = 0; j < BlackJackTablesSeatsPos[i].Count; j++)
                {
                    var colshape = NAPI.ColShape.CreateCylinderColShape(BlackJackTablesSeatsPos[i][j], 1, 2);
                    colshape.SetData("TABLE", i);
                    colshape.SetData("SEAT", j);
                    colshape.OnEntityEnterColShape += (shape, player) =>
                    {
                        player.SetData("INTERACTIONCHECK", 663);
                        player.SetData("TABLE", shape.GetData<int>("TABLE"));
                        player.SetData("SEAT", shape.GetData<int>("SEAT"));
                        Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "E", "сесть за стул" }));
                    };
                    colshape.OnEntityExitColShape += (shape, player) =>
                    {
                        Trigger.ClientEvent(player, "client_press_key_to", "close");
                        player.SetData("INTERACTIONCHECK", 0);
                        player.ResetData("TABLE");
                        player.ResetData("SEAT");
                    };
                }
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnectedhandler(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (player.HasData("BJ_TABLE"))
                {
                    BlackJack.Exit(player);
                    return;
                }
            }
            catch(Exception e)
            {

            }
        }

        public static void BlackJacks(Player player, int tb, int seat)
        {
            player.SetData("BJ_TABLE", tb);
            player.SetData("BJ_SEAT", seat);
            player.SetData("BJ_SUM", 0);
            player.SetData("BJ_CARDS", 0);
            player.SetData("BJ_STATUS", 0);
            player.SetData("BJ_ANSWER", 0);
            player.SetData("BJ_TIMER", 0);

            player.SetData("BJ_INBET", BlackJackTableMinBet[tb]);

            BlackJackTablePlayers[tb][seat] = player;

            BlackJackSeatsPlayers[tb].Add(player);
            BlackJackTables[tb][seat] = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
        }

        [RemoteEvent("blackJackBetDown")]
        public static void BetDown(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;

            if (BlackJackBetTime[player.GetData<int>("BJ_TABLE")] <= 0)
            {
                return;
            }

            int chips = player.GetData<int>("BJ_INBET");

            if (BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")] > chips - BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")])
            {
                return;
            }

            player.SetData("BJ_INBET", chips - BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")]);
            Trigger.ClientEvent(player, "casinoKeys", "setBet", chips - BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")], BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")]);
        }

        [RemoteEvent("blackJackBetUp")]
        public static void BetUp(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;

            if (BlackJackBetTime[player.GetData<int>("BJ_TABLE")] <= 0)
            {
                return;
            }

            int chips = player.GetData<int>("BJ_INBET");

            if (BlackJackTableMaxBet[player.GetData<int>("BJ_TABLE")] < chips + BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")])
            {
                return;
            }

            player.SetData("BJ_INBET", chips + BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")]);
            Trigger.ClientEvent(player, "casinoKeys", "setBet", chips + BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")], BlackJackTableMinBet[player.GetData<int>("BJ_TABLE")]);

        }

        [RemoteEvent("blackJackSetBet")]
        public static void Bet(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;


            if (BlackJackBetTime[player.GetData<int>("BJ_TABLE")] <= 0)
            {
                CMD_Stand(player);
                return;
            }


            int chips = player.GetData<int>("BJ_INBET");

            if (DiamondCasino.GetAllChips(player) < chips)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас недостаточно фишек", 3000);
                return;
            }

            nInventory.Remove(player, ItemType.CasinoChips, chips);

            if (BlackJackPlayers[player.GetData<int>("BJ_TABLE")].Contains(player))
            {
                player.SetData("BJ_BET", player.GetData<int>("BJ_BET") + chips);
            }
            else
            {
                BlackJackPlayers[player.GetData<int>("BJ_TABLE")].Add(player);
                player.SetData("BJ_BET", chips);
            }

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы сделали ставку в размере {chips}", 3000);
            Trigger.ClientEventInRange(player.Position, 50, "bet_blackjack", player.GetData<int>("BJ_TABLE"), player.GetData<int>("BJ_SEAT"), player.Handle);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        public static void Seat(Player player, int table, int seat)
        {
            if (player.HasData("BJ_TABLE"))
            {
                Exit(player);
                return;
            }

            if(BlackJackTablePlayers[table][seat] != null)
            {
                return;
            }

            Trigger.ClientEvent(player, "client_press_key_to", "close");

            BlackJacks(player, table, seat);
            Trigger.ClientEventInRange(GET_TABLE_CORD(table), 20, "seat_to_blackjack_table", table, seat, player.Handle);
            Trigger.ClientEvent(player, "casinoKeys", "show");
            Trigger.ClientEvent(player, "casinoKeys", "setBet", BlackJackTableMinBet[table], BlackJackTableMinBet[table]);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        [Command("sb")]
        public static void SB(Player player)
        {
            Trigger.ClientEvent(player, "openBlackjack");
        }

        [Command("exit")]
        public static void Exit(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;
            if (player.GetData<int>("BJ_STATUS") != 0)
                return;
            if (BlackJackPlayers[player.GetData<int>("BJ_TABLE")].Contains(player))
                BlackJackPlayers[player.GetData<int>("BJ_TABLE")].Remove(player);

            BlackJackSeatsPlayers[player.GetData<int>("BJ_TABLE")].Remove(player);

            Trigger.ClientEvent(player, "casinoKeys", "hide");
            Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 50, "exit_table", player.Handle);
            ExitPlayer(player);
            //Trigger.PlayerEvent(player, "seat_to_blackjack_table", 1, 0);
        }

        public static void StartDealing(object table)
        {
            try
            {
                int tb = Convert.ToInt32(table);

                while (true)
                {
                    if (BlackJackBetTime[tb] != 0)
                    {
                        if (BlackJackSeatsPlayers[tb].Count != 0)
                        {
                            for (int i = 0; i < BlackJackSeatsPlayers[tb].Count; i++)
                            {
                                //Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "blackjack_show_bet", 0, 0);
                                // BlackJackSeatsPlayers[tb][i].SendChatMessage($"Ожидание ставок [ {BlackJackBetTime[tb]} ]");
                                Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "casinoKeys", "setTime", 0, 0);
                            }

                            if (BlackJackSeatsPlayers[tb].Count == BlackJackPlayers[tb].Count)
                            {
                                BlackJackBetTime[tb] = 1;
                            }
                        }
                        BlackJackBetTime[tb] -= 1;

                        Thread.Sleep(1000);

                        if (BlackJackBetTime[tb] != 0)
                            continue;
                    }

                    if (BlackJackPlayers[tb].Count == 0 && BlackJackBetTime[tb] == 0)
                    {
                        BlackJackBetTime[tb] = 15;
                        Thread.Sleep(1000);
                        continue;
                    }

                    for (int i = 0; i < BlackJackSeatsPlayers[tb].Count; i++)
                    {
                        Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "casinoKeys", "toggleStart", true);
                    }

                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        GetCard(BlackJackPlayers[tb][i]);
                        Thread.Sleep(2000);
                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    GetCardDeal(tb);
                    Thread.Sleep(2000);

                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        GetCard(BlackJackPlayers[tb][i]);
                        Thread.Sleep(2000);
                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    GetCardDeal(tb);

                    Thread.Sleep(2000);

                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                    }

                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        BlackJackTablePlayerNow[tb] = BlackJackPlayers[tb][i];
                        BlackJackTableSeatsNow[tb] = BlackJackPlayers[tb][i].GetData<int>("BJ_SEAT");
                        while (true)
                        {
                            try
                            {
                                if (BlackJackPlayers[tb][i].GetData<int>("BJ_ANSWER") == 0)
                                {
                                    BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 1);
                                    int res = CheckPlayerSum(BlackJackPlayers[tb][i]);
                                    if (res <= 0)
                                    {
                                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "toggleStart", false);
                                        //BlackJackPlayers[tb][i].SendChatMessage("Ты типа все");
                                        Notify.Send(BlackJackPlayers[tb][i], NotifyType.Info, NotifyPosition.BottomCenter, "Вы проиграли", 3000);
                                        BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 0);
                                        Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "lose_blackjack", tb);
                                        Thread.Sleep(1500);
                                        CleanPlayer(BlackJackPlayers[tb][i]);
                                        break;
                                    }
                                    else if (res == 1)
                                    {
                                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "toggleStart", false);
                                        //BlackJackPlayers[tb][i].SendChatMessage("У вас блэкджек!");
                                        Notify.Send(BlackJackPlayers[tb][i], NotifyType.Info, NotifyPosition.BottomCenter, "У вас блэкджек", 3000);
                                        BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 0);
                                        Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "win_blackjack", tb);
                                        nInventory.Add(BlackJackPlayers[tb][i], new nItem(ItemType.CasinoChips, BlackJackPlayers[tb][i].GetData<int>("BJ_BET") * 2));
                                        Thread.Sleep(1500);
                                        CleanPlayer(BlackJackPlayers[tb][i]);
                                        break;
                                    }
                                    Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "stand_or_hit", tb, BlackJackPlayers[tb][i].GetData<int>("BJ_SEAT"));
                                    // Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_show_game");

                                }
                                else if (BlackJackPlayers[tb][i].GetData<int>("BJ_ANSWER") == 1)
                                {
                                    if (BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") == 15000)
                                    {
                                        BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 3);
                                        BlackJackPlayers[tb][i].SetData("BJ_TIMER", 0);
                                    }
                                    else
                                    {
                                        Thread.Sleep(1000);
                                        BlackJackPlayers[tb][i].SetData("BJ_TIMER", BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") + 1000);
                                        //BlackJackPlayers[tb][i].SendChatMessage($"{15 - BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") / 1000}");
                                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setTime", 0, 15 - BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") / 1000);
                                        //Trigger.ClientEvent(BlackJackSeatsPlayers[tb][i], "blackjack_update_time", $"{15 - BlackJackPlayers[tb][i].GetData<int>("BJ_TIMER") / 1000}");
                                    }
                                }
                                else
                                {
                                    if (BlackJackPlayers[tb][i].GetData<int>("BJ_ANSWER") == 2)
                                    {
                                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_hide_game");
                                        GetCard(BlackJackPlayers[tb][i]);
                                        BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 0);
                                        BlackJackPlayers[tb][i].SetData("BJ_TIMER", 0);

                                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_update_result", BlackJackDealSum[tb], BlackJackPlayers[tb][i].GetData<int>("SUM"));
                                    }
                                    else
                                    {
                                        BlackJackPlayers[tb][i].SetData("BJ_ANSWER", 0);
                                        BlackJackPlayers[tb][i].SetData("BJ_TIMER", 0);
                                        Thread.Sleep(1500);

                                        break;
                                    }

                                    Thread.Sleep(2500);

                                    Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM"), BlackJackDealSum[tb]);
                                }
                            }
                            catch (Exception e)
                            {
                                Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "clean_cards", tb, BlackJackTableSeatsNow[tb]);
                                Thread.Sleep(2000);
                                Log.Write(e.Message);
                                break;
                            }
                        }

                        BlackJackTablePlayerNow[tb] = null;
                        BlackJackTableSeatsNow[tb] = 0;
                    }

                    if (BlackJackDealCardsNum[tb] == 2)
                    {
                        Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "flip_card", tb);
                        BlackJackDealSum[tb] += Cards[CardsKeys[BlackJackTables[tb][4][1]]];
                        Thread.Sleep(3000);
                        for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                        {
                            Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                        }
                    }

                    while (BlackJackDealSum[tb] < 17)
                    {
                        GetCardDeal(tb);
                        Thread.Sleep(2500);
                        for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                        {
                            Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM").ToString(), BlackJackDealSum[tb].ToString());
                        }
                    }


                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "setChips", BlackJackPlayers[tb][i].GetData<int>("BJ_SUM"), BlackJackDealSum[tb]);
                        BlackJackTables[tb][BlackJackPlayers[tb][i].GetData<int>("BJ_SEAT")] = new List<int> { 0, 0, 0, 0, 0, 0, 0, };

                        Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "clean_cards", tb, BlackJackPlayers[tb][i].GetData<int>("BJ_SEAT"));
                        //Trigger.ClientEvent(BlackJackPlayers[tb][i], "blackjack_hide_game");
                        CheckFinalSum(BlackJackPlayers[tb][i]);
                        Thread.Sleep(2000);
                    }


                    for (int i = 0; i < BlackJackPlayers[tb].Count; i++)
                    {
                        Trigger.ClientEvent(BlackJackPlayers[tb][i], "casinoKeys", "toggleStart", false);
                    }

                    BlackJackPlayers[tb].Clear();

                    Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "clean_cards", tb, 4);
                    BlackJackTables[tb][4] = new List<int> { 0, 0, 0, 0, 0, 0, 0, };
                    BlackJackDealSum[tb] = 0;
                    BlackJackDealCardsNum[tb] = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        if (BlackJackTables[tb][i].Sum(x => Convert.ToInt32(x)) > 0)
                        {
                            Trigger.ClientEventInRange(GET_TABLE_CORD(tb), 20, "clean_cards", tb, i);
                            BlackJackTables[tb][i] = new List<int> { 0, 0, 0, 0, 0, 0, 0, };
                            Thread.Sleep(1500);
                        }
                    }    

                    Thread.Sleep(2000);
                }
            }
            catch(Exception e)
            {
                Log.Write("Dealing: " + e.Message);
            }
        }

        public static void CleanPlayer(Player player)
        {
            int seat = player.GetData<int>("BJ_SEAT");
            int table = player.GetData<int>("BJ_TABLE");
            BlackJackTables[table][seat] = new List<int> { 0, 0, 0, 0, 0, 0, 0, };

            player.SetData("BJ_TABLE", table);
            player.SetData("BJ_SEAT", seat);
            player.SetData("BJ_SUM", 0);
            player.SetData("BJ_CARDS", 0);
            player.SetData("BJ_STATUS", 0);
            player.SetData("BJ_ANSWER", 0);
            player.SetData("BJ_TIMER", 0);
            player.SetData("BJ_BET", 0);
        }

        public static void ExitPlayer(Player player)
        {
            int seat = player.GetData<int>("BJ_SEAT");
            int table = player.GetData<int>("BJ_TABLE");
            BlackJackTables[table][seat] = new List<int> { 0, 0, 0, 0, 0, 0, 0, };
            BlackJackTablePlayers[table][seat] = null;

            player.ResetData("BJ_TABLE");
            player.ResetData("BJ_SEAT");
            player.ResetData("BJ_SUM");
            player.ResetData("BJ_CARDS");
            player.ResetData("BJ_STATUS");
            player.ResetData("BJ_ANSWER");
            player.ResetData("BJ_TIMER");
            player.ResetData("BJ_BET");

            Trigger.ClientEvent(player, "blackjack_hide");
        }


        public static int CheckPlayerSum(Player player)
        {
            if (player.GetData<int>("BJ_STATUS") == 0)
                return -1;

            int table = player.GetData<int>("BJ_TABLE");

            //player.SendChatMessage($"У дилера [ {BlackJackDealSum[table]} ] . У вас [ {player.GetData<int>("BJ_SUM")} ] ");

            if (player.GetData<int>("BJ_SUM") > 21)
            {
                // lose
                player.SetData("BJ_STATUS", 0);
                return -1;
            }
            else if (player.GetData<int>("BJ_SUM") == 21)
            {
                // mb win

                return 1;
            }
            else
            {
                // stand or hit

                player.SetData("BJ_STATUS", 2);
                return 2;
            }
        }

        public static void CheckFinalSum(Player player)
        {
            if (player.GetData<int>("BJ_STATUS") == 0)
                return;

            int table = player.GetData<int>("BJ_TABLE");
            int seat = player.GetData<int>("BJ_SEAT");
            int pSum = player.GetData<int>("BJ_SUM");
            int bet = player.GetData<int>("BJ_BET");

            //player.SendChatMessage($"У дилера [ {BlackJackDealSum[table]} ] . У вас [ {player.GetData<int>("BJ_SUM")} ] ");
            int mainSum = BlackJackDealSum[table];

            if (mainSum > 21 && pSum > 21)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {bet} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "lose_blackjack", player.GetData<int>("BJ_TABLE"));
                Thread.Sleep(1500);
            }
            else if (mainSum > 21 && pSum <= 21)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выиграли {bet * 2} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "win_blackjack", player.GetData<int>("BJ_TABLE"));
                nInventory.Add(player, new nItem(ItemType.CasinoChips, bet * 2));
                Thread.Sleep(1500);
            }
            else if (mainSum < 21 && pSum > 21)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {bet} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "lose_blackjack", player.GetData<int>("BJ_TABLE"));
                Thread.Sleep(1500);
            }
            else if (mainSum < 21 && pSum < 21 && mainSum < pSum)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выиграли {bet * 2} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "win_blackjack", player.GetData<int>("BJ_TABLE"));
                nInventory.Add(player, new nItem(ItemType.CasinoChips, bet * 2));
                Thread.Sleep(1500);

            }
            else if (mainSum < 21 && pSum < 21 && mainSum > pSum)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {bet} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "lose_blackjack", player.GetData<int>("BJ_TABLE"));
                Thread.Sleep(1500);
            }
            else if (mainSum < 21 && pSum < 21 && mainSum == pSum)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Ничья", 3000);
                nInventory.Add(player, new nItem(ItemType.CasinoChips, bet));
            }
            else if (mainSum == 21 && pSum == 21)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Ничья", 3000);
                nInventory.Add(player, new nItem(ItemType.CasinoChips, bet));

                Thread.Sleep(1500);
            }
            else if (mainSum == 21 && pSum < 21)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы проиграли {bet} фишек", 3000);
                Trigger.ClientEventInRange(GET_TABLE_CORD(player.GetData<int>("BJ_TABLE")), 20, "lose_blackjack", player.GetData<int>("BJ_TABLE"));
                Thread.Sleep(1500);
            }
            else
            {
                player.SendChatMessage("Тут хз");
            }

            CleanPlayer(player);
        }


        public static void GetCard(Player player)
        {
            int table = player.GetData<int>("BJ_TABLE");
            int seat = player.GetData<int>("BJ_SEAT");
            int sum = player.GetData<int>("BJ_SUM");
            int cards = player.GetData<int>("BJ_CARDS");
            player.SetData("BJ_STATUS", 1);

            int emptySlot = cards;
            int randomCard = 0;

            Random rand = new Random();
            randomCard = rand.Next(0, Cards.Count - 1);

            sum += Cards[CardsKeys[randomCard]];
            BlackJackTables[table][seat][emptySlot] = randomCard;
            cards++;

            player.SetData("BJ_SUM", sum);
            player.SetData("BJ_CARDS", cards);

            Trigger.ClientEventInRange(GET_TABLE_CORD(table), 20, "client_bj_give_card", table, seat, emptySlot, randomCard);
        }


        [RemoteEvent("blackJackHit")]
        public static void CMD_Hit(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;

            int tb = player.GetData<int>("BJ_TABLE");
            int seat = player.GetData<int>("BJ_SEAT");

            if (player.GetData<int>("BJ_ANSWER") == 1)
            {
                Trigger.ClientEventInRange(player.Position, 150, "decline_card", player.Handle);
                player.SetData("BJ_ANSWER", 3);
            }
        }


        public static void CMD_Stand(Player player)
        {
            if (!player.HasData("BJ_TABLE"))
                return;

            int tb = player.GetData<int>("BJ_TABLE");
            int seat = player.GetData<int>("BJ_SEAT");

            if (player.GetData<int>("BJ_ANSWER") == 1)
            {
                Trigger.ClientEventInRange(player.Position, 150, "request_card", player.Handle);
                player.SetData("BJ_ANSWER", 2);
            }
        }

        public static void GetCardDeal(int table)
        {
            int emptySlot = BlackJackDealCardsNum[table];
            int randomCard = 0;

            Random rand = new Random();
            randomCard = rand.Next(0, Cards.Count - 1);

            BlackJackDealSum[table] += emptySlot != 1 ? Cards[CardsKeys[randomCard]] : 0;
            BlackJackTables[table][4][emptySlot] = randomCard;
            BlackJackDealCardsNum[table]++;

            Trigger.ClientEventInRange(GET_TABLE_CORD(table), 20, "client_bj_give_card", table, 4, emptySlot, randomCard);
        }

        public static Vector3 GET_TABLE_CORD(int iParam0)  // GET_TABLE_CORD
        {

            switch (iParam0)
            {
                case 0:
                    return new Vector3(1148.837f, 269.747f, -52.8409f);

                case 1:
                    return new Vector3(1151.84f, 266.747f, -52.8409f);

                case 2:
                    return new Vector3(1129.406f, 262.3578f, -52.041f);

                case 3:
                    return new Vector3(1144.429f, 247.3352f, -52.041f);
            }

            return new Vector3(0f, 0f, 0f);
        }

        public static Vector3 GET_CARD_OFFSET(int iParam0, int iParam1, bool bParam2)  // GET CARD OFFSET
        {
            if (!bParam2)
            {
                switch (iParam1)
                {
                    case 0:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(0.5737f, 0.2376f, 0.948025f);

                            case 1:
                                return new Vector3(0.562975f, 0.2523f, 0.94875f);

                            case 2:
                                return new Vector3(0.553875f, 0.266325f, 0.94955f);

                            case 3:
                                return new Vector3(0.5459f, 0.282075f, 0.9501f);

                            case 4:
                                return new Vector3(0.536125f, 0.29645f, 0.95085f);

                            case 5:
                                return new Vector3(0.524975f, 0.30975f, 0.9516f);

                            case 6:
                                return new Vector3(0.515775f, 0.325325f, 0.95235f);
                        }
                        break;

                    case 1:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(0.2325f, -0.1082f, 0.94805f);

                            case 1:
                                return new Vector3(0.23645f, -0.0918f, 0.949f);

                            case 2:
                                return new Vector3(0.2401f, -0.074475f, 0.950225f);

                            case 3:
                                return new Vector3(0.244625f, -0.057675f, 0.951125f);

                            case 4:
                                return new Vector3(0.249675f, -0.041475f, 0.95205f);

                            case 5:
                                return new Vector3(0.257575f, -0.0256f, 0.9532f);

                            case 6:
                                return new Vector3(0.2601f, -0.008175f, 0.954375f);

                        }
                        break;

                    case 2:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(-0.2359f, -0.1091f, 0.9483f);

                            case 1:
                                return new Vector3(-0.221025f, -0.100675f, 0.949f);

                            case 2:
                                return new Vector3(-0.20625f, -0.092875f, 0.949725f);

                            case 3:
                                return new Vector3(-0.193225f, -0.07985f, 0.950325f);

                            case 4:
                                return new Vector3(-0.1776f, -0.072f, 0.951025f);

                            case 5:
                                return new Vector3(-0.165f, -0.060025f, 0.951825f);

                            case 6:
                                return new Vector3(-0.14895f, -0.05155f, 0.95255f);
                        }
                        break;

                    case 3:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(-0.5765f, 0.2229f, 0.9482f);

                            case 1:
                                return new Vector3(-0.558925f, 0.2197f, 0.949175f);

                            case 2:
                                return new Vector3(-0.5425f, 0.213025f, 0.9499f);

                            case 3:
                                return new Vector3(-0.525925f, 0.21105f, 0.95095f);

                            case 4:
                                return new Vector3(-0.509475f, 0.20535f, 0.9519f);

                            case 5:
                                return new Vector3(-0.491775f, 0.204075f, 0.952825f);

                            case 6:
                                return new Vector3(-0.4752f, 0.197525f, 0.9543f);
                        }
                        break;
                }
            }
            else
            {
                switch (iParam1)
                {
                    case 0:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(0.6083f, 0.3523f, 0.94795f);

                            case 1:
                                return new Vector3(0.598475f, 0.366475f, 0.948925f);

                            case 2:
                                return new Vector3(0.589525f, 0.3807f, 0.94975f);

                            case 3:
                                return new Vector3(0.58045f, 0.39435f, 0.950375f);

                            case 4:
                                return new Vector3(0.571975f, 0.4092f, 0.951075f);

                            case 5:
                                return new Vector3(0.5614f, 0.4237f, 0.951775f);

                            case 6:
                                return new Vector3(0.554325f, 0.4402f, 0.952525f);

                        }
                        break;

                    case 1:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(0.3431f, -0.0527f, 0.94855f);

                            case 1:
                                return new Vector3(0.348575f, -0.0348f, 0.949425f);

                            case 2:
                                return new Vector3(0.35465f, -0.018825f, 0.9502f);

                            case 3:
                                return new Vector3(0.3581f, -0.001625f, 0.95115f);

                            case 4:
                                return new Vector3(0.36515f, 0.015275f, 0.952075f);

                            case 5:
                                return new Vector3(0.368525f, 0.032475f, 0.95335f);

                            case 6:
                                return new Vector3(0.373275f, 0.0506f, 0.9543f);
                        }
                        break;

                    case 2:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(-0.116f, -0.1501f, 0.947875f);

                            case 1:
                                return new Vector3(-0.102725f, -0.13795f, 0.948525f);

                            case 2:
                                return new Vector3(-0.08975f, -0.12665f, 0.949175f);

                            case 3:
                                return new Vector3(-0.075025f, -0.1159f, 0.949875f);

                            case 4:
                                return new Vector3(-0.0614f, -0.104775f, 0.9507f);

                            case 5:
                                return new Vector3(-0.046275f, -0.095025f, 0.9516f);

                            case 6:
                                return new Vector3(-0.031425f, -0.0846f, 0.952675f);

                        }
                        break;

                    case 3:
                        switch (iParam0)
                        {
                            case 0:
                                return new Vector3(-0.5205f, 0.1122f, 0.9478f);

                            case 1:
                                return new Vector3(-0.503175f, 0.108525f, 0.94865f);

                            case 2:
                                return new Vector3(-0.485125f, 0.10475f, 0.949175f);

                            case 3:
                                return new Vector3(-0.468275f, 0.099175f, 0.94995f);

                            case 4:
                                return new Vector3(-0.45155f, 0.09435f, 0.95085f);

                            case 5:
                                return new Vector3(-0.434475f, 0.089725f, 0.95145f);

                            case 6:
                                return new Vector3(-0.415875f, 0.0846f, 0.9523f);
                        }
                        break;
                }
            }
            return new Vector3(0f, 0f, 0f);
        }

        public static float GET_TABLE_HEADING(int iParam0)
        {

            switch (iParam0)
            {
                case 0:
                    return -134.69f;

                case 1:
                    return 45.31f;

                case 2:
                    return 135.31f;

                case 3:
                    return 135.31f;
            }

            return 0f;
        }

        public static List<float> GET_CARD_ROTATION(int iParam0, int iParam1, bool bParam2, bool bParam3) // GET_CARD_ROTATION
        {
            List<float> vVar0;

            vVar0 = new List<float> { 0f, 0f, 0f };
            if (!bParam2)
            {
                switch (iParam1)
                {
                    case 0:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, 69.12f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, 67.8f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, 66.6f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, 70.44f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, 70.84f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, 67.88f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, 69.56f };
                                break;
                        }
                        break;

                    case 1:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, 22.11f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, 22.32f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, 20.8f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, 19.8f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, 19.44f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, 26.28f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, 22.68f };
                                break;
                        }
                        break;

                    case 2:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, -21.43f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, -20.16f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, -16.92f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, -23.4f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, -21.24f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, -23.76f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, -19.44f };
                                break;
                        }
                        break;

                    case 3:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, -67.03f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, -69.12f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, -64.44f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, -67.68f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, -63.72f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, -68.4f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, -64.44f };
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (iParam1)
                {
                    case 0:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, 68.57f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, 67.52f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, 67.76f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, 67.04f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, 68.84f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, 65.96f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, 67.76f };
                                break;
                        }
                        break;

                    case 1:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, 22.11f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, 22f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, 24.44f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, 21.08f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, 25.96f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, 26.16f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, 28.76f };
                                break;
                        }
                        break;

                    case 2:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, -14.04f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, -15.48f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, -16.56f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, -15.84f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, -16.92f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, -14.4f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, -14.28f };
                                break;
                        }
                        break;

                    case 3:
                        switch (iParam0)
                        {
                            case 0:
                                vVar0 = new List<float> { 0f, 0f, -67.03f };
                                break;

                            case 1:
                                vVar0 = new List<float> { 0f, 0f, -67.6f };
                                break;

                            case 2:
                                vVar0 = new List<float> { 0f, 0f, -69.4f };
                                break;

                            case 3:
                                vVar0 = new List<float> { 0f, 0f, -69.04f };
                                break;

                            case 4:
                                vVar0 = new List<float> { 0f, 0f, -68.68f };
                                break;

                            case 5:
                                vVar0 = new List<float> { 0f, 0f, -66.16f };
                                break;

                            case 6:
                                vVar0 = new List<float> { 0f, 0f, -63.28f };
                                break;
                        }
                        break;
                }
            }

            return vVar0;
        }

    }
}
