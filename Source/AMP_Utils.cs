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
    public static class AMP_Utils
    {
        public static List<Faction> ReturnAMPFactions()
        {
            List<Faction> amps = Find.FactionManager.AllFactions.Where(f => (f.def == AMP_DefOf.AMP_FactionFriendly || f.def == AMP_DefOf.AMP_FactionHostile)).ToList();

            return amps;
        }

        public static List<Faction> ReturnHostileAMPFactions()
        {
            List<Faction> hamps = ReturnAMPFactions().Where(f => f.HostileTo(Find.FactionManager.OfPlayer)).ToList();

            return hamps;
        }

        public static List<Site> FindAMPSitesFor(Faction fact)
        {
            List<Site> ampSites = new List<Site>();

            List<Site> worldSites = Find.WorldObjects.Sites;
            foreach (Site site in worldSites)
            {
                if (site == null)
                    continue;
                if (site.Faction != fact)
                    continue;
                if (site.MainSitePartDef == null)
                    continue;
                if (site.MainSitePartDef.tags.Any(t => t.StartsWith("AMP_")))
                {
                    ampSites.Add(site);
                    continue;
                }
            }

            return ampSites;
        }

        public static List<SitePartDef> FindAMPSiteDefsFor(Faction fact)
        {
            List<SitePartDef> ampSitePartDefs = new List<SitePartDef>();

            List<Site> ampSites = FindAMPSitesFor(fact);

            if (!ampSites.NullOrEmpty())
            {
                foreach (Site site in ampSites)
                {
                    ampSitePartDefs.Add(site.MainSitePartDef);
                }
            }

            return ampSitePartDefs;
        }
    }
}