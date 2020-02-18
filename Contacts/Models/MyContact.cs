using SQLite;

namespace Contacts.Models
{
    [Table(nameof(MyContact))]
    public class MyContact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string ProfilePic { get; set; }
        public bool IsFavourite { get; set; }
    }
}
