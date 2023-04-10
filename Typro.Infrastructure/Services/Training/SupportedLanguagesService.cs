using FluentResults;
using Typro.Application.Services.Training;
using Typro.Application.UnitsOfWork;
using Typro.Domain.Database.Models;

namespace Typro.Infrastructure.Services.Training;

public class SupportedLanguagesService : ISupportedLanguagesService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupportedLanguagesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<SupportedLanguage>>> GetSupportedLanguagesAsync()
    {
        var supportedLanguages =
            await _unitOfWork.SupportedLanguagesRepository.GetSupportedLanguagesAsync();

        return Result.Ok(supportedLanguages);
    }
}