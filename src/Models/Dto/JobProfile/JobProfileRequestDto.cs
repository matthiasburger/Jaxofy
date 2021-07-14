namespace Jaxofy.Models.Dto.JobProfile
{
    public class JobProfileRequestDto
    {
        public long ClientSupplierId { get; set; }
        public string Title { get; set; }
        public decimal Factor { get; set; }
    }
}