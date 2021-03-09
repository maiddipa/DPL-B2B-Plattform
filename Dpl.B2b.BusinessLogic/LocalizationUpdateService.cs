#region #region Copyright © 2019 DPL Systems, Incorporated

// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: LocalizationService.cs
// Date:     05/12/2019
// Author:   Schlicht

#endregion

using Dpl.B2b.BusinessLogic.Model.PoEditTermsList;
using Dpl.B2b.Contracts;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DevExpress.Office.Internal;
using DevExpress.Utils.Text;
using Dpl.B2b.Contracts.Localizable;
using Olma = Dpl.B2b.Dal.Models;


namespace Dpl.B2b.BusinessLogic
{
    public class LocalizationUpdateService : ILocalizationUpdateService
    {
        //POEdit from Alexander
        private static readonly string POEDIT_BASE_URL = "https://api.poeditor.com/v2";
        private static readonly string POEDIT_API_TOKEN = "33635e136120c4d10d168cd0a3b227ba ";
        private static readonly string POEDIT_PROJECT_ID = "300403";


        private readonly OlmaDbContext _olmaDbContext;
        private readonly IRepository<Olma.LocalizationItem> _localizationItemRepo;
        private readonly IRepository<Olma.LocalizationLanguage> _localizationLanguageRepo;
        private readonly IRepository<Olma.LocalizationText> _localizationTextRepo;

        public LocalizationUpdateService(IRepository<Olma.LocalizationItem> localizationItemRepo,
            IRepository<Olma.LocalizationText> localizationTextRepo,
            IRepository<Olma.LocalizationLanguage> localizationLanguageRepo,
            OlmaDbContext olmaDbContext)
        {
            _localizationItemRepo = localizationItemRepo;
            _localizationTextRepo = localizationTextRepo;
            _localizationLanguageRepo = localizationLanguageRepo;

            _olmaDbContext = olmaDbContext;
        }

        #region Private Functions

        private static IEnumerable<LocalizationView> CreateLocalizationIds(IEnumerable<RecordType> records)
        {
            string BuildTermId(RecordType i, string id)
            {
                return i.WithinDatabase
                    ? string.IsNullOrEmpty(i.FieldName)
                        ? $"{i.Entity}|{id}"
                        : $"{i.Entity}|{id}|{i.FieldName}"
                    : string.IsNullOrEmpty(i.FieldName)
                        ? $"{id}"
                        : $"{id}|{i.FieldName}";
            }

            return records.SelectMany(i => i.Ids.Select(id => new LocalizationView()
            {
                FullName = i.FullName,
                Entity = i.Entity,
                Id = id,
                LocalizationItemType = i.LocalizationItemType.GetValueOrDefault(),
                TermId = BuildTermId(i, id),
                FieldName = i.FieldName,
                Comment = i.Comment
            }));
        }

        private List<Type> GetAllLocalizableMessageTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(type => typeof(ILocalizableMessage).IsAssignableFrom(type)
                               && !type.IsInterface
                               && !type.IsAbstract)
                .Select(x => x)
                .ToList();
        }

        private List<RecordType> CreateEntityRecords(
            ILookup<Olma.LocalizationItemType, Olma.LocalizationItem> localizationItemsLookup)
        {
            var fieldName = "Id";
            var entityNames = localizationItemsLookup[Olma.LocalizationItemType.Entity];

            var records = new List<RecordType>();

            foreach (var item in entityNames)
            {
                var tableName = item.Name;
                var sqlQuery = $"SELECT {fieldName} from {tableName}";

                var record = new RecordType();

                record.LocalizationItemType = Olma.LocalizationItemType.Entity;
                record.WithinDatabase = true;
                record.FullName = tableName;
                record.Entity = tableName;
                record.FieldName = item.FieldName;
                record.Ids = _olmaDbContext.LocalizationItems
                    .FromSqlRaw(sqlQuery)
                    .Select(i => i.Id.ToString()).ToList();

                records.Add(record);
            }

            return records;
        }

        private List<RecordType> CreateEnumsRecords(
            ILookup<Olma.LocalizationItemType, Olma.LocalizationItem> localizationItemsLookup)
        {
            var enumItems = localizationItemsLookup[Olma.LocalizationItemType.Enum].ToList();

            var types = GetEnumTypes(enumItems
                    .Select(i => i.Reference)
                    .Distinct())
                .ToDictionary(k => k.FullName, k => k);

            var records = new List<RecordType>();

            foreach (var item in enumItems)
            {
                var record = new RecordType();

                var type = types[item.Reference];
                record.LocalizationItemType = Olma.LocalizationItemType.Enum;
                record.WithinDatabase = true;
                record.FullName = item.Reference;
                record.Entity = item.Name;
                record.FieldName = item.FieldName;
                record.Ids = Enum.GetNames(type).ToList();

                records.Add(record);
            }

            return records;
        }

        private List<RecordType> CreateServerSideRecords()
        {
            var records = new List<RecordType>();

            var localizableMessages = GetAllLocalizableMessageTypes();
            foreach (var localizableMessage in localizableMessages)
            {
                var record = new RecordType();

                if (!(Activator.CreateInstance(localizableMessage) is ILocalizableMessage ruleMessage)) continue;

                record.LocalizationItemType = null;
                record.WithinDatabase = false;
                record.FullName = localizableMessage.FullName;
                record.Entity = localizableMessage.Name;
                record.Ids = new List<string> { ruleMessage.Id };
                record.Comment = ruleMessage.Description;

                records.Add(record);
            }


            return records;
        }

        [Obsolete]
        private List<RecordType> CreateRecords(ILookup<Olma.LocalizationItemType, Olma.LocalizationItem> localizationItemsLookup, Olma.LocalizationItemType localizationItemType)
        {
            var items = localizationItemsLookup[localizationItemType].ToList();


            var records = new List<RecordType>();

            var types = GetClassTypes(items
                    .Select(i => i.Reference)
                    .Distinct())
                .ToDictionary(k => k.FullName, k => k);

            foreach (var item in items)
            {
                var record = new RecordType();

                if (!types.ContainsKey(item.Reference)) continue;

                var type = types[item.Reference];

                if (!(Activator.CreateInstance(type) is ILocalizableMessage ruleMessage)) continue;

                record.LocalizationItemType = localizationItemType;
                record.WithinDatabase = true;
                record.FullName = item.Reference;
                record.Entity = item.Name;
                record.FieldName = item.FieldName;
                record.Ids = new List<string> { ruleMessage.Id };
                record.Comment = ruleMessage.Description;

                records.Add(record);
            }

            return records;
        }

        [Obsolete]
        private IEnumerable<Type> GetClassTypes(IEnumerable<string> references)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(i => i.FullName.StartsWith("Dpl.B2b")).ToList();


            var types = new List<Type>();

            foreach (var reference in references)
                foreach (var assembly in assemblies)
                {
                    var type = assembly.GetType(reference);

                    if (type == null)
                        continue;
                    if (type.IsClass) types.Add(type);
                }

            return types;
        }

        /// <summary>
        /// API request for POEditor terms of a specific project
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Response ExecutePoEditTermsList(Request data)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("terms/list", Method.POST);

            //request.AddParameter("api_token", PO_API_TOKEN);
            //request.AddParameter("id", PO_PROJECT_ID);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123");    // replaces matching token in request.Resource

            request.AddObject(data);
            //request.AddJsonBody(data);

            var response = client.Execute<Response>(request);

            return response.Data;
        }

        /// <summary>
        /// API request for add term to specific project of POEditor
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Model.PoEditTermsAdd.Response ExecutePoEditTermsAdd(Model.PoEditTermsAdd.Request data)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("terms/add", Method.POST);

            //request.AddParameter("api_token", PO_API_TOKEN);
            //request.AddParameter("id", PO_PROJECT_ID);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123");    // replaces matching token in request.Resource

            request.AddParameter("api_token", data.api_token);
            request.AddParameter("id", data.id);
            request.AddParameter("data", JsonConvert.SerializeObject(data.data));

            //request.AddObject(data);
            //request.AddJsonBody(data);

            var response = client.Execute<Model.PoEditTermsAdd.Response>(request);

            return response.Data;
        }

        private static Model.PoEditTermsDelete.Response ExecutePoEditTermsDelete(Model.PoEditTermsDelete.Request data)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("terms/delete", Method.POST);

            //request.AddParameter("api_token", PO_API_TOKEN);
            //request.AddParameter("id", PO_PROJECT_ID);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123");    // replaces matching token in request.Resource

            request.AddParameter("api_token", data.api_token);
            request.AddParameter("id", data.id);
            request.AddParameter("data", JsonConvert.SerializeObject(data.data));

            //request.AddObject(data);
            //request.AddJsonBody(data);

            var response = client.Execute<Model.PoEditTermsDelete.Response>(request);

            return response.Data;
        }

        /// <summary>
        /// API request for POEditor languages of a specific project
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Model.PoEditLanguageList.Response ExcecutePoEditLanguageList(
            Model.PoEditLanguageList.Request data)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("languages/list", Method.POST);
            request.AddParameter("api_token", data.api_token);
            request.AddParameter("id", data.id);

            var response = client.Execute<Model.PoEditLanguageList.Response>(request);

            return response.Data;
        }

        private static Model.PoEditExport.Response ExecutePoExport(string languageCode)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("projects/export", Method.POST);
            var data = new Model.PoEditExport.Request()
            {
                api_token = POEDIT_API_TOKEN,
                id = POEDIT_PROJECT_ID,
                language = languageCode,
                filters = "translated",
                type = "xliff"
            };

            request.AddObject(data);

            var response = client.Execute<Model.PoEditExport.Response>(request);

            return response.Data;
        }

        private static Model.PoEditUpload.Response ExecutePoUpload(string filePath, string languageCode)
        {
            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("projects/upload", Method.POST);
            var data = new Model.PoEditUpload.Request()
            {
                api_token = POEDIT_API_TOKEN,
                id = POEDIT_PROJECT_ID,
                language = languageCode,
                updating = "terms_translations", // only inserts terms that do not exist yet
                read_from_source = 1 // reads translations from source and copies them to provide languageCode
            };

            request.AddObject(data);
            request.AddFile("file", filePath);

            var response = client.Execute<Model.PoEditUpload.Response>(request);

            return response.Data;
        }

        private void CreateOrUpdateText(string language, Response response)
        {
            var records = GenerateDbRecords();
            var localizationIds = CreateLocalizationIds(records);

            // Für die LocalizationLanguage die Daten abrufen
            var languageEntity = _localizationLanguageRepo.FindAll()
                .Include("LocalizationTexts")
                .Include("LocalizationTexts.LocalizationItem")
                .FirstOrDefault(l => l.Locale == language);

            // Keine Sprache dann abbrechen
            // TODO Hier könnte/müsste die Sprache angelegt werden, sofern POEditor die Sprachen definiert
            if (languageEntity == null) return;

            // Doppelte Einträge - abfragen (terms ohne context oder ohne context und mit pipe)  
            var filteredTermsToDelete = TermWithoutContextAndPipe(response);

            if (filteredTermsToDelete.Count() > 0)
            {
                // Doppelte Einträge - löschen 
                var result = DeleteTermsApiCall(filteredTermsToDelete);

                // Ausgabe Console 
                Console.WriteLine($"{result.Result.Content}");
            }

            // Bereinigen des Results für weiteren Prozess
            var withoutDoublets = response.result.terms.Except(filteredTermsToDelete);

            // Dictionary mit den POEdit Term Daten
            // wird zum nachschlagen der Übersetzungen benötigt
            var dic = withoutDoublets.ToDictionary(i => i.term, i => i);

            // Durchlauf über die Übersetzungsdaten
            // Es wird für jeden POEditor Term nachgesehen ob ein Eintrag in der LocalizationTexts vorhanden ist.
            // Wenn der Eintrag definiert ist wird ein Eintrag angelegt bzw. aktualisiert
            foreach (var localization in localizationIds)
            {
                //string pattern = @"^(?<namespace>.+)\.(?<poedit_key>[^\/]+)";
                //var match1 = Regex.Match(localization.FullName, pattern, RegexOptions.IgnoreCase);
                //var a = match1.Groups[0].Value;
                //var b = match1.Groups[1].Value;
                //var c = match1.Groups[2].Value;

                var generatedId = $"{language}|{localization.TermId}";

                if (dic.ContainsKey(localization.TermId))
                {
                    var term = dic[localization.TermId];

                    try
                    {
                        var localizationItem = _localizationItemRepo.FindAll().Single(i =>
                            i.Name == localization.Entity
                            && i.Type == localization.LocalizationItemType
                            && i.FieldName == localization.FieldName);
                        var text = languageEntity.LocalizationTexts.FirstOrDefault(l => l.GeneratedId == generatedId);
                        if (text == null)
                        {
                            // Create
                            languageEntity.LocalizationTexts.Add(new Olma.LocalizationText()
                            {
                                GeneratedId = generatedId,
                                LocalizationItem = localizationItem,
                                LocalizationItemId = localizationItem.Id,
                                Text = term?.translation?.content,
                                Status = Olma.LocalizationTextStatus.Confirmed
                            });
                        }
                        else
                        {
                            // Update
                            text.Status = Olma.LocalizationTextStatus.Confirmed;
                            text.Text = term?.translation?.content;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error on processing translation", ex);
                    }
                }
            }

            _localizationLanguageRepo.Update(languageEntity);
            _localizationLanguageRepo.Save();
        }

        private async Task<IRestResponse> DeleteTermsApiCall(IList<Term> filteredTermsToDelete)
        {
            var data = filteredTermsToDelete.Select(t => new { t.term, t.context });

            var client = new RestClient(POEDIT_BASE_URL);

            var request = new RestRequest("terms/delete", Method.POST);
            request.AddParameter("api_token", POEDIT_API_TOKEN);
            request.AddParameter("id", POEDIT_PROJECT_ID);
            request.AddParameter("data", JsonConvert.SerializeObject(data));

            return await client.ExecuteAsync(request);
        }

        private IList<Term> TermWithoutContextAndPipe(Response response)
        {
            var filteredList = response.result.terms
                .Where(t => string.IsNullOrWhiteSpace(t.context)
                            && !t.term.Contains("|"))
                .ToList();

            return filteredList;
        }

        private IEnumerable<RecordType> GenerateDbRecords()
        {
            var localizationItemsLookup = CreateLocalizationItemsLookup();
            var enumsRecords = CreateEnumsRecords(localizationItemsLookup);
            var entityRecords = CreateEntityRecords(localizationItemsLookup);
            //var errorRecords = CreateRecords(localizationItemsLookup, Olma.LocalizationItemType.Error);
            //var warningRecords = CreateRecords(localizationItemsLookup, Olma.LocalizationItemType.Warning);

            var unionRecords = enumsRecords.Union(entityRecords);

            return unionRecords;
        }

        private ILookup<Olma.LocalizationItemType, Olma.LocalizationItem> CreateLocalizationItemsLookup()
        {
            var localizationItemsLookup = _localizationItemRepo.FindAll()
                .AsNoTracking()
                .ToList()
                .ToLookup(i => i.Type, i => i);

            return localizationItemsLookup;
        }

        private IEnumerable<Type> GetEnumTypes(IEnumerable<string> enumNames)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(i => i.FullName.StartsWith("Dpl.B2b")
                            || i.FullName.StartsWith("System.Runtime"))
                .ToList(); // Muss gefiltert werden da es mehrere Assemblies gibt die DayOfWeek definieren

            var types = new List<Type>();
            var enums = enumNames.ToList();

            foreach (var enumName in enums)
                foreach (var assembly in assemblies)
                {
                    var type = assembly.GetType(enumName);

                    if (type == null)
                        continue;
                    if (type.IsEnum) types.Add(type);
                }

            return types;
        }

        #endregion

        #region ILocalizationService Members

        public async Task GenerateLocalizableEntries()
        {
            // get enums
            // get enum names (not int values but the strings)
            // get localization items

            var records = GenerateDbRecords().Union(CreateServerSideRecords());
            var localizationIds = CreateLocalizationIds(records);

            // get all entries from po editor
            // identify the entries that do not exist yet
            // insert entries that do not exist yet

            var listResult = ExecutePoEditTermsList(new Request
            {
                api_token = POEDIT_API_TOKEN,
                id = POEDIT_PROJECT_ID
            });

            var localizationViews = localizationIds.ToList();
            var dic = localizationViews.ToDictionary(i => i.TermId, view => view);

            var current = listResult.result.terms.Select(t => t.term).ToList();
            var target = dic.Keys.Except(current);

            var addTerms = dic.Where(k => target.Contains(k.Key))
                .Select(k => new Model.PoEditTermsAdd.Term() { term = k.Value.TermId, comment = k.Value.Comment }).ToList();

            foreach (var newItem in addTerms)
            {
                newItem.comment = dic[newItem.term].Comment;
            }

            if (addTerms.Count == 0)
            {
                Console.WriteLine($"Nothing to do, all terms are available!");

                return;
            }

            var addResult = ExecutePoEditTermsAdd(new Model.PoEditTermsAdd.Request()
            {
                api_token = POEDIT_API_TOKEN,
                id = POEDIT_PROJECT_ID,
                data = addTerms
            });

            Console.WriteLine($"parsed:{addResult.result.terms.parsed} updated:{addResult.result.terms.parsed}");
        }

        public async Task UpdateLocalizationTexts()
        {
            // get all translated entries entries from po editor
            // identify the entries that do not exist yet
            // insert entries that do not exist yet

            var languageListRequest = new Model.PoEditLanguageList.Request()
            { api_token = POEDIT_API_TOKEN, id = POEDIT_PROJECT_ID };
            var languageListResponse = ExcecutePoEditLanguageList(languageListRequest);

            foreach (var language in languageListResponse.result.languages)
            {
                var request = new Request
                { api_token = POEDIT_API_TOKEN, id = POEDIT_PROJECT_ID, language = language.code };
                var response = ExecutePoEditTermsList(request);

                CreateOrUpdateText(language.code, response);
            }
        }

        public async Task ExportLocalizationsToPath(string path)
        {
            // list all languages
            // export all languages in xliff format and save to disk
            // file name should have language code

            var languageListRequest = new Model.PoEditLanguageList.Request()
            { api_token = POEDIT_API_TOKEN, id = POEDIT_PROJECT_ID };
            var languageListResponse = ExcecutePoEditLanguageList(languageListRequest);

            foreach (var language in languageListResponse.result.languages)
            {
                var response = ExecutePoExport(language.code);
                var webClient = new WebClient();
                webClient.DownloadFile(response.result.url, Path.Combine(path, $"messages.{language.code}.xlf"));
            }
        }

        public async Task UploadLocalizationsFromFile(string filePath)
        {
            // grab messages.xliff file
            // upload file to poe
            // add all new terms to poedit
            // take source translations and copy them to the given german

            var response = ExecutePoUpload(filePath, "de");

            Console.WriteLine(
                $"Terms uploaded ({response.result.terms.parsed}, added: {response.result.terms.updated}, updated: {response.result.terms.updated})");
            Console.WriteLine(
                $"Translations uploaded (parsed: {response.result.translations.parsed}, added: {response.result.translations.updated}, updated: {response.result.translations.updated})");
        }

        public async Task DeleteFrontendTermsWithoutContext()
        {
            var requestList = new Request { api_token = POEDIT_API_TOKEN, id = POEDIT_PROJECT_ID };
            var responseList = ExecutePoEditTermsList(requestList);

            var termsToDelete = responseList.result.terms
                .Where(i => string.IsNullOrEmpty(i.context) && !i.term.Contains('|'))
                .Select(i => new Model.PoEditTermsDelete.Term() { term = i.term, context = i.context })
                .ToList();

            var response = ExecutePoEditTermsDelete(new Model.PoEditTermsDelete.Request()
            {
                api_token = POEDIT_API_TOKEN,
                id = POEDIT_PROJECT_ID,
                data = termsToDelete
            });

            Console.WriteLine(
    $"Terms cleaned (parsed: {response.result.terms.parsed}, deleted: {response.result.terms.deleted})");
        }

        #endregion

        private class RecordType
        {
            #region Properties

            public Olma.LocalizationItemType? LocalizationItemType { get; set; }

            public bool WithinDatabase { get; set; }

            public string FieldName { get; set; }

            public string FullName { get; set; }

            public string Entity { get; set; }

            public List<string> Ids { get; set; }

            public string Comment { get; set; }

            #endregion
        }
    }

    internal class LocalizationView
    {
        #region Properties

        public Olma.LocalizationItemType LocalizationItemType { get; set; }

        public string FullName { get; set; }

        public string Entity { get; set; }

        public string Id { get; set; }

        public string TermId { get; set; }

        public string FieldName { get; set; }

        public string Comment { get; set; }

        #endregion
    }

    #region Model Class for the PO Request/Response

    namespace Model
    {
        namespace PoEditTermsList
        {
            public class Request : ApiRequestBase
            {
                public string language { get; set; }
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public List<Term> terms { get; set; }
            }

            public class Term
            {
                public string term { get; set; }
                public string context { get; set; }
                public string plural { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
                public string reference { get; set; }
                public List<string> tags { get; set; }
                public string comment { get; set; }
                public Translation translation { get; set; }
            }

            public class Translation
            {
                public string content { get; set; }
                public int fuzzy { get; set; }
                public int proofread { get; set; }
                public DateTime updated { get; set; }
            }
        }

        namespace PoEditTermsAdd
        {
            public class Request : ApiRequestBase
            {
                public List<Term> data { get; set; }
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public Info terms { get; set; }
            }

            public class Info
            {
                public int parsed { get; set; }
                public int updated { get; set; }
            }

            public class Term
            {
                public string term { get; set; }

                public string context { get; set; }

                public string plural { get; set; }

                public string reference { get; set; }

                public List<string> tags { get; set; }

                public string comment { get; set; }
            }
        }

        namespace PoEditTermsDelete
        {
            public class Request : ApiRequestBase
            {
                public List<Term> data { get; set; }
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public Info terms { get; set; }
            }

            public class Info
            {
                public int parsed { get; set; }
                public int deleted { get; set; }
            }

            public class Term
            {
                public string term { get; set; }

                public string context { get; set; }
            }
        }

        namespace PoEditLanguageList
        {
            public class Request : ApiRequestBase
            {
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public List<Language> languages { get; set; }
            }

            public class Language
            {
                public string name { get; set; }
                public string code { get; set; }
                public int translations { get; set; }
                public float percentage { get; set; }
                public DateTime updated { get; set; }
            }
        }

        namespace PoEditExport
        {
            public class Request : ApiRequestBase
            {
                public string language { get; set; }
                public string type { get; set; }
                public string filters { get; set; }
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public string url { get; set; }
            }
        }

        namespace PoEditUpload
        {
            public class Request : ApiRequestBase
            {
                public string updating { get; set; }
                public string language { get; set; }
                public int read_from_source { get; set; }
            }

            public class Response : ApiResponse<Result>
            {
            }

            public class Result
            {
                public Info terms { get; set; }
                public Info translations { get; set; }
            }

            public class Info
            {
                public int parsed { get; set; }
                public int added { get; set; }
                public int updated { get; set; }
            }
        }

        public class ApiRequestBase
        {
            public string api_token { get; set; }
            public string id { get; set; }
        }

        public class ApiResponse<T>
        {
            public Response response { get; set; }
            public T result { get; set; }
        }

        public class Response
        {
            public string status { get; set; }

            public string code { get; set; }

            public string message { get; set; }
        }
    }

    #endregion
}