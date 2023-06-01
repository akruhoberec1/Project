using AnimalTest.Repository.Common;
using AnimalTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalTest.Models;
using Npgsql;
using System.Configuration;

namespace AnimalTest.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public Employee GetEmployeeById(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Salary, b.Certified FROM Employee as b INNER JOIN Person as a ON a.Id = b.Id WHERE a.Id=@Id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);


                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Employee employee = new Employee();
                    employee.Id = id;
                    employee.FirstName = (string)reader["FirstName"];
                    employee.LastName = (string)reader["LastName"];
                    employee.OIB = (string)reader["OIB"];
                    employee.Salary = (decimal)reader["Salary"];
                    employee.Certified = (bool)reader["Certified"];

                    return employee;
                }

                return null;
            }

        }
    }
}
