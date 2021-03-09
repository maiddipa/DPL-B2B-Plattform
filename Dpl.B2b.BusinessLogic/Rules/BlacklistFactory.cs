using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.CodeParser;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;
using Olma = Dpl.B2b.Dal.Models;
using Newtonsoft.Json;

namespace Dpl.B2b.BusinessLogic.Rules
{
    //TODO Daten in Resource auslagern
    public class BlacklistFactory : IBlacklistFactory
    {
        // TODO Aus dem Cache laden
        private static readonly Blacklist Blacklist = Blacklist.ReadFromFile(Blacklist.BlacklistPath);

        public IEnumerable<Regex> CreateCommonFieldsBlacklist()
        {
            return Blacklist.Values
                .Where(blacklistItem => blacklistItem.FieldName == "*")
                .Select(blackListItem => blackListItem.Regex).ToImmutableList();
        }

        public IEnumerable<Regex> CreateLicensePlateBlacklist([NotNull] string fieldName, [CanBeNull] params string[] companyNameList)
        {
            var licensePlateItems = Blacklist.Values.Where(item => item.FieldName == "LicensePlate").ToList();

            foreach (var item in licensePlateItems)
            {
                if (item.Criterion == null)
                    yield return item.Regex;
                else
                {
                    if (companyNameList == null) continue;

                    foreach (var companyName in companyNameList)
                    {
                        if (companyName == null) continue;
                        foreach (var criteria in item.Criterion.Where(c => c.FieldName == fieldName))
                        {
                            if (criteria.Regex.Match(companyName).Success)
                                yield return item.Regex;
                        }
                    }
                }
            }
        }
    }

    public interface IBlacklistFactory
    {
        IEnumerable<Regex> CreateCommonFieldsBlacklist();
        IEnumerable<Regex> CreateLicensePlateBlacklist(string fieldName, params string[] companyName);
    }
    
    public class Blacklist: Dictionary<string, BlacklistItem>
    {
        private static string BlacklistFilename { get; set; } = "blacklist.json";
        
        private static string root;
        public static string Root
        {
            get => root??=Directory.GetCurrentDirectory();
            private set => root = value;
        }
        
        public static string BlacklistPath => Path.Combine(Root, BlacklistFilename);

        /// <summary>
        /// Store default blacklist to file. If path is null default path is chosen
        /// </summary>
        /// <param name="path">blacklist path</param>
        public static void WriteToFile([CanBeNull] string path=null)
        {
            var blacklist = Create();

            var settings = new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore};
            //AppContext.BaseDirectory
            File.WriteAllText(path ?? BlacklistPath, JsonConvert.SerializeObject(blacklist, Formatting.Indented,settings));
        } 
        
        /// <summary>
        /// Read default blacklist to file. If path is null default path is chosen
        /// On any error default blacklist is return 
        /// </summary>
        /// <param name="path">blacklist path</param>
        public static Blacklist ReadFromFile(string path=null)
        {
            path ??= BlacklistPath;
            
            try
            {
                var settings = new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore};
                var json = File.ReadAllText(path);
                var blacklist = JsonConvert.DeserializeObject<Blacklist>(json, settings);
                
                // deserialize JSON directly from a file
                // using (StreamReader file = File.OpenText(BlacklistPath))
                // {
                //     JsonSerializer serializer = new JsonSerializer(){NullValueHandling = NullValueHandling.Ignore};
                //     blacklist = serializer.Deserialize(file, typeof(Blacklist)) as Blacklist;
                // }

                return blacklist;

            }
            catch (Exception)
            {
                return Create();
            }
        } 
            
        public static Blacklist Create()
        {
            var blacklist = new Blacklist();
            
            blacklist["PAKI"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}p[^a-z]{0,3}a[^a-z]{0,3}k[^a-z]{0,3}i[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["FHG"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}d[^a-z]{0,3}e[^a-z]{0,3}f[^a-z]{0,3}r[^a-z]{0,3}u[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["DEFRU"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}d[^a-z]{0,3}e[^a-z]{0,3}f[^a-z]{0,3}r[^a-z]{0,3}u[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["DEFRO"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}d[^a-z]{0,3}e[^a-z]{0,3}f[^a-z]{0,3}r[^a-z]{0,3}o[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["ARLA"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}a[^a-z]{0,3}r[^a-z]{0,3}l[^a-z]{0,3}a[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["INTERPAL"] = new BlacklistItem()
            {
                FieldName = "*",
                Regex = new Regex(@"[^a-z]{0,3}i[^a-z]{0,3}n[^a-z]{0,3}t[^a-z]{0,3}e[^a-z]{0,3}r[^a-z]{0,3}p[^a-z]{0,3}a[^a-z]{0,3}l[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };
            blacklist["MILCHUNION"] = new BlacklistItem()
            {
                FieldName = "*",
                Regex = new Regex(@"[^a-z]{0,3}m[^a-z]{0,3}i[^a-z]{0,3}l[^a-z]{0,3}c[^a-z]{0,3}h[^a-z]{0,3}u[^a-z]{0,3}n[^a-z]{0,3}i[^a-z]{0,3}o[^a-z]{0,3}n[^a-z]{0,3}",
                    RegexOptions.IgnoreCase)
            };
            blacklist["MUH"] = new BlacklistItem()
            {
                FieldName = "*", 
                Regex = new Regex(@"[^a-z]{0,3}m[^a-z]{0,3}u[^a-z]{0,3}h[^a-z]{0,3}", RegexOptions.IgnoreCase)
            };

            blacklist["LICENSE_PLATE_NWM"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"(NWM).*(AF).*(343|345|346|347|348|349|350|351|352|353)", RegexOptions.IgnoreCase)
            };
            
            blacklist["LICENSE_PLATE_NWM"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"^(NWM).*(AF).*(343|345|346|347|348|349|350|351|352|353)", RegexOptions.IgnoreCase)
            };
            
            blacklist["LICENSE_PLATE_BIT"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex =  new Regex(@"^(BIT).*(MU).*(26.|336|339|352|374|40.|502|516|527|53.|54.|554|559|56.|57.|58.|617|64.|65.|67.|708|709|71.|72.|738|765)", RegexOptions.IgnoreCase)
            };

            blacklist["KeinGCDKennzeichenFürArla"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"^(GC|DU|SLF|GZ|GT|GS|GL|GG|GD)[^\w]{0,3}D", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex(@"a[^a-z]{0,3}r[^a-z]{0,3}l[^a-z]{0,3}a[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            
            blacklist["KeinBITKennzeichenFürArla"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"^BIT[^\w]{0,3}MU", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex(@"a[^a-z]{0,3}r[^a-z]{0,3}l[^a-z]{0,3}a[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            blacklist["KeinBITKennzeichenFürMuh"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"^BIT[^\w]{0,3}MU", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex(@"[^a-z]{0,3}m[^a-z]{0,3}u[^a-z]{0,3}h[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            
            blacklist["KeinBITKennzeichenFürMilchunion"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"^BIT[^\w]{0,3}MU", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex( @"[^a-z]{0,3}m[^a-z]{0,3}i[^a-z]{0,3}l[^a-z]{0,3}c[^a-z]{0,3}h[^a-z]{0,3}u[^a-z]{0,3}n[^a-z]{0,3}i[^a-z]{0,3}o[^a-z]{0,3}n[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            
            blacklist["KeinNWMKennzeichenFürMUH"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"[^a-z]{0,3}n[^a-z]{0,3}w[^a-z]{0,3}m[^a-z]{0,3}", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex(@"[^a-z]{0,3}m[^a-z]{0,3}u[^a-z]{0,3}h[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            
            blacklist["KeinNWMKennzeichenFürMILCHUNION"] = new BlacklistItem()
            {
                FieldName = "LicensePlate",
                Regex = new Regex(@"[^a-z]{0,3}n[^a-z]{0,3}w[^a-z]{0,3}m[^a-z]{0,3}", RegexOptions.IgnoreCase),
                Criterion = new List<Criteria>()
                {
                    new Criteria()
                    {
                        FieldName = "CompanyName",
                        Regex = new Regex( @"[^a-z]{0,3}m[^a-z]{0,3}i[^a-z]{0,3}l[^a-z]{0,3}c[^a-z]{0,3}h[^a-z]{0,3}u[^a-z]{0,3}n[^a-z]{0,3}i[^a-z]{0,3}o[^a-z]{0,3}n[^a-z]{0,3}", RegexOptions.IgnoreCase)
                    }
                }
            };
            
            return blacklist;
        }

        public static void Init(string rootPath, string blacklistJsonFilename)
        {
            if (!string.IsNullOrEmpty(root))
            {
                Root = rootPath;
            }
            if (!string.IsNullOrEmpty(blacklistJsonFilename))
            {
                BlacklistFilename = blacklistJsonFilename;
            }
        }
    }

    public class BlacklistItem
    {
        public string FieldName { get; set; }
        public Regex Regex { get; set; } 
        public List<Criteria> Criterion { get; set; }
    }

    public class Criteria
    {
        public string FieldName { get; set; }
        public Regex Regex { get; set; }
    }
}