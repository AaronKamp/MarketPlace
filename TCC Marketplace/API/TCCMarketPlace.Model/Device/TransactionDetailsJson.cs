using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCMarketPlace.Model
{
    public class LatestTransactionDetails
    {
        public int Transaction_Id { get; set; }

        public int Service_Id { get; set; }

        public int User_Id { get; set; }

        public int Thermostat_Id { get; set; }

        public int Status_Id { get; set; }
    }

    public class TransactionDetails : LatestTransactionDetails
    {
        public int User_Transaction_Id { get; set; }

        public string InApp_Purchase_Id { get; set; }

        public int Device_Type { get; set; }
    }
}
