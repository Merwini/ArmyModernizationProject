using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.ArmyModernizationProject
{
    public class IncidentWorker_ExpansionAMP : IncidentWorker
    {
        internal List<Faction> ampFactions;
        //internal List<SitePartDef> existingAMPSites;
        internal List<SitePartDef> eligibleAMPSites;
        

        internal Faction faction;
        internal SitePartDef ampDef;
        internal int tile;
        internal Site site;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            //TODO casualty tracking influencing how often raids occur
            return true;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (!TryResolveExpansionFaction(parms))
                return false;

            if (!TryResolveExpansionDef())
                return false;

            if (!TrySelectTile())
                return false;

            if (!TryMakeSite())
                return false;

            if (!TryPlaceSite())
                return false;

            SendStandardLetter(parms, site); //TODO set up letter and translate

            return true;
        }

        internal bool TryResolveExpansionFaction(IncidentParms parms)
        {
            //Early return for debug tool forcing a chosen faction
            if (parms.faction != null && ((parms.faction.def == AMP_DefOf.AMP_FactionHostile) || (parms.faction.def == AMP_DefOf.AMP_FactionFriendly)))
            {
                faction = parms.faction;
                return true;
            }

            //Get a list of AMP factions. Incident fails if no legal targets are available.
            ampFactions = Find.FactionManager.AllFactions.Where(f => (f.def == AMP_DefOf.AMP_FactionFriendly || f.def == AMP_DefOf.AMP_FactionHostile)).ToList();
            if (!AMP_Settings.FriendlyAMPsCanExpand)
            {
                ampFactions = ampFactions.Where(f => f.HostileTo(Find.FactionManager.OfPlayer)).ToList();
            }

            if (ampFactions.NullOrEmpty())
                return false;

            return ampFactions.TryRandomElement(out faction);
        }

        internal virtual bool TryResolveExpansionDef()
        {
            eligibleAMPSites = AMP_Utils.FindEligibleAMPSiteDefsFor(faction);

            if (eligibleAMPSites.NullOrEmpty())
                return false;

            //TODO custom expansion patterns for different storytellers? e.x. has to spawn X minor expansions before doing a major one, random, spawn in a set pattern
            ampDef = eligibleAMPSites.RandomElementByWeight(s => s.selectionWeight);

            return ampDef != null;
        }

        internal bool TrySelectTile()
        {
            TileFinder.TryFindNewSiteTile(out tile, AMP_Settings.minimumExpansionDistance, AMP_Settings.maximumExpansionDistance, false, TileFinderMode.Near);

            return tile >= 0;
        }
        
        internal bool TryMakeSite()
        {
            site = SiteMaker.MakeSite(ampDef, tile, faction);
            site.sitePartsKnown = true; //TODO site that hides information for sites

            return site != null;
        }

        internal bool TryPlaceSite()
        {
            Find.WorldObjects.Add(site);
            return true;
        }
    }

}

