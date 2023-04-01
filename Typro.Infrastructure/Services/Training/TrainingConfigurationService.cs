using FluentResults;
using Typro.Application.Models.Training;
using Typro.Application.Services.Training;
using Typro.Application.Services.User;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;
using Typro.Domain.Models.Result.Errors;

namespace Typro.Infrastructure.Services.Training;

public class TrainingConfigurationService : ITrainingConfigurationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentityService _userIdentityService;
    private readonly IUserService _userService;

    public TrainingConfigurationService(
        IUnitOfWork unitOfWork,
        IUserIdentityService userIdentityService,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userIdentityService = userIdentityService;
        _userService = userService;
    }

    public async Task<Result<int>> CreateDefaultTrainingConfigurationAsync()
    {
        var generatedTrainingConfigurationId =
            await _unitOfWork.TrainingConfigurationRepository.CreateDefaultTrainingConfigurationAsync();

        return Result.Ok(generatedTrainingConfigurationId);
    }

    public async Task<Result<TrainingConfiguration>> GetTrainingConfigurationByIdAsync(int trainingConfigurationId)
    {
        var trainingConfiguration =
            await _unitOfWork.TrainingConfigurationRepository
                .GetTrainingConfigurationByIdAsync(trainingConfigurationId);

        return trainingConfiguration is null
            ? Result.Fail(new NotFoundError("Training configuration not found."))
            : Result.Ok(trainingConfiguration);
    }

    public async Task<Result> UpdateTrainingConfigurationAsync(TrainingConfigurationDto dto)
    {
        var userId = _userIdentityService.UserId;
        var userResult = await _userService.GetUserByIdAsync(userId);
        if (userResult.IsFailed)
        {
            return userResult.ToResult();
        }

        var user = userResult.Value;

        var trainingConfiguration = new TrainingConfiguration
        {
            Id = user.TrainingConfigurationId,
            AreNumbersEnabled = dto.AreNumbersEnabled,
            LanguageId = dto.LanguageId,
            TimeModeType = dto.TimeMode,
            WordsModeType = dto.WordsMode,
            IsPunctuationEnabled = dto.IsPunctuationEnabled
        };

        await _unitOfWork.TrainingConfigurationRepository.UpdateTrainingConfigurationAsync(trainingConfiguration);

        return Result.Ok();
    }
}