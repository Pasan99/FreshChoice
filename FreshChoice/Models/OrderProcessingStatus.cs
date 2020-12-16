using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreshChoice.Models
{
    public class OrderProcessingStatus
    {
        public static int PLACED = 1;
        public static int ONSALES = 2;
        public static int READY = 3;
        public static int COMPLETED = 4;
        public static int DELIVERED = 5;
    }
}