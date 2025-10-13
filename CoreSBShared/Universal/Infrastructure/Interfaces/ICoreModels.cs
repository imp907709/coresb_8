using System;
using MongoDB.Bson;

namespace CoreSBShared.Universal.Infrastructure.Interfaces
{
    //GN
    //Generic id entity
    //Id is always named as Id but type varies
    //can be used for typed store - when store used with entities of int or string type, but not with all
    //or with generic key store - when key type passed on instance
    public interface ICoreDal<TKey>
    {
        public TKey Id { get; set; }
    }
    
    public interface ICoreDalGnInt : ICoreDal<int>
    {
    }

    public interface ICoreDalGnStr : ICoreDal<string>
    {
    }

    public interface IEntityGuidId : ICoreDal<Guid>
    {
    }


    //TC
    //Type containing Id entities
    public interface ICoreDalInt
    {
        public int Id { get; set; }
    }
    public interface ICoreDalString
    {
        public string Id { get; set; }
    }
    public interface ICoreDalGuid
    {
        public Guid Id { get; set; }
    }
    
    public interface ICoreDalObj
    {
        public ObjectId Id { get; set; }
    }
  

    

    //Id independent 
    public interface ICoreCreated
    {
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
        
    public interface IMessage
    {
        public string? Message { get; set; } 
    }
    
    
    //infrastructure specific
    //mongo specific
    public interface ICoreDalObjg : ICoreDal<ObjectId>
    {
    }
}
