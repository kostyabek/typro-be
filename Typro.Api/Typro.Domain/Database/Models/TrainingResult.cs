using Typro.Domain.Enums.Training;

namespace Typro.Domain.Database.Models;

public class TrainingResult : BaseIdEntity
{
    public float WordsPerMinute { get; set; }
    public float Accuracy { get; set; }
    public int TimeInMilliseconds { get; set; }
    public int CorrectLetters { get; set; }
    public int IncorrectLetters { get; set; }
    public int ExtraLetters { get; set; }
    public int InitialLetters { get; set; }
    public int LanguageId { get; set; }
    public TimeModeType TimeModeType { get; set; }
    public WordsModeType WordsModeType { get; set; }
}
