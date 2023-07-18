using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace ComputerHouse.DataLayer.Models
{
    public class KeyValue
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public T GetValue<T>()
        {
            return Value != null ? JsonSerializer.Deserialize<T>(Value): default(T);
        }

        public void SetValue<T>(T value)
        {
            if (value != null)
                Value = JsonSerializer.Serialize(value);
        }
    }
}
