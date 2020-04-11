using AI_Research_1.Helpers;

namespace AI_Research_1.Interfaces
{
    public class Solution
    {
        public ICommand[] FirstCarCommands { get; }
        public ICommand[] SecondCarCommands { get; }
        
        public Solution(ICommand[] firstCarCommands, ICommand[] secondCarCommands)
        {
            FirstCarCommands = firstCarCommands;
            SecondCarCommands = secondCarCommands;
        }
    }
}