using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CasperInc.MainSiteCore.ViewModels {

    
    [JsonObjectAttribute(MemberSerialization.OptOut)] //All properties are serailized by defaut
    public class Narrative {
        public int Id { get; set;}
        public string Title { get; set;}
        public string Description { get; set;}
        public string BodyHtml { get; set;}
        public string[] Authors { get; set;}
        public List<Tag> Tags  { get; set;}
        public DateTime CreatedOn { get; set;}
        public DateTime UpdatedOn  {get; set;}
    }
}