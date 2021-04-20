using System;
using System.Collections.Generic;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Data
{
    public static class MockData
    {
        public static List<UserViewModel> Users = new List<UserViewModel>{
            new UserViewModel {Id = 0, FirstName = "Princess", LastName = "Buttercup"},
            new UserViewModel {Id = 1, FirstName = "Inigo", LastName = "Montoya", }
        };


        public static List<GroupViewModel> Groups = new List<GroupViewModel>{
            new GroupViewModel {Name = "The best group"},
            new GroupViewModel {Name = "The second best group"}
        };

        public static List<GiftViewModel> Gifts = new List<GiftViewModel>{
            new GiftViewModel {Id = 0, Title = "Really Cute Gift", Description = "Simple Description", Url = "www.gifturl.com", UserId = 0},
            new GiftViewModel {Id = 1, Title = "Nice new Barbeque", Description = "Sick Description", Url = "www.gifturl.com", UserId = 1},
            new GiftViewModel {Id = 2, Title = "New Clothes", Description = "Adorable", Url = "www.gifturl.com", UserId = 0},
            new GiftViewModel {Id = 3, Title = "Gift Card to Lowes", Description = "Good for tools", Url = "www.gifturl.com", UserId = 1},
            new GiftViewModel {Id = 4, Title = "Smoked Salmon", Description = "Yummy", Url = "www.gifturl.com", UserId = 0}
        };
    }
}