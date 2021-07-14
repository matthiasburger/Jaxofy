namespace DasTeamRevolution.Models.Dto.JobProfile
{
    public class JobProfileResponseDto
    {
        public long Id { get; set; }
        public long ClientSupplierId { get; set; }
        public string Title { get; set; }
        public decimal Factor { get; set; }
    }
}