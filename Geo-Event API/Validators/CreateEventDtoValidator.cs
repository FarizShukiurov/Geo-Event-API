using FluentValidation;
using GeoEventApi.DTOs;

namespace GeoEventApi.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title cannot be empty")
                .Length(3, 200)
                .WithMessage("Title must be between 3 and 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.EventDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("EventDate cannot be in the past. Please provide a future date");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180");
        }
    }
}
