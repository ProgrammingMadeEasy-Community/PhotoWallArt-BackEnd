using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class ResponseMessages
    {
        public const string RECORDFOUND = "Record with ID {0} found successfully.";
        public const string RECORDNOTFOUND = "Record with ID {0} not found.";

        public const string RECORDCREATED = "Record with ID {0} created successfully.";
        public const string RECORDCREATIONFAILED = "Failed to create record with ID {0}.";

        public const string RECORDDELETED = "Record with ID {0} deleted successfully.";
        public const string RECORDDELETIONFAILED = "Failed to delete record with ID {0}.";

        public const string RECORDUPDATED = "Record with ID {0} updated successfully.";
        public const string RECORDUPDATEFAILED = "Failed to update record with ID {0}.";

        public const string SOMEERROROCCURED = "Something went wrong, please contact the admin";
    }

    public class Statuscodes
    {
        public const string GET_UPDATE = "200";
        public const string CREATED = "201";
        public const string DELETED = "204";
        public const string BADREQUEST = "400";
        public const string NOTFOUND = "404";
        public const string SERVER_ERROR = "500";
    }

}
