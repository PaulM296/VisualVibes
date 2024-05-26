namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Image : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
