using System;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkspaceFolder
    {
        [JsonProperty]
        public Uri Uri { get; set; }

        [JsonProperty]
        public string Name { get; set; }
    }
}
