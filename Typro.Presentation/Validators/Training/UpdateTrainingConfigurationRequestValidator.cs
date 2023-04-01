using FluentValidation;
using Typro.Domain.Enums.Training;
using Typro.Presentation.Models.Request.Training;

namespace Typro.Presentation.Validators.Training;

public class UpdateTrainingConfigurationRequestValidator : AbstractValidator<UpdateTrainingConfigurationRequest>
{
    public UpdateTrainingConfigurationRequestValidator()
    {
        When(e => e.WordsModeType != WordsModeType.TurnedOff && Enum.IsDefined(e.WordsModeType), () =>
        {
            RuleFor(r => r.TimeModeType)
                .Equal(TimeModeType.TurnedOff)
                .WithMessage("Time mode must be disabled when words mode in on.");
        });
        When(e => e.TimeModeType != TimeModeType.TurnedOff && Enum.IsDefined(e.TimeModeType), () =>
        {
            RuleFor(r => r.WordsModeType)
                .Equal(WordsModeType.TurnedOff)
                .WithMessage("Words mode must be disabled when time mode in on.");
        });

        RuleFor(e => e.WordsModeType)
            .Must((request, _) => Enum.IsDefined(request.WordsModeType))
            .WithMessage("Invalid words mode.");
        
        RuleFor(e => e.TimeModeType)
            .Must((request, _) => Enum.IsDefined(request.TimeModeType))
            .WithMessage("Invalid time mode.");
    }
}