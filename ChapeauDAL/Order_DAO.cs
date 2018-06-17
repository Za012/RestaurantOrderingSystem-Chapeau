﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Order_DAO : Base
    {
        public void Db_add_order(int table_id, int emp_id, uint nrOfGuests)
        {
            string query = string.Format("INSERT INTO[ORDER] (table_id, emp_id, nr_of_guests) VALUES(@tableid, @empid, @nrOfGuests)");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@tableid", SqlDbType.Int)
            {
                Value = table_id
            };
            sqlParameters[1] = new SqlParameter("@empid", SqlDbType.Int)
            {
                Value = emp_id
            };
            sqlParameters[2] = new SqlParameter("@nrOfGuests", SqlDbType.Int)
            {
                Value = nrOfGuests
            };
            //ExecuteEditQuery(query, sqlParameters);
        }
        public DataTable Db_select_order(int order_id)
        {
            string query = string.Format("SELECT order_id, order_comment, table_id, emp_id FROM [ORDER] WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ExecuteSelectQuery(query, sqlParameters);
        }

        public void Db_delete_order(int order_id)
        {
            string query = string.Format("DELETE FROM [ORDER] WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            //ExecuteEditQuery(query, sqlParameters);
        }
    }
}
