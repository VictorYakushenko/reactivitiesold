using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var acttivity = await _context.Activities.FindAsync(request.Id);
                if (acttivity == null)
                {
                    throw new Exception("Could not find activity");
                }

                acttivity.Title = request.Title ?? acttivity.Title;
                acttivity.Description = request.Description ?? acttivity.Description;
                acttivity.Category = request.Category ?? acttivity.Category;
                acttivity.Date = request.Date ?? acttivity.Date;
                acttivity.City = request.City ?? acttivity.City;
                acttivity.Venue = request.Venue ?? acttivity.Venue;

                var succes = await _context.SaveChangesAsync() > 0;
                if (succes) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}