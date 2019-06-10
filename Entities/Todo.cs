using System;

namespace ApiCoreNHibernateCrud.Entities
{
    public class Todo
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Completed { get; set; }
        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual DateTime UpdatedAt { get; set; }
    }
}