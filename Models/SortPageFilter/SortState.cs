using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models.SortPageFilter
{
    public enum SortState
    {
        //Asc по возрастанию
        //Desc по убыванию
        dateOrderAsc,
        dateOrderDesc,
        clientDataAsc,
        clientDataDesc,
        originalLanguageAsc,
        originalLanguageDesc,
        translateLanguageAsc,
        translateLanguageDesc,
        costOfWorkAsc,
        costOfWorkDesc,
        costOfTranslationServicesAsc,
        costOfTranslationServicesDesc,
        TranslatorAsc,
        TranslatorDesc
    }
}
