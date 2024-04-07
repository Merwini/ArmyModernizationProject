using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace nuff.ArmyModernizationProject
{
    public class AMP_FactionDef : FactionDef
    {
        new public List<AMP_PawnGroupMaker> pawnGroupMakers;
    }
}