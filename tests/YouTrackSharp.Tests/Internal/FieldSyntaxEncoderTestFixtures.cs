using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Tests.Internal {
  [UsedImplicitly]
  public class FieldSyntaxEncoderTestFixtures {
    public class FlatType {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Verbose]
        [JsonProperty("description")]
        public string Description { get; set; }
      }

      public class NestedTypes {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }
      }

      public class DeepNestedTypes {
        [JsonProperty("id")]
        public int Id { get; set; }

        [Verbose]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public DetailedContact Contact { get; set; }
      }

      public class DeepNestedTypesWithInheritance {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }
      }

      public class DeepNestedTypesWithCollection {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("contacts")]
        public IList<Contact> Contacts { get; set; }
      }

      public class CyclicReference {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
        
        [Verbose]
        [JsonProperty("cyclic")]
        public CyclicReference Cyclic { get; set; }
      }

      [KnownType(typeof(SimpleContact))]
      [KnownType(typeof(DetailedContact))]
      public class Contact {
        [JsonProperty("id")]
        public int Id { get; set; }
      }

      public class SimpleContact : Contact {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [Verbose]
        [JsonProperty("info")]
        public string Info { get; set; }
      }

      public class DetailedContact : Contact {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Verbose]
        [JsonProperty("info")]
        public Info Info { get; set; }
      }

      public class Info {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
      }
  }
}