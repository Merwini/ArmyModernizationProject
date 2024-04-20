using CombatExtended;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using UnityEngine;

namespace nuff.ArmyModernizationProject
{
    public class DamageWorker_SelfDestruct : DamageWorker
    {
		public override DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageResult result = base.Apply(dinfo, victim);
			Thing kamikaze = dinfo.Instigator;
			IntVec3 c = kamikaze.Position;
			ThingDef projectileDef = dinfo.Def.explosionCellMote;

			var projectile = (ProjectileCE)ThingMaker.MakeThing(projectileDef);
			projectile.Launch(
				launcher: kamikaze,
				origin: new Vector2(c.x, c.z)
				);

			kamikaze.Kill();

			return result;
		}
	}
}