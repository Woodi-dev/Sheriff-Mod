﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Net;


namespace SheriffMod
{







    [BepInPlugin("org.bepinex.plugins.SheriffMod", "Sheriff Mod", "1.2.3.0")]
    public class SheriffMod : BasePlugin
    {
        public static BepInEx.Logging.ManualLogSource log;
        private readonly Harmony harmony;
        public static ConfigEntry<string> Name { get; set; }
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }


        public SheriffMod()
        {
            log = Log;
            this.harmony = new Harmony("Sheriff Mod");

        }
        public override void Load()
        {
            log.LogMessage("Sheriff Mod loaded");

            Name = Config.Bind("Custom", "Name", "Custom");
            Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
            Port = Config.Bind("Custom", "Port", (ushort)22023);

            var defaultRegions = ServerManager.DefaultRegions.ToList();
            var ip = Ip.Value;
            if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
            {
                log.LogMessage("Resolving " + ip + " ...");
                try
                {
                    foreach (IPAddress address in Dns.GetHostAddresses(Ip.Value))
                    {
                        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ip = address.ToString(); break;
                        }
                    }
                }
                catch
                {
                    log.LogMessage("Hostname could not be resolved" + ip);
                }

                log.LogMessage("IP is " + ip);

            }


            var port = Port.Value;
        
            //adding custom server region
            defaultRegions.Insert(defaultRegions.Count, new StaticRegionInfo(
                Name.Value, (StringNames)CustomStringNames.CustomServerName, ip, new[]
                {
                    new BHEGGGJBBNF($"{Name.Value}-Master-1", ip, port)
                }).Duplicate()
            ) ;

            ServerManager.DefaultRegions = defaultRegions.ToArray();
            

            this.harmony.PatchAll();


        }
    }




   



    







   


   

}


