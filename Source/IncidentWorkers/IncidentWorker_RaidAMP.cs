using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.ArmyModernizationProject
{
	public class IncidentWorker_RaidAMP : IncidentWorker_RaidEnemy
	{
        List<SitePartDef> ampSites;

		protected override bool CanFireNowSub(IncidentParms parms)
        {
			IEnumerable<Faction> amps = Find.FactionManager.AllFactions.Where(f => (f.def == AMP_DefOf.AMP_FactionFriendly || f.def == AMP_DefOf.AMP_FactionHostile)
																					&& f.HostileTo(Find.FactionManager.OfPlayer));

			if (amps.Count() == 0)
            {
				return false;
            }

            ampSites = AMP_Utils.FindAmpSites();

            //TODO casualty tracker - faction will send fewer raids the higher their casualty rate is

            return true;
        }

        public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
        {
            List<RaidStrategyDef> availableRaidStrats = new List<RaidStrategyDef>();

            //fallback strat
            if (ampSites.NullOrEmpty())
            {
                availableRaidStrats.Add(RaidStrategyDefOf.ImmediateAttack);
            }

            //iterate through active amp sites to make a list of what raid strats they enable
            foreach (SitePartDef spd in ampSites)
            {
                foreach (string tag in spd.tags)
                {
                    RaidStrategyDef strategyDef = DefDatabase<RaidStrategyDef>.GetNamedSilentFail(tag);
                    if (strategyDef != null)
                    {
                        availableRaidStrats.Add(strategyDef);
                    }
                }
            }

            //modification of vanilla ResolveRaidStrategy which selects from availableRaidStrats list instead of all in DefDatabase
            if (!CanUseStrategy(parms.raidStrategy))
            {
                Map map = (Map)parms.target;
                if (!availableRaidStrats.Where(def => CanUseStrategy(def)).TryRandomElementByWeight(def => def.Worker.SelectionWeightForFaction(map, parms.faction, parms.points), out parms.raidStrategy))
                {
                    Log.Error($"No raid strategy found, defaulting to ImmediateAttack. Faction={parms.faction.def.defName}, points={parms.points}, groupKind={groupKind}, parms={parms}");
                    parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
                }
            }

            bool CanUseStrategy(RaidStrategyDef def)
            {
                if (def != null && def.Worker.CanUseWith(parms, groupKind))
                {
                    if (parms.raidArrivalMode == null)
                    {
                        if (def.arriveModes != null)
                        {
                            return def.arriveModes.Any(x => x.Worker.CanUseWith(parms));
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}

