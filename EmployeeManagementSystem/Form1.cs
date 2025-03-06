using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EmployeeManagementSystem
{
    public partial class Form1 : Form
    {
        private List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();
        }

        // ADD EMPLOYEE
        private void btnAdd_Click(object sender, EventArgs e)
        {
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
        }

        // EDIT EMPLOYEE
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int editId))
            {
                MessageBox.Show("Invalid ID");
                return;
            }

            var employee = employees.FirstOrDefault(emp => emp.Id == editId);
            if (employee != null)
            {
                employee.Name = txtName.Text.Trim();
                employee.Department = txtDepartment.Text.Trim();
                employee.Email = txtEmail.Text.Trim();

                RefreshEmployeeList();
            }
            else
            {
                MessageBox.Show("Employee not found.");
            }
        }

        // DELETE EMPLOYEE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int deleteId))
            {
                MessageBox.Show("Invalid ID");
                return;
            }

            var employee = employees.FirstOrDefault(emp => emp.Id == deleteId);
            if (employee != null)
            {
                employees.Remove(employee);
                RefreshEmployeeList();
            }
            else
            {
                MessageBox.Show("Employee not found.");
            }
        }

        // SEARCH EMPLOYEE
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

        // REFRESH EMPLOYEE LIST (helper method)
        private void RefreshEmployeeList(List<Employee> listToShow = null)
        {
            lstEmployees.Items.Clear();
            var list = listToShow ?? employees;

            foreach (var emp in list)
            {
                lstEmployees.Items.Add($"{emp.Id} - {emp.Name} - {emp.Department} - {emp.Email}");
            }
        }
    }
}
