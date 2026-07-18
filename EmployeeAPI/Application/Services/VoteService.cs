using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;

namespace EmployeeAPI.Application.Services
{
    // Application/Services/VoteService.cs
    public class VoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IPollRepository _pollRepository;

        public VoteService(IVoteRepository voteRepository, IPollRepository pollRepository)
        {
            _voteRepository = voteRepository;
            _pollRepository = pollRepository;
        }

        public async Task<bool> CastVoteAsync(Guid userId, Guid pollId, Guid optionId)
        {
            // Check if user already voted
            if (await _voteRepository.HasUserVotedAsync(userId, pollId))
                throw new InvalidOperationException("User has already voted on this poll");

            var vote = new Vote
            {
                UserId = userId,
                PollId = pollId,
                OptionId = optionId
            };

            await _voteRepository.AddAsync(vote);

            // Update vote counts
            await _pollRepository.IncrementTotalVotesAsync(pollId);

            return true;
        }

        public async Task<List<Vote>> GetPollVotesAsync(Guid pollId)
        {
            return await _voteRepository.GetVotesByPollAsync(pollId);
        }
    }
}
