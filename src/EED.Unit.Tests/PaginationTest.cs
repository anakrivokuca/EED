using EED.Ui.Web.Helpers.Pagination;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace EED.Unit.Tests
{
    [TestFixture]
    public class PaginationTest
    {
        private PagingInfo _pageInfo;

        [SetUp]
        public void SetUp_PaginationTest()
        {
            // Arrange
            _pageInfo = new PagingInfo()
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalNumberOfItems = 27
            };
        }

        [Test]
        public void CalculateTotalNumberOfPages_GivenTwentySevenItemsAndTwoItemsPerPage_ReturnsThreePages()
        {
            // Act
            var result = _pageInfo.CalculateNumberOfPages();
            
            // Assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void PageLinks_GivenThreePages_ReturnsThreePageLinksWithSelectedPage()
        {
            // Arrange
            HtmlHelper htmlHelper = null;
            Func<int, string> pageUrl = i => "Page" + i;
            
            // Act
            var result = htmlHelper.PageLinks(_pageInfo, pageUrl);

            // Assert
            Assert.AreEqual(@"<a href=""Page1"">1</a>" 
                + @"<a class=""selected"" href=""Page2"">2</a>" 
                + @"<a href=""Page3"">3</a>", result.ToString());
        }
    }
}
