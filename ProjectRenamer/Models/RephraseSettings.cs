namespace ProjectRenamer.Models
{
    public class RephraseSettings()
    {
        public string? Path { get; set; } 
        public string? OldWord { get; set; } 
        public string? NewWord { get; set; } 
        public string[]? TypesToOpen { get; set; }
    }
}
