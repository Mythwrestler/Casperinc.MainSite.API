using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CasperInc.MainSiteCore.ViewModels {

    
    [JsonObjectAttribute(MemberSerialization.OptOut)] //All properties are serailized by defaut
    public class Tag {
            public string KeyWord { get; set; }
            public List<Narrative> Narratives{ get; set;}
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
    }
}