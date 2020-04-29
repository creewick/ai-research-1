using System;
using System.Collections.Generic;

namespace AI_Research_1.Solvers.HillClimbing
{
    public class UniversalHillClimbingSolverTelemetry
    {
        public int TotalBestSolutionsWinsCount = 0;
        public int BestSolutionsWinsCount = 0;
        public int TotalMutationsCount;
        public int MutationsCount;
        public int TotalImprovementsCount;
        public int ImprovementsCount;

        public readonly Dictionary<Type, int> MutationsWinsCounts = new Dictionary<Type, int>();
        public int TicksCount { get; set; }

        public void AddMutationWin(IMutation mutation)
        {
            if (MutationsWinsCounts.ContainsKey(mutation.GetType()))
                MutationsWinsCounts[mutation.GetType()]++;
            else
                MutationsWinsCounts[mutation.GetType()] = 1;
        }
    }
}