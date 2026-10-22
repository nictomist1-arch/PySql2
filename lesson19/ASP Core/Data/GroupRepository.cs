using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using college_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace college_DB.Data
{
    class GroupRepository
    {
        private readonly string _conn_str;
        public GroupRepository(string connection_string)
        {
            _conn_str = connection_string;
        }
        public List<Group> GetAllGroups()
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection
                    .Query<Group>("SELECT GroupId, GroupName FROM Groups")
                    .ToList();
            }
        }

        public int CreateGroup(string groupName)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = "INSERT INTO Groups (GroupName) OUTPUT INSERTED.GroupId VALUES (@GroupName)";
                return connection.QuerySingle<int>(sql, new { GroupName = groupName });
            }
        }

        public Group? GetGroupById(string id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                var group = connection.QueryFirstOrDefault<Group>(
                    "SELECT GroupId, GroupName FROM Groups WHERE GroupId = @id",
                    new { id });

                if (group != null)
                {
                    group.Students = connection.Query<Student>(
                        "SELECT StudentId, FirstName, LastName, Age FROM Students WHERE GroupId = @id",
                        new { id }).ToList();
                }

                return group;
            }
        }

        public List<Group> GetAllGroups2()
        {
            var groups = new List<Group>();
            var connection = new SqlConnection(_conn_str);
            connection.Open();
            string sql = "SELECT GroupId, GroupName FROM Groups";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                groups.Add(
                   new Group
                   {
                       GroupId = reader.GetInt32(0),
                       GroupName = reader.GetString(1),   
                   }
                );
            }
            connection.Close();
            return groups;
        }
    }
}
