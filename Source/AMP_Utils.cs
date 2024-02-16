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
        //TODO rewrite this to return the sites themselves, and a second method to return the defs from that
        public static List<SitePartDef> FindAmpSites()
        {
            List<SitePartDef> activeAmpSitePartDefs = new List<SitePartDef>();

            List<Site> worldSites = Find.WorldObjects.Sites;
            foreach (Site site in worldSites)
            {
                if (site == null)
                    break;

                SitePartDef mainSitePartDef = site.MainSitePartDef;
                if (mainSitePartDef == null)
                    break;

                foreach (string tag in mainSitePartDef.tags)
                {
                    if (tag.StartsWith("amp_"))
                    {
                        activeAmpSitePartDefs.Add(mainSitePartDef);
                    }
                }
            }

            return activeAmpSitePartDefs;
        }
    }
}