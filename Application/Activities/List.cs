using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class ActivitiesEnvelope
        {
            public List<ActivityDto> Activities { get; set; }
            public int ActivityCount { get; set; }
        }

        public class Query : IRequest<ActivitiesEnvelope>
        {

            public Query(int? limit, int? offest, bool isGoing, bool isHost, DateTime? startDate)
            {
                IsGoing = isGoing;
                IsHost = isHost;
                StartDate = startDate ?? DateTime.Now;
                Limit = limit;
                Offest = offest;

            }
            public int? Limit { get; set; }
            public int? Offest { get; set; }
            public DateTime? StartDate { get; set; }
            public bool IsHost { get; set; }
            public bool IsGoing { get; set; }
        }

        public class Handler : IRequestHandler<Query, ActivitiesEnvelope>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            private readonly ISP_Call _spCall;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, ISP_Call spCall)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
                _spCall = spCall;
            }


            public async Task<ActivitiesEnvelope> Handle(Query request,
                 CancellationToken cancellationToken)
            {
                var queryable = _context.Activities
                    .Where(x => x.Date >= request.StartDate)
                    .OrderBy(x => x.Date)
                    .AsQueryable();

                //var list = _spCall.ReturnList<Activity>("GetActivities_SP")
                //    .Where(x => x.Date >= request.StartDate)
                //    .OrderBy(x => x.Date)
                //    .AsQueryable();


                if (request.IsGoing && !request.IsHost)
                {
                    queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName ==
                    _userAccessor.GetCurrentUsername()));
                }

                if (request.IsHost && !request.IsGoing)
                {
                    queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName ==
                    _userAccessor.GetCurrentUsername() && a.IsHost == true));
                }

                var activities = await queryable
                    .Skip(request.Offest ?? 0)
                    .Take(request.Limit ?? 3).ToListAsync();

                return new ActivitiesEnvelope
                {
                    Activities = _mapper.Map<List<Activity>, List<ActivityDto>>(activities),
                    ActivityCount = queryable.Count()
                };
            }
        }
    }
}