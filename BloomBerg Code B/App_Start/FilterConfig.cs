﻿using System.Web;
using System.Web.Mvc;

namespace BloomBerg_Code_B {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
