using System;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.Contracts;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;
using Microsoft.Extensions.DependencyInjection;
using Dpl.B2b.Contracts.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Dpl.B2b.BusinessLogic
{
    public class LocalizationService : ILocalizationService
    {
        private int _languageIdGerman;
        private IList<Language> _languages;
        private IDictionary<int, string> _localeDict;
        private IDictionary<string, string> _localizationItemsEnumsDict;
        private IDictionary<string, string> _localizationsDict;

        public LocalizationService(IServiceProvider serviceProvider)
        {
            this.PopulateCache(serviceProvider);
        }

        private void PopulateCache(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var olmaLocalizationLanguageRepo = scope.ServiceProvider.GetService<IRepository<Olma.LocalizationLanguage>>();
                _languages = olmaLocalizationLanguageRepo.FindAll()
                    .AsNoTracking()
                    .ProjectTo<Language>(mapper.ConfigurationProvider)
                    .ToList();

                _localeDict = _languages
                    .ToDictionary(i => i.Id, i => i.Locale);

                _languageIdGerman = _languages
                    .Single(i => i.Locale == "de")
                    .Id;

                var olmaLocalizationItemRepo = scope.ServiceProvider.GetService<IRepository<Olma.LocalizationItem>>();
                _localizationItemsEnumsDict = olmaLocalizationItemRepo.FindAll()
                    .AsNoTracking()
                    .Where(i => i.Type == Olma.LocalizationItemType.Enum)
                    .ToDictionary(i => CreateLocalizationEnumKey(i.Reference, i.FieldName), i => i.Name);

                var olmaLocalizationTextRepo = scope.ServiceProvider.GetService<IRepository<Olma.LocalizationText>>();
                _localizationsDict = olmaLocalizationTextRepo.FindAll()
                    .AsNoTracking()
                    .ToDictionary(i => i.GeneratedId, i => i.Text);
            }
        }

        public int GetGermanLanguageId()
        {
            return _languageIdGerman;
        }

        public IList<Language> GetSupportedLanguages()
        {
            return _languages;
        }

        public string GetLocale(int languageId)
        {
            return _localeDict[languageId];
        }

        public string GetLocalizationTextForEnum<T>(int languageId, T value, string fieldName=null)
            where T : System.Enum
        {
            var type = typeof(T);
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Argument is not enum type");
            }

            var key = CreateLocalizationEnumKey(type.FullName, fieldName);

            var localizationItemName = _localizationItemsEnumsDict[key];
            string enumStringValue = Enum.GetName(type, value);
            var generatedId = GetGeneratedId(languageId, localizationItemName, enumStringValue, fieldName);

            return GetLocalizationTextForGeneratedId(generatedId);
        }

        public string GetLocalizationTextForEntity(int languageId, string localizationItemName, int id, string fieldName = null)
        {
            var generatedId = GetGeneratedId(languageId, localizationItemName, id.ToString(), fieldName);
            return GetLocalizationTextForGeneratedId(generatedId);
        }

        private string GetGeneratedId(int languageId, string localizationItemName, string idOrEnumValue, string fieldName = null)
        {
            var generatedId = $"{_localeDict[languageId]}|{localizationItemName}|{idOrEnumValue}";
            if (fieldName != null)
            {
                generatedId += "|" + fieldName;
            }
            return generatedId;
        }

        private static string CreateLocalizationEnumKey(string fullName, string fieldName)
        {
            var key = fullName;
            if (!string.IsNullOrEmpty(fieldName)) key = $"{key}|{fieldName}";
            return key;
        }

        private string GetLocalizationTextForGeneratedId(string generatedId)
        {
            if (!_localizationsDict.ContainsKey(generatedId))
            {
                return generatedId;
            }
            return _localizationsDict[generatedId];
        }
    }
}