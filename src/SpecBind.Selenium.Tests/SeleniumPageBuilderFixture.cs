﻿// <copyright file="SeleniumPageBuilderFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Selenium.Tests
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using SpecBind.Pages;

    /// <summary>
    /// A test fixture for the PageBuilder class.
    /// </summary>
    [TestClass]
    public class SeleniumPageBuilderFixture
    {
        /// <summary>
        /// Tests the create page method.
        /// </summary>
        [TestMethod]
        public void TestCreatePage()
        {
            var driver = new Mock<IWebDriver>(MockBehavior.Strict);

            var listItem = new Mock<IWebElement>(MockBehavior.Loose);
            listItem.Setup(l => l.Displayed).Returns(true);

            var itemList = new ReadOnlyCollection<IWebElement>(new[] { listItem.Object });

            // Setup list mock
            var listElement = new Mock<IWebElement>(MockBehavior.Strict);
            listElement.Setup(l => l.FindElements(By.TagName("LI"))).Returns(itemList);

            // Setup mock for list
            driver.Setup(d => d.FindElement(By.Id("ListDiv"))).Returns(listElement.Object);


            var pageFunc = new SeleniumPageBuilder().CreatePage(typeof(BuildPage));

            var pageObject = pageFunc(driver.Object, null);
            var page = pageObject as BuildPage;

            Assert.IsNotNull(page);
            
            Assert.IsNotNull(page.TestButton);
            AssertLocatorValue(page.TestButton, By.Id("MyControl"));
            
            Assert.IsNotNull(page.CombinedControl);
            AssertLocatorValue(page.CombinedControl, new ByChained(By.Id("Field1"), By.LinkText("The Button")));

            Assert.IsNotNull(page.UserName);
            AssertLocatorValue(page.UserName, By.Name("UserName"));
            
            // Nesting Test
            Assert.IsNotNull(page.MyDiv);
            AssertLocatorValue(page.MyDiv, By.ClassName("btn"));
            
            Assert.IsNotNull(page.MyDiv.InternalButton);
            AssertLocatorValue(page.MyDiv.InternalButton, By.Id("InternalItem"));
            
            //List Test
            Assert.IsNotNull(page.MyCollection);
            Assert.IsInstanceOfType(page.MyCollection, typeof(SeleniumListElementWrapper<IWebElement, ListItem>));

            var propertyList = (SeleniumListElementWrapper<IWebElement, ListItem>)page.MyCollection;
            Assert.IsNotNull(propertyList.Parent);
            AssertLocatorValue(propertyList.Parent, By.Id("ListDiv"));
            
            // Test First Element
            var element = propertyList.FirstOrDefault();
			
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.MyTitle);
            AssertLocatorValue(element.MyTitle, By.Id("itemTitle"));

            listElement.VerifyAll();
            listItem.VerifyAll();
            driver.VerifyAll();
        }

        /// <summary>
        /// Tests the create page method with mixed attributes.
        /// </summary>
        [TestMethod]
        public void TestCreatePageWithNativeAttributes()
        {
            var driver = new Mock<IWebDriver>();

            var pageFunc = new SeleniumPageBuilder().CreatePage(typeof(NativeAttributePage));

            var pageObject = pageFunc(driver.Object, null);
            var page = pageObject as NativeAttributePage;

            Assert.IsNotNull(page);
            Assert.IsNotNull(page.NativeElement);
            AssertLocatorValue(page.NativeElement, By.Id("nativeElement"));
        }

        /// <summary>
        /// Tests the create page method with mixed attributes.
        /// </summary>
        [TestMethod]
        public void TestCreatePageWithCombinedNativeAndLocatorAttributes()
        {
            var driver = new Mock<IWebDriver>();

            var pageFunc = new SeleniumPageBuilder().CreatePage(typeof(NativeAttributePage));

            var pageObject = pageFunc(driver.Object, null);
            var page = pageObject as NativeAttributePage;

            Assert.IsNotNull(page);
            Assert.IsNotNull(page.CombinedElement);
            AssertLocatorValue(page.CombinedElement, new ByChained(By.Id("combined"), By.TagName("DIV")));
        }

        /// <summary>
        /// Tests the create page method with duplicate attributes.
        /// </summary>
        [TestMethod]
        public void TestCreatePageWithDuplicateNativeAndLocatorAttributes()
        {
            var driver = new Mock<IWebDriver>();

            var pageFunc = new SeleniumPageBuilder().CreatePage(typeof(NativeAttributePage));

            var pageObject = pageFunc(driver.Object, null);
            var page = pageObject as NativeAttributePage;

            Assert.IsNotNull(page);
            Assert.IsNotNull(page.DuplicateElement);
            AssertLocatorValue(page.DuplicateElement, By.Id("1234"));
        }

        /// <summary>
        /// Tests the create page method.
        /// </summary>
        [TestMethod]
        public void TestCreatePageWithNativeProperties()
        {
            var driver = new Mock<IWebDriver>(MockBehavior.Strict);

            var listItem = new Mock<IWebElement>(MockBehavior.Loose);
            listItem.Setup(l => l.Displayed).Returns(true);

            var itemList = new ReadOnlyCollection<IWebElement>(new[] { listItem.Object });

            // Setup list mock
            var listElement = new Mock<IWebElement>(MockBehavior.Strict);
            listElement.Setup(l => l.FindElements(By.TagName("LI"))).Returns(itemList);

            // Setup mock for list
            driver.Setup(d => d.FindElement(By.Id("ListDiv"))).Returns(listElement.Object);


            var pageFunc = new SeleniumPageBuilder().CreatePage(typeof(BuildPage));

            var pageObject = pageFunc(driver.Object, null);
            var page = pageObject as BuildPage;

            Assert.IsNotNull(page);

            Assert.IsNotNull(page.TestButton);
            AssertLocatorValue(page.TestButton, By.Id("MyControl"));

            Assert.IsNotNull(page.CombinedControl);
            AssertLocatorValue(page.CombinedControl, new ByChained(By.Id("Field1"), By.LinkText("The Button")));

            Assert.IsNotNull(page.UserName);
            AssertLocatorValue(page.UserName, By.Name("UserName"));

            // Nesting Test
            Assert.IsNotNull(page.MyDiv);
            AssertLocatorValue(page.MyDiv, By.ClassName("btn"));

            Assert.IsNotNull(page.MyDiv.InternalButton);
            AssertLocatorValue(page.MyDiv.InternalButton, By.Id("InternalItem"));

            //List Test
            Assert.IsNotNull(page.MyCollection);
            Assert.IsInstanceOfType(page.MyCollection, typeof(SeleniumListElementWrapper<IWebElement, ListItem>));

            var propertyList = (SeleniumListElementWrapper<IWebElement, ListItem>)page.MyCollection;
            Assert.IsNotNull(propertyList.Parent);
            AssertLocatorValue(propertyList.Parent, By.Id("ListDiv"));

            // Test First Element
            var element = propertyList.FirstOrDefault();

            Assert.IsNotNull(element);
            Assert.IsNotNull(element.MyTitle);
            AssertLocatorValue(element.MyTitle, By.Id("itemTitle"));

            listElement.VerifyAll();
            listItem.VerifyAll();
            driver.VerifyAll();
        }

        /// <summary>
        /// Tests the constructor scenario where there is no argument.
        /// </summary>
        [TestMethod]
        public void TestMissingArgumentConstructor()
        {
            var builder = new SeleniumPageBuilder();

            var pageFunc = builder.CreatePage(typeof(NoConstructorElement));

            Assert.IsNotNull(pageFunc);

            var driver = new Mock<IWebDriver>();
            var page = pageFunc(driver.Object, null);

            Assert.IsNotNull(page);
            Assert.IsInstanceOfType(page, typeof(NoConstructorElement));
        }

        /// <summary>
        /// Tests the type of the generic.
        /// </summary>
        [TestMethod]
        public void TestGenericType()
        {
            var baseType = typeof(IElementList<IWebElement, IWebElement>);
            var concreteType = typeof(SeleniumListElementWrapper<,>).MakeGenericType(baseType.GetGenericArguments());

            Assert.IsTrue(baseType.IsGenericType, "Not a generic type");
            Assert.IsTrue(typeof(IElementList<,>).IsAssignableFrom(baseType.GetGenericTypeDefinition()));
            Assert.AreEqual(typeof(SeleniumListElementWrapper<IWebElement, IWebElement>), concreteType);
        }

        ///// <summary>
        ///// Tests the frame document creation. Save for when frames are supported.
        ///// </summary>
        //[TestMethod]
        //public void TestFrameDocument()
        //{
        //    var docType = typeof(MasterDocument);
        //    var property = docType.GetProperty("FrameNavigation");

        //    var window = new BrowserWindow();
        //    var pageFunc = PageBuilder<BrowserWindow, HtmlControl>.CreateFrameLocator(docType, property);
        //    var page = pageFunc(window);

        //    Assert.IsNotNull(page);
        //    Assert.IsInstanceOfType(page, typeof(HtmlFrame));
        //    Assert.AreEqual("1234", page.SearchProperties[HtmlControl.PropertyNames.Id]);
        //}

        /// <summary>
        /// Asserts the locator value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="findBy">The find by.</param>
        [ExcludeFromCodeCoverage]
        private static void AssertLocatorValue(IWebElement element, By findBy)
        {
            var proxy = element as WebElement;
            if (proxy != null)
            {
                if (proxy.Locators.Any(l => l == findBy))
                {
                    return;
                }

                var properties = proxy.Locators.Count > 0 
                    ? string.Join(", ", proxy.Locators) 
                    : "NONE";
                Assert.Fail(
                    "Element should have contained property '{0}' but did not. Available Properties: {1}", 
                    findBy, 
                    properties);
            }

            Assert.Fail("Instance of this class cannot be inspected.");
        }

        #region Class - FrameDocument

        /// <summary>
        /// A test class for seeing if frames will resolve.
        /// </summary>
        public class MasterDocument
        {
            /// <summary>
            /// Gets or sets the frameNavigation.
            /// </summary>
            /// <value>The frame1.</value>
            [ElementLocator(Id = "1234")]
            public IWebElement FrameNavigation { get; set; }
        }

        #endregion

        #region Class - BuildPage

        /// <summary>
        /// A test class for the page builder
        /// </summary>
        [PageNavigation("/builds")]
        public class BuildPage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BuildPage" /> class.
            /// </summary>
            /// <param name="driver">The driver.</param>
            public BuildPage(IWebDriver driver)
            {
                this.Driver = driver;
            }

            /// <summary>
            /// Gets the driver.
            /// </summary>
            /// <value>The driver.</value>
            public IWebDriver Driver { get; private set; }

            /// <summary>
            /// Gets or sets the test button.
            /// </summary>
            /// <value>
            /// The test button.
            /// </value>
            [ElementLocator(Id = "MyControl")]
            public IWebElement TestButton { get; set; }

            /// <summary>
            /// Gets or sets the combined locator button.
            /// </summary>
            /// <value>
            /// The test button.
            /// </value>
            [ElementLocator(Id = "Field1", Text = "The Button")]
            public IWebElement CombinedControl { get; set; }

            /// <summary>
            /// Gets or sets my div.
            /// </summary>
            /// <value>
            /// My div.
            /// </value>
            [ElementLocator(Class = "btn")]
            public CustomDiv MyDiv { get; set; }

            /// <summary>
            /// Gets or sets the name of the user.
            /// </summary>
            /// <value>
            /// The name of the user.
            /// </value>
            [ElementLocator(Name = "UserName")]
            public IWebElement UserName { get; set; }

            /// <summary>
            /// Gets or sets my collection.
            /// </summary>
            /// <value>
            /// My collection.
            /// </value>
            [ElementLocator(Id = "ListDiv")]
            public IElementList<IWebElement, ListItem> MyCollection { get; set; }
        }

        /// <summary>
        /// A custom div element.
        /// </summary>
        public class CustomDiv : WebElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WebElement" /> class.
            /// </summary>
            /// <param name="searchContext">The driver used to search for elements.</param>
            public CustomDiv(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            /// <summary>
            /// Gets or sets the test button.
            /// </summary>
            /// <value>
            /// The test button.
            /// </value>
            [ElementLocator(Id = "InternalItem")]
            public IWebElement InternalButton { get; set; }
        }

        /// <summary>
        /// An inner list item.
        /// </summary>
        [ElementLocator(TagName = "LI")]
        public class ListItem : WebElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WebElement" /> class.
            /// </summary>
            /// <param name="searchContext">The driver used to search for elements.</param>
            protected internal ListItem(ISearchContext searchContext)
                : base(searchContext)
            {
            }

            /// <summary>
            /// Gets or sets my title.
            /// </summary>
            /// <value>
            /// My title.
            /// </value>
            [ElementLocator(Id = "itemTitle")]
            public IWebElement MyTitle { get; set; }
        }

        #endregion

        #region Class - NativeAttributePage

        /// <summary>
        /// A test class with native combined and duplicated attributes.
        /// </summary>
        public class NativeAttributePage
        {
            /// <summary>
            /// Gets or sets an element that has combined properties.
            /// </summary>
            /// <value>The element.</value>
            [ElementLocator(Id = "combined")]
            [FindsBy(How = How.TagName, Using = "DIV")]
            public IWebElement CombinedElement { get; set; }

            /// <summary>
            /// Gets or sets an element that has duplicate properties.
            /// </summary>
            /// <value>The element.</value>
            [ElementLocator(Id = "1234")]
            [FindsBy(How = How.Id, Using = "1234")]
            public IWebElement DuplicateElement { get; set; }

            /// <summary>
            /// Gets or sets an element that has only native properties.
            /// </summary>
            /// <value>The element.</value>
            [FindsBy(How = How.Id, Using = "nativeElement")]
            public IWebElement NativeElement { get; set; }
        }

        #endregion

        #region Class - NoConstructorElement

        /// <summary>
        /// An invalid class that has no constructor.
        /// </summary>
        public class NoConstructorElement
        {
        }

        #endregion
    }
}