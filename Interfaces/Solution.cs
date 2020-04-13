using System.Collections.Generic;
using System.Linq;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        public IEnumerable<Command> FirstCarCommandsList { get; }
        public IEnumerable<Command> SecondCarCommandsList { get; }

        public Solution(IEnumerable<Command> firstCarCommandsList, IEnumerable<Command> secondCarCommandsList)
        {
            FirstCarCommandsList = firstCarCommandsList;
            SecondCarCommandsList = secondCarCommandsList;
        }
        
        public Command FirstCarCommand => FirstCarCommandsList.First();
        public Command SecondCarCommand => SecondCarCommandsList.First();
        public Solution GetNextTick() => new Solution(FirstCarCommandsList.Skip(1), SecondCarCommandsList.Skip(1));
    }
}