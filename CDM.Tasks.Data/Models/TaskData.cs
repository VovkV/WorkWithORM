using MicroLite.Mapping;
using MicroLite.Mapping.Attributes;

namespace CDM.Tasks.Data.Models
{
    [Table("Tasks")]
    public class TaskData
    {
        public TaskData(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public TaskData() { }

        [Column("task_id")]
        [Identifier(IdentifierStrategy.Assigned)]
        public int Id { get; set; }

        [Column("task_text")]
        public string Text { get; set; }
    }
}
