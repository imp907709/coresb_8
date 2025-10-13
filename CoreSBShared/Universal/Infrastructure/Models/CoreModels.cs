using System;
using CoreSBShared.Universal.Infrastructure.Interfaces;
using MongoDB.Bson;
using Nest;

namespace CoreSBShared.Universal.Infrastructure.Models
{
    /// <summary>
    ///     Created modified date time tracking
    ///     Core class, shared by all layers and models
    /// </summary>
    public class CoreCreated : ICoreCreated
    {
        // If created with some Id entity - user, person ...
        public int? CreatedById { get; set; } = null;

        // Not only user with Id, system, unknown and others is possible
        public string? CreatedBy { get; set; } = null;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; } = null;
    }

    public interface ICoreTag
    {
        public int Index { get; set; }
        public string Text { get; set; }
    }

    
    
    //TS
    // Type containing Id
    public class CoreDalint : CoreCreated, ICoreDalInt
    {
        public int Id { get; set; }
    }
    public class CoreDalObj : CoreCreated, ICoreDalObj
    {
        public ObjectId Id { get; set; }
    }
    public class CoreDalStrg : ICoreDalGnStr
    {
        public string Id { get; set; }
    }
    
    
    
    //GN
    //generic id 
    public class CoreDalInt : ICoreDal<int>
    {
        public int Id { get; set; }
    }

    public class CoreDalString : ICoreDal<string>
    {
        public string Id { get; set; }
    }
    
    
    
    //TS
    //Core dal type specific ids
    public class CoreDalGnInt : CoreCreated, ICoreDalGnInt
    {
        public int Id { get; set; }
    }
    public class CoreDalGnObj : CoreCreated, ICoreDalObjg
    {
        public ObjectId Id { get; set; }
    }
    public class CoreDalStringg : CoreCreated, ICoreDalGnStr
    {
        public string Id { get; set; }
    }
    
    
    
    //Infrastructure specific
    //elk model
    [ElasticsearchType(IdProperty = "Id")]
    public class CoreElastic : CoreDalStrg
    {}
    
    
    
    /// <summary>
    ///     Bl and API layer specific models
    /// </summary>
    public class CoreBL : CoreDalint
    {
    }

    public class CoreAPI : CoreDalint
    {
    }
}
