using System;
using System.Web.Mvc;
using EED.Ui.Web.Helpers.Pagination;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    public class PaginationTest
    {
        private PagingInfo _pageInfo;

        [SetUp]
        public void Set_Up_PaginationTest()
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
        public void Can_Calculate_Total_Number_Of_Pages()
        {
            // Act
            var result = _pageInfo.CalculateNumberOfPages();
            
            // Assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Can_Generate_Page_Links()
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
