using AdminService.Application.DTOs;
using AdminService.Application.Interfaces.Repositories;
using AdminService.Application.Services;
using AdminService.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AdminService.Tests.Services
{
[TestFixture]
    public class CampaignServiceTests
    {
        private Mock<ICampaignRepository> _campaignsMock;
        private Mock<IActivityLogRepository> _logsMock;
        private Mock<ILogger<CampaignServiceImpl>> _loggerMock;
        private CampaignServiceImpl _service;

        [SetUp]
        public void SetUp()
        {
            _campaignsMock = new Mock<ICampaignRepository>();
            _logsMock = new Mock<IActivityLogRepository>();
            _loggerMock = new Mock<ILogger<CampaignServiceImpl>>();

            _service = new CampaignServiceImpl(
                _campaignsMock.Object,
                _logsMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var campaigns = new List<CampaignDto>
            {
                new CampaignDto { Id = Guid.NewGuid(), Name = "Summer Promo" }
            };
            var paginatedResult = new PaginatedResult<CampaignDto>
            {
                Items = campaigns,
                Page = 1,
                TotalCount = 1,
                PageSize = 10
            };
            
            _campaignsMock.Setup(x => x.GetPagedAsync(1, 10, null))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _service.GetAllAsync(1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Name.Should().Be("Summer Promo");
        }

        [Test]
        public async Task GetByIdAsync_WhenExists_ShouldReturnCampaign()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var campaign = new Campaign { Id = campaignId, Name = "Black Friday" };
            
            _campaignsMock.Setup(x => x.FindByIdAsync(campaignId))
                .ReturnsAsync(campaign);

            // Act
            var result = await _service.GetByIdAsync(campaignId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(campaignId);
            result.Name.Should().Be("Black Friday");
        }

        [Test]
        public async Task GetByIdAsync_WhenNotExists_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _campaignsMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Campaign?)null);

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Campaign not found.");
        }

        [Test]
        public async Task CreateAsync_ValidRequest_ShouldCreateCampaign()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var request = new CreateCampaignRequest
            {
                Name = "New Year Promo",
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(10),
                TriggerType = "TopUp"
            };

            _campaignsMock.Setup(x => x.ExistsByNameAsync(request.Name)).ReturnsAsync(false);

            // Act
            var result = await _service.CreateAsync(adminId, request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            
            _campaignsMock.Verify(x => x.AddAsync(It.Is<Campaign>(c => c.Name == request.Name)), Times.Once);
            _campaignsMock.Verify(x => x.SaveAsync(), Times.Once);
            _logsMock.Verify(x => x.AddAsync(It.Is<AdminActivityLog>(l => l.Action == "CampaignCreated")), Times.Once);
        }

        [Test]
        public async Task CreateAsync_InvalidDates_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var request = new CreateCampaignRequest
            {
                Name = "Invalid Promo",
                StartsAt = DateTime.UtcNow.AddDays(10),
                EndsAt = DateTime.UtcNow // Ends before starts
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(adminId, request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("End date must be after start date.");
        }

        [Test]
        public async Task CreateAsync_DuplicateName_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var request = new CreateCampaignRequest
            {
                Name = "Dupe Promo",
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(10)
            };

            _campaignsMock.Setup(x => x.ExistsByNameAsync(request.Name)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(adminId, request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"A campaign named '{request.Name}' already exists.");
        }

        [Test]
        public async Task UpdateAsync_ValidRequest_ShouldUpdateCampaign()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var adminId = Guid.NewGuid();
            
            var existingCampaign = new Campaign 
            { 
                Id = campaignId, 
                Name = "Old Name" 
            };
            
            var updateRequest = new UpdateCampaignRequest
            {
                Description = "Updated description",
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(5),
                IsActive = false
            };

            _campaignsMock.Setup(x => x.FindByIdAsync(campaignId)).ReturnsAsync(existingCampaign);

            // Act
            var result = await _service.UpdateAsync(campaignId, adminId, updateRequest);

            // Assert
            result.Should().NotBeNull();
            existingCampaign.Description.Should().Be(updateRequest.Description);
            existingCampaign.IsActive.Should().BeFalse();
            
            _campaignsMock.Verify(x => x.SaveAsync(), Times.Once);
            _logsMock.Verify(x => x.AddAsync(It.Is<AdminActivityLog>(l => l.Action == "CampaignUpdated")), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenExists_ShouldDeleteCampaign()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var adminId = Guid.NewGuid();
            var campaign = new Campaign { Id = campaignId, Name = "To Be Deleted" };

            _campaignsMock.Setup(x => x.FindByIdAsync(campaignId)).ReturnsAsync(campaign);

            // Act
            await _service.DeleteAsync(campaignId, adminId);

            // Assert
            _campaignsMock.Verify(x => x.RemoveAsync(campaign), Times.Once);
            _campaignsMock.Verify(x => x.SaveAsync(), Times.Once);
            _logsMock.Verify(x => x.AddAsync(It.Is<AdminActivityLog>(l => l.Action == "CampaignDeleted")), Times.Once);
        }
    }
}
