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
        private const string FilePath = "employees.xml";

        public Form1()
        {
            InitializeComponent();
            LoadEmployees();
        }

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
            SaveEmployees();
            ClearFields();
        }

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
                SaveEmployees();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Employee not found.");
            }
        }

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
                SaveEmployees();
            }
            else
            {
                MessageBox.Show("Employee not found.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            var results = employees.Where(emp =>
                emp.Id.ToString() == searchTerm ||
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

        private void SaveEmployees()
        {
            var xDocument = new XDocument(
                new XElement("Employees",
                    employees.Select(emp =>
                        new XElement("Employee",
                            new XElement("Id", emp.Id),
                            new XElement("Name", emp.Name),
                            new XElement("Department", emp.Department),
                            new XElement("Email", emp.Email)
                        )
                    )
                )
            );

            xDocument.Save(FilePath);
        }

        private void LoadEmployees()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                employees = new List<Employee>();
                return;
            }

            var xDocument = XDocument.Load(FilePath);

            employees = xDocument.Root.Elements("Employee")
                .Select(emp => new Employee
                {
                    Id = int.Parse(emp.Element("Id").Value),
                    Name = emp.Element("Name").Value,
                    Department = emp.Element("Department").Value,
                    Email = emp.Element("Email").Value
                })
                .ToList();

            RefreshEmployeeList();
        }

        private void ClearFields()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtDepartment.Text = "";
            txtEmail.Text = "";
        }
    }
}
