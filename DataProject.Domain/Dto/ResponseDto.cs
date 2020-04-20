using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace DataProject.Domain.Dto
{
    [KnownType(typeof(List<PersonelDto>))]
    [KnownType(typeof(DataTable))]
    [DataContract]
    [Serializable]
    public class ResponseDto : IDeserializationCallback
    {
        [DataMember]
        public Hashtable Data { get; set; }

        public ResponseDto()
        {
            Data = new Hashtable();
        }

        public void OnDeserialization(Object o)
        {
            ;
        }

        public T GetDto<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                T data = (T)Data[key];
                return data;
            }
            return default(T);
        }

        public void AddDto<T>(string key, T data)
        {
            Data[key] = data;
        }

        public void AddDto(string key, object data)
        {
            Data[key] = data;
        }

        public void Add(string key, object data)
        {
            Data[key] = data;
        }

        public void SetError(string message)
        {
            Data["ERROR"] = message;
        }

    }
}
