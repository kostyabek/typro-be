using FluentResults;
using Typro.Application.Models.Training;
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

    public async Task<Result<IEnumerable<SupportedLanguageDto>>> GetSupportedLanguagesAsync()
    {
        IEnumerable<SupportedLanguage> supportedLanguages =
            await _unitOfWork.SupportedLanguagesRepository.GetSupportedLanguagesAsync();

        IEnumerable<SupportedLanguageDto> dtos = supportedLanguages.Select(e =>
            new SupportedLanguageDto(e.Id, e.Name,
                e.Name.Equals("english", StringComparison.InvariantCultureIgnoreCase)));

        return Result.Ok(dtos);
    }
}