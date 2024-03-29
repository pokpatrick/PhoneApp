﻿using System;
using System.Collections.Generic;

namespace PhoneApp.Model
{
    public class Profile
    {
        public string family_name { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string link { get; set; }
        public string given_name { get; set; }
        public string id { get; set; }
        public bool? verified_email { get; set; }
    }
}
