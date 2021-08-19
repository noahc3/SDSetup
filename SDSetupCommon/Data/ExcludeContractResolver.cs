using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSetupCommon.Data {
    public class ExcludeContractResolver : DefaultContractResolver {
        private readonly string[] props;

        public ExcludeContractResolver(params string[] excludedProps) {
            this.props = excludedProps;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            IList<JsonProperty> retval = base.CreateProperties(type, memberSerialization);

            // retorna todas as propriedades que não estão na lista para ignorar
            retval = retval.Where(p => !this.props.Contains(p.PropertyName)).ToList();

            return retval;
        }
    }
}
