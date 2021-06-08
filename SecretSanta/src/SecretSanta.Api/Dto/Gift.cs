namespace SecretSanta.Api.Dto
{
    public class Gift
    {
        public int GiftId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public int Priority { get; set; }

        public static Gift? ToDto(Data.Gift? gift)
        {
            if (gift is null) return null;
            return new Gift
            {
                GiftId = gift.GiftId,
                Name = gift.Name,
                Url = gift.Url,
                Priority = gift.Priority,
                UserId = gift.UserId,
                Description = gift.Description
            };
        }

        public static Data.Gift? FromDto(Gift? gift)
        {
            if (gift is null) return null;
            return new Data.Gift
            {
                GiftId = gift.GiftId,
                Name = gift.Name ?? "",
                Url = gift.Url,
                Priority = gift.Priority,
                UserId = gift.UserId,
                Description = gift.Description
            };
        }
    }
}
