namespace MoneyFlowApp.DTOs // Nhớ đổi namespace cho đúng tên project của bạn
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}