using System.Collections.Generic;
namespace SecretSanta.Api.Dto
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<User> AssignmentList {get; set;} = new();
        public List<Gift> GiftList {get; set;} = new();

        public static User? ToDto(Data.User? user, List<User>? assignmentList = null, List<Gift>? giftList = null)
        {
            if (user is null) return null;
            return new User
            {
                FirstName = user.FirstName,
                Id = user.UserId,
                LastName = user.LastName,
                AssignmentList = assignmentList ?? new(),
                GiftList = giftList ?? new()
            };
        }

        public static Data.User? FromDto(User? user)
        {
            if (user is null) return null;
            return new Data.User
            {
                UserId = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? ""
            };
        }
    }
}
