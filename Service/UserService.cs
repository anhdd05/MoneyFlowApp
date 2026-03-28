using BusinessObject.Models;
using Repositories;
using System;
using System.Threading.Tasks;

namespace Service;

public class UserService : IUserService
{
    private readonly UserRepository _repository = new UserRepository();
    private readonly UserRepository userRepo = new UserRepository();
    private readonly EmailService emailService = new EmailService();
    

    // 1. LOGIN
    public User Login(string email, string password)
    {
        var user = userRepo.GetByEmail(email);
        if (user == null) throw new Exception("Email không tồn tại trong hệ thống!");

        var authUser = userRepo.Login(email, password);
        if (authUser == null) throw new Exception("Mật khẩu không chính xác. Vui lòng thử lại!");

        return authUser;
    }

    // 2. REGISTER
    // 2. REGISTER - ĐÃ SỬA ĐỂ KHÔNG LỖI DATABASE
    public void Register(string fullName, string email, string password)
    {
        
        var existingUser = _repository.GetUserByEmail(email);
        if (existingUser != null)
        {
            throw new Exception("Email này đã được sử dụng! Vui lòng chọn Email khác.");
        }

        // Nếu chưa có thì mới tiến hành tạo mới
        var newUser = new User
        {
            FullName = fullName,
            Email = email,
            Password = password, 
            Role = "User"
        };
        _repository.Add(newUser);
    }

    // 3. CHANGE PASSWORD (Dùng khi đã đăng nhập vào trong App)
    public void ChangePassword(string email, string oldPass, string newPass)
    {
        // Kiểm tra mật khẩu cũ có đúng không
        var user = userRepo.Login(email, oldPass);
        if (user == null) throw new Exception("Mật khẩu cũ không chính xác!");

        // Kiểm tra mật khẩu mới không được trùng mật khẩu cũ
        if (oldPass == newPass) throw new Exception("Mật khẩu mới không được trùng mật khẩu cũ!");

        user.Password = newPass;
        userRepo.Update(user);
    }

    // 4. FORGOT PASSWORD (Gửi mã xác nhận ra Email)
    public async Task ForgotPassword(string email)
    {
        var user = userRepo.GetByEmail(email);
        if (user == null) throw new Exception("Email không tồn tại!");

        // Tạo mã Token 6 số ngẫu nhiên
        user.ResetToken = new Random().Next(100000, 999999).ToString();
        user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);
        userRepo.Update(user);

        string body = $@"
            <h3>Yêu cầu đặt lại mật khẩu</h3>
            <p>Mã xác nhận của bạn là: <b>{user.ResetToken}</b></p>
            <p>Mã này có hiệu lực trong 15 phút. Nếu không phải bạn yêu cầu, hãy bỏ qua email này.</p>";

        try
        {
            await emailService.SendEmail(email, "MoneyFlow - Mã xác nhận đặt lại mật khẩu", body);
        }
        catch
        {
            throw new Exception("Lỗi hệ thống: Không thể gửi mail lúc này!");
        }
    }

    // 5. RESET PASSWORD (Dùng mã Token từ Email để đổi Pass khi quên)
    public void ResetPassword(string email, string token, string newPass)
    {
        var user = userRepo.GetByEmail(email);
        if (user == null) throw new Exception("Email không hợp lệ!");

        if (user.ResetToken != token)
            throw new Exception("Mã xác nhận (Token) không chính xác!");

        if (user.ResetTokenExpiry < DateTime.Now)
            throw new Exception("Mã xác nhận đã hết hạn hiệu lực!");

        user.Password = newPass;
        user.ResetToken = null; // Xóa token sau khi dùng xong
        user.ResetTokenExpiry = DateTime.MinValue;
        userRepo.Update(user);
    }
    public (bool success, string message) UpdateProfile(string newName, int userId)
    {
        throw new NotImplementedException();
    }
    
    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

}