namespace DasTeamRevolution.Models.Dto.General
{
    public class KeyValueSet<TKey, TValue>
    {
        public TKey Id { get; set; }
        public TValue Value { get; set; }
    }
}