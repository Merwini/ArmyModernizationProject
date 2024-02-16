using CombatExtended;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace nuff.ArmyModernizationProject
{
    public static class DebugActions
    {
        [DebugAction("Army Modernization Project", "List Active AMP Sites", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing)]
        public static void ListAmpSites() 
        {
            List<SitePartDef> ampSites = AMP_Utils.FindAmpSites();
            if (ampSites.NullOrEmpty())
            {
                Log.Warning("No active AMP Sites found");
            }
            else
            {
                foreach (SitePartDef spd in ampSites)
                {
                    Log.Warning($"{spd.label}"); //TODO after rewriting the FindAmpSites to return the sites themselves, have this log info about the sites' age etc
                }
            }
        }



    }
}