using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Training;

public class TrainingConfigurationService(
    IUnitOfWork unitOfWork,
    IUserIdentityService userIdentityService,
    IUserService userService) : ITrainingConfigurationService
{
    public async Task<Result<int>> CreateDefaultTrainingConfigurationAsync()
    {
        int generatedTrainingConfigurationId =
            await unitOfWork.TrainingConfigurationRepository.CreateDefaultTrainingConfigurationAsync();

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<TrainingConfiguration>> GetTrainingConfigurationByIdAsync(int trainingConfigurationId)
    {
        TrainingConfiguration? trainingConfiguration =
            await unitOfWork.TrainingConfigurationRepository
                .GetTrainingConfigurationByIdAsync(trainingConfigurationId);

        return trainingConfiguration is null
            ? Result.Fail(new NotFoundError("Training configuration not found."))
            : Result.Ok(trainingConfiguration);
    }

    public async Task<Result> UpdateTrainingConfigurationAsync(TrainingConfigurationDto dto)
    {
        int userId = userIdentityService.UserId;
        Result<Domain.Database.Models.User>? userResult = await userService.GetUserByIdAsync(userId);
        if (userResult.IsFailed)
        {
            return userResult.ToResult();
        }

        Domain.Database.Models.User? user = userResult.Value;

        var trainingConfiguration = new TrainingConfiguration
        {
            Id = user.TrainingConfigurationId,
            AreNumbersEnabled = dto.AreNumbersEnabled,
            LanguageId = dto.LanguageId,
            TimeModeType = dto.TimeMode,
            WordsModeType = dto.WordsMode,
            IsPunctuationEnabled = dto.IsPunctuationEnabled
        };

        await unitOfWork.TrainingConfigurationRepository.UpdateTrainingConfigurationAsync(trainingConfiguration);

        return Result.Ok();
    }
}