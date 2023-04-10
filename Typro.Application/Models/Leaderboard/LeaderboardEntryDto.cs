namespace Typro.Application.Models.Leaderboard;

public record LeaderboardEntryDto(
    int Place,
    string Nickname,
    double WordsPerMinute,
    double Accuracy,
    DateTime DateConducted);