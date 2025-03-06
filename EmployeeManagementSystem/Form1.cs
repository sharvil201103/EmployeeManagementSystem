using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class Form1 : Form
    {
        private List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = employees.Count > 0 ? employees.Max(emp => emp.Id) + 1 : 1;

            Employee newEmployee = new Employee
            {
                Id = newId,
                Name = txtName.Text.Trim(),
                Department = txtDepartment.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };

            employees.Add(newEmployee);
            RefreshEmployeeList();
            ClearInputFields();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int editId))
            {
                MessageBox.Show("Invalid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employee = employees.FirstOrDefault(emp => emp.Id == editId);
            if (employee != null)
            {
                employee.Name = txtName.Text.Trim();
                employee.Department = txtDepartment.Text.Trim();
                employee.Email = txtEmail.Text.Trim();

                RefreshEmployeeList();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Employee not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int deleteId))
            {
                MessageBox.Show("Invalid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employee = employees.FirstOrDefault(emp => emp.Id == deleteId);
            if (employee != null)
            {
                employees.Remove(employee);
                RefreshEmployeeList();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Employee not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            var results = employees.Where(emp =>
                emp.Name.ToLower().Contains(searchTerm) ||
                emp.Department.ToLower().Contains(searchTerm) ||
                emp.Email.ToLower().Contains(searchTerm)
            ).ToList();

            RefreshEmployeeList(results);
        }

        private void RefreshEmployeeList(List<Employee> listToShow = null)
        {
            lstEmployees.Items.Clear();
            var list = listToShow ?? employees;

            foreach (var emp in list)
            {
                lstEmployees.Items.Add($"{emp.Id} - {emp.Name} - {emp.Department} - {emp.Email}");
            }
        }

        private void ClearInputFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtDepartment.Clear();
            txtEmail.Clear();
            txtSearch.Clear();
        }
    }
}
