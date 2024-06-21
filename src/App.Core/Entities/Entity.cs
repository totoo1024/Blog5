using SqlSugar;

namespace App.Core.Entities
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        [SugarColumn(IsPrimaryKey = true)]
        public virtual TKey Id { get; set; }
    }
}