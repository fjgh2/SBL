namespace SBL.Api.Dtos;
public class UpdateCouponDto
{
    public int Id { get; set; }
    
    public string Code { get; set; }
    
    public double Percentage { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int UsedCount { get; set; }
}
