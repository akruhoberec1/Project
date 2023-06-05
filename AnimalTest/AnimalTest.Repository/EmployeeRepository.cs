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
using AnimalTest.Common;
using PagedList;
using System.Reflection;

namespace AnimalTest.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Salary, b.Certified FROM Employee as b INNER JOIN Person as a ON a.Id = b.Id WHERE a.Id=@Id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);


                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
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
        public async Task<bool> CreateEmployeeAsync(Employee employee) { 

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            bool checkedEmployee = EmployeeValidation(employee);
            if(checkedEmployee == true)
            {
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

                        int affectedRowsPerson = await cmd.ExecuteNonQueryAsync();


                        NpgsqlCommand cmdEmployee = new NpgsqlCommand($"INSERT INTO Employee (Id, Salary, Certified) VALUES(@Id,@Salary,@Certified)", connection);

                        cmdEmployee.Parameters.AddWithValue("Id", id);
                        cmdEmployee.Parameters.AddWithValue("Salary", employee.Salary);
                        cmdEmployee.Parameters.AddWithValue("Certified", employee.Certified);

                        int affectedRowsEmployee = await cmdEmployee.ExecuteNonQueryAsync();

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
            return false;
            
        }

        /*
         * UPDATE METHOD HERE
         * */

        public async Task<bool> UpdateEmployeeAsync(Guid id, Employee employee)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());   

            bool checkedEmployee = EmployeeValidation(employee);
            if(checkedEmployee == true)
            {
                using (connection)
                {
                    StringBuilder queryBuilderPerson = new StringBuilder();
                    NpgsqlCommand cmd = new NpgsqlCommand("", connection);
                    queryBuilderPerson.Append("UPDATE Person SET ");
                    connection.Open();
                    char commaToRemove = ',';

                    NpgsqlTransaction transaction = connection.BeginTransaction();

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
                    await cmd.ExecuteNonQueryAsync();

                    int affectedRowsPerson = await cmd.ExecuteNonQueryAsync();


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
                    await cmdEmployee.ExecuteNonQueryAsync();

                    int affectedRowsEmployee = await cmdEmployee.ExecuteNonQueryAsync();

                    transaction.Commit();

                    if (affectedRowsPerson > 0 && affectedRowsEmployee > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
            
            
        /*
         * GET ALL METHOD HERE
         * */
        public async Task<PagedList<Employee>> GetAllEmployeesFilteredAsync(Paging paging, Sorting sorting, Filtering filtering)
        {
            List<Employee> employees = new List<Employee>();

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            

            using (connection)
            {
                
                connection.Open();

                StringBuilder sqlQuery = new StringBuilder();

                NpgsqlCommand cmd = new NpgsqlCommand("", connection);

                sqlQuery.Append("SELECT a.FirstName, a.LastName, a.OIB, b.Salary, b.Certified FROM Employee as b INNER JOIN Person as a ON a.Id = b.Id");

                if(filtering != null)
                {
                    sqlQuery.Append(" WHERE 1=1");
                    if (!string.IsNullOrEmpty(filtering.SearchQuery))
                    {
                        sqlQuery.Append(" AND a.FirstName ILIKE @SearchQuery AND a.LastName ILIKE @SearchQuery AND b.salary between @minSalary and @maxSalary");
                        cmd.Parameters.AddWithValue("@SearchQuery", "%" + filtering.SearchQuery + "%");
                    }

                    if (filtering.MaxSalary != null)
                    {
                        
                        cmd.Parameters.AddWithValue("maxSalary", filtering.MaxSalary);
                    }

                    if (filtering.MinSalary != null)
                    {
                        cmd.Parameters.AddWithValue("minSalary", filtering.MinSalary);
                    }
                }          

                sqlQuery.Append(" ORDER BY a.@SortBy @OrderBy");

                if (!string.IsNullOrEmpty(sorting.SortBy))
                {

                    cmd.Parameters.AddWithValue("@SortBy", "a." + sorting.SortBy);
                }

                if (!string.IsNullOrEmpty(sorting.OrderBy))
                {
                    cmd.Parameters.AddWithValue("@OrderBy", sorting.OrderBy);
                }


                sqlQuery.Append(" OFFSET @Offset LIMIT @Limit");

                cmd.CommandText = sqlQuery.ToString();
                cmd.Parameters.AddWithValue("@Offset", paging.PageSize * (paging.PageNumber - 1));
                cmd.Parameters.AddWithValue("@Limit", paging.PageSize);


                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee()
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            OIB = reader["OIB"].ToString(),
                            Salary = (decimal)reader["Salary"],
                            Certified = (bool)reader["Certified"]

                        });
                    }

                    //ne mogu poslati totalCount
                    PagedList<Employee> pagedEmployees = new PagedList<Employee>(employees, paging.PageNumber ?? 1, paging.PageSize);
                    return pagedEmployees;
                }            
            return null;    
            }
        }

        //napraviti delete
        /*
         * DELETE METHOD
         * */

        public async Task<bool> DeleteEmployeeAsync(Guid id)
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
                await cmdPerson.ExecuteNonQueryAsync();

                transaction.Commit();

                
                return true;
            }
        }


 
        private static bool OibChecker(string oib)
        {
            if (oib.Length != 11)
            {
                return false; 
            }

            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                int number = int.Parse(oib[i].ToString());
                sum += number;
                sum %= 10;
                if (sum == 0)
                {
                    sum = 10;
                }
                sum *= 2;
                sum %= 11;
            }

            int controlNumber = 11 - sum;
            controlNumber %= 10;

            int lastNumber = int.Parse(oib[10].ToString());

            return controlNumber == lastNumber;
        }

        private static bool EmployeeValidation(Employee employee)
        {
            bool checkedOIB = OibChecker(employee.OIB);

            if (employee.FirstName == null || employee.FirstName == ""  || employee.LastName == null || employee.LastName == ""
               || employee.Salary <= 553 || checkedOIB == false)
            {
                return false;
            }
            return true;
        }




    }

}





//NpgsqlCommand cmdCount = new NpgsqlCommand("", connection);
//StringBuilder countQuery = new StringBuilder();
//countQuery.Append("SELECT COUNT(*) FROM (");
//countQuery.Append(@sqlQuery);
//countQuery.Append(") as TotalCount");
//cmdCount.CommandText = countQuery.ToString();

//cmdCount.Parameters.AddWithValue("@SearchQuery", filtering.SearchQuery);
//cmdCount.Parameters.AddWithValue("@SortBy", sorting.SortBy);
//cmdCount.Parameters.AddWithValue("OrderBy", sorting.OrderBy);

////dobijemo ukupan zbroj
//int totalCount = Convert.ToInt32(await cmdCount.ExecuteScalarAsync());


//dodamo vrijednosti za perPage i koliko preskace
