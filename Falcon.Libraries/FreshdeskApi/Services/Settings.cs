/*
 * Copyright 2015 Beckersoft, Inc.
 *
 * Author(s):
 *  John Becker (john@beckersoft.com)
 *  
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
namespace FreshdeskApi.Services
{
    public static class Settings
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Freshdesk")]
        public static string FreshdeskApiKey
        {
            get
            {
                //return System.Environment.GetEnvironmentVariable("FRESHDESK_APIKEY");
                // return Environment.GetEnvironmentVariable("FRESHDESK_APIKEY");
                return ConfigurationManager.AppSettings["FRESHDESK_APIKEY"];

            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Freshdesk")]
        public static Uri FreshdeskUri
        {
            get
            {
                //return new Uri(System.Environment.GetEnvironmentVariable("FRESHDESK_URL"));
                //return new Uri(Environment.GetEnvironmentVariable("FRESHDESK_URL"));
                return new Uri(ConfigurationManager.AppSettings["FRESHDESK_URL"]);
            }
        }
    }
}