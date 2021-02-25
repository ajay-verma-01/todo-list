// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TodoListService.Models
{
    public partial class Todo
    {
        public long id { get; set; }
        public string userId { get; set; }
        public string description { get; set; }
        public long isCompleted { get; set; }
        public long isActive { get; set; }
    }
}
