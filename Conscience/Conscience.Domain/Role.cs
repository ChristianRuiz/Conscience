﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Role
    {
        public int Id
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }
    }

    public enum RoleTypes
    {
        Admin,
        CompanyAdmin,
        CompanyQA,
        CompanyBehaviour,
        CompanyPlot,
        CompanyMaintenance,
        Host,
        Anonymous
    }
}
