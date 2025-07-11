﻿using AutoMapper;
using Classifieds.Data.DTOs.BidDtos;
using Classifieds.Data.Entities;
using Classifieds.Data.Enums;
using Classifieds.Repository;
using Classifieds.Services.IServices;
using Classifieds.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Classifieds.Services.Services
{
    public class BidService : IBidService
    {
        private readonly IDBRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHubContext<AuctionHub> _auctionHub;

        public BidService(IDBRepository repository, IMapper mapper, IHubContext<AuctionHub> auctionHub)
        {
            _repository = repository;
            _mapper = mapper;
            _auctionHub = auctionHub;
        }
        public async Task<BidDto?> CreateBidAsync(CreateBidRequest request, Guid userId)
        {
            var post = await _repository.FindForUpdateAsync<Post>(s => s.Id == request.PostId);
            if (post == null)
            {
                throw new Exception("Post is not existed");
            }
            if(post.AuctionStatus == AuctionStatus.Closed)
            {
                throw new Exception("Post's auction is closed");
            }
            
            Bid bid = new Bid
            {
                PostId = request.PostId,
                BidderId = userId,
                Amount = request.Amount,
                BidTime = DateTime.UtcNow
            };
            if (post.StartAmount >= request.Amount)
            {
                throw new Exception("Can't bid with the value less than start value");
            }
            if (post.CurrentAmount >= request.Amount)
            {
                throw new Exception("Can't bid with the value less than current value");
            }
            post.CurrentAmount = bid.Amount;
            post.CurrentBidderId = userId;
            var entity = await _repository.AddAsync(bid);
            await _repository.UpdateAsync(post);
            
            if (entity != null)
            {
                var dto = _mapper.Map<BidDto>(entity);
                await _auctionHub.Clients.Group($"Post-{post.Id}").SendAsync("BidPlaced", dto);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BidDto>> GetBidsOfPost(Guid postId)
        {
            var bids = await _repository.GetAsync<Bid, BidDto>(s => new BidDto
            {
                PostId = s.PostId,
                Amount= s.Amount,
                User = new Data.DTOs.UserDto
                {
                    Id = s.User.Id,
                    AccountName = s.User.AccountName,
                    Email = s.User.Email,
                    Avatar = s.User.Avatar,
                    CreatedAt = s.User.CreatedAt,
                    PhoneNumber = s.User.PhoneNumber,
                    Name = s.User.Name, 
                    Role = s.User.Role,
                }
                
            } ,s => s.PostId == postId);
            if(bids != null)
            {
                bids = bids.OrderByDescending(b => b.Amount).Take(10).ToList();
            }
            return bids;

        }
    }
}
