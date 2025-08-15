namespace WorkFlowHR.UI.Areas.Manager.Models.ManagerVMs
{
    public class ManagerIndexVM
    {
        public Dictionary<string, int> ExpenseCounts { get; set; }
        public Dictionary<string, int> AdvanceCounts { get; set; }
        public Dictionary<string, int> LeaveCounts { get; set; }
    }
}
