using Typro.Domain.Enums.Training;

namespace Typro.Domain.Database.Models;

public class TrainingConfiguration : BaseIdEntity
{
    public bool IsPunctuationEnabled { get; set; }
    public bool AreNumbersEnabled { get; set; }
    public WordsModeType WordsModeType { get; set; }
    public TimeModeType TimeModeType { get; set; }
    public int LanguageId { get; set; }
}