using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GTANetworkAPI;
using NeptuneEvo;
using Newtonsoft.Json;
using Redage;
using Redage.SDK;

namespace NeptuneEvo.Core
{
    class InsideTrack : Script
    {
        public static Vector3 ScreenPosition = new Vector3(1092.75, 264.56, -51.24);

        public static int Timer = 0;
        public static bool WaitBets = false;
        public static bool IsStartRace = false;
        public static int LastHorse = -1;
        public static int ShowHorse = -1;

        public static int WinnerHorse = -1;
        public static int LastWinnerHorse = -1;

        public static List<int> Horses = new List<int>() { 1, 2, 3, 4, 5, 6 };
        public static List<int> StyleHorses = new List<int>() { 1, 2, 3, 4, 5, 6 };

        public static List<ITBet> Bets = new List<ITBet>() { };

        public static List<Vector3> Seats = new List<Vector3>() { 
            new Vector3(1091.418, 257.544, -52.2409),
            new Vector3(1092.072, 258.1984, -52.2409),
            new Vector3(1094.473, 260.5995, -52.2409),
            new Vector3(1095.127, 261.254, -52.2409),
            new Vector3(1095.852, 261.978, -52.2409),
            new Vector3(1096.506, 262.6327, -52.2409),
            new Vector3(1098.902, 265.0287, -52.2409),
            new Vector3(1099.556, 265.683, -52.2409),
            new Vector3(1092.974, 255.211, -52.2409f),
            new Vector3(1093.628, 255.865, -52.2409),
            new Vector3(1096.417, 258.6552, -52.2409),
            new Vector3(1097.072, 259.309, -52.2409),
            new Vector3(1097.797, 260.034, -52.2409),
            new Vector3(1098.451, 260.6878, -52.2409),
            new Vector3(1101.236, 263.473, -52.2409),
            new Vector3(1101.89, 264.127, -52.2409),
        };

        public static List<bool> CheckSeats = new List<bool>();

        [ServerEvent(Event.ResourceStart)]
        public static void OnResourceStart()
        {

            Bets.Clear();

            int i = 0;
            foreach(Vector3 pos in Seats)
            {
                var sp = NAPI.ColShape.CreateCylinderColShape(pos, 1f, 2f);
                sp.SetData("NUM", i);
                sp.OnEntityEnterColShape += (shape, player) =>
                {
                    player.SetData("INTERACTIONCHECK", 1050);
                    player.SetData("COMP_NUM", shape.GetData<int>("NUM"));

                    if (!player.HasData("OPEN_IT"))
                    {
                        if (player.HasData("IT_SEAT"))
                        {
                            Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "SPACE", "открыть меню ставок" }));
                        }
                        else
                        {
                            Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "E", "сесть за стул" }));
                        }
                    }
                };
                sp.OnEntityExitColShape += (shape, player) =>
                {
                    player.SetData("INTERACTIONCHECK", 0);
                    player.ResetData("COMP_NUM");
                    Trigger.ClientEvent(player, "client_press_key_to", "close");
                };
                i++;

                CheckSeats.Add(false);
            }

            var shape = NAPI.ColShape.CreateCylinderColShape(ScreenPosition, 25f, 10f);
            shape.OnEntityEnterColShape += (shape, player) =>
            {
                ShowMainScreen(player);
            };
            shape.OnEntityExitColShape += (shape, player) =>
            {
                HideMainScreen(player);
            };

            Thread thd = new Thread(StartRaceInsideTrack);
            thd.Start();

            //GetRandomHorse();
        }

        [Command("pan")]
        public static void Pan(Player player, string anim, string dict, int flag)
        {
            player.PlayAnimation(anim, dict, flag);
        }

        [RemoteEvent("openInsideTrack")]
        public static void OpenInsideTrack(Player player)
        {
            Trigger.ClientEvent(player, "client_press_key_to", "close");
            player.SetData("OPEN_IT", true);
        }

        [RemoteEvent("hideInsideTrack")]
        public static void HideInsideTrack(Player player)
        {
            Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "SPACE", "открыть меню ставок" }));
            player.ResetData("OPEN_IT");
        }

        public static void SeatAtTable(Player player)
        {
            int seat = player.GetData<int>("COMP_NUM");

            if (CheckSeats[seat])
            {
                return;
            }

            player.SetData("IT_SEAT", true);
            player.SetData("SEAT", seat);

            CheckSeats[seat] = true;

            // Trigger.ClientEvent(player, "client_press_key_to", "close");
            Trigger.ClientEvent(player, "setHorses", JsonConvert.SerializeObject(StyleHorses));
            Trigger.ClientEvent(player, "client_press_key_to", "open", JsonConvert.SerializeObject(new List<object>() { "SPACE", "открыть меню ставок" }));
            Trigger.ClientEventInRange(player.Position, 30f, "seatAtComp", player.Handle, Seats[seat].X, Seats[seat].Y, Seats[seat].Z);
        }

        public static void EixtTable(Player player)
        {
            if (player.HasData("OPEN_IT"))
                return;

            player.ResetData("IT_SEAT");

            int seat = player.GetData<int>("SEAT");

            CheckSeats[seat] = false;

            player.ResetData("SEAT");

            Trigger.ClientEventInRange(player.Position, 30f, "exitComp", player.Handle);

            Trigger.ClientEvent(player, "client_press_key_to", "close");

        }

        public static void StartRaceInsideTrack()
        {
            while (true)
            {
                Timer = 300;
                WaitBets = true;
                WinnerHorse = -1;
                ShowHorse = -1;
                LastHorse = -1;

                StyleHorses = GetRandomArr(99);

                SetHorses();

                ShowAllHorses();

                while (Timer > 0)
                {
                    Timer--;

                    if (Timer % 10 == 0)
                    {
                        if (ShowHorse == -1)
                        {
                            if (LastHorse == -1)
                            {
                                ShowHorse = 1;
                                LastHorse = 1;
                                ShowHorseBigScreen(1);
                            }
                            else if (LastHorse >= 6)
                            {
                                ShowHorse = 1;
                                LastHorse = 1;
                                ShowHorseBigScreen(1);
                            }
                            else
                            {
                                LastHorse += 1;
                                ShowHorse = LastHorse;
                                ShowHorseBigScreen(LastHorse);
                            }
                        }
                        else
                        {
                            ShowHorse = -1;
                            ShowAllHorses();
                        }
                    }

                    UpdateCountdown();
                    Thread.Sleep(1000);

                }

                WinnerHorse = GetRandomHorse();
                LastWinnerHorse = WinnerHorse;

                StartRace();
                IsStartRace = true;

                Timer = 55;

                while(Timer > 0)
                {
                    Timer--;
                    Thread.Sleep(1000);
                }

                IsStartRace = false;

                Payout();

                ClearPlayers();

            }

        }

    
        public static void UpdateCountdown()
        {
            Trigger.ClientEventInRange(ScreenPosition, 30f, "updateCountdown", Timer);
        }

        public static int GetRandomHorse()
        {
            Shuffle(Horses);

            while (Horses[0] == LastWinnerHorse)
            {
                Shuffle(Horses);
            }

            return Horses[0];
        }

        public static void Payout()
        {
            foreach(ITBet bet in Bets)
            {
                if (bet.Horse != WinnerHorse)
                {
                    try
                    {
                        Notify.Send(bet.Pl, NotifyType.Info, NotifyPosition.BottomCenter, $"Ваша ставка проиграла", 3000);
                    }
                    catch { }
                    continue;
                }

                try
                {
                    nInventory.Add(bet.Pl, new nItem(ItemType.CasinoChips, bet.Payout));

                    Notify.Send(bet.Pl, NotifyType.Success, NotifyPosition.BottomCenter, $"Ваша лошадка победила, выигрыш {bet.Payout}", 3000);

                    UpdateBalance(bet.Pl);
                }
                catch { }
            }
        }

        public static int GetAllChips(Player player)
        {
            if (!Main.Players.ContainsKey(player))
                return 0;

            var item = nInventory.Find(Main.Players[player].UUID, ItemType.CasinoChips);

            int count = 0;

            if (item != null)
            {
                count = item.Count;
            }

            return count;

        }


        [RemoteEvent("addbet")]
        public static void AddBet(Player player, int horse, int bet)
        {

            if(GetAllChips(player) < bet)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас нехватает фишек", 3000);
                return;
            }

            nInventory.Remove(player, ItemType.CasinoChips, bet);

            Bets.Add(new ITBet(player, horse, bet, bet * 2));

          
            Trigger.ClientEventInRange(ScreenPosition, 30f, "addBet", player.Name, horse, bet);
            Trigger.ClientEvent(player, "setbet", true);

            UpdateBalance(player);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Ставка принята на лошадку #{horse}", 3000);
        }

        [RemoteEvent("winnerBet")]
        public static void WinnerBet(Player player, int bet)
        {

            nInventory.Add(player, new nItem(ItemType.CasinoChips, bet * 2));

            UpdateBalance(player);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Ваша лошадка победила. Выигрыш {bet * 2}", 3000);

            //player.SendChatMessage($"Ваша лошадка победила. Выигрыш {bet * 2}");
        }

        [RemoteEvent("trySingleBet")]
        public static void TryBet(Player player, int bet)
        {
            if (GetAllChips(player) < bet)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У вас нехватает фишек", 3000);
                return;
            }

            nInventory.Remove(player, ItemType.CasinoChips, bet);

            UpdateBalance(player);

            Trigger.ClientEvent(player, "startSingleRace");

            //player.SendChatMessage($"Ваша лошадка победила. Выигрыш {bet * 2}");
        }


        public static void ShowHorseBigScreen(int num)
        {
            Trigger.ClientEventInRange(ScreenPosition, 30f, "showHorse", num);
        }

        public static void UpdateBalance(Player player)
        {
            Trigger.ClientEvent(player, "updateBalance", GetAllChips(player));
        }

        public static void ClearPlayers()
        {
            Bets.Clear();
            Trigger.ClientEventInRange(ScreenPosition, 30f, "clearPlayers");
            Trigger.ClientEventInRange(ScreenPosition, 30f, "setbet", false);
        }


        public static void SetHorses()
        {
            Trigger.ClientEventInRange(ScreenPosition, 30f, "setHorses", JsonConvert.SerializeObject(StyleHorses));
        }

        public static void ShowAllHorses()
        {
            Trigger.ClientEventInRange(ScreenPosition, 30f, "showMain");
        }

        public static void StartRace()
        {
            Trigger.ClientEventInRange(ScreenPosition, 30f, "startRace", JsonConvert.SerializeObject(Horses));
        }

        public static void ShowMainScreen(Player player)
        {
    
            if (IsStartRace)
            {

                Trigger.ClientEvent(player, "showMain");
                Trigger.ClientEvent(player, "setHorses", JsonConvert.SerializeObject(StyleHorses));
                Trigger.ClientEvent(player, "setMainEvent", true);
            }
            else
            {
                Trigger.ClientEvent(player, "setHorses", JsonConvert.SerializeObject(StyleHorses));

                Trigger.ClientEvent(player, "addBetsInside", JsonConvert.SerializeObject(Bets));

                if(ShowHorse != -1)
                {
                    Trigger.ClientEvent(player, "showHorse", ShowHorse);
                }
               
            }

            UpdateBalance(player);
        }

        public static void HideMainScreen(Player player)
        {
            Trigger.ClientEvent(player, "clearPlayers");
        }

        public static void Shuffle(List<int> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static List<int> GetRandomArr(int max)
        {
            Random rand = new Random();

            List<int> arr = new List<int>();

            for(int i = 0; i < 6; i++)
            {
                int val = rand.Next(1, max);

                while(arr.Contains(val))
                    val = rand.Next(1, max);

                arr.Add(val);
            }

            return arr;
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnectedhandler(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (player.HasData("IT_SEAT"))
                {
                    InsideTrack.EixtTable(player);
                    return;
                }
            }
            catch (Exception e)
            {

            }
        }
    }

    class ITBet
    {
        public int Horse = -1;
        public int BetSize = 0;
        public int Payout = 0;
        public string Name;

        [JsonIgnore]
        public Player Pl = null;

        public ITBet(Player player, int horse, int bet, int payout)
        {
            Horse = horse;
            BetSize = bet;
            Payout = payout;
            Pl = player;
            Name = player.Name;
        }
    }
}
