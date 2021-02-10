using JsonSubTypes;
using Newtonsoft.Json;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class SerializationSettings
    {
        private static JsonSerializerSettings settings;

        public static JsonSerializerSettings current { get
            {
                if (settings == null)
                    settings = CreateSettings();

                return settings;
            } }

        public static JsonSerializerSettings CreateSettings()
        {
            var stg = new JsonSerializerSettings();
            stg.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(Descriptor), "Type") // type property is only defined here
                .RegisterSubtype<ItemDescriptor>("Item")
                .RegisterSubtype<EquipmentDescriptor>("Equipment")
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build());
            return stg;
        }
    }
}
