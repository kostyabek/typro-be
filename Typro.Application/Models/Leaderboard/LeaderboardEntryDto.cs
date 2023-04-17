namespace Typro.Application.Models.Leaderboard;

public record LeaderboardEntryDto(
    long Place,
    string Nickname,
    double WordsPerMinute,
    double Accuracy,
    DateTime DateConducted);