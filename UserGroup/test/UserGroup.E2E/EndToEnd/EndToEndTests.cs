using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightSharp;
using System.Linq;

namespace UserGroup.Web.Tests
{
    [TestClass]
    public class EndToEndTests
    {
        private static WebHostServerFixture<Web.Startup, UserGroup.Api.Startup> Server;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Server = new();
        }

        [TestMethod]
        public async Task LaunchHomepage()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            var headerContent = await page.GetTextContentAsync("body > header > div > a");
            Assert.AreEqual("User Group", headerContent);
        }

        [TestMethod]
        public async Task CreateSpeaker()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Speakers");

            // make sure we have 10 speakers here
            var speakers = await page.QuerySelectorAllAsync("body > section > section");
            Assert.AreEqual(10, speakers.Count());

            await page.ClickAsync("text=Create");

            await page.TypeAsync("input#FirstName", "Michael");
            await page.TypeAsync("input#LastName", "Stokesbary");
            await page.TypeAsync("input#EmailAddress", "mike@stokesbary.me");

            await page.ClickAsync("text=Create");

            // make sure we have 10 + 1 speakers here
            speakers = await page.QuerySelectorAllAsync("body > section > section");
            Assert.AreEqual(11, speakers.Count());
        }

        [TestMethod]
        public async Task DeleteSpeaker()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Speakers");

            // make sure we have 10 + 1 speakers here
            var speakers = await page.QuerySelectorAllAsync("body > section > section");
            Assert.AreEqual(11, speakers.Count());

            page.Dialog += (_, args) => args.Dialog.AcceptAsync();

            await page.ClickAsync("body > section > section:last-child > form > button");

            // make sure we have 10 speakers here
            speakers = await page.QuerySelectorAllAsync("body > section > section");
            Assert.AreEqual(10, speakers.Count());
        }

        [TestMethod]
        public async Task ValidateLastSpeakerText()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Speakers");

            var sectionText = await page.GetTextContentAsync("body > section > section:last-child");

            Assert.IsTrue(sectionText.Contains("Princess Buttercup"));
        }

        [TestMethod]
        public async Task NavigateToEvents()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Events");

            await page.ScreenshotAsync("events.png");
        }
    }
}