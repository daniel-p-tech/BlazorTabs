using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTabs.Models;

namespace BlazorTabs
{
    public class SampleData
    {
        public readonly static IEnumerable<Document> Documents = new Document[]
        {
            new Document { DocumentId = 1, Description = "Complaint" },
            new Document { DocumentId = 2, Description = "Answer" },
            new Document { DocumentId = 3, Description = "Counterclaim" },
            new Document { DocumentId = 4, Description = "Motion" },
            new Document { DocumentId = 5, Description = "Subpoena" },
        };
    }
}
