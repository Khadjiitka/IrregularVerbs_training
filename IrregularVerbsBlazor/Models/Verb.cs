namespace IrregularVerbsBlazor.Models;

public class Verb
{
    public int Id { get; set; }
    public string Infinitive { get; set; } = string.Empty;
    public string PastSimple { get; set; } = string.Empty;
    public string PastParticiple { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
}