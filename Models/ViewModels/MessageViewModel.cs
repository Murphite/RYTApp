namespace RYT.Models.ViewModels
{
    public class MessageViewModel
    {
        public string PhotoUrl { get; set; }
        public string LastText { get; set; }
        public string ThreadId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime ReadOn { get; set; }
        public DateTime DeliverOn { get; set; }
        public string DeletedOn { get; set; }


    }

    public class MessageThread
    {
        public string PhotoUrl { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string TimeStamp { get; set; }
    }

    public class MVModel
    {
        public List<MessageThread> MessageThreads { get; set; } = new List<MessageThread>();
        public List<MessageViewModel> SideThreads { get; set; } = new List<MessageViewModel>();
        public string CurrentThreadInUse { get; set; }
        public string receiverName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string NewChat { get; set; }
    }
}

