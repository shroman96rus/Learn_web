using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Models.SortPageFilter
{
    public class SortViewModel
    {
        public SortState dateOrderSort { get; private set; }

        public SortState clientDataSort { get; private set; }

        public SortState originalLanguageSort { get; private set; }

        public SortState translateLanguageSort { get; private set; }

        public SortState costOfWorkSort { get; private set; }

        public SortState costOfTranslationServicesSort { get; private set; }

        public SortState TranslatorSort { get; private set; }

        public SortState current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            dateOrderSort = sortOrder == SortState.dateOrderAsc ? SortState.dateOrderDesc : SortState.dateOrderAsc;
            clientDataSort = sortOrder == SortState.clientDataAsc ? SortState.clientDataDesc : SortState.clientDataAsc;
            originalLanguageSort = sortOrder == SortState.originalLanguageAsc ? SortState.originalLanguageDesc : SortState.originalLanguageAsc;
            translateLanguageSort = sortOrder == SortState.translateLanguageAsc ? SortState.translateLanguageDesc : SortState.translateLanguageAsc;
            costOfWorkSort = sortOrder == SortState.costOfWorkAsc ? SortState.costOfWorkDesc : SortState.costOfWorkAsc;
            costOfTranslationServicesSort = sortOrder == SortState.costOfTranslationServicesAsc ? SortState.costOfTranslationServicesDesc : SortState.costOfTranslationServicesAsc;
            TranslatorSort = sortOrder == SortState.TranslatorAsc ? SortState.TranslatorDesc : SortState.TranslatorAsc;
            current = sortOrder;
        }
    }
}
