namespace IrregularVerbsApp.Models;

public class IrregularVerb
{
    public int Id { get; set; }

    // V1 (Infinitive), например: "go"
    public string Infinitive { get; set; } = string.Empty;

    // V2 (Past Simple), например: "went"
    public string PastSimple { get; set; } = string.Empty;

    // V3 (Past Participle), например: "gone"
    public string PastParticiple { get; set; } = string.Empty;

    // Перевод на русский
    public string Translation { get; set; } = string.Empty;

    // Уровень сложности (1 - базовый, 2 - средний, 3 - сложный)
    public int Difficulty { get; set; } = 1;
}