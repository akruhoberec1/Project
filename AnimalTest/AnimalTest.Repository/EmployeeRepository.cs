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
using System.Net;
using System.Linq.Expressions;
using System.Collections.Specialized;

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

        /*
         * INSERT METHOD
         * */
        public bool CreateEmployee(Employee employee) { 

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                try
                {

                    Guid id = Guid.NewGuid();
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();


                    NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Person (Id, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)", connection);

                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("OIB", employee.OIB);

                    int affectedRowsPerson = cmd.ExecuteNonQuery();



                    NpgsqlCommand cmdEmployee = new NpgsqlCommand($"INSERT INTO Employee (Id, Salary, Certified) VALUES(@Id,@Salary,@Certified)", connection);

                    cmdEmployee.Parameters.AddWithValue("Id", id);
                    cmdEmployee.Parameters.AddWithValue("Salary", employee.Salary);
                    cmdEmployee.Parameters.AddWithValue("Certified", employee.Certified);

                    int affectedRowsEmployee = cmdEmployee.ExecuteNonQuery();

                    transaction.Commit();
                    


                    if (affectedRowsPerson > 0 && affectedRowsEmployee > 0)
                    {
                        return true;
                    }

                    transaction.Rollback();
                    return false;

                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        /*
         * UPDATE METHOD HERE
         * */

        public bool UpdateEmployee(Guid id, Employee employee)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                StringBuilder queryBuilderPerson = new StringBuilder();
                NpgsqlCommand cmd = new NpgsqlCommand("", connection);
                queryBuilderPerson.Append("UPDATE Person SET ");
                connection.Open();
                char commaToRemove = ',';

                NpgsqlTransaction transaction = connection.BeginTransaction();
                //person values
                queryBuilderPerson.Append("FirstName = @firstName,");
                cmd.Parameters.AddWithValue("@firstName", employee.FirstName);
                queryBuilderPerson.Append(" LastName = @lastName,");
                cmd.Parameters.AddWithValue("@lastName", employee.LastName);
                queryBuilderPerson.Append(" OIB = @OIB,");
                cmd.Parameters.AddWithValue("@OIB", employee.OIB);

                string queryPerson = queryBuilderPerson.ToString().TrimEnd(commaToRemove);
                StringBuilder finalPersonQuery = new StringBuilder();
                finalPersonQuery.Append(queryPerson);

                finalPersonQuery.Append(" WHERE Id = @id");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandText = finalPersonQuery.ToString();
                cmd.ExecuteNonQuery();

                int affectedRowsPerson = cmd.ExecuteNonQuery();


                //employee values

                StringBuilder queryBuilderEmployee = new StringBuilder();
                NpgsqlCommand cmdEmployee = new NpgsqlCommand("", connection);
                queryBuilderEmployee.Append("UPDATE Employee SET ");

                queryBuilderEmployee.Append("Salary = @salary,");
                cmdEmployee.Parameters.AddWithValue("@salary", employee.Salary);
                queryBuilderEmployee.Append("Certified = @certified,");
                cmdEmployee.Parameters.AddWithValue("@certified", employee.Certified);


                string queryEmployee = queryBuilderEmployee.ToString().TrimEnd(commaToRemove);
                StringBuilder finalEmployeeQuery = new StringBuilder();
                finalEmployeeQuery.Append(queryEmployee);

                finalEmployeeQuery.Append(" WHERE Id = @id");
                cmdEmployee.Parameters.AddWithValue("@id", id);
                cmdEmployee.CommandText = finalEmployeeQuery.ToString();
                cmdEmployee.ExecuteNonQuery();

                int affectedRowsEmployee = cmdEmployee.ExecuteNonQuery();

                transaction.Commit();

                if(affectedRowsPerson > 0 && affectedRowsEmployee > 0)
                {
                    return true;
                }  
                return false;   
            }
        }
            
            
        /*
         * GET ALL METHOD HERE
         * */
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            

            using (connection)
            {

                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Salary, b.Certified FROM Employee as b INNER JOIN Person as a ON a.Id = b.Id", connection);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //popunimo listu objektima
                        employees.Add(new Employee()
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            OIB = reader["OIB"].ToString(),
                            Salary = (decimal)reader["Salary"],
                            Certified = (bool)reader["Certified"]

                        });
                    }
                    return employees;
                }            
            return null;    
            }
        }

        //napraviti delete
        /*
         * DELETE METHOD
         * */

        public bool DeleteEmployee(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Employee WHERE Id=@id", connection);
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                NpgsqlCommand cmdPerson = new NpgsqlCommand("DELETE FROM Person WHERE Id=@id", connection);
                cmdPerson.Transaction = transaction;
                cmdPerson.Parameters.AddWithValue("@id", id);
                cmdPerson.ExecuteNonQuery();

                transaction.Commit();

                
                return true;
            }
        }
        

     
    }
}
