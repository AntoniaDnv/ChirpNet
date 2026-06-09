using ChirpNet.Data.Data;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Tests.Services
{
    [TestFixture]
    public class SearchServiceTests
    {
        private ApplicationDbContext dbContext = null!;
        private SearchService searchService = null!;

        [SetUp]
        public void SetUp()
        {
            this.dbContext =TestDbContextFactory.CreateDbContext();
            this.searchService = new SearchService(this.dbContext); 

        }
        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task SearchAsync_ShouldReturnEmptyResult_WhenKeywordIsNull()
        {
            var result = await this.searchService.SearchAsync(null);
            Assert.That(result.Posts.Count, Is.EqualTo(0));
            Assert.That(result.Users.Count, Is.EqualTo(0));
        }
       
    }
}
