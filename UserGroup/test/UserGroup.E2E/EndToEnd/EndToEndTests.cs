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
            var speakers = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(5, speakers.Count());

            await page.ClickAsync("text=Create");

            await page.TypeAsync("input#FirstName", "Michael");
            await page.TypeAsync("input#LastName", "Stokesbary");
            await page.TypeAsync("input#EmailAddress", "mike@stokesbary.me");

            await page.ClickAsync("text=Create");

            // make sure we have 10 + 1 speakers here
            speakers = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(6, speakers.Count());
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

            await page.ScreenshotAsync("speakersStart.png");
            // make sure we have 10 + 1 speakers here
            var speakers = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(6, speakers.Count());

            page.Dialog += (_, args) => args.Dialog.AcceptAsync();

            await page.ClickAsync("body > section > section > section:last-child > a > section > form > button");

            // make sure we have 10 speakers here
            speakers = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(5, speakers.Count());

            await page.ScreenshotAsync("speakersFinal.png");
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

            var sectionText = await page.GetTextContentAsync("body > section > section > section:last-child > a > section > div");

            Assert.AreEqual("Count Rugen", sectionText);
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

        [TestMethod]
        public async Task CreateEvent()
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

            await page.WaitForSelectorAsync("body > section > section > section");
            var events = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(3, events.Count());

            await page.ClickAsync("text=Create");

            await page.TypeAsync("input#Title", "Test Event");
            await page.TypeAsync("input#Description", "Just a quick event created by Playwright");
            await page.TypeAsync("input#Date", "5/13/2021 6 PM");
            await page.TypeAsync("input#Location", "Online Teams");
            // await page.SelectOptionAsync("select#SpeakerId", "2");

            await page.ClickAsync("text=Create");

            await page.WaitForSelectorAsync("body > section > section > section");
            events = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(4, events.Count());
        }
    }
}