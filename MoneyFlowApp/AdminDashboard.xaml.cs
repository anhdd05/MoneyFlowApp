using System;
using System.Collections.Generic;
using System.Windows;
using BusinessObjects;
using Repositories;

namespace MoneyFlowApp
{
    public partial class AdminDashboard : Window
    {
        // Giả sử ông đã có UserRepository trong project Repositories
        private readonly UserRepository _userRepo = new UserRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Lấy tất cả user từ Database
                List<User> users = _userRepo.GetAll();

                // Hiển thị lên bảng
                UserGrid.ItemsSource = users;

                // Cập nhật con số tổng
                TxtTotalUsers.Text = users.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}